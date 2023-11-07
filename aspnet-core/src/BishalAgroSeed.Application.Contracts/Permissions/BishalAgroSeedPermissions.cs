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
    public static class Brands
    {
        public const string Default = GroupName + ".Brands";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
    public static class Categories
    {
        public const string Default = GroupName + ".Categories";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
    public static class CompanyInfos
    {
        public const string Default = GroupName + ".CompanyInfos";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
    public static class Products
    {
        public const string Default = GroupName + ".Products";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
    public static class OpeningBalances
    {
        public const string Default = GroupName + ".OpeningBalances";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
    public static class NumberGenerations
    {
        public const string Default = GroupName + ".NumberGenerations";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class FiscalYears
    {
        public const string Default = GroupName + ".FiscalYears";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
    public static class UnitTypes
    {
        public const string Default = GroupName + ".UnitTypes";
    }

    public static class NumberGenerationTypes
    {
        public const string Default = GroupName + ".NumberGenerationTypes";
    }
    public static class CycleCounts
    {
        public const string Default = GroupName + ".CycleCounts";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Close = Default + ".Close";
    }
    public static class Trades
    {
        public const string Default = GroupName + ".Trades";
        public const string Create = Default + ".Create";
    }
    public static class TransactionTypes
    {
        public const string Default = GroupName + ".TransactionTypes";
    }
    public static class CashTransactions
    {
        public const string Default = GroupName + ".CashTransactions";
        public const string Create = Default + ".Create";
    }
}