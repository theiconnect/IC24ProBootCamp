IF NOT EXISTS(SELECT 1 FROM CityMaster where Name = 'Nellore' and StateId = 1)
BEGIN
	Insert into CityMaster (Name, StateId)
	values('Nellore', 1)
END
GO
IF NOT EXISTS(SELECT 1 FROM CityMaster where Name = 'Udayagiri' and StateId = 1)
BEGIN
	Insert into CityMaster (Name, StateId)
	values('Udayagiri', 1)
END
GO
IF NOT EXISTS(SELECT 1 FROM CityMaster where Name = 'Vijayawada' and StateId = 1)
BEGIN
	Insert into CityMaster (Name, StateId)
	values('Vijayawada', 1)
END
GO
IF NOT EXISTS(SELECT 1 FROM CityMaster where Name = 'Warangal' and StateId = 2)
BEGIN
	Insert into CityMaster (Name, StateId)
	values('Warangal', 2)
END
GO
IF NOT EXISTS(SELECT 1 FROM CityMaster where Name = 'RangaReddy' and StateId = 2)
BEGIN
	Insert into CityMaster (Name, StateId)
	values('RangaReddy', 2)
END
GO
IF NOT EXISTS(SELECT 1 FROM CityMaster where Name = 'KarimNagar' and StateId = 2)
BEGIN
	Insert into CityMaster (Name, StateId)
	values('KarimNagar', 2)
END
GO
