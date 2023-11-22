using BishalAgroSeed.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace BishalAgroSeed.Permissions;

public class BishalAgroSeedPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var bishalAgroSeedGroup = context.AddGroup(BishalAgroSeedPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(BishalAgroSeedPermissions.MyPermission1, L("Permission:MyPermission1"));

        var configurationPermission = bishalAgroSeedGroup.AddPermission(BishalAgroSeedPermissions.Configurations.Default, L("Permission:Configurations"));
        configurationPermission.AddChild(BishalAgroSeedPermissions.Configurations.Create, L("Permission:Configurations.Create"));
        configurationPermission.AddChild(BishalAgroSeedPermissions.Configurations.Edit, L("Permission:Configurations.Edit"));
        configurationPermission.AddChild(BishalAgroSeedPermissions.Configurations.Delete, L("Permission:Configurations.Delete"));

        var customerPermission = bishalAgroSeedGroup.AddPermission(BishalAgroSeedPermissions.Customers.Default, L("Permission:Customers"));
        customerPermission.AddChild(BishalAgroSeedPermissions.Customers.Create, L("Permission:Customers.Create"));
        customerPermission.AddChild(BishalAgroSeedPermissions.Customers.Edit, L("Permission:Customers.Edit"));
        customerPermission.AddChild(BishalAgroSeedPermissions.Customers.Delete, L("Permission:Customers.Delete"));

        var brandPermission = bishalAgroSeedGroup.AddPermission(BishalAgroSeedPermissions.Brands.Default, L("Permission:Brands"));
        brandPermission.AddChild(BishalAgroSeedPermissions.Brands.Create, L("Permission:Brands.Create"));
        brandPermission.AddChild(BishalAgroSeedPermissions.Brands.Edit, L("Permission:Brands.Edit"));
        brandPermission.AddChild(BishalAgroSeedPermissions.Brands.Delete, L("Permission:Brands.Delete"));

        var categoryPermission = bishalAgroSeedGroup.AddPermission(BishalAgroSeedPermissions.Categories.Default, L("Permission:Categories"));
        categoryPermission.AddChild(BishalAgroSeedPermissions.Categories.Create, L("Permission:Categories.Create"));
        categoryPermission.AddChild(BishalAgroSeedPermissions.Categories.Edit, L("Permission:Categories.Edit"));
        categoryPermission.AddChild(BishalAgroSeedPermissions.Categories.Delete, L("Permission:Categories.Delete"));

        var companyInfoPermission = bishalAgroSeedGroup.AddPermission(BishalAgroSeedPermissions.CompanyInfos.Default, L("Permission:CompanyInfos"));
        companyInfoPermission.AddChild(BishalAgroSeedPermissions.CompanyInfos.Create, L("Permission:CompanyInfos.Create"));
        companyInfoPermission.AddChild(BishalAgroSeedPermissions.CompanyInfos.Edit, L("Permission:CompanyInfos.Edit"));
        companyInfoPermission.AddChild(BishalAgroSeedPermissions.CompanyInfos.Delete, L("Permission:CompanyInfos.Delete"));

        var productPermission = bishalAgroSeedGroup.AddPermission(BishalAgroSeedPermissions.Products.Default, L("Permission:Products"));
        productPermission.AddChild(BishalAgroSeedPermissions.Products.Create, L("Permission:Products.Create"));
        productPermission.AddChild(BishalAgroSeedPermissions.Products.Edit, L("Permission:Products.Edit"));
        productPermission.AddChild(BishalAgroSeedPermissions.Products.Delete, L("Permission:Products.Delete"));

        var openingBalancePermission = bishalAgroSeedGroup.AddPermission(BishalAgroSeedPermissions.OpeningBalances.Default, L("Permission:OpeningBalances"));
        openingBalancePermission.AddChild(BishalAgroSeedPermissions.OpeningBalances.Create, L("Permission:OpeningBalances.Create"));
        openingBalancePermission.AddChild(BishalAgroSeedPermissions.OpeningBalances.Edit, L("Permission:OpeningBalances.Edit"));
        openingBalancePermission.AddChild(BishalAgroSeedPermissions.OpeningBalances.Delete, L("Permission:OpeningBalances.Delete"));

        var numberGenerationPermission = bishalAgroSeedGroup.AddPermission(BishalAgroSeedPermissions.NumberGenerations.Default, L("Permission:NumberGenerations"));
        numberGenerationPermission.AddChild(BishalAgroSeedPermissions.NumberGenerations.Create, L("Permission:NumberGenerations.Create"));
        numberGenerationPermission.AddChild(BishalAgroSeedPermissions.NumberGenerations.Edit, L("Permission:NumberGenerations.Edit"));
        numberGenerationPermission.AddChild(BishalAgroSeedPermissions.NumberGenerations.Delete, L("Permission:NumberGenerations.Delete"));

        var fiscalYearPermission = bishalAgroSeedGroup.AddPermission(BishalAgroSeedPermissions.FiscalYears.Default, L("Permission:FiscalYears"));
        fiscalYearPermission.AddChild(BishalAgroSeedPermissions.FiscalYears.Create, L("Permission:FiscalYears.Create"));
        fiscalYearPermission.AddChild(BishalAgroSeedPermissions.FiscalYears.Edit, L("Permission:FiscalYears.Edit"));
        fiscalYearPermission.AddChild(BishalAgroSeedPermissions.FiscalYears.Delete, L("Permission:FiscalYears.Delete"));

        var cycleCountPermission = bishalAgroSeedGroup.AddPermission(BishalAgroSeedPermissions.CycleCounts.Default, L("Permission:CycleCounts"));
        cycleCountPermission.AddChild(BishalAgroSeedPermissions.CycleCounts.Create, L("Permission:CycleCounts.Create"));
        cycleCountPermission.AddChild(BishalAgroSeedPermissions.CycleCounts.Edit, L("Permission:CycleCounts.Edit"));
        cycleCountPermission.AddChild(BishalAgroSeedPermissions.CycleCounts.Close, L("Permission:CycleCounts.Close"));

        bishalAgroSeedGroup.AddPermission(BishalAgroSeedPermissions.UnitTypes.Default, L("Permission:UnitTypes"));
        bishalAgroSeedGroup.AddPermission(BishalAgroSeedPermissions.NumberGenerationTypes.Default, L("Permission:NumberGenerationTypes"));

        var tradePermission = bishalAgroSeedGroup.AddPermission(BishalAgroSeedPermissions.Trades.Default, L("Permission:Trades"));
        tradePermission.AddChild(BishalAgroSeedPermissions.Trades.Create, L("Permission:Trades.Create"));

        bishalAgroSeedGroup.AddPermission(BishalAgroSeedPermissions.TransactionTypes.Default, L("Permission:TransactionTypes"));

        var cashTransactionPermission = bishalAgroSeedGroup.AddPermission(BishalAgroSeedPermissions.CashTransactions.Default, L("Permission:CashTransactions"));
        cashTransactionPermission.AddChild(BishalAgroSeedPermissions.CashTransactions.Create, L("Permission:CashTransactions.Create"));

        var ledgerAccountPermission = bishalAgroSeedGroup.AddPermission(BishalAgroSeedPermissions.LedgerAccounts.Default, L("Permission:LedgerAccounts"));
        ledgerAccountPermission.AddChild(BishalAgroSeedPermissions.LedgerAccounts.Create, L("Permission:LedgerAccounts.Create"));
}


    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<BishalAgroSeedResource>(name);
    }
}