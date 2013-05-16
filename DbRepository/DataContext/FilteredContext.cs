using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using DbLayer.DataModel;

namespace DbLayer.DataContext
{
	public partial class FilteredContext: IDisposable
	{
		#region Properties and Constructors

		private readonly int _companyId;
		public int CompanyId{ get { return _companyId; } }
		private readonly AASEntities _context;

		public FilteredContext(int companyId)
		{
			_companyId = companyId;
			_context = new AASEntities();
		}

		#endregion

        #region Transaction

        public DbTransaction BeginTransaction()
        {
            if (_context.Connection.State != ConnectionState.Open)
                _context.Connection.Open();
            return _context.Connection.BeginTransaction();
        }

        #endregion
        
        #region IDisposable Members

        public void Dispose()
        {
            _context.Dispose();
        }
        #endregion

        #region Other Methods

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void DeleteObject(object record)
        {
            _context.DeleteObject(record);
        }
        #endregion

        #region All Company Tables

        public IQueryable<TransactionType> AllCompaniesTransactionTypes
        {
            get { return _context.TransactionTypes; }
        }

        public IQueryable<Expression> AllCompaniesExpressions
        {
            get { return _context.Expressions; }
        }

        public IQueryable<Validation> AllCompaniesVlidations
        {
            get { return _context.Validations; }
        }

        public IQueryable<Duty> AllCompaniesDuties
        {
            get { return _context.Duties; }
        }

        public IQueryable<Company> AllCompanies
        {
            get { return _context.Companies; }
        }
        #endregion
    }
}
