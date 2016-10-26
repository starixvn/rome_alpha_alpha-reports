namespace Alpha.Reports.Tests.Services
{
    using System.Collections.Generic;
    using Alpha.Reports.Models;
    using Criteria;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;
    using Params;
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
            var param = new GetProductByCustomerParam(custId);
            var service = new ProductServiceImpl(repository);

            // Act
            var productList = service.GetProductsByCustomer(param);

            // Assert
            CollectionAssert.IsEmpty(productList);
        }

        [Test]
        public void GetProductByCustomer_CusIdIsGreaterThanZero_ReturnDataGetFromRepository()
        {
            // Arrange                                   
            var param = new GetProductByCustomerParam(90);
            var expected = (new Fixture()).Create<List<Product>>();
            repository = Substitute.For<IDynamicRepository>();
            repository
                .Get<List<Product>>(param)
                .Returns(expected);
            var service = new ProductServiceImpl(repository);

            // Act
            var actual = service.GetProductsByCustomer(param);

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
