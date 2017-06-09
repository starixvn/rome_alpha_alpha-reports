namespace Alpha.Reports.Tests.Models.WinLossByProduct
{
    using System.Collections.Generic;
    using KellermanSoftware.CompareNetObjects;
    using Reports.Models.WinlossByBetType;
    using Reports.Models.WinLossByProduct;
    using Site.Tests.TestCases.WinLossByProduct;
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
            IEnumerable<WinLossBySportModel> winLossBySportModels,
            WinLossByBetTypeModel expectedTotal)
        {
            // Act
            var winLossByProductModel = new WinLossByProductModel(
                    productId: 71,
                    productName: "Live Casino",
                    winLossBySportModels: winLossBySportModels);

            // Assert
            var result = compareLogic.Compare(winLossByProductModel.Total, expectedTotal);
            Assert.True(result.AreEqual);
        }
    }
}