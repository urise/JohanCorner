//=========================================================================
// THIS IS AUTOMATICALLY GENERATED FILE! PLEASE DON'T CHANGE IT MANUALLY!
//=========================================================================
using System;
using Interfaces.DbInterfaces;

namespace CommonClasses.DbClasses
{
	public class ImageDb: IImageDb
	{
		public int ImageId { get; set; }
		public int InstanceId { get; set; }
		public int BlobId { get; set; }
		public string ImageName { get; set; }
		public string ImageType { get; set; }
		public int ImageSize { get; set; }
	}
}
