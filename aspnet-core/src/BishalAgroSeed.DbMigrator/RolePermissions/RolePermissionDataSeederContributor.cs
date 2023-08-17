using BishalAgroSeed.Constants;
using BishalAgroSeed.Permissions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace BishalAgroSeed.RolePermissions;
public class RolePermissionDataSeederContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IGuidGenerator _guidGenerator;
    private readonly IdentityRoleManager _identityRoleManager;
    private readonly IOptions<IdentityOptions> _identityOptions;
    private readonly IPermissionDataSeeder _permissionDataSeeder;

    public RolePermissionDataSeederContributor(
        IGuidGenerator guidGenerator,
        IdentityRoleManager identityRoleManager,
        IOptions<IdentityOptions> identityOptions,
        IPermissionDataSeeder permissionDataSeeder

        )
    {
        _guidGenerator = guidGenerator;
        _identityRoleManager = identityRoleManager;
        _identityOptions = identityOptions;
        _permissionDataSeeder = permissionDataSeeder;
    }

    [UnitOfWork]
    public async Task SeedAsync(DataSeedContext context)
    {
        await _identityOptions.SetAsync();
        var doesRoleExists = await _identityRoleManager.RoleExistsAsync(Roles.Operator);
        if (!doesRoleExists) {
            var role = new IdentityRole(_guidGenerator.Create(), Roles.Operator)
            {
                IsDefault = true,
                IsStatic = true,
            };
            await _identityRoleManager.CreateAsync(role);
        }

        var operatorRole = await _identityRoleManager.FindByNameAsync(Roles.Operator);
        if (operatorRole != null) {
            var permissionNames = new List<string>{
                BishalAgroSeedPermissions.Customers.Default,
                BishalAgroSeedPermissions.Customers.Create,
                BishalAgroSeedPermissions.Customers.Edit,
                BishalAgroSeedPermissions.Customers.Delete,
            };

            await _permissionDataSeeder.SeedAsync(
                RolePermissionValueProvider.ProviderName,
                Roles.Operator,
                permissionNames
                );
        }
    }
}
