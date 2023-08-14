using BishalAgroSeed.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace BishalAgroSeed.Configurations;
[Authorize(BishalAgroSeedPermissions.Configurations.Default)]
public class ConfigurationAppService : CrudAppService<Configuration, ConfigurationDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateConfigurationDto>, IConfigurationAppService
{
    public ConfigurationAppService(IRepository<Configuration, Guid> repository) : base(repository)
    {
        GetPolicyName = BishalAgroSeedPermissions.Configurations.Default;
        GetListPolicyName = BishalAgroSeedPermissions.Configurations.Default;
        CreatePolicyName = BishalAgroSeedPermissions.Configurations.Create;
        UpdatePolicyName = BishalAgroSeedPermissions.Configurations.Edit;
        DeletePolicyName = BishalAgroSeedPermissions.Configurations.Delete;
    }
}
