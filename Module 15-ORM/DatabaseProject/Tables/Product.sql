CREATE TABLE [dbo].[Product]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(30) NULL, 
    [Description] NVARCHAR(50) NULL, 
    [Weight] INT NULL, 
    [Height] INT NULL, 
    [Width] INT NULL, 
    [Length] INT NULL 
)
