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

		private readonly int _instanceId;
		public int InstanceId{ get { return _instanceId; } }
		private readonly DataEntities _context;

		public FilteredContext(int instanceId)
		{
			_instanceId = instanceId;
			_context = new DataEntities();
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

    }
}
