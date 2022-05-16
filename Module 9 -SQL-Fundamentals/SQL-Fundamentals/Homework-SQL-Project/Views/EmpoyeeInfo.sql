CREATE VIEW [dbo].[EmployeeInfo]
	AS SELECT e.[Id] AS EmployeeId,
			  ISNULL(e.[EmployeeName], CONCAT(p.[FirstName],' ', p.[LastName])) AS EmployeeFullName,
			  CONCAT(a.[ZipCode],'_',a.[State],',',a.[City],'-',a.[Street]) AS EmployeeFullAddress,
			  CONCAT(e.[CompanyName],'(',e.[Position],')') AS EmployeeCompanyInfo
	FROM [Employee] e
	JOIN [Address] a ON e.[AddressId] = a.[Id]
	JOIN [Person] p ON e.[PersonId] = p.[Id]
	-- is not supported in views
	-- ORDER BY e.CompanyName, a.City 
	
