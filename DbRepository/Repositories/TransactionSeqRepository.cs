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
		public int SaveTransactionSeq(ITransactionSeqDb transactionSeqDb, int? transactionNumber = null)
		{
			TransactionSeq record;
			var recordOld = new TransactionSeq();
			if (transactionSeqDb.TransactionSeqId == 0)
			{
				record = new TransactionSeq();
				Context.AddToTransactionSeqs(record);
			}
			else
			{
				record = Context.TransactionSeqs.Where(r => r.TransactionSeqId == transactionSeqDb.TransactionSeqId).First();
				ReflectionHelper.CopyAllProperties(record, recordOld);
			}


			Context.SaveChanges();
			if (transactionSeqDb.TransactionSeqId == 0)
			{
				transactionSeqDb.TransactionSeqId = record.TransactionSeqId;
				LogUnlinkedToDb(UserId, "TransactionSeqs", record.TransactionSeqId, "I", XmlHelper.GetObjectXml(record), transactionNumber);
			}
			else
			{
				LogUnlinkedToDb(UserId, "TransactionSeqs", record.TransactionSeqId, "U", XmlHelper.GetDifferenceXml(recordOld, record), transactionNumber);
			}

			return transactionSeqDb.TransactionSeqId;
		}

		public TransactionSeqDb GetTransactionSeqById(int id)
		{
			return (from r in Context.TransactionSeqs
				where r.TransactionSeqId == id
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).FirstOrDefault();
		}

		public List<TransactionSeqDb> GetAllTransactionSeqs()
		{
			return (from r in Context.TransactionSeqs
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).ToList();
		}

		public void DeleteTransactionSeq(int id, string reason = null, int? transactionNumber = null)		{
			var record = Context.TransactionSeqs.First(r => r.TransactionSeqId == id);
				LogUnlinkedToDb(UserId, "TransactionSeqs", record.TransactionSeqId, "D", XmlHelper.GetObjectXml(record, reason), transactionNumber);
			Context.DeleteObject(record);
			Context.SaveChanges();
		}

	}
}
