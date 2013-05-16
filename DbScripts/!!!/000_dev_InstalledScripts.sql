if OBJECT_ID('dbo.dev_InstalledScripts', 'table') is not null
begin
	return
end
print 'creating table dev_InstalledScripts ...'
create table dbo.dev_InstalledScripts (
	[id] int IDENTITY(1,1) not null,
	[date] datetime not null default getdate(),
	[release] varchar(32) not null,
	[iteration] int not null,
	[dbawebPackage] varchar(256) null,
	[filename] varchar(256) null,
	[versionNumber] int not null,
	[success] bit not null,
	[errorMessage] varchar(max) null,
	[errorLine] int null,
	constraint PK_dev_InstalledScripts
	  primary key(id)
)
go

if OBJECT_ID('dbo.dev_fnVersionInstalled', 'function') is not null
begin
	print 'dropping function dbo.dev_fnVersionInstalled ...'
	drop function dbo.dev_fnVersionInstalled
end
go
print 'creating function dbo.dev_fnVersionInstalled ...'
go
create function dbo.dev_fnVersionInstalled ()
returns int
as
begin

declare @lastVersion int
select @lastVersion = max(versionNumber) 
from dev_InstalledScripts
where success = 1

return IsNull(@lastVersion, 0)
end
go

if OBJECT_ID('dbo.dev_InsertInstalledScripts', 'procedure') is not null
begin
	print 'dropping procedure dbo.dev_InsertInstalledScripts ...'
	drop procedure dbo.dev_InsertInstalledScripts
end
go
print 'creating procedure dbo.dev_InsertInstalledScripts ...'
go
create procedure dbo.dev_InsertInstalledScripts
	@release varchar(32),
	@iteration int, 
	@dbawebPackage varchar(256),
	@filename varchar(256),
	@versionNumber int,
	@success bit = 1,
	@errorMessage varchar(max) = null,
	@errorLine int = null
as

insert into dev_InstalledScripts (
  [date], [release], [iteration], [dbawebPackage], [filename], [versionNumber], [success], 
  [errorMessage], [errorLine])
values
  (GETDATE(), @release, @iteration, @dbawebPackage, @filename, @versionNumber, @success,
   @errorMessage, @errorLine)
go
