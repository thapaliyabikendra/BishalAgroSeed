using BishalAgroSeed.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore;

namespace BishalAgroSeed.DbMigrator.DateTimes;
public class DateTimeDataSeederContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IDbContextProvider<BishalAgroSeedDbContext> _dbContextProvider;
    private readonly IRepository<BishalAgroSeed.DateTimes.DateTime> _repository;

    public DateTimeDataSeederContributor(IDbContextProvider<BishalAgroSeedDbContext> dbContextProvider,
        IRepository<BishalAgroSeed.DateTimes.DateTime> repository)
    {
        _dbContextProvider = dbContextProvider;
        _repository = repository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (!await _repository.AnyAsync())
        {
            var path = Path.Combine("Scripts", "DateTime.sql");
            if (File.Exists(path))
            {
                //var fileContent = File.ReadAllText(path);
                //if (!string.IsNullOrWhiteSpace(fileContent))
                //{
                //    var dbContext = await _dbContextProvider.GetDbContextAsync();
                //    await dbContext.Database.ExecuteSqlRawAsync(fileContent);
                //}
            }
        }
    }
}
