using BishalAgroSeed.Constants;
using BishalAgroSeed.Containers;
using BishalAgroSeed.CycleCountDetails;
using BishalAgroSeed.Dtos;
using BishalAgroSeed.FileUploadExtensions;
using BishalAgroSeed.NumberGenerations;
using BishalAgroSeed.Options;
using BishalAgroSeed.Permissions;
using BishalAgroSeed.Products;
using BishalAgroSeed.TransactionDetails;
using BishalAgroSeed.Transactions;
using BishalAgroSeed.TranscationTypes;
using ExcelMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace BishalAgroSeed.CycleCounts;
[Authorize(BishalAgroSeedPermissions.CycleCounts.Default)]
public class CycleCountAppService : ApplicationService, ICycleCountAppService
{
    private readonly IRepository<NumberGeneration, Guid> _numberGenerationRepository;
    private readonly IRepository<CycleCount, Guid> _cycleCountRepository;
    private readonly IRepository<CycleCountDetail, Guid> _cycleCountDetailRepository;
    private readonly IRepository<Product, Guid> _productRepository;
    private readonly IRepository<Transaction, Guid> _transactionRepository;
    private readonly IRepository<TransactionType, Guid> _transactionTypeRepository;
    private readonly IRepository<TransactionDetail, Guid> _transactionDetailRepository;
    private readonly IRepository<IdentityUser, Guid> _identityUserRepository;
    private readonly ILogger<CycleCountAppService> _logger;
    private readonly IBlobContainer<TemplateFileContainer> _templateFileContainer;
    private readonly BulkUploadCycleCountOption _bulkUploadCycleCountOption;

    public CycleCountAppService(
        IRepository<NumberGeneration, Guid> numberGenerationRepository,
        IRepository<CycleCount, Guid> cycleCountRepository,
        IRepository<CycleCountDetail, Guid> cycleCountDetailRepository,
        IRepository<Product, Guid> productRepository,
        IRepository<Transaction, Guid> transactionRepository,
        IRepository<TransactionType, Guid> transactionTypeRepository,
        IRepository<TransactionDetail, Guid> transactionDetailRepository,
        IRepository<IdentityUser, Guid> identityUserRepository,
        ILogger<CycleCountAppService> logger,
        IBlobContainer<TemplateFileContainer> templateFileContainer,
        IOptions<BulkUploadCycleCountOption> bulkUploadCycleCountOption)

    {
        _numberGenerationRepository = numberGenerationRepository;
        _cycleCountRepository = cycleCountRepository;
        _cycleCountDetailRepository = cycleCountDetailRepository;
        _productRepository = productRepository;
        _transactionRepository = transactionRepository;
        _transactionTypeRepository = transactionTypeRepository;
        _transactionDetailRepository = transactionDetailRepository;
        _identityUserRepository = identityUserRepository;
        _logger = logger;
        _templateFileContainer = templateFileContainer;
        _bulkUploadCycleCountOption = bulkUploadCycleCountOption.Value;
    }

    [Authorize(BishalAgroSeedPermissions.CycleCounts.Close)]
    public async Task CloseAsync(Guid id)
    {
        _logger.LogInformation($"CycleCountAppService.CloseAsync - Started");
        var cycleCount = await _cycleCountRepository.FirstOrDefaultAsync(s => s.Id == id);
        if (cycleCount == null)
        {
            var msg = "Cycle Count Not Found !!";
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
                new  ValidationResult(msg, new [] {"id"})
            });
        }

        cycleCount.IsClosed = true;
        cycleCount.ClosedDate = DateTime.Now;
        cycleCount.ClosedBy = CurrentUser.Id;
        await _cycleCountRepository.UpdateAsync(cycleCount);
        _logger.LogInformation($"CycleCountAppService.CloseAsync - Updated CycleCount entity with Id : {cycleCount.Id}");

        _logger.LogInformation($"CycleCountAppService.CloseAsync - Ended");
    }

    [Authorize(BishalAgroSeedPermissions.CycleCounts.Create)]
    public async Task CreateAsync()
    {
        _logger.LogInformation($"CycleCountAppService.CreateAsync - Started");

        var numberGeneration = await _numberGenerationRepository.FirstOrDefaultAsync(s => s.NumberGenerationTypeId == NumberGenerationTypes.NumberGenerationTypes.CycleCount);
        if (numberGeneration == null)
        {
            var msg = "Cycle Count Number Generation is not setup!!";
            _logger.LogInformation($"CycleCountAppService.CreateAsync - Validation : {msg}");
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
                new  ValidationResult(msg, new [] {"numberGeneration"})
            });
        }

        ++numberGeneration.Number;
        var cciNumber = $"{numberGeneration.Prefix}{numberGeneration.Number}{numberGeneration.Suffix}";
        await _numberGenerationRepository.UpdateAsync(numberGeneration);
        _logger.LogInformation($"CycleCountAppService.CreateAsync - Updated NumberGeneration entity with Id : {numberGeneration.Id}");

        var cycleCount = new CycleCount
        {
            CCINumber = cciNumber
        };
        await _cycleCountRepository.InsertAsync(cycleCount);
        _logger.LogInformation($"CycleCountAppService.CreateAsync - Inserted CycleCount entity with Id : {cycleCount.Id}");

        var _transactions = await _transactionRepository.GetQueryableAsync();
        var _transactionTypes = await _transactionTypeRepository.GetQueryableAsync();
        var _transactionDetails = await _transactionDetailRepository.GetQueryableAsync();

        var cycleCountDetails = (
                                     from t in _transactions
                                     join tt in _transactionTypes on t.TransactionTypeId equals tt.Id
                                     join td in _transactionDetails on t.TransactionTypeId equals td.Id
                                     where tt.DisplayName == Constants.TransactionTypes.PURCHASE || tt.DisplayName == Constants.TransactionTypes.SALES
                                     group new { TransactionType = tt.DisplayName, td.Quantity } by new { td.ProductId } into g
                                     select new CycleCountDetail
                                     {
                                         ProductId = g.Key.ProductId,
                                         CycleCountId = cycleCount.Id,
                                         SystemQuantity = g.Where(s => s.TransactionType == Constants.TransactionTypes.PURCHASE).Sum(s => s.Quantity) -
                                                          g.Where(s => s.TransactionType == Constants.TransactionTypes.SALES).Sum(s => s.Quantity)
                                     }).ToList();

        if (!cycleCountDetails.Any())
        {
            var msg = "Empty Inventory!!";
            _logger.LogInformation($"CycleCountAppService.CreateAsync - Validation : {msg}");
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
                new  ValidationResult(msg, new [] {"productId"})
            });
        }

        await _cycleCountDetailRepository.InsertManyAsync(cycleCountDetails);
        _logger.LogInformation($"CycleCountAppService.CreateAsync - Bulk Inserted CycleCountDetail entity with CycleCountId : {cycleCount.Id}");

        _logger.LogInformation($"CycleCountAppService.CreateAsync - Ended");
    }

    [Authorize(BishalAgroSeedPermissions.CycleCounts.Default)]
    public async Task<CycleCountDto> GetAsync(Guid id)
    {
        _logger.LogInformation($"CycleCountAppService.GetAsync - Started");

        var _cycleCounts = await _cycleCountRepository.GetQueryableAsync();
        var _users = await _identityUserRepository.GetQueryableAsync();
        var cycleCount = (
                            from cc in _cycleCounts
                            join u in _users on cc.ClosedBy equals u.Id into ug
                            from uj in ug.DefaultIfEmpty()
                            where cc.Id == id
                            select new CycleCountDto
                            {
                                CCINumber = cc.CCINumber,
                                IsClosed = cc.IsClosed,
                                ClosedDate = cc.ClosedDate,
                                ClosedByName = uj == null ? null : uj.Name + " " + uj.Surname,
                                CreationTime = cc.CreationTime
                            }).FirstOrDefault();

        _logger.LogInformation($"CycleCountAppService.GetAsync - Ended");
        return cycleCount;
    }

    [Authorize(BishalAgroSeedPermissions.CycleCounts.Default)]
    public async Task<PagedResultDto<CycleCountDto>> GetListByFilterAsync(PagedAndSortedResultRequestDto input, CycleCountFilter filter)
    {
        _logger.LogInformation($"CycleCountAppService.GetListByFilterAsync - Started");

        // Trim
        filter.CCINumber = filter.CCINumber?.Trim()?.ToLower();
        filter.ClosedByName = filter.ClosedByName?.Trim()?.ToLower();

        if (string.IsNullOrWhiteSpace(input.Sorting))
        {
            input.Sorting = $"CreationTime desc";
        }

        var cycleCounts = await _cycleCountRepository.GetQueryableAsync();
        var users = await _identityUserRepository.GetQueryableAsync();
        var queryable = (from cc in cycleCounts
                         join u in users on cc.ClosedBy equals u.Id into ug
                         from uj in ug.DefaultIfEmpty()
                         select new CycleCountDto
                         {
                             CCINumber = cc.CCINumber,
                             IsClosed = cc.IsClosed,
                             ClosedDate = cc.ClosedDate,
                             ClosedByName = uj == null ? null : uj.Name + " " + uj.Surname,
                             CreationTime = cc.CreationTime
                         })
                         .WhereIf(!string.IsNullOrWhiteSpace(filter.CCINumber), s => s.CCINumber.ToLower().Contains(filter.CCINumber))
                         .WhereIf(filter.IsClosed.HasValue, s => s.IsClosed == filter.IsClosed)
                         .WhereIf(filter.ClosedFromDate.HasValue, s => s.ClosedDate.HasValue && s.ClosedDate.Value.Date >= filter.ClosedFromDate.Value.Date)
                         .WhereIf(filter.ClosedToDate.HasValue, s => s.ClosedDate.HasValue && s.ClosedDate.Value.Date <= filter.ClosedToDate.Value.Date)
                         .WhereIf(!string.IsNullOrWhiteSpace(filter.ClosedByName), s => s.ClosedByName.ToLower().Contains(filter.ClosedByName))
                         .WhereIf(filter.OpenedFromDate.HasValue, s => s.CreationTime.Date >= filter.OpenedFromDate.Value.Date)
                         .WhereIf(filter.OpenedToDate.HasValue, s => s.CreationTime.Date <= filter.OpenedToDate.Value.Date);

        var totalCount = queryable.Count();
        var data = queryable.Skip(input.SkipCount).Take(input.MaxResultCount).OrderBy(input.Sorting).ToList();
        _logger.LogInformation($"CycleCountAppService.GetListByFilterAsync - Ended");

        return new PagedResultDto<CycleCountDto>(totalCount, data);
    }

    [Authorize(BishalAgroSeedPermissions.CycleCounts.Default)]
    public async Task<PagedResultDto<CycleCountDetailDto>> GetCycleCountDetailListByFilterAsync(PagedAndSortedResultRequestDto input, CycleCountDetailFilter filter)
    {
        _logger.LogInformation($"CycleCountAppService.GetCycleCountDetailListByFilterAsync - Started");

        // Trim
        filter.ProductName = filter.ProductName?.Trim()?.ToLower();
        filter.Remarks = filter.Remarks?.Trim()?.ToLower();

        if (string.IsNullOrWhiteSpace(input.Sorting))
        {
            input.Sorting = $"CreationTime desc";
        }

        var cycleCountDetails = await _cycleCountDetailRepository.GetQueryableAsync();
        var products = await _productRepository.GetQueryableAsync();
        var queryable = (from cc in cycleCountDetails
                         join p in products on cc.ProductId equals p.Id
                         where cc.Id == filter.CycleCountId
                         select new CycleCountDetailDto
                         {
                             Id = cc.Id,
                             ProductId = cc.ProductId,
                             ProductName = p.DisplayName,
                             CycleCountId = cc.CycleCountId,
                             SystemQuantity = cc.SystemQuantity,
                             PhysicalQuantity = cc.PhysicalQuantity,
                             Remarks = cc.Remarks
                         })
                         .WhereIf(!string.IsNullOrWhiteSpace(filter.ProductName), s => s.ProductName.ToLower().Contains(filter.ProductName))
                         .WhereIf(!string.IsNullOrWhiteSpace(filter.Remarks), s => s.Remarks.ToLower().Contains(filter.Remarks));

        var totalCount = queryable.Count();
        var data = queryable.Skip(input.SkipCount).Take(input.MaxResultCount).OrderBy(input.Sorting).ToList();
        _logger.LogInformation($"CycleCountAppService.GetCycleCountDetailListByFilterAsync - Ended");

        return new PagedResultDto<CycleCountDetailDto>(totalCount, data);
    }

    [Authorize(BishalAgroSeedPermissions.CycleCounts.Edit)]
    public async Task BulkUpdateCycleCountDetailAsync([Required] Guid cycleCountId, [Required] List<UpdateCycleCountDetailDto> input)
    {
        _logger.LogInformation($"CycleCountAppService.BulkUpdateCycleCountDetailAsync - Started");

        await BulkUpdateCycleCountDetailDataAsync(cycleCountId, input);

        _logger.LogInformation($"CycleCountAppService.BulkUpdateCycleCountDetailAsync - Ended");
    }

    [Authorize(BishalAgroSeedPermissions.CycleCounts.Default)]
    [HttpGet]
    public async Task<FileBlobDto> DownloadBulkUpdateCycleCountDetailByExcelTemplateAsync()
    {
        _logger.LogInformation($"CycleCountAppService.DownloadBulkUpdateCycleCountDetailByExcelTemplateAsync - Started");
        if (!(await _templateFileContainer.ExistsAsync(Global.UPDATE_CYCLE_COUNT_TEMPLATE_FILE_NAME)))
        {
            var msg = "Template not found";
            _logger.LogInformation($"CycleCountAppService.DownloadBulkUpdateCycleCountDetailByExcelTemplateAsync - Validation : {msg}");
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
                new ValidationResult(msg, new [] {"fileName"})
            });
        }
        var content = await _templateFileContainer.GetAllBytesAsync(Global.UPDATE_CYCLE_COUNT_TEMPLATE_FILE_NAME);
        _logger.LogInformation($"CycleCountAppService.DownloadBulkUpdateCycleCountDetailByExcelTemplateAsync - Downloaded Bulk Update Cycle Count Detail");

        _logger.LogInformation($"CycleCountAppService.DownloadBulkUpdateCycleCountDetailByExcelTemplateAsync - Ended");
        return new FileBlobDto(content, Global.UPDATE_CYCLE_COUNT_TEMPLATE_FILE_NAME);
    }

    [Authorize(BishalAgroSeedPermissions.CycleCounts.Edit)]
    public async Task BulkUpdateCycleCountDetailByExcelAsync([Required][FromForm] UpdateCycleCountDetailFileDto input)
    {
        try
        {
            _logger.LogInformation($"CycleCountAppService.BulkUpdateCycleCountDetailByExcelAsync - Started");

            var fileExtension = new FileInfo(input.File.FileName).Extension;
            if (!BulkCycleCountUpdateFileExtension.Allowed.Any(s => string.Equals(s, fileExtension, StringComparison.OrdinalIgnoreCase)))
            {
                var msg = $"{fileExtension} Unsupported File Extension.";
                _logger.LogInformation($"CycleCountAppService.BulkUpdateCycleCountDetailByExcelAsync - Validation : {msg}");
                throw new AbpValidationException(msg, new List<ValidationResult>()
                {
                    new  ValidationResult(msg, new [] {"fileName"})
                });
            }

            if (input.File.Length > (_bulkUploadCycleCountOption.FileSizeLimitInKB * 1024))
            {
                var msg = "File Size Exceeded.";
                _logger.LogInformation($"CycleCountAppService.BulkUpdateCycleCountDetailByExcelAsync - Validation : {msg}");
                throw new AbpValidationException(msg, new List<ValidationResult>()
                {
                    new  ValidationResult(msg, new [] {"size"})
                });
            }

            using var stream = new MemoryStream();
            await input.File.CopyToAsync(stream);
            stream.Position = 0;

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using var importer = new ExcelImporter(stream);
            ExcelSheet sheet = importer.ReadSheet();
            var cycleCounts = sheet.ReadRows<UpdateCycleCountDetailDto>()
                .Where(s => !string.IsNullOrWhiteSpace(s.ProductName));

            await BulkUpdateCycleCountDetailDataAsync(input.CycleCountId, cycleCounts.ToList(), isFileUpload: true);

            _logger.LogInformation($"CycleCountAppService.BulkUpdateCycleCountDetailByExcelAsync - Ended");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"CycleCountAppService.BulkUpdateCycleCountDetailByExcelAsync - Exception : {ex.ToString()}");
            throw;
        }
    }

    private static CycleCountDetail UpdateCycleCountDetailAsync(CycleCountDetail ccd, UpdateCycleCountDetailDto inp)
    {
        ccd.PhysicalQuantity = inp.PhysicalQuantity;
        ccd.Remarks = inp.Remarks;
        return ccd;
    }

    private async Task BulkUpdateCycleCountDetailDataAsync([Required] Guid cycleCountId, [Required] List<UpdateCycleCountDetailDto> input, bool isFileUpload = false)
    {
        _logger.LogInformation($"CycleCountAppService.BulkUpdateCycleCountDetailAsync - Started");

        if (!(await _cycleCountRepository.AnyAsync(s => s.Id == cycleCountId)))
        {
            var msg = "Cycle Count not found.";
            _logger.LogInformation($"CycleCountAppService.BulkUpdateCycleCountDetailAsync - Validation : {msg}");
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
                new  ValidationResult(msg, new [] {"cycleCountId"})
            });
        }

        if (!input.Any())
        {
            var msg = "Cycle Count Detail Update data not found.";
            _logger.LogInformation($"CycleCountAppService.BulkUpdateCycleCountDetailAsync - Validation : {msg}");
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
                new  ValidationResult(msg, new [] {"cycleCountId"})
            });
        }

        // SN initialize 
        input = input.Select((item, index) =>
        new UpdateCycleCountDetailDto
        {
            SN = index + 1,
            Id = item.Id,
            ProductName = item.ProductName?.Trim(),
            PhysicalQuantityName = item.PhysicalQuantityName?.Trim(),
            PhysicalQuantity = item.PhysicalQuantity,
            Remarks = item.Remarks.Trim(),
        }).ToList();

        var productQueryable = await _productRepository.GetQueryableAsync();
        var products = productQueryable.Select(s => new
        {
            s.Id,
            ProductName = s.DisplayName
        }).ToList();
        List<ValidationResult> valResults = new List<ValidationResult>();
        var valTitle = "Bulk Update Cycle Count Detail Validations";

        if (isFileUpload)
        {
            input = (from s in input
                     join p in products on s.ProductName?.ToLower() equals p.ProductName?.ToLower() into pg
                     from pj in pg.DefaultIfEmpty()
                     select new UpdateCycleCountDetailDto
                     {
                         SN = s.SN,
                         Id = pj == null ? null : pj.Id,
                         ProductName = s.ProductName,
                         PhysicalQuantityName = s.PhysicalQuantityName,
                         PhysicalQuantity = int.TryParse(s.PhysicalQuantityName, out int physicalQuantity) ? physicalQuantity : null,
                         Remarks = s.Remarks
                     }).ToList();
        }

        if (isFileUpload)
        {
            // Product Name validation
            var valIdIsRequired = (from s in input
                                   where s.Id == null
                                   select new ValidationResult($"Row {s.SN} - {s.ProductName} Invalid Product Name.", new[] { "productName" })
                                   ).ToList();
            valResults.AddRange(valIdIsRequired);
        }
        else
        {
            // Id required validation
            var valIdIsRequired = (from s in input
                                   where s.Id == null
                                   select new ValidationResult($"Row {s.SN} - Id is required.", new[] { "id" })
                                   ).ToList();
            valResults.AddRange(valIdIsRequired);
        }

        // Physical Quantity required validation
        var valPhysicalQuantityIsRequired =
            (from s in input
             where (!isFileUpload && s.PhysicalQuantity == null) || (isFileUpload && string.IsNullOrWhiteSpace(s.PhysicalQuantityName))
             select new ValidationResult($"Row {s.SN} - Physical Quantity is required.", new[] { "physicalQuantity" })
            ).ToList();
        valResults.AddRange(valPhysicalQuantityIsRequired);

        // Invalid Physical Quantity validation
        var valPhysicalQuantityIsInvalid =
            (from s in input
             where (s.PhysicalQuantity != null && s.PhysicalQuantity < 0) || (s.PhysicalQuantity == null && !string.IsNullOrWhiteSpace(s.PhysicalQuantityName))
             select new ValidationResult($"Row {s.SN} - Physical Quantity is invalid.", new[] { "physicalQuantity" })
             ).ToList();
        valResults.AddRange(valPhysicalQuantityIsInvalid);

        if (valResults.Any())
        {
            _logger.LogInformation($"CycleCountAppService.BulkUpdateCycleCountDetailAsync - Throw Validation exception");
            throw new AbpValidationException(valTitle, valResults);
        }

        var cycleCountDetailQueryable = await _cycleCountDetailRepository.GetQueryableAsync();
        var cycleCountDetails = cycleCountDetailQueryable.Where(s => s.CycleCountId == cycleCountId).ToList();

        if (!cycleCountDetails.Any())
        {
            var msg = "Cycle Count Detail data not found.";
            _logger.LogInformation($"CycleCountAppService.BulkUpdateCycleCountDetailAsync - Validation : {msg}");
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
                new  ValidationResult(msg, new [] {"cycleCountDetailId"})
            });
        }

        var cycleCountDetailsUpdate = (from ccd in cycleCountDetails
                                       join inp in input on ccd.Id equals inp.Id
                                       select UpdateCycleCountDetailAsync(ccd, inp)).ToList();
        if (!cycleCountDetails.Any())
        {
            var msg = "Cycle Count Detail Update data not found.";
            _logger.LogInformation($"CycleCountAppService.BulkUpdateCycleCountDetailAsync - Validation : {msg}");
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
                new  ValidationResult(msg, new [] {"cycleCountDetailId"})
            });
        }
        await _cycleCountDetailRepository.UpdateManyAsync(cycleCountDetailsUpdate);
        _logger.LogInformation($"CycleCountAppService.BulkUpdateCycleCountDetailAsync - Bulk Updated Cycle Count Detail");

        _logger.LogInformation($"CycleCountAppService.BulkUpdateCycleCountDetailAsync - Ended");
    }
}