USE AdventureWorks2017
GO

--task-1
SELECT TOP 8 *  
FROM HumanResources.Department
ORDER BY Name DESC;
GO

--task-2
SELECT NationalIDNumber, BusinessEntityID, JobTitle, BirthDate, HireDate
FROM HumanResources.Employee
WHERE YEAR(HireDate) - YEAR(BirthDate) = 22;
GO

--task-3
SELECT  BusinessEntityID, NationalIDNumber, JobTitle, BirthDate, MaritalStatus
FROM HumanResources.Employee
WHERE JobTitle IN ('Design Engineer', 'Tool Designer', 'Engineering Manager', 'Production Control Manager')
AND MaritalStatus = 'M'
ORDER BY BirthDate;
GO

--task-4
SELECT BusinessEntityID, JobTitle, Gender, BirthDate, HireDate
FROM HumanResources.Employee
WHERE MONTH(HireDate) = 3 
AND DAY(HireDate)= 5
ORDER BY BusinessEntityID
OFFSET 1 ROW
FETCH NEXT 5 ROWS ONLY;
GO

--task5
SELECT BusinessEntityID, JobTitle, Gender, HireDate, REPLACE(LoginID, 'adventure-works','adventure-works2017') as LoginId
FROM HumanResources.Employee
WHERE Gender = 'F' 
AND DATENAME(dw, HireDate) = 'Wednesday';
GO

--task6
SELECT SUM(VacationHours) AS VacationSumInHours, SUM(SickLeaveHours) AS SicknessSumInHours
FROM HumanResources.Employee;
GO

--task7
SELECT DISTINCT TOP 8 JobTitle, 
	  (CASE 
		WHEN CHARINDEX(' ', REVERSE(TRIM(JobTitle))) = 0 THEN JobTitle
		ELSE RIGHT(TRIM(JobTitle), CHARINDEX(' ', REVERSE(TRIM(JobTitle))) - 1)
	  END) AS LastWord	
FROM HumanResources.Employee
ORDER BY JobTitle DESC;
GO

--OR

SELECT DISTINCT TOP 8 JobTitle, 
	(SELECT TOP 1 REVERSE(VALUE) 
		FROM HumanResources.Employee 
		CROSS APPLY STRING_SPLIT(REVERSE(TRIM(JobTitle)),' ')
		WHERE BusinessEntityID = e.BusinessEntityID 
	) AS LastWord
FROM HumanResources.Employee as e
ORDER BY JobTitle DESC;
GO

--task8
SELECT  BusinessEntityID, JobTitle, Gender, BirthDate, HireDate
FROM HumanResources.Employee
WHERE JobTitle LIKE '%control%';
GO