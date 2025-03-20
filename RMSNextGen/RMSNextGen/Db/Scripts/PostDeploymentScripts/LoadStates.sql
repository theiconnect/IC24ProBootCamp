IF NOT EXISTS(SELECT 1 FROM StateMaster where Name = 'Andhra Pradesh')
BEGIN
	Insert into StateMaster (Name)
	values('AndhraPradesh')
END
GO
IF NOT EXISTS(SELECT 1 FROM StateMaster where Name = 'Telangana')
BEGIN
	Insert into StateMaster (Name)
	values('Telangana')
END
GO