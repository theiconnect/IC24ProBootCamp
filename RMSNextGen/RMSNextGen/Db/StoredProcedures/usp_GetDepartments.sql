CREATE PROCEDURE dbo.usp_GetDepartments
AS
BEGIN
	SELECT DepartmentIdPk, Department FROM dbo.DepartmentMaster
END