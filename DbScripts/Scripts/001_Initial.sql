declare @ScriptRelease varchar(32), @ScriptIteration int, @ScriptDbawebPackage varchar(256), 
	@ScriptFileName varchar(256), @ScriptVersionNumber int, @ScriptVersionExpected int
set @ScriptRelease = '1'
set @ScriptIteration = 1
set @ScriptDbawebPackage = null
set @ScriptFileName = '001_Initial'
set @ScriptVersionNumber = 1

select @ScriptVersionExpected = dbo.dev_fnVersionInstalled() + 1
if @ScriptVersionNumber <> @ScriptVersionExpected
begin
	print 'Cannot install script ' + cast(@ScriptVersionNumber as varchar) +
	      '. Script ' + cast(@ScriptVersionExpected as varchar) + ' is expected.'
end
else
begin try

create table Instances(
	InstanceId int not null identity,
	InstanceName varchar(200) not null,
	constraint PK_Instances
		primary key(InstanceId)
)
print 'Table Instances has been created'

create table Users(
	UserId int not null identity,
	Login varchar(128) not null,
	Password varchar(1024) not null,
	Email varchar(100),
	UserFIO varchar(100),
	RegistrationCode varchar(10),
	IsActive bit not null default 1,
	constraint PK_Users
		primary key(UserId)
)
print 'Table Users has been created'

create table Variables(
	VariableId int not null identity,
	InstanceId int not null,
	VariableKey varchar(128) not null,
	VariableValue varchar(max) not null,
	constraint PK_Variables
		primary key(VariableId),
	constraint FK_Variables_Instances
		foreign key(InstanceId)
		references Instances(InstanceId)
)
print 'Table Variables has been created'

create table DataLogs(
	DataLogId int not null identity,
	InstanceId int,
	UserId int not null,
	OperationTime datetime not null,
	TableName varchar(128) not null,
	RecordId int not null,
	Operation varchar(1) not null,
	Details varchar(max) not null,
	TransactionNumber int,
	constraint PK_DataLogs
		primary key(DataLogId),
	constraint FK_DataLogs_Instances
		foreign key(InstanceId)
		references Instances(InstanceId),
	constraint FK_DataLogs_Users
		foreign key(UserId)
		references Users(UserId)
)
print 'Table DataLogs has been created'

create table Blobs(
	BlobId int not null identity,
	InstanceId int not null,
	Data varbinary(max) not null,
	constraint PK_Blobs
		primary key(BlobId),
	constraint FK_Blobs_Instances
		foreign key(InstanceId)
		references Instances(InstanceId)
)
print 'Table Blobs has been created'

create table Images(
	ImageId int not null identity,
	InstanceId int not null,
	BlobId int not null,
	ImageName varchar(128) not null,
	ImageType varchar(4) not null,
	ImageSize int not null,
	constraint PK_Images
		primary key(ImageId),
	constraint FK_Images_Instances
		foreign key(InstanceId)
		references Instances(InstanceId),
	constraint FK_Images_Blobs
		foreign key(BlobId)
		references Blobs(BlobId)
)
print 'Table Images has been created'

create table UserInstanceUsages(
	UserInstanceUsageId int not null identity,
	UserId int not null,
	UsedInstanceId int not null,
	Date datetime not null,
	constraint PK_UserInstanceUsages
		primary key(UserInstanceUsageId),
	constraint FK_UserInstanceUsages_Users
		foreign key(UserId)
		references Users(UserId),
	constraint FK_UserInstanceUsagesUsedInstanceId_Instances
		foreign key(UsedInstanceId)
		references Instances(InstanceId)
)
print 'Table UserInstanceUsages has been created'

create table TemporaryCodes(
	TemporaryCodeId int not null identity,
	UserId int not null,
	Code varchar(10) not null,
	ExpireDate datetime not null,
	constraint PK_TemporaryCodes
		primary key(TemporaryCodeId),
	constraint FK_TemporaryCodes_Users
		foreign key(UserId)
		references Users(UserId)
)
print 'Table TemporaryCodes has been created'

create table Roles(
	RoleId int not null identity,
	InstanceId int not null,
	Name varchar(128) not null,
	Type int not null,
	constraint PK_Roles
		primary key(RoleId),
	constraint FK_Roles_Instances
		foreign key(InstanceId)
		references Instances(InstanceId)
)
print 'Table Roles has been created'

create table UsersToRoles(
	UserRoleId int not null identity,
	InstanceId int not null,
	UserId int not null,
	RoleId int not null,
	constraint PK_UsersToRoles
		primary key(UserRoleId),
	constraint FK_UsersToRoles_Instances
		foreign key(InstanceId)
		references Instances(InstanceId),
	constraint FK_UsersToRoles_Users
		foreign key(UserId)
		references Users(UserId),
	constraint FK_UsersToRoles_Roles
		foreign key(RoleId)
		references Roles(RoleId)
)
print 'Table UsersToRoles has been created'

create table Components(
	ComponentId int not null,
	Name varchar(128) not null,
	IsReadOnlyAccess bit not null,
	constraint PK_Components
		primary key(ComponentId)
)
print 'Table Components has been created'

create table ComponentsToRoles(
	ComponentToRoleId int not null identity,
	InstanceId int not null,
	ComponentId int not null,
	RoleId int not null,
	AccessLevel int not null,
	constraint PK_ComponentsToRoles
		primary key(ComponentToRoleId),
	constraint FK_ComponentsToRoles_Instances
		foreign key(InstanceId)
		references Instances(InstanceId),
	constraint FK_ComponentsToRoles_Components
		foreign key(ComponentId)
		references Components(ComponentId),
	constraint FK_ComponentsToRoles_Roles
		foreign key(RoleId)
		references Roles(RoleId)
)
print 'Table ComponentsToRoles has been created'

create table TransactionSeqs(
	TransactionSeqId int not null identity,
	constraint PK_TransactionSeqs
		primary key(TransactionSeqId)
)
print 'Table TransactionSeqs has been created'

exec dbo.dev_InsertInstalledScripts @ScriptRelease, @ScriptIteration, @ScriptDbawebPackage, 
	@ScriptFileName, @ScriptVersionNumber
print 'Completed successfully'
end try


begin catch
	declare @ErrorMessage varchar(max), @ErrorLine int
	set @ErrorMessage = ERROR_MESSAGE()
	set @ErrorLine = ERROR_LINE()
	exec dbo.dev_InsertInstalledScripts @ScriptRelease, @ScriptIteration, @ScriptDbawebPackage, 
		@ScriptFileName, @ScriptVersionNumber, 0, @ErrorMessage, @ErrorLine
	set @ErrorMessage = 'Line ' + cast(@ErrorLine as varchar) + ': ' + @ErrorMessage
	RAISERROR(@ErrorMessage, 16, 1)
end catch
go
