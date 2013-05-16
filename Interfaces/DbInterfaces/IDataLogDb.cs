//=========================================================================
// THIS IS AUTOMATICALLY GENERATED FILE! PLEASE DON'T CHANGE IT MANUALLY!
//=========================================================================
using System;

namespace Interfaces.DbInterfaces
{
	public interface IDataLogDb
	{
		int DataLogId { get; set; }
		int? InstanceId { get; set; }
		int UserId { get; set; }
		DateTime OperationTime { get; set; }
		string TableName { get; set; }
		int RecordId { get; set; }
		string Operation { get; set; }
		string Details { get; set; }
		int? TransactionNumber { get; set; }
	}
}
