namespace Alpha.Reports.Tests.Models
{
    using Constants;
    using Reports.Models;
    using Reports.Models.Enumerations;
    using TestCases;
    using Xunit;

    public class UserLevelTests
    {
        [Fact]
        public void IsAgentLevel_UserLevelIsAgent_ReturnTrue()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Agent.Value);

            // Act
            var result = userLevel.IsAgentLevel();

            // Assert
            Assert.Equal(true, result);
        }

        [Fact]
        public void IsNotAgentLevel_UserLevelIsAgent_ReturnFalse()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Agent.Value);

            // Act
            var result = userLevel.IsNotAgentLevel();

            // Assert
            Assert.Equal(false, result);
        }

        [Fact]
        public void IsMasterLevel_UserLevelIsMaster_ReturnTrue()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Master.Value);

            // Act
            var result = userLevel.IsMasterLevel();

            // Assert
            Assert.Equal(true, result);
        }

        [Fact]
        public void IsNotMasterLevel_UserLevelIsMaster_ReturnFalse()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Master.Value);

            // Act
            var result = userLevel.IsNotMasterLevel();

            // Assert
            Assert.Equal(false, result);
        }

        [Fact]
        public void IsSuperLevel_UserLevelIsSuper_ReturnTrue()
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
        public void IsMasterOrSuperLevel_UserLevelIsSuperOrMaster_ReturnTrue(byte levelId)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(levelId);

            // Act
            var result = userLevel.IsMasterOrSuperLevel();

            // Assert
            Assert.Equal(true, result);
        }

        [Fact]
        public void IsMemberLevel_UserLevelIsMember_ReturnTrue()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Member.Value);

            // Act
            var result = userLevel.IsMemberLevel();

            // Assert
            Assert.Equal(true, result);
        }

        [Fact]
        public void IsNotSuperLevel_UserLevelIsSuper_ReturnFalse()
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(UserLevel.Super.Value);

            // Act
            var result = userLevel.IsNotSuperLevel();

            // Assert
            Assert.Equal(false, result);
        }

        [Fact]
        public void IsDirectUplineOf_CustomerLevelIsNull_ReturnFalse()
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
        public void IsDirectUplineOf_Always_ReturnValidResult(UserLevel uplineLevel, byte downlineLevelValue, bool result)
        {
            var actualResult = uplineLevel.IsDirectUplineOf(downlineLevelValue);

            Assert.Equal(result, actualResult);
        }

        [Theory]
        [MemberData("UplineUserLevelAndDownlineValueDataSources", MemberType = typeof(UserLevelTestCases))]
        public void IsDirectUplineOf_ValidCustomerLevel_ReturnValidValue(UserLevel uplineLevel, byte downlineLevelValue, bool result)
        {
            // Act
            var actualResult = uplineLevel
                .IsDirectUplineOf(Enumeration.FromValue<UserLevel>(downlineLevelValue));

            // Assert
            Assert.Equal(result, actualResult);
        }

        [Theory]
        [MemberData("UplineUserLevelAndDownlineValueDataSources", MemberType = typeof(UserLevelTestCases))]
        public void IsNotDirectUplineOf_ValidDownlineLevelValue_ReturnValidResult(UserLevel uplineLevel, byte downlineLevelValue, bool result)
        {
            var actualResult = uplineLevel.IsNotDirectUplineOf(downlineLevelValue);

            Assert.Equal(!result, actualResult);
        }

        [Theory]
        [MemberData("UplineUserLevelAndDownlineValueDataSources", MemberType = typeof(UserLevelTestCases))]
        public void IsNotDirectUplineOf_DownlineLevelIsNotNull_ReturnValidResult(UserLevel uplineLevel, byte downlineLevelValue, bool result)
        {
            // Arrange
            var downLineLevel = Enumeration.FromValue<UserLevel>(downlineLevelValue);

            // Act
            var actualResult = uplineLevel.IsNotDirectUplineOf(downLineLevel);

            // Assert
            Assert.Equal(!result, actualResult);
        }

        [Fact]
        public void IsDirectDownlineOf_CustomerLevelIsNull_ReturnFalse()
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
        public void IsDirectDownlineOf_CustomerLevelIsNotNull_ReturnValidResult(UserLevel downlineLevel, byte uplineLevelValue, bool result)
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
        public void IsNotDirectDownlineOf_CustomerLevelIsNotNull_ReturnValidResult(UserLevel downlineLevel, byte uplineLevelValue, bool result)
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
        public void Is_UserLevelIsNull_ReturnFalse(byte userLevelValue)
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
        public void Is_UserLevelIsNotNull_ReturnExpectedResult(UserLevel userLevel1, UserLevel userLevel2, bool result)
        {
            var actualResult = userLevel1.Is(userLevel2);

            Assert.Equal(result, actualResult);
        }

        [Theory]
        [MemberData("SameUseLevelDataSource", MemberType = typeof(UserLevelTestCases))]
        public void IsNot_UserLevelIsNotNull_ReturnExpectedResult(UserLevel userLevel1, UserLevel userLevel2, bool result)
        {
            var actualResult = userLevel1.IsNot(userLevel2);

            Assert.Equal(!result, actualResult);
        }
    }
}
