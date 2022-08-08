using DB_Operations.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DB_Operations
{
    public class DatabaseOperations_Disconnected
    {
        private SqlConnection _connection;
        private string _connectionString;
        private SqlDataAdapter _adapter;
        private DataSet _dataSet;

        public DatabaseOperations_Disconnected(string connectionString)
        {
            _connectionString = connectionString;
            _adapter = GetAdapter();
            _dataSet = new DataSet();
            _adapter.Fill(_dataSet, "Product");
        }
        public SqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new SqlConnection(_connectionString);
                };
                return _connection;
            }

            set
            {
                _connection = value;
            }
        }

        private SqlDataAdapter GetAdapter()
        {
            var adapter = new SqlDataAdapter();
            adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            // Define commands.
            adapter.InsertCommand = new SqlCommand("INSERT INTO [Product] (Name, Description, Weight, Height, Width, Length) " + "VALUES (@Name, @Description, @Weight, @Height, @Width, @Length)", Connection);
            adapter.SelectCommand = new SqlCommand("SELECT * FROM [Product]", Connection);
            adapter.UpdateCommand = new SqlCommand("UPDATE [Product] " +
                    "SET [Name]=@Name, [Description]=@Description, [Weight]=@Weight, [Height]=@Height, [Width]=@Width, [Length]=@Length " +
                    "WHERE [Id]=@ProductId", Connection);
            adapter.DeleteCommand = new SqlCommand("DELETE FROM [Product] WHERE [Id]=@ProductId", Connection);

            // Define parameters.
            adapter.InsertCommand.Parameters.Add("@Name", SqlDbType.NVarChar, 50, "Name");
            adapter.InsertCommand.Parameters.Add("@Description", SqlDbType.NVarChar, 50, "Description");
            adapter.InsertCommand.Parameters.Add("@Weight", SqlDbType.Decimal, 1, "Weight");
            adapter.InsertCommand.Parameters.Add("@Height", SqlDbType.Decimal, 1, "Height");
            adapter.InsertCommand.Parameters.Add("@Width", SqlDbType.Decimal, 1, "Width");
            adapter.InsertCommand.Parameters.Add("@Length", SqlDbType.Decimal, 1, "Length");

            adapter.UpdateCommand.Parameters.Add("@Name", SqlDbType.NVarChar, 50, "Name");
            adapter.UpdateCommand.Parameters.Add("@Description", SqlDbType.NVarChar, 50, "Description");
            adapter.UpdateCommand.Parameters.Add("@Weight", SqlDbType.Decimal, 1, "Weight");
            adapter.UpdateCommand.Parameters.Add("@Height", SqlDbType.Decimal, 1, "Height");
            adapter.UpdateCommand.Parameters.Add("@Width", SqlDbType.Decimal, 1, "Width");
            adapter.UpdateCommand.Parameters.Add("@Length", SqlDbType.Decimal, 1, "Length");
            var parameter = adapter.UpdateCommand.Parameters.Add("@ProductId", SqlDbType.Int, 1, "Id");
            parameter.SourceVersion = DataRowVersion.Original;

            parameter = adapter.DeleteCommand.Parameters.Add("@ProductId", SqlDbType.Int, 1, "Id");
            parameter.SourceVersion = DataRowVersion.Original;

            return adapter;
        }

        public bool InsertProduct(ProductModel product)
        {
            var productTable = _dataSet.Tables["Product"];
            var newRow = productTable.NewRow();
            newRow["Name"] = product.Name;
            newRow["Description"] = product.Description;
            newRow["Weight"] = product.Weight;
            newRow["Height"] = product.Height;
            newRow["Width"] = product.Width;
            newRow["Length"] = product.Length;
            productTable.Rows.Add(newRow);

            var rowsAffected = _adapter.Update(_dataSet, "Product");
            return rowsAffected > 0;
        }

        public ProductModel ReadProduct(int productId)
        {
            var product = _dataSet.Tables["Product"].Rows.Find(productId);
            ProductModel productModel = null;
            if (product != null)
            {
                productModel = new ProductModel()
                {
                    Name = (string)product["Name"],
                    Description = (string)product["Description"],
                    Weight = (decimal)product["Weight"],
                    Height = (decimal)product["Height"],
                    Width = (decimal)product["Width"],
                    Length = (decimal)product["Length"]
                };
            }

            return productModel;
        }

        public ProductModel UpdateProductDescription(int productId, string newDescription)
        {
            // Update description of the product.
            var product = _dataSet.Tables["Product"].Rows.Find(productId);
            product["Description"] = newDescription;
            var rowsAffected = _adapter.Update(_dataSet, "Product");

            // Read and return updated product.
            ProductModel updatedProduct = null;
            if (rowsAffected > 0)
            {
                updatedProduct = ReadProduct(productId);
            }

            return updatedProduct;
        }

        public bool DeleteProduct(int productId)
        {
            var productRow = _dataSet.Tables["Product"].Rows.Find(productId);
            productRow.Delete();
            _adapter.Update(_dataSet, "Product");
            var isDeleted = _dataSet.Tables["Product"].Rows.Find(productId) == null;

            return isDeleted;
        }

        public List<ProductModel> FetchAllProducts()
        {
            var allProducts = _dataSet.Tables["Product"].AsEnumerable().Select(p =>
                new ProductModel()
                {
                    Name = (string)p["Name"],
                    Description = (string)p["Description"],
                    Weight = (decimal)p["Weight"],
                    Height = (decimal)p["Height"],
                    Width = (decimal)p["Width"],
                    Length = (decimal)p["Length"]
                }).ToList();
            return allProducts;
        }

        public bool DeleteOrdersInBulk(OrderStatus? status = null, int? createdYear = null, int? updatedMonth = null, int? productId = null)
        {
            var selectCommand = new SqlCommand()
            {
                CommandText = "[sp_GetOrdersFiltered]",
                CommandType = CommandType.StoredProcedure,
                Connection = Connection
            };
            selectCommand.Parameters.AddWithValue("@Status", status.ToString());
            selectCommand.Parameters.AddWithValue("@CreatedYear", createdYear);
            selectCommand.Parameters.AddWithValue("@UpdatedMonth", updatedMonth);
            selectCommand.Parameters.AddWithValue("@ProductId", productId);

            var updateCommand = new SqlCommand()
            {
                CommandText = "UPDATE [Order] " +
                    "SET [Status]=@Status, [CreatedDate]=@CreatedDate, [UpdatedDate]=@UpdatedDate, [ProductId]=@ProductId " +
                    "WHERE [Id]=@OrderId",
                CommandType = CommandType.Text,
                Connection = Connection
            };
            updateCommand.Parameters.Add("@Status", SqlDbType.NVarChar, 50, "Status");
            updateCommand.Parameters.Add("@CreatedDate", SqlDbType.DateTime, 1, "CreatedDate");
            updateCommand.Parameters.Add("@UpdatedDate", SqlDbType.DateTime, 1, "UpdatedDate");
            var parameter = updateCommand.Parameters.Add("@OrderId", SqlDbType.Int, 1, "Id");
            parameter.SourceVersion = DataRowVersion.Original;

            
            var deleteCommand = new SqlCommand()
            {
                CommandText = "DELETE FROM [Order] WHERE [Id] = @OrderId",
                Connection = Connection,
                CommandType = CommandType.Text
            };
            deleteCommand.Parameters.Add("@OrderId", SqlDbType.Int, 1, "Id");

            using var ordersAdapter = new SqlDataAdapter(selectCommand);
            ordersAdapter.SelectCommand = selectCommand;
            ordersAdapter.DeleteCommand = deleteCommand;
            ordersAdapter.UpdateCommand = updateCommand;
            ordersAdapter.UpdateBatchSize = 30;

            var ordersDataSet = new DataSet();
            ordersAdapter.Fill(ordersDataSet, "Order");

            foreach (DataRow row in ordersDataSet.Tables["Order"].Rows)
            {
                row.Delete();
            }

            ordersAdapter.Update(ordersDataSet, "Order");
            return true;
        }
    }
}
