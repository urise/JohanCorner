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
		public int SaveBlob(IBlobDb blobDb, int? transactionNumber = null)
		{
			if (blobDb.InstanceId != _instanceId)
				throw new Exception("Attempt to save Blob with wrong InstanceId");

			Blob record;
			var recordOld = new Blob();
			if (blobDb.BlobId == 0)
			{
				record = new Blob();
				Context.AddToBlobs(record);
			}
			else
			{
				record = Context.Blobs.Where(r => r.BlobId == blobDb.BlobId).First();
				ReflectionHelper.CopyAllProperties(record, recordOld);
			}

			record.InstanceId = _instanceId;
			record.Data = blobDb.Data;

			Context.SaveChanges();
			if (blobDb.BlobId == 0)
			{
				blobDb.BlobId = record.BlobId;
				LogToDb(UserId, "Blobs", record.BlobId, "I", XmlHelper.GetObjectXml(record), transactionNumber);
			}
			else
			{
				LogToDb(UserId, "Blobs", record.BlobId, "U", XmlHelper.GetDifferenceXml(recordOld, record), transactionNumber);
			}

			return blobDb.BlobId;
		}

		public BlobDb GetBlobById(int id)
		{
			return (from r in Context.Blobs
				where r.BlobId == id
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).FirstOrDefault();
		}

		public List<BlobDb> GetAllBlobs()
		{
			return (from r in Context.Blobs
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).ToList();
		}

		public void DeleteBlob(int id, string reason = null, int? transactionNumber = null)		{
			var record = Context.Blobs.First(r => r.BlobId == id);
				LogToDb(UserId, "Blobs", record.BlobId, "D", XmlHelper.GetObjectXml(record, reason), transactionNumber);
			Context.DeleteObject(record);
			Context.SaveChanges();
		}

	}
}
