USE AdventureWorks2017
GO

--task1
CREATE TABLE dbo.Person 
(
BusinessEntityID int NOT NULL,
PersonType nchar(2) NOT NULL,
NameStyle namestyle NOT NULL,
Title nvarchar(8),
FirstName name NOT NULL,
MiddleName name,
LastName name NOT NULL,
Suffix nvarchar(10),
EmailPromotion int NOT NULL,
ModifiedDate datetime NOT NULL
);
GO

--task2
ALTER TABLE dbo.Person
ADD PersonID int IDENTITY(3, 5),
CONSTRAINT PK_Person_PersonID PRIMARY KEY (PersonID);
GO

--task3
ALTER TABLE dbo.Person
ADD CONSTRAINT CK_Person_MiddleName CHECK(UPPER(MiddleName) ='J' OR UPPER(MiddleName) ='L');
GO

--task4
ALTER TABLE dbo.Person
ADD CONSTRAINT DF_Person_Title DEFAULT 'N/A' FOR Title;
GO

--task5
INSERT INTO dbo.Person (BusinessEntityID, EmailPromotion, FirstName, LastName, MiddleName, ModifiedDate, NameStyle, PersonType, Suffix, Title)
SELECT p.BusinessEntityID, p.EmailPromotion, p.FirstName, p.LastName, IIF(UPPER(p.MiddleName) = 'L' OR UPPER(p.MiddleName) = 'J' , p.MiddleName, NULL), p.ModifiedDate, p.NameStyle, p.PersonType, p.Suffix, p.Title
FROM Person.Person AS p
INNER JOIN HumanResources.Employee as e
ON e.BusinessEntityID = p.BusinessEntityID
INNER JOIN HumanResources.EmployeeDepartmentHistory as h
ON e.BusinessEntityID = h.BusinessEntityID
INNER JOIN HumanResources.Department  as d
ON d.DepartmentID = h.DepartmentID
WHERE d.Name <> 'Finance';
GO

--task6
ALTER TABLE dbo.Person
ALTER COLUMN Title nvarchar(6);
GO