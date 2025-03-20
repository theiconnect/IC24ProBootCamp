IF NOT EXISTS (SELECT 1 FROM [dbo].[DepartmentMaster] WHERE [Department] = 'Store Management')
BEGIN
    INSERT INTO [dbo].[DepartmentMaster] ([DepartmentIdPk], [Department]) VALUES (1, 'Store Management');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[DepartmentMaster] WHERE [Department] = 'HR')
BEGIN
    INSERT INTO [dbo].[DepartmentMaster] ([DepartmentIdPk], [Department]) VALUES (2, 'HR');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[DepartmentMaster] WHERE [Department] = 'Sales Associates/Customer Service')
BEGIN
    INSERT INTO [dbo].[DepartmentMaster] ([DepartmentIdPk], [Department]) VALUES (3, 'Sales Associates/Customer Service');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[DepartmentMaster] WHERE [Department] = 'Inventory/Stocking')
BEGIN
    INSERT INTO [dbo].[DepartmentMaster] ([DepartmentIdPk], [Department]) VALUES (4, 'Inventory/Stocking');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[DepartmentMaster] WHERE [Department] = 'Security')
BEGIN
    INSERT INTO [dbo].[DepartmentMaster] ([DepartmentIdPk], [Department]) VALUES (5, 'Security');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[DepartmentMaster] WHERE [Department] = 'Maintenance')
BEGIN
    INSERT INTO [dbo].[DepartmentMaster] ([DepartmentIdPk], [Department]) VALUES (6, 'Maintenance');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[DepartmentMaster] WHERE [Department] = 'Logistics')
BEGIN
    INSERT INTO [dbo].[DepartmentMaster] ([DepartmentIdPk], [Department]) VALUES (7, 'Logistics');
END;
