//=========================================================================
// THIS IS AUTOMATICALLY GENERATED FILE! PLEASE DON'T CHANGE IT MANUALLY!
//=========================================================================
using CommonClasses.DbClasses;

namespace DbLayer.DataModel
{
	public partial class Instance
	{
		public static implicit operator InstanceDb(Instance instance)
		{
			if (instance == null) return null;

			return new InstanceDb
			{
				InstanceId = instance.InstanceId,
				InstanceName = instance.InstanceName
			};
		}
	}

	public partial class User
	{
		public static implicit operator UserDb(User user)
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
	}

	public partial class Variable
	{
		public static implicit operator VariableDb(Variable variable)
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
	}

	public partial class DataLog
	{
		public static implicit operator DataLogDb(DataLog dataLog)
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
	}

	public partial class Blob
	{
		public static implicit operator BlobDb(Blob blob)
		{
			if (blob == null) return null;

			return new BlobDb
			{
				BlobId = blob.BlobId,
				InstanceId = blob.InstanceId,
				Data = blob.Data
			};
		}
	}

	public partial class Image
	{
		public static implicit operator ImageDb(Image image)
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
	}

	public partial class UserInstanceUsage
	{
		public static implicit operator UserInstanceUsageDb(UserInstanceUsage userInstanceUsage)
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
	}

	public partial class TemporaryCode
	{
		public static implicit operator TemporaryCodeDb(TemporaryCode temporaryCode)
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
	}

	public partial class Role
	{
		public static implicit operator RoleDb(Role role)
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
	}

	public partial class UsersToRole
	{
		public static implicit operator UsersToRoleDb(UsersToRole usersToRole)
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
	}

	public partial class Component
	{
		public static implicit operator ComponentDb(Component component)
		{
			if (component == null) return null;

			return new ComponentDb
			{
				ComponentId = component.ComponentId,
				Name = component.Name,
				IsReadOnlyAccess = component.IsReadOnlyAccess
			};
		}
	}

	public partial class ComponentsToRole
	{
		public static implicit operator ComponentsToRoleDb(ComponentsToRole componentsToRole)
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
	}

	public partial class TransactionSeq
	{
		public static implicit operator TransactionSeqDb(TransactionSeq transactionSeq)
		{
			if (transactionSeq == null) return null;

			return new TransactionSeqDb
			{
				TransactionSeqId = transactionSeq.TransactionSeqId
			};
		}
	}

}
