CREATE TABLE [dbo].[Employee]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AddressId] INT NOT NULL, 
    [PersonId] INT NOT NULL,
    [CompanyName] NVARCHAR(20) NOT NULL, 
    [Position] NVARCHAR(30) NULL, 
    [EmployeeName] NVARCHAR(100) NULL, 
    CONSTRAINT [FK_Employee_Address] FOREIGN KEY ([AddressId]) REFERENCES [Address]([Id]), 
    CONSTRAINT [FK_Employee_Person] FOREIGN KEY (PersonId) REFERENCES Person(Id)
)

GO

CREATE TRIGGER [dbo].[Trigger_EmployeeOnInsert]
    ON [dbo].[Employee]
    AFTER INSERT
    AS
    BEGIN
        SET NoCount ON;
        -- Insert values into Company from rows of Inserted which do not already exist in Company.
        INSERT INTO [Company] ([Name], [AddressId])
        SELECT DISTINCT i.[CompanyName], i.[AddressId] FROM [inserted] i
        WHERE NOT EXISTS (SELECT * FROM [Company] c 
                          WHERE c.[Name] = i.[CompanyName] AND c.[AddressId] = i.[AddressId])
    END;