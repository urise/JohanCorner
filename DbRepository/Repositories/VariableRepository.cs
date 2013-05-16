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
		public int SaveVariable(IVariableDb variableDb, int? transactionNumber = null)
		{
			if (variableDb.InstanceId != _instanceId)
				throw new Exception("Attempt to save Variable with wrong InstanceId");

			Variable record;
			var recordOld = new Variable();
			if (variableDb.VariableId == 0)
			{
				record = new Variable();
				Context.AddToVariables(record);
			}
			else
			{
				record = Context.Variables.Where(r => r.VariableId == variableDb.VariableId).First();
				ReflectionHelper.CopyAllProperties(record, recordOld);
			}

			record.InstanceId = _instanceId;
			record.VariableKey = variableDb.VariableKey;
			record.VariableValue = variableDb.VariableValue;

			Context.SaveChanges();
			if (variableDb.VariableId == 0)
			{
				variableDb.VariableId = record.VariableId;
				LogToDb(UserId, "Variables", record.VariableId, "I", XmlHelper.GetObjectXml(record), transactionNumber);
			}
			else
			{
				LogToDb(UserId, "Variables", record.VariableId, "U", XmlHelper.GetDifferenceXml(recordOld, record), transactionNumber);
			}

			return variableDb.VariableId;
		}

		public VariableDb GetVariableById(int id)
		{
			return (from r in Context.Variables
				where r.VariableId == id
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).FirstOrDefault();
		}

		public List<VariableDb> GetAllVariables()
		{
			return (from r in Context.Variables
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).ToList();
		}

		public void DeleteVariable(int id, string reason = null, int? transactionNumber = null)		{
			var record = Context.Variables.First(r => r.VariableId == id);
				LogToDb(UserId, "Variables", record.VariableId, "D", XmlHelper.GetObjectXml(record, reason), transactionNumber);
			Context.DeleteObject(record);
			Context.SaveChanges();
		}

	}
}
