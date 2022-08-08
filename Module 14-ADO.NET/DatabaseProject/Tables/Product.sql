CREATE TABLE [dbo].[Product]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NULL, 
    [Description] NVARCHAR(50) NULL, 
    [Weight] DECIMAL(18, 2) NULL, 
    [Height] DECIMAL(18, 2) NULL, 
    [Width] DECIMAL(18, 2) NULL, 
    [Length] DECIMAL(18, 2) NULL
)
