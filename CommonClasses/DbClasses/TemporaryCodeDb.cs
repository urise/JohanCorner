//=========================================================================
// THIS IS AUTOMATICALLY GENERATED FILE! PLEASE DON'T CHANGE IT MANUALLY!
//=========================================================================
using System;
using Interfaces.DbInterfaces;

namespace CommonClasses.DbClasses
{
	public class TemporaryCodeDb: ITemporaryCodeDb
	{
		public int TemporaryCodeId { get; set; }
		public int UserId { get; set; }
		public string Code { get; set; }
		public DateTime ExpireDate { get; set; }
	}
}
