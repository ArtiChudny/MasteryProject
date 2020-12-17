--task1
CREATE DATABASE Something;
GO

--task2
USE Something;

SELECT db.name, db.create_date 
FROM sys.databases as db;
GO

--task3
CREATE TABLE Whicked 
(
	Id int NOT NULL
);
GO

--task4
BACKUP DATABASE Something
TO DISK = 'C:\SomethingDB.bak';
GO

--task5
USE master

DROP DATABASE Something;
GO

--task6
RESTORE DATABASE Something
FROM DISK = 'C:\SomethingDB.bak';