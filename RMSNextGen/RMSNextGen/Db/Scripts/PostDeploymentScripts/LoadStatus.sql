MERGE INTO dbo.[StatusMaster] AS Target
USING (
    SELECT 1 AS StatusIDPk, 'INPROGRESS' AS StatusCode, 'In-Progress' AS [StatusName]
    UNION ALL
    SELECT 2, 'COMPLETED', 'Completed'
    UNION ALL
    SELECT 3, 'APPROVED', 'Approved'
) AS Source
ON Target.StatusIDPk = Source.StatusIDPk
WHEN MATCHED AND 
    (Target.StatusCode <> Source.StatusCode OR Target.[StatusName] <> Source.[StatusName])
    THEN UPDATE 
         SET Target.StatusCode = Source.StatusCode,
             Target.[StatusName]     = Source.[StatusName]
WHEN NOT MATCHED BY TARGET 
    THEN INSERT (StatusIDPk, StatusCode, [StatusName])
         VALUES (Source.StatusIDPk, Source.StatusCode, Source.[StatusName])
WHEN NOT MATCHED BY SOURCE 
    THEN DELETE;
--OUTPUT $action, inserted.*, deleted.*;
