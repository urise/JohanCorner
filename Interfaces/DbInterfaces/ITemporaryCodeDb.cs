//=========================================================================
// THIS IS AUTOMATICALLY GENERATED FILE! PLEASE DON'T CHANGE IT MANUALLY!
//=========================================================================
using System;

namespace Interfaces.DbInterfaces
{
	public interface ITemporaryCodeDb
	{
		int TemporaryCodeId { get; set; }
		int UserId { get; set; }
		string Code { get; set; }
		DateTime ExpireDate { get; set; }
	}
}
