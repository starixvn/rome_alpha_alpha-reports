namespace Alpha.Reports.Tests.Models
{
    using System.Collections.Generic;
    using KellermanSoftware.CompareNetObjects;
    using Reports.Models.WinlossByBetType;
    using Reports.Models.WinLossByProduct;
    using Site.Tests.TestCases;
    using Xunit;

    [Trait("Category", "WinLossByProduct")]
    public class WinLossByProductModelTest
    {
        private readonly CompareLogic compareLogic;

        public WinLossByProductModelTest()
        {
            compareLogic = new CompareLogic();
        }

        [Theory]
        [MemberData("WinLossByProductModelSources", MemberType = typeof(WinLossByProductModelTestCases))]
        public void AccumulateTotal_ValidWinLossByBetTypeData_ReturnCorrectTotal(
            IEnumerable<WinLossByBetTypeModel> winLossByBetTypeData,
            WinLossByBetTypeModel expectedTotal)
        {
            // Arrange

            // Act
            var winLossByProductModel = new WinLossByProductModel(
                    productId: 71,
                    productName: "Live Casino",
                    winLossByBetTypeData: winLossByBetTypeData);

            // Assert
            var result = compareLogic.Compare(winLossByProductModel.Total, expectedTotal);
            Assert.True(result.AreEqual);
        }
    }
}