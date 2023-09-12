using BishalAgroSeed.NumberGenerations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace BishalAgroSeed.DbMigrator.NumberGenerations;
public class NumberGenerationDataSeederContributor : IDataSeedContributor, ITransientDependency
{
    private readonly NumberGeneration _numberGeneration;
    private readonly IRepository<NumberGeneration> _repository;

    public NumberGenerationDataSeederContributor
        (
        IOptions<NumberGeneration> options,
        IRepository<NumberGeneration> repository
        )
    {
        _numberGeneration = options.Value;
        _repository = repository;
    }
    public async Task SeedAsync(DataSeedContext context)
    {
        if (!(await _repository.AnyAsync(s => s.NumberGenerationTypeId == _numberGeneration.NumberGenerationTypeId))) {
            await _repository.InsertAsync(_numberGeneration);
        }
    }
}
