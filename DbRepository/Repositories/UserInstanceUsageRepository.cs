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
		public int SaveUserInstanceUsage(IUserInstanceUsageDb userInstanceUsageDb, int? transactionNumber = null)
		{
			UserInstanceUsage record;
			var recordOld = new UserInstanceUsage();
			if (userInstanceUsageDb.UserInstanceUsageId == 0)
			{
				record = new UserInstanceUsage();
				Context.AddToUserInstanceUsages(record);
			}
			else
			{
				record = Context.UserInstanceUsages.Where(r => r.UserInstanceUsageId == userInstanceUsageDb.UserInstanceUsageId).First();
				ReflectionHelper.CopyAllProperties(record, recordOld);
			}

			record.UserId = userInstanceUsageDb.UserId;
			record.UsedInstanceId = userInstanceUsageDb.UsedInstanceId;
			record.Date = userInstanceUsageDb.Date;

			Context.SaveChanges();
			if (userInstanceUsageDb.UserInstanceUsageId == 0)
			{
				userInstanceUsageDb.UserInstanceUsageId = record.UserInstanceUsageId;
				LogUnlinkedToDb(UserId, "UserInstanceUsages", record.UserInstanceUsageId, "I", XmlHelper.GetObjectXml(record), transactionNumber);
			}
			else
			{
				LogUnlinkedToDb(UserId, "UserInstanceUsages", record.UserInstanceUsageId, "U", XmlHelper.GetDifferenceXml(recordOld, record), transactionNumber);
			}

			return userInstanceUsageDb.UserInstanceUsageId;
		}

		public UserInstanceUsageDb GetUserInstanceUsageById(int id)
		{
			return (from r in Context.UserInstanceUsages
				where r.UserInstanceUsageId == id
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).FirstOrDefault();
		}

		public List<UserInstanceUsageDb> GetAllUserInstanceUsages()
		{
			return (from r in Context.UserInstanceUsages
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).ToList();
		}

		public void DeleteUserInstanceUsage(int id, string reason = null, int? transactionNumber = null)		{
			var record = Context.UserInstanceUsages.First(r => r.UserInstanceUsageId == id);
				LogUnlinkedToDb(UserId, "UserInstanceUsages", record.UserInstanceUsageId, "D", XmlHelper.GetObjectXml(record, reason), transactionNumber);
			Context.DeleteObject(record);
			Context.SaveChanges();
		}

	}
}
