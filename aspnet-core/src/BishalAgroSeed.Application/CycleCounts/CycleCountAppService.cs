using BishalAgroSeed.Categories;
using BishalAgroSeed.Constants;
using BishalAgroSeed.Containers;
using BishalAgroSeed.CycleCountDetails;
using BishalAgroSeed.Dtos;
using BishalAgroSeed.FileUploadExtensions;
using BishalAgroSeed.NumberGenerations;
using BishalAgroSeed.Options;
using BishalAgroSeed.Permissions;
using BishalAgroSeed.Products;
using BishalAgroSeed.Services;
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
    private readonly IRepository<Category, Guid> _categoryRepository;
    private readonly IRepository<Transaction, Guid> _transactionRepository;
    private readonly IRepository<TransactionType, Guid> _transactionTypeRepository;
    private readonly IRepository<TransactionDetail, Guid> _transactionDetailRepository;
    private readonly IRepository<IdentityUser, Guid> _identityUserRepository;
    private readonly ILogger<CycleCountAppService> _logger;
    private readonly IBlobContainer<TemplateFileContainer> _templateFileContainer;
    private readonly IExcelService _excelService;
    private readonly BulkUploadCycleCountOption _bulkUploadCycleCountOption;
    private static readonly Dictionary<string, Func<CycleCountDetailDto, object>> _mapConfig = new()
    {
        { "CCI Number", item => item.CCINumber},
        { "Category", item => item.CategoryName},
        { "Product", item => item.ProductName},
        { "System Quantity", item => item.SystemQuantity},
        { "Physical Quantity", item => item.PhysicalQuantity},
        { "Remarks", item => item.Remarks},
    };
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
        IOptions<BulkUploadCycleCountOption> bulkUploadCycleCountOption,
        IExcelService excelService,
        IRepository<Category,Guid> categoryRepository)
    {
        _numberGenerationRepository = numberGenerationRepository;
        _cycleCountRepository = cycleCountRepository;
        _cycleCountDetailRepository = cycleCountDetailRepository;
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _transactionRepository = transactionRepository;
        _transactionTypeRepository = transactionTypeRepository;
        _transactionDetailRepository = transactionDetailRepository;
        _identityUserRepository = identityUserRepository;
        _logger = logger;
        _templateFileContainer = templateFileContainer;
        _excelService = excelService;
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
            _logger.LogInformation($"CycleCountAppService.CloseAsync - Validation : {msg}");
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
                new ValidationResult(msg, new [] {"id"})
            });
        }

        if (cycleCount.IsClosed) 
        {
            var msg = "Cycle Count is already closed !!";
            _logger.LogInformation($"CycleCountAppService.CloseAsync - Validation : {msg}");
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
               new ValidationResult(msg, new[] {"isClosed"})
            });
        }

        cycleCount.IsClosed = true;
        cycleCount.ClosedDate = DateTime.Now;
        cycleCount.ClosedBy = CurrentUser.Id;
        await _cycleCountRepository.UpdateAsync(cycleCount);
        _logger.LogInformation("CycleCountAppService.CloseAsync - Updated CycleCount entity with Id : {CycleCountId}", cycleCount.Id);

        _logger.LogInformation($"CycleCountAppService.CloseAsync - Ended");
    }

    [Authorize(BishalAgroSeedPermissions.CycleCounts.Create)]
    public async Task CreateAsync()
    {
        _logger.LogInformation($"CycleCountAppService.CreateAsync - Started");

        var numberGeneration = await _numberGenerationRepository.FirstOrDefaultAsync(s => s.NumberGenerationTypeId == NumberGenerationTypes.NumberGenerationTypes.CycleCount);
        if (numberGeneration == null)
        {
            var msg = "Cycle Count Number Generation is not setup !!";
            _logger.LogInformation($"CycleCountAppService.CreateAsync - Validation : {msg}");
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
                new  ValidationResult(msg, new [] {"numberGeneration"})
            });
        }

        if ((await _cycleCountRepository.AnyAsync(s => !s.IsClosed)))
        {
            var msg = "Open Cycle Count exists !!";
            _logger.LogInformation($"CycleCountAppService.CreateAsync - Validation : {msg}");
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
                new  ValidationResult(msg, new [] {"isClose"})
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
        var cycleCount = (
                            from cc in _cycleCounts
                            where cc.Id == id
                            select new CycleCountDto
                            {
                                CCINumber = cc.CCINumber,
                                IsClosed = cc.IsClosed,
                                ClosedAt = cc.ClosedDate,
                                RequestedAt = cc.CreationTime,
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
            input.Sorting = $"RequestedAt desc";
        }

        var cycleCounts = await _cycleCountRepository.GetQueryableAsync();
        var users = await _identityUserRepository.GetQueryableAsync();
        var queryable = (from cc in cycleCounts
                         join ur in users on cc.CreatorId equals ur.Id
                         join u in users on cc.ClosedBy equals u.Id into ug
                         from uj in ug.DefaultIfEmpty()
                         select new CycleCountDto
                         {
                             Id = cc.Id,
                             CCINumber = cc.CCINumber,
                             IsClosed = cc.IsClosed,
                             ClosedAt = cc.ClosedDate,
                             ClosedByName = uj == null ? "" : uj.Name + " " + uj.Surname,
                             RequestedAt = cc.CreationTime,
                             RequestedByName = ur.Name + " " + ur.Surname
                         })
                         .WhereIf(!string.IsNullOrWhiteSpace(filter.CCINumber), s => s.CCINumber.ToLower().Contains(filter.CCINumber))
                         .WhereIf(filter.IsClosed.HasValue, s => s.IsClosed == filter.IsClosed)
                         .WhereIf(filter.ClosedFromDate.HasValue, s => s.ClosedAt.HasValue && s.ClosedAt.Value.Date >= filter.ClosedFromDate.Value.Date)
                         .WhereIf(filter.ClosedToDate.HasValue, s => s.ClosedAt.HasValue && s.ClosedAt.Value.Date <= filter.ClosedToDate.Value.Date)
                         .WhereIf(!string.IsNullOrWhiteSpace(filter.ClosedByName), s => s.ClosedByName.ToLower().Contains(filter.ClosedByName))
                         .WhereIf(filter.OpenedFromDate.HasValue, s => s.RequestedAt.Date >= filter.OpenedFromDate.Value.Date)
                         .WhereIf(filter.OpenedToDate.HasValue, s => s.RequestedAt.Date <= filter.OpenedToDate.Value.Date)
                         .WhereIf(!string.IsNullOrWhiteSpace(filter.RequestedByName), s => s.RequestedByName.ToLower().Contains(filter.RequestedByName));

        var totalCount = queryable.Count();
        var data = queryable.Skip(input.SkipCount).Take(input.MaxResultCount).OrderBy(input.Sorting).ToList();
        _logger.LogInformation($"CycleCountAppService.GetListByFilterAsync - Ended");

        return new PagedResultDto<CycleCountDto>(totalCount, data);
    }

    [Authorize(BishalAgroSeedPermissions.CycleCounts.Default)]
    public async Task<PagedResultDto<CycleCountDetailDto>> GetCycleCountDetailListByFilterAsync(PagedAndSortedResultRequestDto input, CycleCountDetailFilter filter)
    {
        try
        {
            _logger.LogInformation($"CycleCountAppService.GetCycleCountDetailListByFilterAsync - Started");

            if (string.IsNullOrWhiteSpace(input.Sorting))
            {
                input.Sorting = $"CreationTime desc";
            }

            var queryable = await GetCycleCountDetailDataByFilterAsync(filter);

            var totalCount = queryable.Count();
            var data = queryable.OrderBy(input.Sorting).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            _logger.LogInformation($"CycleCountAppService.GetCycleCountDetailListByFilterAsync - Ended");
            return new PagedResultDto<CycleCountDetailDto>(totalCount, data);
        }
        catch (Exception ex) {
            _logger.LogInformation($"CycleCountAppService.GetCycleCountDetailListByFilterAsync - Exception : {ex}");
            throw;
        }
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
        var fileName = string.Format(Global.UPDATE_CYCLE_COUNT_TEMPLATE_FILE_NAME, "");
        _logger.LogInformation($"CycleCountAppService.DownloadBulkUpdateCycleCountDetailByExcelTemplateAsync - Started");
        if (!(await _templateFileContainer.ExistsAsync(fileName)))
        {
            var msg = "Template not found";
            _logger.LogInformation("CycleCountAppService.DownloadBulkUpdateCycleCountDetailByExcelTemplateAsync - Validation : {Msg}", msg);
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
                new ValidationResult(msg, new [] {"fileName"})
            });
        }
        var content = await _templateFileContainer.GetAllBytesAsync(fileName);
        _logger.LogInformation($"CycleCountAppService.DownloadBulkUpdateCycleCountDetailByExcelTemplateAsync - Downloaded Bulk Update Cycle Count Detail");

        _logger.LogInformation($"CycleCountAppService.DownloadBulkUpdateCycleCountDetailByExcelTemplateAsync - Ended");
        return new FileBlobDto(content, fileName);
    }

    [Authorize(BishalAgroSeedPermissions.CycleCounts.Default)]
    [HttpGet]
    public async Task<FileBlobDto> ExportCycleCountDetailExcelAsync(CycleCountDetailFilter filter)
    {
        try
        {
            _logger.LogInformation($"CycleCountAppService.ExportCycleCountDetailExcelAsync - Started");

            var cycleCount = await _cycleCountRepository.FirstOrDefaultAsync(s => s.Id == filter.CycleCountId);
            if (cycleCount == null)
            {
                var msg = "Cycle Count not found.";
                _logger.LogInformation($"CycleCountAppService.ExportCycleCountDetailExcelAsync - Validation : {msg}");
                throw new AbpValidationException(msg, new List<ValidationResult>()
                {
                    new  ValidationResult(msg, new [] {"cycleCountId"})
                });
            }
            var data = await GetCycleCountDetailDataByFilterAsync(filter);

            var content = await _excelService.ExportAsync(data.ToList(), _mapConfig);
            var fileName = string.Format(Global.UPDATE_CYCLE_COUNT_TEMPLATE_FILE_NAME, $"_{cycleCount.CCINumber}_{DateTime.Now:yyyy/MM/dd HH:mm}");

            _logger.LogInformation($"CycleCountAppService.ExportCycleCountDetailExcelAsync - Ended");
            return new FileBlobDto(content, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"CycleCountAppService.ExportCycleCountDetailExcelAsync - Exception : {ex}");
            throw;
        }
    }

    [Authorize(BishalAgroSeedPermissions.CycleCounts.Edit)]
    public async Task BulkUpdateCycleCountDetailByExcelAsync([Required][FromForm] UpdateCycleCountDetailFileDto input)
    {
        try
        {
            _logger.LogInformation($"CycleCountAppService.BulkUpdateCycleCountDetailByExcelAsync - Started");

            var fileExtension = new FileInfo(input.File.FileName).Extension;
            if (!Array.Exists(BulkCycleCountUpdateFileExtension.Allowed, s => string.Equals(s, fileExtension, StringComparison.OrdinalIgnoreCase)))
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
                .Where(s => !string.IsNullOrWhiteSpace(s.ProductName)).ToList();

            await BulkUpdateCycleCountDetailDataAsync(input.CycleCountId, cycleCounts, isFileUpload: true);

            _logger.LogInformation($"CycleCountAppService.BulkUpdateCycleCountDetailByExcelAsync - Ended");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"CycleCountAppService.BulkUpdateCycleCountDetailByExcelAsync - Exception : {ex}");
            throw;
        }
    }

    private static CycleCountDetail UpdateCycleCountDetailAsync(CycleCountDetail ccd, UpdateCycleCountDetailDto inp)
    {
        ccd.PhysicalQuantity = inp.PhysicalQuantity;
        ccd.Remarks = inp.Remarks;
        return ccd;
    }

    private async Task BulkUpdateCycleCountDetailDataAsync( Guid cycleCountId, List<UpdateCycleCountDetailDto> input, bool isFileUpload = false)
    {
        _logger.LogInformation($"CycleCountAppService.BulkUpdateCycleCountDetailAsync - Started");

        var cycleCountQueryable = await _cycleCountRepository.GetQueryableAsync();
        var cycleCount = cycleCountQueryable
        .Select(s => new
        {
            s.Id,
            s.IsClosed
        }).FirstOrDefault(s => s.Id == cycleCountId);

        if (cycleCount == null)
        {
            var msg = "Cycle Count not found.";
            _logger.LogInformation($"CycleCountAppService.BulkUpdateCycleCountDetailAsync - Validation : {msg}");
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
                new  ValidationResult(msg, new [] {"cycleCountId"})
            });
        }

        if (cycleCount.IsClosed)
        {
            var msg = "Cycle Count already closed !!";
            _logger.LogInformation($"CycleCountAppService.CreateAsync - Validation : {msg}");
            throw new AbpValidationException(msg, new List<ValidationResult>()
            {
                new  ValidationResult(msg, new [] {"isClose"})
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
            Remarks = item.Remarks?.Trim(),
        }).ToList();

        var productQueryable = await _productRepository.GetQueryableAsync();
        var products = productQueryable.Select(s => new
        {
            s.Id,
            ProductName = s.DisplayName
        }).ToList();

        var valResults = new List<ValidationResult>();
        var valTitle = "Bulk Update Cycle Count Detail Validations";

        if (isFileUpload)
        {
            input = (from s in input
                     join p in products on s.ProductName?.ToLower() equals p.ProductName?.ToLower() into pg
                     from pj in pg.DefaultIfEmpty()
                     select new UpdateCycleCountDetailDto
                     {
                         SN = s.SN,
                         Id = pj?.Id,
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
            if (valIdIsRequired.Any())
            {
                valResults.AddRange(valIdIsRequired);
                _logger.LogInformation($"CycleCountAppService.BulkUpdateCycleCountDetailAsync - Validation : Invalid Product Name");
            }
        }
        else
        {
            // Id required validation
            var valIdIsRequired = (from s in input
                                   where s.Id == null
                                   select new ValidationResult($"Row {s.SN} - Id is required.", new[] { "id" })
                                   ).ToList();
            if (valIdIsRequired.Any())
            {
                valResults.AddRange(valIdIsRequired);
                _logger.LogInformation($"CycleCountAppService.BulkUpdateCycleCountDetailAsync - Validation : Id is required");
            }
        }

        // Physical Quantity required validation
        var valPhysicalQuantityIsRequired =
            (from s in input
             where (!isFileUpload && s.PhysicalQuantity == null) || (isFileUpload && string.IsNullOrWhiteSpace(s.PhysicalQuantityName))
             select new ValidationResult($"Row {s.SN} - Physical Quantity is required.", new[] { "physicalQuantity" })
            ).ToList();
        if (valPhysicalQuantityIsRequired.Any())
        {
            valResults.AddRange(valPhysicalQuantityIsRequired);
            _logger.LogInformation($"CycleCountAppService.BulkUpdateCycleCountDetailAsync - Validation : Physical Quantity is required");
        }

        // Invalid Physical Quantity validation
        var valPhysicalQuantityIsInvalid =
            (from s in input
             where (s.PhysicalQuantity != null && s.PhysicalQuantity < 0) || (s.PhysicalQuantity == null && !string.IsNullOrWhiteSpace(s.PhysicalQuantityName))
             select new ValidationResult($"Row {s.SN} - Physical Quantity is invalid.", new[] { "physicalQuantity" })
             ).ToList();
        if (valPhysicalQuantityIsInvalid.Any())
        {
            valResults.AddRange(valPhysicalQuantityIsInvalid);
            _logger.LogInformation($"CycleCountAppService.BulkUpdateCycleCountDetailAsync - Validation : Physical Quantity is invalid");
        }
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
                                       join inp in input on ccd.ProductId equals inp.Id
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

    private async Task<IQueryable<CycleCountDetailDto>> GetCycleCountDetailDataByFilterAsync(CycleCountDetailFilter filter)
    {
        try
        {
            _logger.LogInformation($"CycleCountAppService.GetCycleCountDetailDataByFilterAsync - Started");

            // Trim
            filter.ProductName = filter.ProductName?.Trim()?.ToLower();
            filter.Remarks = filter.Remarks?.Trim()?.ToLower();

            var products = await _productRepository.GetQueryableAsync();
            var cycleCounts = await _cycleCountRepository.GetQueryableAsync();
            var cycleCountDetails = await _cycleCountDetailRepository.GetQueryableAsync();
            var categories = await _categoryRepository.GetQueryableAsync();

            var queryable = (from c in cycleCounts
                             join cc in cycleCountDetails on c.Id equals cc.CycleCountId
                             join p in products on cc.ProductId equals p.Id
                             join ct in categories on p.CategoryId equals ct.Id
                             where cc.CycleCountId == filter.CycleCountId
                             select new CycleCountDetailDto
                             {
                                 Id = cc.Id,
                                 ProductId = cc.ProductId,
                                 CCINumber = c.CCINumber,
                                 CategoryName = ct.DisplayName,
                                 ProductName = p.DisplayName,
                                 SystemQuantity = cc.SystemQuantity,
                                 PhysicalQuantity = cc.PhysicalQuantity,
                                 Remarks = cc.Remarks,
                                 CreationTime = cc.CreationTime,
                             })
                             .WhereIf(!string.IsNullOrWhiteSpace(filter.CategoryName), s => s.CategoryName.ToLower().Contains(filter.CategoryName))
                             .WhereIf(!string.IsNullOrWhiteSpace(filter.ProductName), s => s.ProductName.ToLower().Contains(filter.ProductName))
                             .WhereIf(!string.IsNullOrWhiteSpace(filter.Remarks), s => s.Remarks.ToLower().Contains(filter.Remarks))
                             .OrderBy(s => s.ProductName);

            _logger.LogInformation($"CycleCountAppService.GetCycleCountDetailDataByFilterAsync - Ended");
            return queryable;
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"CycleCountAppService.GetCycleCountDetailDataByFilterAsync - Exception : {ex}");
            throw;
        }
    }
}