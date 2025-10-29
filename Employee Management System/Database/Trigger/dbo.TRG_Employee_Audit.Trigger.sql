USE [EmployeeManagement]
GO

/****** Object:  Trigger [dbo].[TRG_Employee_Audit]    Script Date: 10/29/2025 4:28:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER  TRIGGER [dbo].[TRG_Employee_Audit]
ON [dbo].[Employee]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @AuditUser NVARCHAR(100) = ISNULL(CONVERT(NVARCHAR(100), CONTEXT_INFO()), SUSER_SNAME());
    DECLARE @ChangeType NVARCHAR(10);
    
    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
        SET @ChangeType = 'UPDATE';
    ELSE IF EXISTS (SELECT 1 FROM inserted)
        SET @ChangeType = 'INSERT';
    ELSE IF EXISTS (SELECT 1 FROM deleted)
        SET @ChangeType = 'DELETE';
    
    -- Auditing INSERT and UPDATE
    IF @ChangeType IN ('INSERT', 'UPDATE')
    BEGIN
        INSERT INTO EmployeeAudit (EmployeeId, ChangeType, FieldName, OldValue, NewValue, ChangedBy)
        SELECT 
            i.EmployeeId, 
            @ChangeType, 
            FieldName,
            OldValue,
            NewValue,
            @AuditUser
        FROM inserted i
        FULL OUTER JOIN deleted d ON i.EmployeeId = d.EmployeeId
        CROSS APPLY (
            -- FirstName
            SELECT 'FirstName', CAST(d.FirstName AS NVARCHAR(MAX)), CAST(i.FirstName AS NVARCHAR(MAX))
            WHERE @ChangeType = 'INSERT' OR CAST(d.FirstName AS NVARCHAR(MAX)) <> CAST(i.FirstName AS NVARCHAR(MAX))
            UNION ALL
            -- LastName
            SELECT 'LastName', CAST(d.LastName AS NVARCHAR(MAX)), CAST(i.LastName AS NVARCHAR(MAX))
            WHERE @ChangeType = 'INSERT' OR CAST(d.LastName AS NVARCHAR(MAX)) <> CAST(i.LastName AS NVARCHAR(MAX))
            UNION ALL
            -- Email
            SELECT 'Email', CAST(d.Email AS NVARCHAR(MAX)), CAST(i.Email AS NVARCHAR(MAX))
            WHERE @ChangeType = 'INSERT' OR CAST(d.Email AS NVARCHAR(MAX)) <> CAST(i.Email AS NVARCHAR(MAX))
            UNION ALL
            -- HireDate
            SELECT 'HireDate', CAST(d.HireDate AS NVARCHAR(MAX)), CAST(i.HireDate AS NVARCHAR(MAX))
            WHERE @ChangeType = 'INSERT' OR CAST(d.HireDate AS NVARCHAR(MAX)) <> CAST(i.HireDate AS NVARCHAR(MAX))
            UNION ALL
            -- Salary
            SELECT 'Salary', CAST(d.Salary AS NVARCHAR(MAX)), CAST(i.Salary AS NVARCHAR(MAX))
            WHERE @ChangeType = 'INSERT' OR CAST(d.Salary AS NVARCHAR(MAX)) <> CAST(i.Salary AS NVARCHAR(MAX))
            UNION ALL
            -- IsActive
            SELECT 'IsActive', CAST(d.IsActive AS NVARCHAR(MAX)), CAST(i.IsActive AS NVARCHAR(MAX))
            WHERE @ChangeType = 'INSERT' OR CAST(d.IsActive AS NVARCHAR(MAX)) <> CAST(i.IsActive AS NVARCHAR(MAX))
            UNION ALL
            -- DepartmentId
            SELECT 'DepartmentId', CAST(d.DepartmentId AS NVARCHAR(MAX)), CAST(i.DepartmentId AS NVARCHAR(MAX))
            WHERE @ChangeType = 'INSERT' OR ISNULL(CAST(d.DepartmentId AS NVARCHAR(MAX)), '') <> ISNULL(CAST(i.DepartmentId AS NVARCHAR(MAX)), '')
            UNION ALL
            -- ManagerId
            SELECT 'ManagerId', CAST(d.ManagerId AS NVARCHAR(MAX)), CAST(i.ManagerId AS NVARCHAR(MAX))
            WHERE @ChangeType = 'INSERT' OR CAST(d.ManagerId AS NVARCHAR(MAX)) <> CAST(i.ManagerId AS NVARCHAR(MAX))
            UNION ALL
            -- IsManager
            SELECT 'IsManager', CAST(d.IsManager AS NVARCHAR(MAX)), CAST(i.IsManager AS NVARCHAR(MAX))
            WHERE @ChangeType = 'INSERT' OR CAST(d.IsManager AS NVARCHAR(MAX)) <> CAST(i.IsManager AS NVARCHAR(MAX))
            UNION ALL
            -- HierarchyLevel
            SELECT 'HierarchyLevel', CAST(d.HierarchyLevel AS NVARCHAR(MAX)), CAST(i.HierarchyLevel AS NVARCHAR(MAX))
            WHERE @ChangeType = 'INSERT' OR CAST(d.HierarchyLevel AS NVARCHAR(MAX)) <> CAST(i.HierarchyLevel AS NVARCHAR(MAX))

        ) AS Changes(FieldName, OldValue, NewValue);
    END

    -- Auditing DELETE
    IF @ChangeType = 'DELETE'
    BEGIN
         INSERT INTO EmployeeAudit (EmployeeId, ChangeType, FieldName, OldValue, NewValue, ChangedBy)
         SELECT 
             d.EmployeeId, 
             @ChangeType, 
             'Record', 
             'Entire record deleted', 
             'NULL', 
             @AuditUser
         FROM deleted d;
    END
END
GO

ALTER TABLE [dbo].[Employee] ENABLE TRIGGER [TRG_Employee_Audit]
GO

