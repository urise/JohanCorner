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
		public int SaveInstance(IInstanceDb instanceDb, int? transactionNumber = null)
		{
			if (instanceDb.InstanceId != _instanceId)
				throw new Exception("Attempt to save Instance with wrong InstanceId");

			Instance record;
			var recordOld = new Instance();
			if (instanceDb.InstanceId == 0)
			{
				record = new Instance();
				Context.AddToInstances(record);
			}
			else
			{
				record = Context.Instances.Where(r => r.InstanceId == instanceDb.InstanceId).First();
				ReflectionHelper.CopyAllProperties(record, recordOld);
			}

			record.InstanceName = instanceDb.InstanceName;

			Context.SaveChanges();
			if (instanceDb.InstanceId == 0)
			{
				instanceDb.InstanceId = record.InstanceId;
				LogToDb(UserId, "Instances", record.InstanceId, "I", XmlHelper.GetObjectXml(record), transactionNumber);
			}
			else
			{
				LogToDb(UserId, "Instances", record.InstanceId, "U", XmlHelper.GetDifferenceXml(recordOld, record), transactionNumber);
			}

			return instanceDb.InstanceId;
		}

		public InstanceDb GetInstanceById(int id)
		{
			return (from r in Context.Instances
				where r.InstanceId == id
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).FirstOrDefault();
		}

		public List<InstanceDb> GetAllInstances()
		{
			return (from r in Context.Instances
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).ToList();
		}

		public void DeleteInstance(int id, string reason = null, int? transactionNumber = null)		{
			var record = Context.Instances.First(r => r.InstanceId == id);
				LogToDb(UserId, "Instances", record.InstanceId, "D", XmlHelper.GetObjectXml(record, reason), transactionNumber);
			Context.DeleteObject(record);
			Context.SaveChanges();
		}

	}
}
