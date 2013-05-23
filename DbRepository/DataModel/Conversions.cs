//=========================================================================
// THIS IS AUTOMATICALLY GENERATED FILE! PLEASE DON'T CHANGE IT MANUALLY!
//=========================================================================
using CommonClasses.DbClasses;
using Interfaces.MiscInterfaces;

namespace DbLayer.DataModel
{
	public static class Conversions
	{
		public static InstanceDb ToCommon(this Instance instance, IEncryptor encryptor)
		{
			if (instance == null) return null;

			return new InstanceDb
			{
				InstanceId = instance.InstanceId,
				InstanceName = instance.InstanceName
			};
		}

		public static UserDb ToCommon(this User user, IEncryptor encryptor)
		{
			if (user == null) return null;

			return new UserDb
			{
				UserId = user.UserId,
				Login = user.Login,
				Password = user.Password,
				Email = user.Email,
				UserFIO = user.UserFIO,
				RegistrationCode = user.RegistrationCode,
				IsActive = user.IsActive
			};
		}

		public static VariableDb ToCommon(this Variable variable, IEncryptor encryptor)
		{
			if (variable == null) return null;

			return new VariableDb
			{
				VariableId = variable.VariableId,
				InstanceId = variable.InstanceId,
				VariableKey = variable.VariableKey,
				VariableValue = variable.VariableValue
			};
		}

		public static DataLogDb ToCommon(this DataLog dataLog, IEncryptor encryptor)
		{
			if (dataLog == null) return null;

			return new DataLogDb
			{
				DataLogId = dataLog.DataLogId,
				InstanceId = dataLog.InstanceId,
				UserId = dataLog.UserId,
				OperationTime = dataLog.OperationTime,
				TableName = dataLog.TableName,
				RecordId = dataLog.RecordId,
				Operation = dataLog.Operation,
				Details = dataLog.Details,
				TransactionNumber = dataLog.TransactionNumber
			};
		}

		public static BlobDb ToCommon(this Blob blob, IEncryptor encryptor)
		{
			if (blob == null) return null;

			return new BlobDb
			{
				BlobId = blob.BlobId,
				InstanceId = blob.InstanceId,
				Data = blob.Data
			};
		}

		public static ImageDb ToCommon(this Image image, IEncryptor encryptor)
		{
			if (image == null) return null;

			return new ImageDb
			{
				ImageId = image.ImageId,
				InstanceId = image.InstanceId,
				BlobId = image.BlobId,
				ImageName = image.ImageName,
				ImageType = image.ImageType,
				ImageSize = image.ImageSize
			};
		}

		public static UserInstanceUsageDb ToCommon(this UserInstanceUsage userInstanceUsage, IEncryptor encryptor)
		{
			if (userInstanceUsage == null) return null;

			return new UserInstanceUsageDb
			{
				UserInstanceUsageId = userInstanceUsage.UserInstanceUsageId,
				UserId = userInstanceUsage.UserId,
				UsedInstanceId = userInstanceUsage.UsedInstanceId,
				Date = userInstanceUsage.Date
			};
		}

		public static TemporaryCodeDb ToCommon(this TemporaryCode temporaryCode, IEncryptor encryptor)
		{
			if (temporaryCode == null) return null;

			return new TemporaryCodeDb
			{
				TemporaryCodeId = temporaryCode.TemporaryCodeId,
				UserId = temporaryCode.UserId,
				Code = temporaryCode.Code,
				ExpireDate = temporaryCode.ExpireDate
			};
		}

		public static RoleDb ToCommon(this Role role, IEncryptor encryptor)
		{
			if (role == null) return null;

			return new RoleDb
			{
				RoleId = role.RoleId,
				InstanceId = role.InstanceId,
				Name = role.Name,
				Type = role.Type
			};
		}

		public static UsersToRoleDb ToCommon(this UsersToRole usersToRole, IEncryptor encryptor)
		{
			if (usersToRole == null) return null;

			return new UsersToRoleDb
			{
				UserRoleId = usersToRole.UserRoleId,
				InstanceId = usersToRole.InstanceId,
				UserId = usersToRole.UserId,
				RoleId = usersToRole.RoleId
			};
		}

		public static ComponentDb ToCommon(this Component component, IEncryptor encryptor)
		{
			if (component == null) return null;

			return new ComponentDb
			{
				ComponentId = component.ComponentId,
				Name = component.Name,
				IsReadOnlyAccess = component.IsReadOnlyAccess
			};
		}

		public static ComponentsToRoleDb ToCommon(this ComponentsToRole componentsToRole, IEncryptor encryptor)
		{
			if (componentsToRole == null) return null;

			return new ComponentsToRoleDb
			{
				ComponentToRoleId = componentsToRole.ComponentToRoleId,
				InstanceId = componentsToRole.InstanceId,
				ComponentId = componentsToRole.ComponentId,
				RoleId = componentsToRole.RoleId,
				AccessLevel = componentsToRole.AccessLevel
			};
		}

		public static TransactionSeqDb ToCommon(this TransactionSeq transactionSeq, IEncryptor encryptor)
		{
			if (transactionSeq == null) return null;

			return new TransactionSeqDb
			{
				TransactionSeqId = transactionSeq.TransactionSeqId
			};
		}

	}
}
