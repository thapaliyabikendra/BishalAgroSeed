using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BishalAgroSeed.Configurations;
public interface IConfigurationAppService: ICrudAppService<ConfigurationDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateConfigurationDto>
{
}
