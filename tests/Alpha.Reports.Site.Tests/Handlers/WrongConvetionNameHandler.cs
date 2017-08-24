namespace InvalidNamespace
{
    using System;
    using Alpha.Reports.Site.Handlers;
    using Xunit;

    public class WrongConvetionNameHandler : BaseHandler
    {
        [Fact]
        public void GetExcelTemplateFolder_Always_ThrowsArgumentException()
        {
            // Arrange
            var handler = new WrongConvetionNameHandler();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => handler.GetExcelTemplateFolder());
        }
    }
}
