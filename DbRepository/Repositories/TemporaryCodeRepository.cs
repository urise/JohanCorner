//=========================================================================
// THIS IS AUTOMATICALLY GENERATED FILE! PLEASE DON'T CHANGE IT MANUALLY!
//=========================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using CommonClasses.DbClasses;
using DbLayer.DataModel;
using Interfaces.DbInterfaces;

namespace DbLayer.Repositories
{
	public partial class DbRepository
	{
		public int SaveTemporaryCode(ITemporaryCodeDb temporaryCodeDb, int? transactionNumber = null)
		{
			TemporaryCode record;
			if (temporaryCodeDb.TemporaryCodeId == 0)
			{
				record = new TemporaryCode();
				Context.AddToTemporaryCodes(record);
			}
			else
			{
				record = Context.TemporaryCodes.Where(r => r.TemporaryCodeId == temporaryCodeDb.TemporaryCodeId).First();
			}

			record.UserId = temporaryCodeDb.UserId;
			record.Code = temporaryCodeDb.Code;
			record.ExpireDate = temporaryCodeDb.ExpireDate;

			Context.SaveChanges();
			if (temporaryCodeDb.TemporaryCodeId == 0)
			{
				temporaryCodeDb.TemporaryCodeId = record.TemporaryCodeId;
			}

			return temporaryCodeDb.TemporaryCodeId;
		}

		public TemporaryCodeDb GetTemporaryCodeById(int id)
		{
			return (from r in Context.TemporaryCodes
				where r.TemporaryCodeId == id
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).FirstOrDefault();
		}

		public List<TemporaryCodeDb> GetAllTemporaryCodes()
		{
			return (from r in Context.TemporaryCodes
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).ToList();
		}

		public void DeleteTemporaryCode(int id, string reason = null, int? transactionNumber = null)		{
			var record = Context.TemporaryCodes.First(r => r.TemporaryCodeId == id);
			Context.DeleteObject(record);
			Context.SaveChanges();
		}

	}
}
