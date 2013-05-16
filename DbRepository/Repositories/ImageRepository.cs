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
		public int SaveImage(IImageDb imageDb, int? transactionNumber = null)
		{
			if (imageDb.InstanceId != _instanceId)
				throw new Exception("Attempt to save Image with wrong InstanceId");

			Image record;
			var recordOld = new Image();
			if (imageDb.ImageId == 0)
			{
				record = new Image();
				Context.AddToImages(record);
			}
			else
			{
				record = Context.Images.Where(r => r.ImageId == imageDb.ImageId).First();
				ReflectionHelper.CopyAllProperties(record, recordOld);
			}

			record.InstanceId = _instanceId;
			record.BlobId = imageDb.BlobId;
			record.ImageName = imageDb.ImageName;
			record.ImageType = imageDb.ImageType;
			record.ImageSize = imageDb.ImageSize;

			Context.SaveChanges();
			if (imageDb.ImageId == 0)
			{
				imageDb.ImageId = record.ImageId;
				LogToDb(UserId, "Images", record.ImageId, "I", XmlHelper.GetObjectXml(record), transactionNumber);
			}
			else
			{
				LogToDb(UserId, "Images", record.ImageId, "U", XmlHelper.GetDifferenceXml(recordOld, record), transactionNumber);
			}

			return imageDb.ImageId;
		}

		public ImageDb GetImageById(int id)
		{
			return (from r in Context.Images
				where r.ImageId == id
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).FirstOrDefault();
		}

		public List<ImageDb> GetAllImages()
		{
			return (from r in Context.Images
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).ToList();
		}

		public void DeleteImage(int id, string reason = null, int? transactionNumber = null)		{
			var record = Context.Images.First(r => r.ImageId == id);
				LogToDb(UserId, "Images", record.ImageId, "D", XmlHelper.GetObjectXml(record, reason), transactionNumber);
			Context.DeleteObject(record);
			Context.SaveChanges();
		}

	}
}
