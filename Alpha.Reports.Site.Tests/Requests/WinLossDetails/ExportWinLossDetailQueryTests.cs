namespace Alpha.Reports.Site.Tests.Requests.WinLossDetails
{
    using System;
    using Site.Requests.WinLossDetails;
    using ViewModels.WinLossDetails;
    using Xunit;

    public class ExportWinLossDetailQueryTests
    {
        [Fact]
        public void Create_FromNullWinLossDetailViewModel_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossDetailViewModel viewModel = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ExportWinLossDetailQuery(viewModel));
        }
    }
}
