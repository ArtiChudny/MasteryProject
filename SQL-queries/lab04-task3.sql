--task1
SELECT e.BusinessEntityID, e.JobTitle, MAX(h.Rate) AS MaxRate
FROM HumanResources.Employee AS e
INNER JOIN HumanResources.EmployeePayHistory AS h
ON e.BusinessEntityID = h.BusinessEntityID
GROUP BY e.BusinessEntityID, e.JobTitle;
GO

--task2
SELECT  e.BusinessEntityID, e.JobTitle, h.Rate, DENSE_RANK() OVER(ORDER BY h.Rate) AS RateRank 
FROM HumanResources.Employee AS e
INNER JOIN HumanResources.EmployeePayHistory AS h
ON e.BusinessEntityID = h.BusinessEntityID
GO

--task3
SELECT e.BusinessEntityID, e.JobTitle, COUNT(h.BusinessEntityID) AS RateCount
FROM HumanResources.Employee AS e
INNER JOIN HumanResources.EmployeePayHistory AS h
ON e.BusinessEntityID = h.BusinessEntityID
INNER JOIN HumanResources.EmployeeDepartmentHistory as d
ON d.BusinessEntityID = e.BusinessEntityID
WHERE d.EndDate IS NULL
GROUP BY e.BusinessEntityID, e.JobTitle
HAVING COUNT(h.BusinessEntityID) > 1;
GO

--task4
SELECT d.DepartmentID, d.Name, COUNT(h.BusinessEntityID)
FROM HumanResources.Department as d
INNER JOIN HumanResources.EmployeeDepartmentHistory as h
ON h.DepartmentID = d.DepartmentID
WHERE h.EndDate IS NULL
GROUP BY d.DepartmentID, d.Name
ORDER BY d.DepartmentID;
GO

--task5
SELECT e.BusinessEntityID, e.JobTitle, h.Rate, 
	(SELECT MAX(Rate) FROM HumanResources.EmployeePayHistory WHERE Rate < h.rate AND BusinessEntityID = e.BusinessEntityID  ) AS PrevRate,
	(h.Rate - ISNULL((SELECT MAX(Rate) FROM HumanResources.EmployeePayHistory WHERE Rate < h.rate AND BusinessEntityID = e.BusinessEntityID ), 0)) AS DiffRate
FROM HumanResources.Employee AS e
INNER JOIN HumanResources.EmployeePayHistory AS h
ON e.BusinessEntityID = h.BusinessEntityID
ORDER BY e.BusinessEntityID
GO

--task6
SELECT d.Name, h.BusinessEntityID, p.Rate, 
	MAX(p.Rate) OVER (PARTITION BY d.NAME) AS MaxInDepartment, 
	DENSE_RANK() OVER(PARTITION BY d.Name ORDER BY p.Rate) AS RateGroup
FROM  HumanResources.Department as d
INNER JOIN HumanResources.EmployeeDepartmentHistory as h
ON h.DepartmentID = d.DepartmentID 
INNER JOIN HumanResources.EmployeePayHistory as p
ON p.BusinessEntityID = h.BusinessEntityID
WHERE h.EndDate IS NULL 
GROUP BY d.Name, h.BusinessEntityID, p.Rate
GO

--task7
SELECT e.BusinessEntityID, e.JobTitle, s.Name, s.StartTime, s.EndTime
FROM HumanResources.Employee AS e
INNER JOIN HumanResources.EmployeeDepartmentHistory AS h
ON e.BusinessEntityID = h.BusinessEntityID
INNER JOIN HumanResources.Shift AS s
ON s.ShiftID = h.ShiftID
WHERE s.Name = 'Evening';
GO

--task8 
SELECT e.BusinessEntityID, e.JobTitle, d.Name, h.StartDate, h.EndDate, DATEDIFF(year, h.StartDate, ISNULL(h.EndDate, GETDATE())) AS Experience
FROM HumanResources.Employee AS e
INNER JOIN HumanResources.EmployeeDepartmentHistory AS h
ON e.BusinessEntityID = h.BusinessEntityID
INNER JOIN HumanResources.Department AS d
ON d.DepartmentID = h.DepartmentID;
GO