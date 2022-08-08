using BusinessLogic;
using DataBaseAccess.Models;

namespace BusinessLogic_Tests
{
    public class Tests
    {
        private EFOperations _operations;

        [SetUp]
        public void Setup()
        {
            _operations = new EFOperations();
            _operations.DeleteAllTables();
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
            TestScenario_ReadProduct_ProductNumberProvided_ProductIsObtained();
            // update test
            TestScenario_UpdateProductDescription_ProductNumberAndNewDescriptionProvided_ProductIsUpdated();
            // delete test
            TestScenario_DeleteProduct_ProductNumberProvided_ProductIsDeleted();

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
            actualAffectedRows += _operations.InsertProduct(product1);
            actualAffectedRows += _operations.InsertProduct(product2);
            actualAffectedRows += _operations.InsertProduct(product3);
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
            actualAffectedRows += _operations.InsertOrder(order1);
            actualAffectedRows += _operations.InsertOrder(order2);
            actualAffectedRows += _operations.InsertOrder(order3);
            var expectedAffectedRows = 3;

            // assert
            Assert.AreEqual(expectedAffectedRows, actualAffectedRows);
        }

        /// <summary>
        /// This test expects data from Insert test to be present.
        /// </summary>
        private void TestScenario_ReadProduct_ProductNumberProvided_ProductIsObtained()
        {
            // arrange
            var productId = 2;

            // act
            var actualProduct = _operations.ReadProduct(productId);
            var expectedProduct = new ProductModel()
            {
                Id = productId,
                Name = "Product 2",
                Description = "Middle product",
                Height = 9,
                Weight = 10,
                Length = 11,
                Width = 12
            };

            // assert
            TestHelpers.AreEqualByJson(actualProduct, expectedProduct);
        }

        private void TestScenario_UpdateProductDescription_ProductNumberAndNewDescriptionProvided_ProductIsUpdated()
        {
            // arrange
            var productId = 2;
            var newDescription = "This product is yellow.";
            var expectedProduct = new ProductModel()
            {
                Id = productId,
                Name = "Product 2",
                Description = newDescription,
                Height = 9,
                Weight = 10,
                Length = 11,
                Width = 12
            };

            // act
            var actualProduct = _operations.UpdateProductDecription(productId, newDescription);

            // assert
            TestHelpers.AreEqualByJson(expectedProduct, actualProduct);
        }

        private void TestScenario_DeleteProduct_ProductNumberProvided_ProductIsDeleted()
        {
            // arrange
            var productId = 3;

            // act
            var deleteStatusOK = _operations.DeleteProduct(productId);
            var isOrderDeleted = (deleteStatusOK) && (_operations.ReadProduct(productId) == null);

            // assert
            Assert.IsTrue(isOrderDeleted);
        }

        private void TestScenario_FetchAllProducts_AllProductsAreObtained()
        {
            // arrange

            // act
            var allProducts = _operations.FetchAllProducts();

            // assert
            Assert.IsTrue(allProducts.Count > 1);
        }

        private void TestScenario_FetchOrdersFilteredByStatus_FilteredOrdersAreObtained()
        {
            // arrange
            var requiredStatus = OrderStatus.NotStarted; // normally this is Order with Id = 1

            // act
            var filteredOrders = _operations.FetchOrdersFilteredBy(status: requiredStatus);

            // assert
            Assert.IsTrue(filteredOrders.Any() && filteredOrders.All(o => o.Status == requiredStatus));
        }

        private void TestScenario_BulkDeleteOrdersFilteredByCreatedYear_FilteredOrdersAreDeleted()
        {
            // arrange
            var createdYear = 2012; // normally this is Order with Id = 1

            // act
            var deleteStatusOK = _operations.BulkDeleteOrdersFilteredBy(createdYear: createdYear);
            var isOrderDeleted = !_operations.FetchOrdersFilteredBy(createdYear: createdYear).Any();

            // assert
            Assert.IsTrue(deleteStatusOK && isOrderDeleted);
        }
    }
}