namespace Alpha.Reports.Site.Tests.Requests.WinLossDetails
{
    using System;
    using Site.Requests.WinLossDetails;
    using Site.ViewModels.WinLossDetails;
    using Xunit;

    public class ExportWinLossDetailQueryTests
    {
        [Fact]
        public void Create_FromNullWinLossDetailViewModel_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossDetailViewModel nullViewModel = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ExportWinLossDetailQuery(nullViewModel));
        }
    }
}
