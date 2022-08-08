using DB_Operations.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DB_Operations
{
    public class DatabaseOperations_Connected
    {
        private SqlConnection _connection;
        private string _connectionString;

        public DatabaseOperations_Connected(string connectionString)
        {
            _connectionString = connectionString;
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

        public bool InsertOrder(OrderModel order)
        {
            try
            {
                Connection.Open();
                using var insert = new SqlCommand()
                {
                    CommandText = "INSERT INTO [Order] ([Status], [CreatedDate], [UpdatedDate], [ProductId]) VALUES (@Status, @CreatedDate, @UpdatedDate, @ProductId)",
                    Connection = Connection,
                    CommandType = CommandType.Text
                };
                
                insert.Parameters.AddWithValue("@Status", order.Status.ToString());
                insert.Parameters.AddWithValue("@CreatedDate", order.CreatedDate.ToString("s"));
                insert.Parameters.AddWithValue("@UpdatedDate", order.UpdatedDate.ToString("s"));
                insert.Parameters.AddWithValue("@ProductId", order.ProductId);

                var insertedRows = insert.ExecuteNonQuery();
                return insertedRows > 0;
            }
            finally
            {
                Connection.Close();
            }
        }

        public OrderModel ReadOrder(int orderNumber)
        {
            try
            {
                Connection.Open();
                using var read = new SqlCommand()
                {
                    CommandText = "SELECT * FROM [Order] WHERE [Id] = @OrderNumber",
                    Connection = Connection,
                    CommandType = CommandType.Text
                };
                read.Parameters.AddWithValue("@OrderNumber", orderNumber);

                var obtainedOrder = new OrderModel();
                using (var reader = read.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        obtainedOrder.Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), reader["Status"].ToString());
                        obtainedOrder.CreatedDate = (DateTime)reader["CreatedDate"];
                        obtainedOrder.UpdatedDate = (DateTime)reader["UpdatedDate"];
                        obtainedOrder.ProductId = (int)reader["ProductId"];
                    }
                    else
                    {
                        obtainedOrder = null;
                    }
                }
                return obtainedOrder;
            }
            finally
            {
                Connection.Close();
            }
        }

        public OrderModel UpdateOrderStatus(int orderNumber, OrderStatus status)
        {
            // Update status.
            var numberOfUpdatedRows = 0;
            try
            {
                Connection.Open();
                using var update = new SqlCommand()
                {
                    CommandText = "UPDATE [Order] " +
                        "SET [Status] = @Status, [UpdatedDate] = @UpdatedDate " +
                        "WHERE [Id] = @OrderNumber",
                    Connection = Connection,
                    CommandType = System.Data.CommandType.Text
                };
                update.Parameters.AddWithValue("@OrderNumber", orderNumber);
                update.Parameters.AddWithValue("@Status", status.ToString());
                update.Parameters.AddWithValue("@UpdatedDate", DateTime.Now.Date);
            
                numberOfUpdatedRows = update.ExecuteNonQuery();
            }
            finally
            {
                Connection.Close();
            }

            // Read and return the result.
            OrderModel udpatedOrder = null;
            if (numberOfUpdatedRows > 0)
            {
                udpatedOrder = ReadOrder(orderNumber);
            }

            return udpatedOrder;
        }

        public bool DeleteOrder(int orderNumber)
        {
            try
            {
                Connection.Open();
                using var delete = new SqlCommand()
                {
                    CommandText = "DELETE FROM [Order] WHERE [Id] = @OrderNumber",
                    Connection = Connection,
                    CommandType = CommandType.Text
                };
                delete.Parameters.AddWithValue("@OrderNumber", orderNumber);

                var numberOfUpdatedRows = delete.ExecuteNonQuery();
                return numberOfUpdatedRows > 0;
            }
            finally
            {
                Connection.Close();
            }
        }

        public List<OrderModel> FetchOrdersFilterBy(OrderStatus? status = null, int? createdYear = null, int? updatedMonth = null, int? productId = null)
        {
            try
            {
                Connection.Open();
                using var read = new SqlCommand()
                {
                    CommandText = "[sp_GetOrdersFiltered]",
                    CommandType = CommandType.StoredProcedure,
                    Connection = Connection
                };
                read.Parameters.AddWithValue("@Status", status.ToString());
                read.Parameters.AddWithValue("@CreatedYear", createdYear);
                read.Parameters.AddWithValue("@UpdatedMonth", updatedMonth);
                read.Parameters.AddWithValue("@ProductId", productId);

                var foundOrders = new List<OrderModel>();
                using (var reader = read.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obtainedOrder = new OrderModel()
                        {
                            Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), reader["Status"].ToString()),
                            CreatedDate = (DateTime)reader["CreatedDate"],
                            UpdatedDate = (DateTime)reader["UpdatedDate"],
                            ProductId = (int)reader["ProductId"]
                        };
                        foundOrders.Add(obtainedOrder);    
                    };
                }

                return foundOrders;
            }
            finally
            {
                Connection.Close();
            }
        }

    }
}
