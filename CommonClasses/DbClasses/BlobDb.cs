//=========================================================================
// THIS IS AUTOMATICALLY GENERATED FILE! PLEASE DON'T CHANGE IT MANUALLY!
//=========================================================================
using System;
using Interfaces.DbInterfaces;

namespace CommonClasses.DbClasses
{
	public class BlobDb: IBlobDb
	{
		public int BlobId { get; set; }
		public int InstanceId { get; set; }
		public byte[] Data { get; set; }
	}
}
