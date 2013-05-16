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
		public int SaveComponentsToRole(IComponentsToRoleDb componentsToRoleDb, int? transactionNumber = null)
		{
			if (componentsToRoleDb.InstanceId != _instanceId)
				throw new Exception("Attempt to save ComponentsToRole with wrong InstanceId");

			ComponentsToRole record;
			var recordOld = new ComponentsToRole();
			if (componentsToRoleDb.ComponentToRoleId == 0)
			{
				record = new ComponentsToRole();
				Context.AddToComponentsToRoles(record);
			}
			else
			{
				record = Context.ComponentsToRoles.Where(r => r.ComponentToRoleId == componentsToRoleDb.ComponentToRoleId).First();
				ReflectionHelper.CopyAllProperties(record, recordOld);
			}

			record.InstanceId = _instanceId;
			record.ComponentId = componentsToRoleDb.ComponentId;
			record.RoleId = componentsToRoleDb.RoleId;
			record.AccessLevel = componentsToRoleDb.AccessLevel;

			Context.SaveChanges();
			if (componentsToRoleDb.ComponentToRoleId == 0)
			{
				componentsToRoleDb.ComponentToRoleId = record.ComponentToRoleId;
				LogToDb(UserId, "ComponentsToRoles", record.ComponentToRoleId, "I", XmlHelper.GetObjectXml(record), transactionNumber);
			}
			else
			{
				LogToDb(UserId, "ComponentsToRoles", record.ComponentToRoleId, "U", XmlHelper.GetDifferenceXml(recordOld, record), transactionNumber);
			}

			return componentsToRoleDb.ComponentToRoleId;
		}

		public ComponentsToRoleDb GetComponentsToRoleById(int id)
		{
			return (from r in Context.ComponentsToRoles
				where r.ComponentToRoleId == id
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).FirstOrDefault();
		}

		public List<ComponentsToRoleDb> GetAllComponentsToRoles()
		{
			return (from r in Context.ComponentsToRoles
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).ToList();
		}

		public void DeleteComponentsToRole(int id, string reason = null, int? transactionNumber = null)		{
			var record = Context.ComponentsToRoles.First(r => r.ComponentToRoleId == id);
				LogToDb(UserId, "ComponentsToRoles", record.ComponentToRoleId, "D", XmlHelper.GetObjectXml(record, reason), transactionNumber);
			Context.DeleteObject(record);
			Context.SaveChanges();
		}

	}
}
