USE AdventureWorks2017
GO

--task1
CREATE TABLE dbo.Person_New
(
BusinessEntityID int NOT NULL,
PersonType nchar(2) NOT NULL,
NameStyle namestyle NOT NULL,
Title nvarchar(6),
FirstName name NOT NULL,
MiddleName name,
LastName name NOT NULL,
Suffix nvarchar(10),
EmailPromotion int NOT NULL,
ModifiedDate datetime NOT NULL,
PersonID int NOT NULL
);
GO

ALTER TABLE dbo.Person_New
ADD CONSTRAINT PK_Person_PersonID PRIMARY KEY (PersonID),
	CONSTRAINT CK_Person_MiddleName CHECK(UPPER(MiddleName) ='J' OR UPPER(MiddleName) ='L'),
	CONSTRAINT DF_PersonNew_Title DEFAULT 'N/A' FOR Title;
GO

--task2
ALTER TABLE dbo.Person_New
ADD Salutation nvarchar(80);
GO

--task3
INSERT INTO dbo.Person_New (BusinessEntityID, EmailPromotion, FirstName, LastName, MiddleName, ModifiedDate, NameStyle, PersonType, Suffix, Title)
SELECT p.BusinessEntityID, p.EmailPromotion, p.FirstName, p.LastName, p.MiddleName, p.ModifiedDate, p.NameStyle, p.PersonType, p.Suffix, 
	(SELECT 
		CAST( 
			CASE 
			WHEN e.Gender = 'M' THEN 'Mr.'
			WHEN e.Gender = 'F' THEN 'Ms.'
			END AS varchar(2))
	)
FROM Person.Person AS p
INNER JOIN HumanResources.Employee as e
ON e.BusinessEntityID = p.BusinessEntityID
GO

--task4
UPDATE dbo.Person_New
SET Salutation = CONCAT(Title, ' ', FirstName);
GO

--task5
DELETE FROM dbo.Person_New
WHERE LEN(Salutation) > 10;
GO

--task6
ALTER TABLE dbo.Person
DROP DF_Person_Title, CK_Person_MiddleName, PK_Person_PersonID;
GO

--task7
ALTER TABLE dbo.Person
DROP COLUMN PersonId;
GO

--task8
DROP TABLE dbo.Person, dbo.Person_New
GO
