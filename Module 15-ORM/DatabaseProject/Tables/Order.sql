CREATE TABLE [dbo].[Order]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Status] NVARCHAR(30) NULL, 
    [CreatedDate] DATETIME NULL, 
    [UpdatedDate] DATETIME NULL, 
    [ProductId] INT NULL, 
    CONSTRAINT [FK_Order_Product] FOREIGN KEY (ProductId) REFERENCES [Product]([Id])ON DELETE CASCADE 
)
