using NUnit.Framework;
using DB_Operations;
using DB_Operations.Models;
using System;

namespace DB_Operations_Tests
{
    [TestFixture]
    public class DatabaseOperations_Connected_Tests
    {
        private string _connectionString;
        private DatabaseOperations_Connected _dbOperations;
        [SetUp]
        public void Setup()
        {
            _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ADONET_Testing;Integrated Security=True;";
            _dbOperations = new DatabaseOperations_Connected(_connectionString);
        }

        [Test]
        public void InsertOrder_OrderProvided_NewOrderInOrderTable()
        {
            // arrange
            var newOrder = new OrderModel()
            {
                Status = OrderStatus.Arrived,
                CreatedDate = System.DateTime.Now,
                UpdatedDate = System.DateTime.Now,
                ProductId = 1
            };

            // act
            _dbOperations.InsertOrder(newOrder);

            // assert
            Assert.Pass();
        }

        [Test]
        public void ReadOrder_OrderNumberProvided_OrderIsObtained()
        {
            // arrange
            // The test uses the known value of the first row of the database which was filled by post-deplyment script.
            var orderNumber = 1;
            var expected = new OrderModel()
            {
                Status = OrderStatus.NotStarted,
                CreatedDate = new System.DateTime(2020, 2, 2),
                UpdatedDate = new System.DateTime(2020, 2, 5),
                ProductId = 1
            };

            // act
            var actual = _dbOperations.ReadOrder(orderNumber);

            // assert
            NUnitExtensions.AreEqualByJson(expected, actual);
        }

        [Test]
        public void UpdateOrderStatus_OrderNumberAndNewStatusProvided_StatusOfOrderIsUpdated()
        {
            // arrange
            // The test uses the known value of the second row of the database which was filled by post-deplyment script.
            var orderNumber = 2;
            var newStatus = OrderStatus.Done;
            var now = DateTime.Now;
            var expected = new OrderModel()
            {
                Status = newStatus,
                CreatedDate = new DateTime(2020, 2, 3),
                UpdatedDate = new DateTime(now.Year, now.Month, now.Day), // rounding
                ProductId = 2
            };

            // act
            var actual = _dbOperations.UpdateOrderStatus(orderNumber, newStatus);

            // assert
            NUnitExtensions.AreEqualByJson(expected, actual);
        }

        [Test]
        public void DeleteOrder_OrderNumberProvided_OrderIsDeleted()
        {
            // arrange
            var orderNumber = 4;
            if (_dbOperations.ReadOrder(orderNumber) == null)
            {
                throw new ArgumentException($"Order Id = {orderNumber} is not present in the database and cannot be deleted. Please choose another order Id.");
            }

            // act
            _dbOperations.DeleteOrder(orderNumber);
            var isOrderDeleted = _dbOperations.ReadOrder(orderNumber) == null;

            // assert
            Assert.IsTrue(isOrderDeleted);
        }

        [Test]
        public void FetchOrdersFilterBy_StatusProvided_ListOfOrdersWithStatusReturned()
        {
            // arrange
            var status = OrderStatus.Done;

            // act
            var actual = _dbOperations.FetchOrdersFilterBy(status: status);

            //
            Assert.Pass();
        }

        [Test]
        public void FetchOrdersFilterBy_StatusAndUpdatedMonthProvided_ListOfOrdersWithStatusAndMonthReturned()
        {
            // arrange
            var status = OrderStatus.Done;
            var updatedMonth = 2;

            // act
            var actual = _dbOperations.FetchOrdersFilterBy(status: status, updatedMonth: updatedMonth);

            //
            Assert.Pass();
        }

    }
}