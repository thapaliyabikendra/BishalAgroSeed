using BishalAgroSeed.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace BishalAgroSeed.Permissions;

public class BishalAgroSeedPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(BishalAgroSeedPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(BishalAgroSeedPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<BishalAgroSeedResource>(name);
    }
}
