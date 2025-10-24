CREATE TABLE Department (
    DepartmentId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Budget MONEY NOT NULL
);

CREATE TABLE Employee (
    EmployeeId INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    HireDate DATE NOT NULL DEFAULT GETDATE(),
    Salary MONEY NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    DepartmentId INT,
    ManagerId INT,
    HiererKeyLevel INT DEFAULT 1,
    FOREIGN KEY (DepartmentId) REFERENCES Department(DepartmentId),
    FOREIGN KEY (ManagerId) REFERENCES Employee(EmployeeId)
);


CREATE OR ALTER PROCEDURE dbo.GetAllEmployees 
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        e.EmployeeId,
        e.FirstName,
        e.LastName,
        e.Email,
        e.HireDate,
        e.Salary,
        e.IsActive,
        e.HiererKeyLevel,
        
        -- Department Information (d)
        d.DepartmentId, 
        d.Name AS DepartmentName,
        d.Budget,
        
        -- Manager Information (m)
        e.ManagerId,
        m.FirstName AS ManagerFirstName,
        m.LastName AS ManagerLastName
    FROM
        Employee e
    INNER JOIN
        Department d ON e.DepartmentId = d.DepartmentId
    LEFT JOIN
        Employee m ON e.ManagerId = m.EmployeeId
    ORDER BY
        e.LastName;
END