//=========================================================================
// THIS IS AUTOMATICALLY GENERATED FILE! PLEASE DON'T CHANGE IT MANUALLY!
//=========================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using CommonClasses.Helpers;
using CommonClasses.DbClasses;
using DbLayer.DataModel;
using Interfaces.DbInterfaces;

namespace DbLayer.Repositories
{
	public partial class DbRepository
	{
		public int SaveDataLog(IDataLogDb dataLogDb, int? transactionNumber = null)
		{
			if (dataLogDb.InstanceId != _instanceId)
				throw new Exception("Attempt to save DataLog with wrong InstanceId");

			DataLog record;
			var recordOld = new DataLog();
			if (dataLogDb.DataLogId == 0)
			{
				record = new DataLog();
				Context.AddToDataLogs(record);
			}
			else
			{
				record = Context.DataLogs.Where(r => r.DataLogId == dataLogDb.DataLogId).First();
				ReflectionHelper.CopyAllProperties(record, recordOld);
			}

			record.InstanceId = _instanceId;
			record.UserId = dataLogDb.UserId;
			record.OperationTime = dataLogDb.OperationTime;
			record.TableName = dataLogDb.TableName;
			record.RecordId = dataLogDb.RecordId;
			record.Operation = dataLogDb.Operation;
			record.Details = dataLogDb.Details;
			record.TransactionNumber = dataLogDb.TransactionNumber;

			Context.SaveChanges();
			if (dataLogDb.DataLogId == 0)
			{
				dataLogDb.DataLogId = record.DataLogId;
				LogToDb(UserId, "DataLogs", record.DataLogId, "I", XmlHelper.GetObjectXml(record), transactionNumber);
			}
			else
			{
				LogToDb(UserId, "DataLogs", record.DataLogId, "U", XmlHelper.GetDifferenceXml(recordOld, record), transactionNumber);
			}

			return dataLogDb.DataLogId;
		}

		public DataLogDb GetDataLogById(int id)
		{
			return (from r in Context.DataLogs
				where r.DataLogId == id
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).FirstOrDefault();
		}

		public List<DataLogDb> GetAllDataLogs()
		{
			return (from r in Context.DataLogs
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).ToList();
		}

		public void DeleteDataLog(int id, string reason = null, int? transactionNumber = null)		{
			var record = Context.DataLogs.First(r => r.DataLogId == id);
				LogToDb(UserId, "DataLogs", record.DataLogId, "D", XmlHelper.GetObjectXml(record, reason), transactionNumber);
			Context.DeleteObject(record);
			Context.SaveChanges();
		}

	}
}
