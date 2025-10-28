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
    HierarchyLevel INT DEFAULT 1,
    FOREIGN KEY (DepartmentId) REFERENCES Department(DepartmentId),
    FOREIGN KEY (ManagerId) REFERENCES Employee(EmployeeId)
);

