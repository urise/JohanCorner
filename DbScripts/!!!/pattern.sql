declare @ScriptRelease varchar(32), @ScriptIteration int, @ScriptDbawebPackage varchar(256), 
	@ScriptFileName varchar(256), @ScriptVersionNumber int, @ScriptVersionExpected int
set @ScriptRelease = ''
set @ScriptIteration = 
set @ScriptDbawebPackage = null
set @ScriptFileName = ''
set @ScriptVersionNumber = 

select @ScriptVersionExpected = dbo.dev_fnVersionInstalled() + 1
if @ScriptVersionNumber <> @ScriptVersionExpected
begin
	print 'Cannot install script ' + cast(@ScriptVersionNumber as varchar) +
	      '. Script ' + cast(@ScriptVersionExpected as varchar) + ' is expected.'
end
else
begin try

-- !!! PUT YOUR SCRIPT HERE !!!

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