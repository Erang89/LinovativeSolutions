using LinoVative.Service.Core.Sources;

namespace LinoVative.Service.Backend.Helpers
{
    internal static class Message
    {
        public static class Entity
        {
            public const string NotFound = $"Entity.{nameof(NotFound)}";
        }

        public static class Password
        {
            public const string NotStrong = $"Password.NotStrong";
        }

        public static class User
        {
            public const string EmailAlreadyExist = $"User.EmailAlreadyExist";
        }

        public static class EntityName
        {
            public const string Country = "EntityName.Country";
            public const string Currency = "EntityName.Currency";
            public const string TimeZone = "EntityName.TimeZone";
        }
    }
}
