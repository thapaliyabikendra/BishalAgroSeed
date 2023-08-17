﻿using BishalAgroSeed.Localization;
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
        customerPermission.AddChild(BishalAgroSeedPermissions.Customers.Delete,L("Permission:Customers.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<BishalAgroSeedResource>(name);
    }
}