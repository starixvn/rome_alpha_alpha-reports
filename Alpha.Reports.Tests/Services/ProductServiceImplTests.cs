namespace Alpha.Reports.Tests.Services
{
    using System.Collections.Generic;
    using Alpha.Reports.Models;
    using Criteria;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Reports.Services.Impl;

    [TestFixture]
    public class ProductServiceImplTests
    {
        private IDynamicRepository repository;

        [TestCase(0)]
        [TestCase(-1)]
        public void GetProductByCustomer_CusIdIsEqualOrLessThanZero_ReturnEmptyListProduct(int custId)
        {
            // Arrange            
            var criteria = new GetProductByCustomerCriteria(custId);
            var service = new ProductServiceImpl(repository);

            // Act
            var productList = service.GetProductsByCustomer(criteria);

            // Assert
            CollectionAssert.IsEmpty(productList);
        }

        [Test]
        public void GetProductByCustomer_CusIdIsGreaterThanZero_ReturnDataGetFromRepository()
        {
            // Arrange                                   
            var criteria = new GetProductByCustomerCriteria(90);
            var expected = (new Fixture()).Create<List<Product>>();
            repository = Substitute.For<IDynamicRepository>();
            repository
                .Get<List<Product>>(criteria)
                .Returns(expected);
            var service = new ProductServiceImpl(repository);

            // Act
            var actual = service.GetProductsByCustomer(criteria);

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
