USE [EmployeeManagement]
GO

/****** Object:  Trigger [dbo].[TRG_Department_BudgetConstraint]    Script Date: 10/29/2025 4:25:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Sujit K. Roy>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

CREATE OR ALTER   TRIGGER [dbo].[TRG_Department_BudgetConstraint]
ON [dbo].[Department]
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM inserted i
        WHERE i.Spent > i.Budget
    )
    BEGIN
        DECLARE @ExceededDeptName NVARCHAR(100);
        DECLARE @SpentAmount DECIMAL(18, 2);
        DECLARE @SpentAmountStr NVARCHAR(50);

        SELECT TOP 1 
            @ExceededDeptName = i.Name,
            @SpentAmount = i.Spent
        FROM 
            inserted i
        WHERE 
            i.Spent > i.Budget;

        SET @SpentAmountStr = CONVERT(NVARCHAR(50), @SpentAmount, 1);
        
        ROLLBACK TRANSACTION;
        
        RAISERROR(
            'Operation aborted: Cannot set budget for department (%s). The required budget is less than the current spent amount (%s).', 
            16, 
            1, 
            @ExceededDeptName, 
            @SpentAmountStr
        );
        RETURN;
    END

END
GO

ALTER TABLE [dbo].[Department] ENABLE TRIGGER [TRG_Department_BudgetConstraint]
GO


