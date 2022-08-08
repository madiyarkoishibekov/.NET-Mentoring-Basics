CREATE TABLE [dbo].[Order]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Status] NVARCHAR(60) NULL, 
    [CreatedDate] DATETIME NULL, 
    [UpdatedDate] DATETIME NULL, 
    [ProductId] INT NULL, 
    CONSTRAINT [FK_Order_ToProduct] FOREIGN KEY (ProductId) REFERENCES [Product]([Id]) 
    ON DELETE CASCADE
)
