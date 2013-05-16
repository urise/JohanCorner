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
		public int SaveRole(IRoleDb roleDb, int? transactionNumber = null)
		{
			if (roleDb.InstanceId != _instanceId)
				throw new Exception("Attempt to save Role with wrong InstanceId");

			Role record;
			var recordOld = new Role();
			if (roleDb.RoleId == 0)
			{
				record = new Role();
				Context.AddToRoles(record);
			}
			else
			{
				record = Context.Roles.Where(r => r.RoleId == roleDb.RoleId).First();
				ReflectionHelper.CopyAllProperties(record, recordOld);
			}

			record.InstanceId = _instanceId;
			record.Name = roleDb.Name;
			record.Type = roleDb.Type;

			Context.SaveChanges();
			if (roleDb.RoleId == 0)
			{
				roleDb.RoleId = record.RoleId;
				LogToDb(UserId, "Roles", record.RoleId, "I", XmlHelper.GetObjectXml(record), transactionNumber);
			}
			else
			{
				LogToDb(UserId, "Roles", record.RoleId, "U", XmlHelper.GetDifferenceXml(recordOld, record), transactionNumber);
			}

			return roleDb.RoleId;
		}

		public RoleDb GetRoleById(int id)
		{
			return (from r in Context.Roles
				where r.RoleId == id
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).FirstOrDefault();
		}

		public List<RoleDb> GetAllRoles()
		{
			return (from r in Context.Roles
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).ToList();
		}

		public void DeleteRole(int id, string reason = null, int? transactionNumber = null)		{
			var record = Context.Roles.First(r => r.RoleId == id);
				LogToDb(UserId, "Roles", record.RoleId, "D", XmlHelper.GetObjectXml(record, reason), transactionNumber);
			Context.DeleteObject(record);
			Context.SaveChanges();
		}

	}
}
