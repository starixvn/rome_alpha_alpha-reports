namespace Alpha.Reports.Tests.Helpers
{
    using System;
    using Xunit;

    public class ExceptionRecorderTests
    {
        [Fact]
        public void Record_HaveException_ReturnsException()
        {
            // Arrange
            Action action = () =>
            {
                throw new Exception();
            };

            // Act
            var actualException = ExceptionRecorder.Record(action);

            // Assert
            Assert.NotNull(actualException);
        }

        [Fact]
        public void Record_NotHaveException_ReturnsNull()
        {
            // Arrange
            Action action = () => { };

            // Act
            var actualException = ExceptionRecorder.Record(action);

            // Assert
            Assert.Null(actualException);
        }
    }
}
