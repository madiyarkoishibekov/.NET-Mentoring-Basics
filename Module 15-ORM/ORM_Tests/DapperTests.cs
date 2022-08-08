using DapperProject;
using DapperProject.Models;
using NUnit.Framework;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace ORM_Tests
{
    public class DapperTests
    {
        private string _connectionString;
        private DapperOperations _dbOperations;

        [SetUp]
        public void Setup()
        {
            _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ORM_database;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            _dbOperations = new DapperOperations(_connectionString);
            TruncateDB();
        }

        private void TruncateDB()
        {
            using var connection = new SqlConnection(_connectionString);
            using var truncateProducts = new SqlCommand("DELETE FROM [Product]; DBCC CHECKIDENT([Product], RESEED, 0);", connection);
            var truncateOrders = new SqlCommand("DELETE FROM [Order]; DBCC CHECKIDENT([Order], RESEED, 0);", connection);
            connection.Open();
            truncateOrders.ExecuteNonQuery();
            truncateProducts.ExecuteNonQuery();
        }

        /// <summary>
        /// This is a single big test which runs all small tests in specified order.
        /// The order is crucial for database data checking.
        /// </summary>
        [Test]
        public void AllTests()
        {
            // CRUD operations:
            // insert tests
            TestScenario_InsertProduct_ProductModelProvided_ProductCreated();
            TestScenario_InsertOrder_OrderModelProvided_OrdersCreated();
            // read test
            TestScenario_ReadOrder_OrderNumberProvided_OrderIsObtained();
            // update test
            TestScenario_UpdateOrderStatus_OrderNumberAndNewStatusProvided_OrderIsUpdated();
            // delete test
            TestScenario_DeleteOrder_OrderNumberProvided_OrderIsDeleted();

            // fetch all products test
            TestScenario_FetchAllProducts_AllProductsAreObtained();

            // fetch orders filtered by status
            TestScenario_FetchOrdersFilteredByStatus_FilteredOrdersAreObtained();

            // bulk delete orders filtered by created date
            TestScenario_BulkDeleteOrdersFilteredByCreatedYear_FilteredOrdersAreDeleted();
        }

        private void TestScenario_InsertProduct_ProductModelProvided_ProductCreated()
        {
            // arrange
            var product1 = new ProductModel()
            {
                Name = "Product 1",
                Description = "Best product",
                Height = 5,
                Weight = 6,
                Length = 7,
                Width = 8
            };

            var product2 = new ProductModel()
            {
                Name = "Product 2",
                Description = "Middle product",
                Height = 9,
                Weight = 10,
                Length = 11,
                Width = 12
            };

            var product3 = new ProductModel()
            {
                Name = "Product 3",
                Description = "Worst product",
                Height = 13,
                Weight = 14,
                Length = 15,
                Width = 16
            };

            // act
            var actualAffectedRows = 0;
            actualAffectedRows += _dbOperations.InsertProduct(product1);
            actualAffectedRows += _dbOperations.InsertProduct(product2);
            actualAffectedRows += _dbOperations.InsertProduct(product3);
            var expectedAffectedRows = 3;

            // assert
            Assert.AreEqual(expectedAffectedRows, actualAffectedRows);
        }

        private void TestScenario_InsertOrder_OrderModelProvided_OrdersCreated()
        {
            // arrange
            var order1 = new OrderModel()
            {
                CreatedDate = DateTime.Parse("2012-04-06"),
                UpdatedDate = DateTime.Parse("2012-04-20"),
                Status = OrderStatus.NotStarted,
                ProductId = 1
            };
            var order2 = new OrderModel()
            {
                CreatedDate = DateTime.Parse("2015-04-06"),
                UpdatedDate = DateTime.Parse("2015-04-20"),
                Status = OrderStatus.InProgress,
                ProductId = 2
            };
            var order3 = new OrderModel()
            {
                CreatedDate = DateTime.Parse("2018-04-06"),
                UpdatedDate = DateTime.Parse("2018-04-20"),
                Status = OrderStatus.Arrived,
                ProductId = 3
            };

            // act
            var actualAffectedRows = 0;
            actualAffectedRows += _dbOperations.InsertOrder(order1);
            actualAffectedRows += _dbOperations.InsertOrder(order2);
            actualAffectedRows += _dbOperations.InsertOrder(order3);
            var expectedAffectedRows = 3;

            // assert
            Assert.AreEqual(expectedAffectedRows, actualAffectedRows);
        }

        /// <summary>
        /// This test expects data from Insert test to be present.
        /// </summary>
        private void TestScenario_ReadOrder_OrderNumberProvided_OrderIsObtained()
        {
            // arrange
            var orderNumber = 2;

            // act
            var actualOrder = _dbOperations.ReadOrder(orderNumber);
            var expectedOrder = new OrderModel()
            {
                Id = 2,
                CreatedDate = DateTime.Parse("2015-04-06"),
                UpdatedDate = DateTime.Parse("2015-04-20"),
                Status = OrderStatus.InProgress,
                ProductId = 2
            };

            // assert
            TestHelpers.AreEqualByJson(actualOrder, expectedOrder);
        }

        private void TestScenario_UpdateOrderStatus_OrderNumberAndNewStatusProvided_OrderIsUpdated()
        {
            // arrange
            var orderNumber = 2;
            var newStatus = OrderStatus.Done;
            var now = DateTime.Now;
            var expectedOrder = new OrderModel()
            {
                Id = orderNumber,
                CreatedDate = DateTime.Parse("2015-04-06"),
                UpdatedDate = new DateTime(now.Year, now.Month, now.Day),
                Status = newStatus,
                ProductId = 2
            };

            // act
            var actualOrder = _dbOperations.UpdateOrderStatus(orderNumber, newStatus);

            // assert
            TestHelpers.AreEqualByJson(expectedOrder, actualOrder);
        }

        private void TestScenario_DeleteOrder_OrderNumberProvided_OrderIsDeleted()
        {
            // arrange
            var orderNumber = 3;

            // act
            var deleteStatusOK = _dbOperations.DeleteOrder(orderNumber);
            var isOrderDeleted = (deleteStatusOK) && (_dbOperations.ReadOrder(orderNumber) == null);

            // assert
            Assert.IsTrue(isOrderDeleted);
        }

        private void TestScenario_FetchAllProducts_AllProductsAreObtained()
        {
            // arrange

            // act
            var allProducts = _dbOperations.FetchAllProducts();

            // assert
            Assert.IsTrue(allProducts.Count > 1);
        }

        private void TestScenario_FetchOrdersFilteredByStatus_FilteredOrdersAreObtained()
        {
            // arrange
            var requiredStatus = OrderStatus.NotStarted; // normally this is Order with Id = 1

            // act
            var filteredOrders = _dbOperations.FetchOrdersFilteredBy(status: requiredStatus);

            // assert
            Assert.IsTrue(filteredOrders.Any() && filteredOrders.All(o => o.Status == requiredStatus));
        }

        private void TestScenario_BulkDeleteOrdersFilteredByCreatedYear_FilteredOrdersAreDeleted()
        {
            // arrange
            var createdYear = 2012; // normally this is Order with Id = 1

            // act
            var deleteStatusOK = _dbOperations.BulkDeleteOrdersFilteredBy(createdYear: createdYear);
            var isOrderDeleted = !_dbOperations.FetchOrdersFilteredBy(createdYear: createdYear).Any();

            // assert
            Assert.IsTrue(isOrderDeleted);
        }
    }
}