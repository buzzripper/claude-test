namespace Dyvenix.App1.Common.Constants;

/// <summary>
/// App1-specific permission constants.
/// </summary>
public static class Permissions
{
    public static class Products
    {
        public const string Read = "products.read";
        public const string Create = "products.create";
        public const string Update = "products.update";
        public const string Delete = "products.delete";
    }

    public static class Orders
    {
        public const string Read = "orders.read";
        public const string Create = "orders.create";
        public const string Update = "orders.update";
        public const string Approve = "orders.approve";
        public const string Cancel = "orders.cancel";
    }
}
