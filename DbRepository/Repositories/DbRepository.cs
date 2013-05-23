using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using CommonClasses.DbClasses;
using CommonClasses.DbRepositoryInterface;
using CommonClasses.Helpers;
using CommonClasses.InfoClasses;
using DbLayer.DataContext;
using DbLayer.DataModel;

namespace DbLayer.Repositories
{
    public partial class DbRepository: IDbRepository
    {
        #region Properties and Constructors

        protected FilteredContext _context;
        protected FilteredContext Context
        {
            get { return _context; }
        }

        protected int _instanceId;
        public int InstanceId
        {
            get { return _instanceId; }
        }
        protected bool _releaseContext;
        private AuthInfo _authInfo;

        public int UserId
        {
            get { return _authInfo == null ? 0 : _authInfo.UserId; }
        }

        public void SetCompanyId(int instanceId)
        {
            if (_instanceId != 0)
                throw new Exception("Cannot set company id for DbRepository if it's already defined");
            _authInfo.CompanyId = instanceId;
            _instanceId = instanceId;
        }

        public DbRepository(int companyId)
        {
            _context = new FilteredContext(companyId);
            _instanceId = companyId;
            _releaseContext = true;
        }

        public DbRepository(AuthInfo authInfo)
        {
            _authInfo = authInfo;
            _instanceId = authInfo.CompanyId;
            _context = new FilteredContext(authInfo.CompanyId);
            _releaseContext = true;
        }

        //public DbRepository(FilteredContext context)
        //{
        //    _context = context;
        //    _releaseContext = false;
        //    _companyId = context.InstanceId;
        //}

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_releaseContext)
                _context.Dispose();
        }

        #endregion

        #region Logging

        protected void LogToDb(int userId, string tableName, int recordId, string operation, string details, int? transactionNumber)
        {
            InsertLogToDb(_instanceId, userId, tableName, recordId, operation, details, transactionNumber);
        }

        protected void LogUnlinkedToDb(int userId, string tableName, int recordId, string operation, string details, int? transactionNumber)
        {
            InsertLogToDb(_instanceId == 0 ? (int?)null : _instanceId, userId, tableName, recordId, operation, details, transactionNumber);
        }

        private void InsertLogToDb(int? instanceId, int userId, string tableName, int recordId, string operation, string details, int? transactionNumber)
        {
            var dataLog = new DataLog
            {
                InstanceId = instanceId,
                UserId = userId,
                OperationTime = DateTime.Now,
                TableName = tableName,
                RecordId = recordId,
                Operation = operation,
                Details = details,
                TransactionNumber = transactionNumber
            };
            _context.AddToDataLogs(dataLog);
            _context.SaveChanges();
        }

        #endregion

        #region History
        protected List<DataLogDb> GetHistory(int objectId, string tableName, DateTime dateTo)
        {
            return (from r in Context.DataLogs
                    where r.RecordId == objectId && r.TableName == tableName && r.OperationTime <= dateTo && r.Operation != "D"
                    select r).OrderBy(r => r.OperationTime).AsEnumerable()
                .Select(r => r.ToCommon(_authInfo)).ToList();
        }

        #endregion

        #region Methods

        public IDbTransaction BeginTransaction()
        {
            return Context.BeginTransaction();
        }

        #endregion
    }
}
