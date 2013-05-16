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
		public int SaveComponent(IComponentDb componentDb, int? transactionNumber = null)
		{
			Component record;
			var recordOld = new Component();
			if (componentDb.ComponentId == 0)
			{
				record = new Component();
				Context.AddToComponents(record);
			}
			else
			{
				record = Context.Components.Where(r => r.ComponentId == componentDb.ComponentId).First();
				ReflectionHelper.CopyAllProperties(record, recordOld);
			}

			record.Name = componentDb.Name;
			record.IsReadOnlyAccess = componentDb.IsReadOnlyAccess;

			Context.SaveChanges();
			if (componentDb.ComponentId == 0)
			{
				componentDb.ComponentId = record.ComponentId;
				LogUnlinkedToDb(UserId, "Components", record.ComponentId, "I", XmlHelper.GetObjectXml(record), transactionNumber);
			}
			else
			{
				LogUnlinkedToDb(UserId, "Components", record.ComponentId, "U", XmlHelper.GetDifferenceXml(recordOld, record), transactionNumber);
			}

			return componentDb.ComponentId;
		}

		public ComponentDb GetComponentById(int id)
		{
			return (from r in Context.Components
				where r.ComponentId == id
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).FirstOrDefault();
		}

		public List<ComponentDb> GetAllComponents()
		{
			return (from r in Context.Components
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).ToList();
		}

		public void DeleteComponent(int id, string reason = null, int? transactionNumber = null)		{
			var record = Context.Components.First(r => r.ComponentId == id);
				LogUnlinkedToDb(UserId, "Components", record.ComponentId, "D", XmlHelper.GetObjectXml(record, reason), transactionNumber);
			Context.DeleteObject(record);
			Context.SaveChanges();
		}

	}
}
