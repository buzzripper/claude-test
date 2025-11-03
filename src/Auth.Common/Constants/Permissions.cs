namespace Dyvenix.Auth.Common.Constants;

/// <summary>
/// Application-wide permission constants.
/// These are hard-coded by the application and used for authorization.
/// </summary>
public static class Permissions
{
    public static class Users
    {
        public const string Read = "users.read";
        public const string Create = "users.create";
        public const string Update = "users.update";
        public const string Delete = "users.delete";
        public const string Manage = "users.manage";
    }
}
