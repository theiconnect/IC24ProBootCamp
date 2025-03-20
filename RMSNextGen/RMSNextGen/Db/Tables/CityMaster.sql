Create Table CityMaster(
	CityId INT Identity(1,1) Primary Key,
	Name Varchar(256)
	,StateId Int , CONSTRAINT FK_City_State FOREIGN KEY (StateId) 
   REFERENCES StateMaster(StateId))