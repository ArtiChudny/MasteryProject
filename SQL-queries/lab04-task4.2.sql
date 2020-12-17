USE AdventureWorks2017
GO

--task1
CREATE TABLE dbo.StateProvince
(
StateProvinceID int NOT NULL,
StateProvinceCode nchar(3) NOT NULL,
CountryRegionCode nvarchar(3) NOT NULL,
IsOnlyStateProvinceFlag flag NOT NULL,
Name name NOT NULL,
TerritoryID int NOT NULL,
ModifiedDate datetime NOT NULL
);
GO

--task2
ALTER TABLE dbo.StateProvince
ADD CONSTRAINT UQ_StateProvince_Name UNIQUE (Name) ;
GO

--task3
ALTER TABLE dbo.StateProvince
ADD CONSTRAINT CK_StateProvince_CountryRegionCode CHECK (CountryRegionCode NOT LIKE '%[0-9]%');
GO

--task4
ALTER TABLE dbo.StateProvince
ADD CONSTRAINT DF_StateProvince_ModifiedDate DEFAULT GETDATE() FOR ModifiedDate
GO

--task5
INSERT INTO dbo.StateProvince(CountryRegionCode, IsOnlyStateProvinceFlag, ModifiedDate, Name, StateProvinceCode, StateProvinceID, TerritoryID)
SELECT s.CountryRegionCode, s.IsOnlyStateProvinceFlag, s.ModifiedDate, s.Name, s.StateProvinceCode, s.StateProvinceID, s.TerritoryID 
FROM Person.StateProvince AS s
INNER JOIN Person.CountryRegion as c
ON c.CountryRegionCode = s.CountryRegionCode
WHERE c.Name = s.Name;
GO

--task6
ALTER TABLE dbo.StateProvince
DROP COLUMN IsOnlyStateProvinceFlag;
GO

ALTER TABLE dbo.StateProvince
ADD Population int;
GO
