//=========================================================================
// THIS IS AUTOMATICALLY GENERATED FILE! PLEASE DON'T CHANGE IT MANUALLY!
//=========================================================================
using System.Linq;
using DbLayer.DataModel;

namespace DbLayer.DataContext
{
	public partial class FilteredContext
	{
		#region Tables

		public IQueryable<Instance> Instances
		{
			get { return _context.Instances.Where(r => r.InstanceId == _instanceId); }
		}

		public IQueryable<User> Users
		{
			get { return _context.Users; }
		}

		public IQueryable<Variable> Variables
		{
			get { return _context.Variables.Where(r => r.InstanceId == _instanceId); }
		}

		public IQueryable<DataLog> DataLogs
		{
			get { return _context.DataLogs.Where(r => r.InstanceId == _instanceId); }
		}

		public IQueryable<Blob> Blobs
		{
			get { return _context.Blobs.Where(r => r.InstanceId == _instanceId); }
		}

		public IQueryable<Image> Images
		{
			get { return _context.Images.Where(r => r.InstanceId == _instanceId); }
		}

		public IQueryable<UserInstanceUsage> UserInstanceUsages
		{
			get { return _context.UserInstanceUsages; }
		}

		public IQueryable<TemporaryCode> TemporaryCodes
		{
			get { return _context.TemporaryCodes; }
		}

		public IQueryable<Role> Roles
		{
			get { return _context.Roles.Where(r => r.InstanceId == _instanceId); }
		}

		public IQueryable<UsersToRole> UsersToRoles
		{
			get { return _context.UsersToRoles.Where(r => r.InstanceId == _instanceId); }
		}

		public IQueryable<Component> Components
		{
			get { return _context.Components; }
		}

		public IQueryable<ComponentsToRole> ComponentsToRoles
		{
			get { return _context.ComponentsToRoles.Where(r => r.InstanceId == _instanceId); }
		}

		public IQueryable<TransactionSeq> TransactionSeqs
		{
			get { return _context.TransactionSeqs; }
		}

		#endregion

		#region Add To Tables

		public void AddToInstances(Instance record)
		{
			_context.AddToInstances(record);
		}

		public void AddToUsers(User record)
		{
			_context.AddToUsers(record);
		}

		public void AddToVariables(Variable record)
		{
			_context.AddToVariables(record);
		}

		public void AddToDataLogs(DataLog record)
		{
			_context.AddToDataLogs(record);
		}

		public void AddToBlobs(Blob record)
		{
			_context.AddToBlobs(record);
		}

		public void AddToImages(Image record)
		{
			_context.AddToImages(record);
		}

		public void AddToUserInstanceUsages(UserInstanceUsage record)
		{
			_context.AddToUserInstanceUsages(record);
		}

		public void AddToTemporaryCodes(TemporaryCode record)
		{
			_context.AddToTemporaryCodes(record);
		}

		public void AddToRoles(Role record)
		{
			_context.AddToRoles(record);
		}

		public void AddToUsersToRoles(UsersToRole record)
		{
			_context.AddToUsersToRoles(record);
		}

		public void AddToComponents(Component record)
		{
			_context.AddToComponents(record);
		}

		public void AddToComponentsToRoles(ComponentsToRole record)
		{
			_context.AddToComponentsToRoles(record);
		}

		public void AddToTransactionSeqs(TransactionSeq record)
		{
			_context.AddToTransactionSeqs(record);
		}

		#endregion
	}
}
