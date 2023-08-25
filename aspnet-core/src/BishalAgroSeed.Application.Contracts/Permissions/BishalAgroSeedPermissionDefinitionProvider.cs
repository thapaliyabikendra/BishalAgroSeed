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
        brandPermission.AddChild(BishalAgroSeedPermissions.Brands.Create, L("Permissions:Brands.Create"));
        brandPermission.AddChild(BishalAgroSeedPermissions.Brands.Edit, L("Permissions:Brands.Edit"));
        brandPermission.AddChild(BishalAgroSeedPermissions.Brands.Delete, L("Permissions:Brands.Delete"));

        var categoryPermission = bishalAgroSeedGroup.AddPermission(BishalAgroSeedPermissions.Categories.Default, L("Permission:Categories"));
        categoryPermission.AddChild(BishalAgroSeedPermissions.Categories.Create, L("Permissions:Categories.Create"));
        categoryPermission.AddChild(BishalAgroSeedPermissions.Categories.Edit, L("Permissions:Categories.Edit"));
        categoryPermission.AddChild(BishalAgroSeedPermissions.Categories.Delete, L("Permissions:Categories.Delete"));

        var companyInfoPermission = bishalAgroSeedGroup.AddPermission(BishalAgroSeedPermissions.CompanyInfos.Default, L("Permission:CompanyInfos"));
        companyInfoPermission.AddChild(BishalAgroSeedPermissions.CompanyInfos.Create, L("Permissions:CompanyInfos.PermissionCreate"));
        companyInfoPermission.AddChild(BishalAgroSeedPermissions.CompanyInfos.Edit, L("Permissions:CompanyInfos.Edit"));
        companyInfoPermission.AddChild(BishalAgroSeedPermissions.CompanyInfos.Delete, L("Permissions:CompanyInfos.Delete"));


        var productPermission = bishalAgroSeedGroup.AddPermission(BishalAgroSeedPermissions.Products.Default, L("Permission:Products"));
        productPermission.AddChild(BishalAgroSeedPermissions.Products.Create, L("Permissions:Products.Create"));
        productPermission.AddChild(BishalAgroSeedPermissions.Products.Edit, L("Permissions:Products.Edit"));
        productPermission.AddChild(BishalAgroSeedPermissions.Products.Delete, L("Permissions:Products.Delete"));

    }
    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<BishalAgroSeedResource>(name);
    }
}
