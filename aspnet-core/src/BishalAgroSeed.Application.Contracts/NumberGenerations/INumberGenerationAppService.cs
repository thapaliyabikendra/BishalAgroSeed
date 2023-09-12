using BishalAgroSeed.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BishalAgroSeed.NumberGenerations;
public interface INumberGenerationAppService : ICrudAppService<NumberGenerationDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateNumberGenerationDto>
{
    public Task<List<DropdownDto>> GetNumberGenerationTypesAsync();
}
