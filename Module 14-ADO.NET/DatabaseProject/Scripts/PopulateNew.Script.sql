/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

--DELETE FROM [Order];
--DELETE FROM [Product];
--GO

--DBCC CHECKIDENT ([Order], RESEED, 1);
--DBCC CHECKIDENT ([Product], RESEED, 1);
--GO

IF NOT EXISTS (SELECT * FROM [Product])
BEGIN
    INSERT INTO [Product] ([Name], [Description], [Weight], [Height], [Width], [Length])
    VALUES ('Book','Usual Product',12.3,23.5,20.0,43.0),
    ('Flower','Romantic Product',5.5,100.5,10.0,10.0),
    ('Chocolate','Sweet Product',8.7,2.5,5.0,7.0),
    ('Coffe','Morning Product',6.56,2.3,4.5,12.1)
END;

IF NOT EXISTS (SELECT * FROM [Order])
BEGIN
    INSERT INTO [Order] ([Status], [CreatedDate], [UpdatedDate], [ProductId])
    VALUES ('NotStarted','2020-02-02','2020-02-05',1),
           ('Loading','2020-02-03','2020-02-05',2),
           ('InProgress','2020-02-01','2020-02-07',3),
           ('Arrived','2020-02-03','2020-02-08',2),
           ('Unloading','2020-01-28','2020-02-03',2),
           ('Cancelled','2020-01-03','2020-02-04',1),
           ('Done','2020-02-03','2020-02-05',3)
END;


