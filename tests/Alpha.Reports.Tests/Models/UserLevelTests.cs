namespace Alpha.Reports.Tests.Models
{
    using Constants;
    using Reports.Enumerations;
    using Reports.Models;
    using TestCases;
    using Xunit;

    public class UserLevelTests
    {
        [Fact]
        public void IsAgentLevel_UserLevelIsAgent_ReturnsTrue()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Agent.Value);

            // Act
            var result = userLevel.IsAgentLevel();

            // Assert
            Assert.Equal(true, result);
        }

        [Fact]
        public void IsNotAgentLevel_UserLevelIsAgent_ReturnsFalse()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Agent.Value);

            // Act
            var result = userLevel.IsNotAgentLevel();

            // Assert
            Assert.Equal(false, result);
        }

        [Fact]
        public void IsMasterLevel_UserLevelIsMaster_ReturnsTrue()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Master.Value);

            // Act
            var result = userLevel.IsMasterLevel();

            // Assert
            Assert.Equal(true, result);
        }

        [Fact]
        public void IsNotMasterLevel_UserLevelIsMaster_ReturnsFalse()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Master.Value);

            // Act
            var result = userLevel.IsNotMasterLevel();

            // Assert
            Assert.Equal(false, result);
        }

        [Fact]
        public void IsSuperLevel_UserLevelIsSuper_ReturnsTrue()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Super.Value);

            // Act
            var result = userLevel.IsSuperLevel();

            // Assert
            Assert.Equal(true, result);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(3)]
        public void IsMasterOrSuperLevel_UserLevelIsSuperOrMaster_ReturnsTrue(byte levelId)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(levelId);

            // Act
            var result = userLevel.IsMasterOrSuperLevel();

            // Assert
            Assert.Equal(true, result);
        }

        [Fact]
        public void IsMemberLevel_UserLevelIsMember_ReturnsTrue()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Member.Value);

            // Act
            var result = userLevel.IsMemberLevel();

            // Assert
            Assert.Equal(true, result);
        }

        [Fact]
        public void IsNotSuperLevel_UserLevelIsSuper_ReturnsFalse()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Super.Value);

            // Act
            var result = userLevel.IsNotSuperLevel();

            // Assert
            Assert.Equal(false, result);
        }

        [Fact]
        public void IsDirectUplineOf_CustomerLevelIsNull_ReturnsFalse()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Super.Value);

            // Act
            var result = userLevel.IsDirectUplineOf(null);

            // Assert
            Assert.Equal(false, result);
        }

        [Theory]
        [MemberData("UplineUserLevelAndDownlineValueDataSources", MemberType = typeof(UserLevelTestCases))]
        public void IsDirectUplineOf_Always_ReturnsValidResult(UserLevel uplineLevel, byte downlineLevelValue, bool result)
        {
            var actualResult = uplineLevel.IsDirectUplineOf(downlineLevelValue);

            Assert.Equal(result, actualResult);
        }

        [Theory]
        [MemberData("UplineUserLevelAndDownlineValueDataSources", MemberType = typeof(UserLevelTestCases))]
        public void IsDirectUplineOf_ValidCustomerLevel_ReturnsValidValue(UserLevel uplineLevel, byte downlineLevelValue, bool result)
        {
            // Act
            var actualResult = uplineLevel
                .IsDirectUplineOf(Enumeration.FromValue<UserLevel>(downlineLevelValue));

            // Assert
            Assert.Equal(result, actualResult);
        }

        [Theory]
        [MemberData("UplineUserLevelAndDownlineValueDataSources", MemberType = typeof(UserLevelTestCases))]
        public void IsNotDirectUplineOf_ValidDownlineLevelValue_ReturnsValidResult(UserLevel uplineLevel, byte downlineLevelValue, bool result)
        {
            var actualResult = uplineLevel.IsNotDirectUplineOf(downlineLevelValue);

            Assert.Equal(!result, actualResult);
        }

        [Theory]
        [MemberData("UplineUserLevelAndDownlineValueDataSources", MemberType = typeof(UserLevelTestCases))]
        public void IsNotDirectUplineOf_DownlineLevelIsNotNull_ReturnsValidResult(UserLevel uplineLevel, byte downlineLevelValue, bool result)
        {
            // Arrange
            var downLineLevel = Enumeration.FromValue<UserLevel>(downlineLevelValue);

            // Act
            var actualResult = uplineLevel.IsNotDirectUplineOf(downLineLevel);

            // Assert
            Assert.Equal(!result, actualResult);
        }

        [Fact]
        public void IsDirectDownlineOf_CustomerLevelIsNull_ReturnsFalse()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Super.Value);

            // Act
            var result = userLevel.IsDirectDownlineOf(null);

            // Assert
            Assert.Equal(false, result);
        }

        [Theory]
        [MemberData("DownlineUserLevelAndUplineValueDataSources", MemberType = typeof(UserLevelTestCases))]
        public void IsDirectDownlineOf_CustomerLevelIsNotNull_ReturnsValidResult(UserLevel downlineLevel, byte uplineLevelValue, bool result)
        {
            // Arrange
            var uplineLevel = Enumeration.FromValue<UserLevel>(uplineLevelValue);

            // Act
            var actualResult = downlineLevel.IsDirectDownlineOf(uplineLevel);

            // Assert
            Assert.Equal(result, actualResult);
        }

        [Theory]
        [MemberData("DownlineUserLevelAndUplineValueDataSources", MemberType = typeof(UserLevelTestCases))]
        public void IsNotDirectDownlineOf_CustomerLevelIsNotNull_ReturnsValidResult(UserLevel downlineLevel, byte uplineLevelValue, bool result)
        {
            // Arrange
            var uplineLevel = Enumeration.FromValue<UserLevel>(uplineLevelValue);

            // Act
            var actualResult = downlineLevel.IsNotDirectDownlineOf(uplineLevel);

            // Assert
            Assert.Equal(!result, actualResult);
        }

        [Theory]
        [InlineData(RoleId.Super)]
        [InlineData(RoleId.Master)]
        [InlineData(RoleId.Agent)]
        public void Is_UserLevelIsNull_ReturnsFalse(byte userLevelValue)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(userLevelValue);

            // Act
            var result = userLevel.Is(null);

            // Assert
            Assert.Equal(false, result);
        }

        [Theory]
        [MemberData("SameUseLevelDataSource", MemberType = typeof(UserLevelTestCases))]
        public void Is_UserLevelIsNotNull_ReturnsExpectedResult(UserLevel userLevel1, UserLevel userLevel2, bool result)
        {
            var actualResult = userLevel1.Is(userLevel2);

            Assert.Equal(result, actualResult);
        }

        [Theory]
        [MemberData("SameUseLevelDataSource", MemberType = typeof(UserLevelTestCases))]
        public void IsNot_UserLevelIsNotNull_ReturnsExpectedResult(UserLevel userLevel1, UserLevel userLevel2, bool result)
        {
            var actualResult = userLevel1.IsNot(userLevel2);

            Assert.Equal(!result, actualResult);
        }
    }
}
