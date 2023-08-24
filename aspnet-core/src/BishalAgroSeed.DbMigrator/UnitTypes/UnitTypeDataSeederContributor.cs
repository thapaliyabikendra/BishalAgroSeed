using BishalAgroSeed.Constants;
using BishalAgroSeed.Helpers;
using BishalAgroSeed.UnitTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
namespace BishalAgroSeed.DbMigrator.UnitTypes;
public class UnitTypeDataSeederContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<UnitType> _repository;

    public UnitTypeDataSeederContributor(IRepository<UnitType> repository)
    {
        _repository = repository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        var data = DataHelper.GetValues<Constants.UnitTypes>();
        foreach (var d in data)
        {
            var newUnitType = new UnitType()
            {
                DisplayName = d.value,
                Description = d.description
            };

            var unitType = await _repository.FirstOrDefaultAsync(s => String.Equals(s.DisplayName, newUnitType.DisplayName, StringComparison.OrdinalIgnoreCase));
            if (unitType == null)
            {
                await _repository.InsertAsync(newUnitType);
            }
        }
    }
}
