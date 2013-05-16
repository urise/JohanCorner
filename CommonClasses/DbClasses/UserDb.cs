//=========================================================================
// THIS IS AUTOMATICALLY GENERATED FILE! PLEASE DON'T CHANGE IT MANUALLY!
//=========================================================================
using System;
using Interfaces.DbInterfaces;

namespace CommonClasses.DbClasses
{
	public class UserDb: IUserDb
	{
		public int UserId { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public string UserFIO { get; set; }
		public string RegistrationCode { get; set; }
		public bool IsActive { get; set; }
	}
}
