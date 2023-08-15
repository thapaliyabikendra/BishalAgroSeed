using System.Dynamic;

namespace BishalAgroSeed.Permissions;

public static class BishalAgroSeedPermissions
{
    public const string GroupName = "BishalAgroSeed";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";

    public static class Configurations
    {
        public const string Default = GroupName + ".Configurations";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
    public static class Customers
    {
        public const string Default = GroupName + ".Customers";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}
