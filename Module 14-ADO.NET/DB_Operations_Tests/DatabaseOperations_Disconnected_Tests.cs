using NUnit.Framework;
using DB_Operations;
using DB_Operations.Models;
using System;

namespace DB_Operations_Tests
{
    [TestFixture]
    public class DatabaseOperations_Disconnected_Tests
    {
        private string _connectionString;
        private DatabaseOperations_Disconnected _dbOperations;

        [SetUp]
        public void Setup()
        {
            _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ADONET_Testing;Integrated Security=True;";
            _dbOperations = new DatabaseOperations_Disconnected(_connectionString);
        }

        [Test]
        public void InsertProduct_ProductProvided_NewProductInProductTable()
        {
            // arrange
            var newProduct = new ProductModel()
            {
                Name = "Umbrella",
                Description = "Useful product",
                Weight = 3.40m,
                Height = 2.50m,
                Width = 4.10m,
                Length = 34.00m
            };

            // act
            _dbOperations.InsertProduct(newProduct);

            // assert
            Assert.Pass();
        }

        [Test]
        public void ReadProduct_ProductNumberProvided_ProductIsObtained()
        {
            // arrange
            // The test uses the known value of the first row of the database which was filled by post-deplyment script.
            var orderNumber = 1;
            var expected = new ProductModel()
            {
                Name = "Book",
                Description = "Usual Order",
                Weight = 12.30m,
                Height = 23.50m,
                Width = 20.00m,
                Length = 43.00m
            };

            // act
            var actual = _dbOperations.ReadProduct(orderNumber);

            // assert
            NUnitExtensions.AreEqualByJson(expected, actual);
        }

        [Test]
        public void UpdateProductDescription_ProductNumberAndNewDescriptionProvided_DescriptionOfProductIsUpdated()
        {
            // arrange
            // The test uses the known value of the second row of the database which was filled by post-deplyment script.
            var productId = 2;
            var newDescription = "Natural Product";
            var oldProduct = _dbOperations.ReadProduct(productId);
            var expected = new ProductModel()
            {
                Name = oldProduct.Name,
                Description = newDescription,
                Weight = oldProduct.Weight,
                Height = oldProduct.Height,
                Width = oldProduct.Width,
                Length = oldProduct.Length
            };

            // act
            var actual = _dbOperations.UpdateProductDescription(productId, newDescription);

            // assert
            NUnitExtensions.AreEqualByJson(expected, actual);
        }

        [Test]
        public void DeleteProduct_ProductNumberProvided_ProductIsDeleted()
        {
            // arrange
            var productId = 2;
            if (_dbOperations.ReadProduct(productId) == null)
            {
                throw new ArgumentException($"Product Id = {productId} is not present in the database and cannot be deleted. Please choose another product Id.");
            }

            // act
            _dbOperations.DeleteProduct(productId);
            var isProductDeleted = _dbOperations.ReadProduct(productId) == null;

            // assert
            Assert.IsTrue(isProductDeleted);
        }

        [Test]
        public void FetchAllProducts_ListOfAllProductsReturned()
        {
            // arrange

            // act
            var actual = _dbOperations.FetchAllProducts();

            //
            Assert.Pass();
        }

        [Test]
        public void DeleteOrdersFilterBy_StatusProvided_OrdersWithStatusDeleted()
        {
            // arrange
            var status = OrderStatus.Done;

            // act
            var actual = _dbOperations.DeleteOrdersInBulk(status: status);

            //
            Assert.Pass();
        }

        [Test]
        public void DeleteOrdersFilterBy_StatusAndUpdatedMonthProvided_OrdersWithStatusAndMonthDeleted()
        {
            // arrange
            var status = OrderStatus.Arrived;
            var updatedMonth = 2;

            // act
            var actual = _dbOperations.DeleteOrdersInBulk(status: status, updatedMonth: updatedMonth);

            //
            Assert.Pass();
        }

    }
}