
CREATE PROC usp_SearchEmployee
	@EmployeeName VARCHAR(512) = NULL,
	@EmployeeCode VARCHAR(50) = NULL,
	@Mobile		VARCHAR(20) = NULL,
	@DepartmentId INT = NULL
AS
BEGIN
	SELECT  
		EmployeeIdPk,
		Employeecode,
		EmployeeFirstName + ' ' + ISNULL(EmployeeLastName, '') As EmployeeName,
		D.Department As DepartmentName,
		Designation,
		MobileNumber,
		S.StoreCode 
	FROM Employee E
	LEFT JOIN DepartmentMaster D on E.DepartmentIdFk = D.DepartmentIDPk
	INNER JOIN Store S on E.StoreIDfk = S.StoreIdPk
	WHERE 1=1
		AND (@EmployeeCode IS NULL OR EmployeeCode = @EmployeeCode) 
		AND (@EmployeeName IS NULL OR EmployeeFirstName + EmployeeLastName LIKE '%' + @EmployeeName + '%') 
		AND (@Mobile IS NULL OR MobileNumber=@Mobile)
		AND (@DepartmentId IS NULL OR E.DepartmentIdFk = @DepartmentId)
END

