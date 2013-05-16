//=========================================================================
// THIS IS AUTOMATICALLY GENERATED FILE! PLEASE DON'T CHANGE IT MANUALLY!
//=========================================================================
using System;
using Interfaces.DbInterfaces;

namespace CommonClasses.DbClasses
{
	public class DataLogDb: IDataLogDb
	{
		public int DataLogId { get; set; }
		public int? InstanceId { get; set; }
		public int UserId { get; set; }
		public DateTime OperationTime { get; set; }
		public string TableName { get; set; }
		public int RecordId { get; set; }
		public string Operation { get; set; }
		public string Details { get; set; }
		public int? TransactionNumber { get; set; }
	}
}
