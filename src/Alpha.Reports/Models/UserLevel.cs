namespace Alpha.Reports.Models
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Enumerations;

    [Serializable]
    public class UserLevel : Enumeration
    {
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        public static readonly UserLevel Super = new UserLevel(4, "Super");

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        public static readonly UserLevel Master = new UserLevel(3, "Master");

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        public static readonly UserLevel Agent = new UserLevel(2, "Agent");

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        public static readonly UserLevel Member = new UserLevel(1, "Member");

        public UserLevel()
        {
        }

        private UserLevel(byte value, string displayName)
            : base(value, displayName)
        {
        }

        public bool IsAgentLevel() => this == Agent;

        public bool IsNotAgentLevel() => !IsAgentLevel();

        public bool IsMasterLevel() => this == Master;

        public bool IsNotMasterLevel() => !IsMasterLevel();

        public bool IsSuperLevel() => this == Super;

        public bool IsMasterOrSuperLevel() => IsMasterLevel() || IsSuperLevel();

        public bool IsNotSuperLevel() => !IsSuperLevel();

        public bool IsMemberLevel() => this == Member;

        public bool IsDirectUplineOf(byte downlineLevelValue) => GapBetweenTwoLevels(this.Value, downlineLevelValue) == 1;

        public bool IsNotDirectUplineOf(byte downlineLevelValue) => !IsDirectUplineOf(downlineLevelValue);

        public UserLevel GetDirectDownLineLevel() => FromValue<UserLevel>((byte)(this.Value - 1));

        public bool IsDirectUplineOf(Enumeration customerLevel)
        {
            if (customerLevel == null)
            {
                return false;
            }

            return IsDirectUplineOf(customerLevel.Value);
        }

        public bool IsNotDirectUplineOf(Enumeration customerLevel)
        {
            return !IsDirectUplineOf(customerLevel);
        }

        public bool IsDirectDownlineOf(Enumeration uplineLevel)
        {
            if (uplineLevel == null)
            {
                return false;
            }

            return GapBetweenTwoLevels(uplineLevel.Value, this.Value) == 1;
        }

        public bool IsNotDirectDownlineOf(Enumeration uplineLevel) => !IsDirectDownlineOf(uplineLevel);

        public bool Is(Enumeration level)
        {
            if (level != null)
            {
                return this == level;
            }

            return false;
        }

        public bool IsNot(Enumeration level) => !Is(level);

        private static int GapBetweenTwoLevels(byte uplineLevelValue, byte downlineLevelValue) => uplineLevelValue - downlineLevelValue;
    }
}