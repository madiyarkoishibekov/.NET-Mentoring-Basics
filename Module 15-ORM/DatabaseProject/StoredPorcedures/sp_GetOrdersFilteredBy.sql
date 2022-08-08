CREATE PROCEDURE [dbo].[sp_GetOrdersFilteredBy]
	@Status NCHAR(15) = NULL,
	@CreatedYear int = NULL,
	@UpdatedMonth int = NULL,
	@ProductId int = NULL
AS
	SELECT *
	FROM [Order] o
	WHERE 
		(@Status IS NULL OR LEN(@Status) = 0 OR o.[Status] = @Status)
		AND (@CreatedYear IS NULL OR YEAR(o.CreatedDate) = @CreatedYear)
		AND (@UpdatedMonth IS NULL OR MONTH(o.UpdatedDate) = @UpdatedMonth)
		AND (@ProductId IS NULL OR o.ProductId = @ProductId)
RETURN 0
