//=========================================================================
// THIS IS AUTOMATICALLY GENERATED FILE! PLEASE DON'T CHANGE IT MANUALLY!
//=========================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using CommonClasses.DbClasses;
using DbLayer.DataModel;
using Interfaces.DbInterfaces;

namespace DbLayer.Repositories
{
	public partial class DbRepository
	{
		public int SaveUser(IUserDb userDb, int? transactionNumber = null)
		{
			User record;
			if (userDb.UserId == 0)
			{
				record = new User();
				Context.AddToUsers(record);
			}
			else
			{
				record = Context.Users.Where(r => r.UserId == userDb.UserId).First();
			}

			record.Login = userDb.Login;
			record.Password = userDb.Password;
			record.Email = userDb.Email;
			record.UserFIO = userDb.UserFIO;
			record.RegistrationCode = userDb.RegistrationCode;
			record.IsActive = userDb.IsActive;

			Context.SaveChanges();
			if (userDb.UserId == 0)
			{
				userDb.UserId = record.UserId;
			}

			return userDb.UserId;
		}

		public UserDb GetUserById(int id)
		{
			return (from r in Context.Users
				where r.UserId == id
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).FirstOrDefault();
		}

		public List<UserDb> GetAllUsers()
		{
			return (from r in Context.Users
				select r).AsEnumerable()
				.Select(r => r.ToCommon(_authInfo)).ToList();
		}

		public void DeleteUser(int id, string reason = null, int? transactionNumber = null)		{
			var record = Context.Users.First(r => r.UserId == id);
			Context.DeleteObject(record);
			Context.SaveChanges();
		}

	}
}
