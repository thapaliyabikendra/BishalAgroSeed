using BishalAgroSeed.Constants;
using BishalAgroSeed.Containers;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace BishalAgroSeed.DbMigrator.CycleCounts;
public class UpdateDateCountTemplateDataSeeder : IDataSeedContributor, ITransientDependency
{
    private readonly IBlobContainer<TemplateFileContainer> _fileContainer;

    public UpdateDateCountTemplateDataSeeder(
        IBlobContainer<TemplateFileContainer> fileContainer
        )
    {
        _fileContainer = fileContainer;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        var path = Path.Combine(Environment.CurrentDirectory, "CycleCounts", Global.UPDATE_CYCLE_COUNT_TEMPLATE_FILE_NAME);
        if (File.Exists(path)) {
            using (var memoryStream = new MemoryStream()) {
                var file = File.OpenRead(path);
                await file.CopyToAsync(memoryStream);
                await _fileContainer.SaveAsync(Global.UPDATE_CYCLE_COUNT_TEMPLATE_FILE_NAME, memoryStream, true);
            }
        }
    }
}