//=========================================================================
// THIS IS AUTOMATICALLY GENERATED FILE! PLEASE DON'T CHANGE IT MANUALLY!
//=========================================================================
using System;
using Interfaces.DbInterfaces;

namespace CommonClasses.DbClasses
{
	public class UserInstanceUsageDb: IUserInstanceUsageDb
	{
		public int UserInstanceUsageId { get; set; }
		public int UserId { get; set; }
		public int UsedInstanceId { get; set; }
		public DateTime Date { get; set; }
	}
}
