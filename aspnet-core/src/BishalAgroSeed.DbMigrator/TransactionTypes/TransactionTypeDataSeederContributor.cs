using BishalAgroSeed.Helpers;
using BishalAgroSeed.TranscationTypes;
using BishalAgroSeed.UnitTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace BishalAgroSeed.DbMigrator.TransactionTypes;
public class TransactionTypeDataSeederContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<TransactionType> _repository;

    public TransactionTypeDataSeederContributor(IRepository<TransactionType> repository)
    {
        _repository = repository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        var data = DataHelper.GetValues<Constants.TransactionTypes>();
        foreach (var d in data)
        {
            var newTransactionType = new TransactionType()
            {
                DisplayName = d.value,
                Description = d.description
            };

            var transactionType = await _repository.FirstOrDefaultAsync(s => s.DisplayName == newTransactionType.DisplayName);
            if (transactionType == null)
            {
                await _repository.InsertAsync(newTransactionType);
            }
        }
    }
}