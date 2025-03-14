
namespace CloudSales.Domain.Common;

public static class Constants
{
    public static class Validation
    {
        public static class RuleSets
        {
            public const string Create = "Create";
            public const string Update = "Update";
        }
    }

    public static class Auth
    {
        public static class Claims
        {
            public const string UserId = "userId";
            public const string CustomerId = "customerId";
        }
    }
}
