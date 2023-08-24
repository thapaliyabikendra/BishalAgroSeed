using BishalAgroSeed.Configurations;
using BishalAgroSeed.Helpers;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace BishalAgroSeed.DbMigrator.Configurations;
public class ConfigurationDataSeederContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Configuration> _repository;

    public ConfigurationDataSeederContributor(IRepository<Configuration> repository)
    {
        _repository = repository;
    }
    public async Task SeedAsync(DataSeedContext context)
    {
        var data = DataHelper.GetValues<Constants.Configurations>();
        foreach (var d in data)
        {
            var newConfiguration = new Configuration()
            {
                Key = d.value,
                Value = d.description
            };
            var configuration = await _repository.FirstOrDefaultAsync(s => String.Equals(s.Key, newConfiguration.Key, StringComparison.OrdinalIgnoreCase));
            if (configuration == null)
            {
                await _repository.InsertAsync(newConfiguration);
            }
        }
    }
}
