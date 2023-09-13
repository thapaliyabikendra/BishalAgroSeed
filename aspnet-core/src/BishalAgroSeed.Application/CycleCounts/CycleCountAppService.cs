using BishalAgroSeed.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace BishalAgroSeed.CycleCounts;
public class CycleCountAppService : ApplicationService, ICycleCountAppService
{
    private readonly IRepository<Brand, Guid> _repository;

    public CycleCountAppService(IRepository<Brand, Guid> repository)
    {
        _repository = repository;
    }

}
