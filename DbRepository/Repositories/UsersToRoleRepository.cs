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
		public int SaveUsersToRole(IUsersToRoleDb usersToRoleDb, int? transactionNumber = null)
		{
			if (usersToRoleDb.InstanceId != _instanceId)
				throw new Exception("Attempt to save UsersToRole with wrong InstanceId");

			UsersToRole record;
			var recordOld = new UsersToRole();
			if (usersToRoleDb.UserRoleId == 0)
			{
				record = new UsersToRole();
				Context.AddToUsersToRoles(record);
			}
			else
			{
				record = Context.UsersToRoles.Where(r => r.UserRoleId == usersToRoleDb.UserRoleId).First();
				ReflectionHelper.CopyAllProperties(record, recordOld);
			}

			record.InstanceId = _instanceId;
			record.UserId = usersToRoleDb.UserId;
			record.RoleId = usersToRoleDb.RoleId;

			Context.SaveChanges();
			if (usersToRoleDb.UserRoleId == 0)
			{
				usersToRoleDb.UserRoleId = record.UserRoleId;
				LogToDb(UserId, "UsersToRoles", record.UserRoleId, "I", XmlHelper.GetObjectXml(record), transactionNumber);
			}
			else
			{
				LogToDb(UserId, "UsersToRoles", record.UserRoleId, "U", XmlHelper.GetDifferenceXml(recordOld, record), transactionNumber);
			}

			return usersToRoleDb.UserRoleId;
		}

		public UsersToRoleDb GetUsersToRoleById(int id)
		{
			return (from r in Context.UsersToRoles
				where r.UserRoleId == id
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).FirstOrDefault();
		}

		public List<UsersToRoleDb> GetAllUsersToRoles()
		{
			return (from r in Context.UsersToRoles
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).ToList();
		}

		public void DeleteUsersToRole(int id, string reason = null, int? transactionNumber = null)		{
			var record = Context.UsersToRoles.First(r => r.UserRoleId == id);
				LogToDb(UserId, "UsersToRoles", record.UserRoleId, "D", XmlHelper.GetObjectXml(record, reason), transactionNumber);
			Context.DeleteObject(record);
			Context.SaveChanges();
		}

	}
}
