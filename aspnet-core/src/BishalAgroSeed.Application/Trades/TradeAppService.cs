using BishalAgroSeed.Constants;
using BishalAgroSeed.Customers;
using BishalAgroSeed.Dtos;
using BishalAgroSeed.Permissions;
using BishalAgroSeed.Products;
using BishalAgroSeed.Services;
using BishalAgroSeed.TransactionDetails;
using BishalAgroSeed.Transactions;
using BishalAgroSeed.TranscationTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using OfficeOpenXml.Drawing.Chart.ChartEx;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;
using Volo.Abp.Validation;

namespace BishalAgroSeed.Trades;
[Authorize(BishalAgroSeedPermissions.Trades.Default)]
public class TradeAppService : ApplicationService, ITradeAppService
{
    private readonly IRepository<Transaction, Guid> _transactionRepository;
    private readonly IRepository<TransactionDetail, Guid> _transactionDetailRepository;
    private readonly IRepository<Product, Guid> _productRepository;
    private readonly IRepository<Customer, Guid> _customerRepository;
    private readonly ILogger<TradeAppService> _logger;
    private readonly IRepository<TransactionType, Guid> _transactionTypeRepository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IExcelService _excelService;
    private readonly Dictionary<string, Func<TradeDto, object>> _mapconfig = new()
    {
        { "Trade Type", item => item.TradeTypeName },
        { "Customer", item => item.CustomerName },
        { "Discount Amount", item => item.DiscountAmount },
        { "Transport Charge", item => item.TransportCharge },
        { "Voucher No", item => item.VoucherNo },
        { "Tran Date", item => item.TranDate },
        { "Total Amount", item => item.Amount }
    };

    public TradeAppService(
      IRepository<Transaction, Guid> transactionRepository,
      IRepository<TransactionDetail, Guid> transactionDetailRepository,
      IRepository<Product, Guid> productRepository,
      IRepository<Customer, Guid> customerRepository,
       ILogger<TradeAppService> logger,
        IRepository<TransactionType, Guid> transactionTypeRepository,
        IUnitOfWorkManager unitOfWorkManager,
        IExcelService excelService)
    {
        _transactionRepository = transactionRepository;
        _transactionDetailRepository = transactionDetailRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _logger = logger;
        _transactionTypeRepository = transactionTypeRepository;
        _unitOfWorkManager = unitOfWorkManager;
        _excelService = excelService;
    }

    [Authorize(BishalAgroSeedPermissions.Trades.Create)]
    [HttpPost]
    public async Task CreateAsync(CreateTransactionDto input)
    {
        try
        {
            _logger.LogInformation($"TradeAppService.CreateAsync - Started");

            // Trim
            input.VoucherNo = input.VoucherNo?.Trim();

            #region Validations
            var valResults = new List<ValidationResult>();
            var valTitle = "Save Trade Validations";

            var isCustomerValidTask = _customerRepository.AnyAsync(s => s.Id == input.CustomerId);

            if (input.CustomerId == null)
            {
                var msg = $"Customer is required !!";
                valResults.Add(new ValidationResult(msg, new[] { "customerId" }));
                _logger.LogInformation($"TradeAppService.CreateAsync - Validation : {msg}");
            }

            if (input.Amount == null)
            {
                var msg = $"Amount is required !!";
                valResults.Add(new ValidationResult(msg, new[] { "amount" }));
                _logger.LogInformation($"TradeAppService.CreateAsync - Validation : {msg}");
            }

            if (input.Amount < 0)
            {
                var msg = $"Invalid Amount !!";
                valResults.Add(new ValidationResult(msg, new[] { "amount" }));
                _logger.LogInformation($"TradeAppService.CreateAsync - Validation : {msg}");
            }

            if (input.TransactionTypeId == null)
            {
                var msg = $"Transaction Type is required !!";
                valResults.Add(new ValidationResult(msg, new[] { "transactionTypeId" }));
                _logger.LogInformation($"TradeAppService.CreateAsync - Validation : {msg}");
            }

            if (input.TransportCharge < 0)
            {
                var msg = $"Invalid Transport Charge !!";
                valResults.Add(new ValidationResult(msg, new[] { "transportCharge" }));
                _logger.LogInformation($"TradeAppService.CreateAsync - Validation : {msg}");
            }

            var isCustomerValid = await isCustomerValidTask;
            var isTransactionTypeValidTask = _transactionTypeRepository.AnyAsync(s => s.Id == input.TransactionTypeId);

            if (input.DiscountAmount < 0)
            {
                var msg = $"Invalid Discount Amount !!";
                valResults.Add(new ValidationResult(msg, new[] { "discountCharge" }));
                _logger.LogInformation($"TradeAppService.CreateAsync - Validation : {msg}");
            }

            if (string.IsNullOrWhiteSpace(input.VoucherNo))
            {
                var msg = $"Voucher No is required !!";
                valResults.Add(new ValidationResult(msg, new[] { "voucherNo" }));
                _logger.LogInformation($"TradeAppService.CreateAsync - Validation : {msg}");
            }

            if (input.Details == null)
            {
                var msg = $"Transaction Details is required !!";
                valResults.Add(new ValidationResult(msg, new[] { "details" }));
                _logger.LogInformation($"TradeAppService.CreateAsync - Validation : {msg}");
            }

            if (input.Details?.Count <= 0)
            {
                var msg = $"Invalid Transaction Details !!";
                valResults.Add(new ValidationResult(msg, new[] { "details" }));
                _logger.LogInformation($"TradeAppService.CreateAsync - Validation : {msg}");
            }

            var duplicateProducts = input.Details?
                .GroupBy(s => s.ProductId)
                .Select(s => new
                {
                    ProductId = s.Key,
                    Count = s.Count()
                }).Where(s => s.Count > 1);
            if (duplicateProducts.Any())
            {
                var msg = $"Duplicate Product !!";
                valResults.Add(new ValidationResult(msg, new[] { "productId" }));
                _logger.LogInformation($"TradeAppService.CreateAsync - Validation : {msg}");
            }

            if (!isCustomerValid)
            {
                var msg = $"Invalid Customer !!";
                valResults.Add(new ValidationResult(msg, new[] { "customerId" }));
                _logger.LogInformation($"TradeAppService.CreateAsync - Validation : {msg}");
            }

            var isTransactionTypeValid = await isTransactionTypeValidTask;
            if (!isTransactionTypeValid)
            {
                var msg = $"Invalid Transaction Type !!";
                valResults.Add(new ValidationResult(msg, new[] { "transactionTypeId" }));
                _logger.LogInformation($"TradeAppService.CreateAsync - Validation : {msg}");
            }

            var productQuery = await _productRepository.GetQueryableAsync();
            var products = productQuery
            .Select(s => new
            {
                s.Id,
                s.DisplayName
            }).ToList();

            if (!products.Any())
            {
                var msg = "Product Not Found.";
                valResults.Add(new ValidationResult(msg, new[] { "productId" }));
                _logger.LogInformation($"TradeAppService.CreateAsync - Validation : {msg}");
            }

            var productValidations = (from s in input.Details
                                      join p in products on s.ProductId equals p.Id into pg
                                      from pj in pg.DefaultIfEmpty()
                                      where pj == null
                                      select new ValidationResult($"Invalid Product for {s.Quantity} quantity and {s.Price} price.", new[] { "productId" })
                                      ).ToList();
            if (productValidations.Any())
            {
                valResults.AddRange(productValidations);
                _logger.LogInformation($"TradeAppService.CreateAsync - Validation : Invalid Product");
            }

            var casesValidations = (from s in input.Details
                                    join p in products on s.ProductId equals p.Id
                                    where s.Cases <= 0
                                    select new ValidationResult($"Invalid Cases for {p.DisplayName} product", new[] { "productId" })
                                      ).ToList();
            if (casesValidations.Any())
            {
                valResults.AddRange(casesValidations);
                _logger.LogInformation($"TradeAppService.CreateAsync - Validation : Invalid Cases");
            }

            var quantityValidations = (from s in input.Details
                                       join p in products on s.ProductId equals p.Id
                                       where s.Quantity <= 0
                                       select new ValidationResult($"Invalid Quantity for {p.DisplayName} product", new[] { "quantity" })
                                     ).ToList();
            if (quantityValidations.Any())
            {
                valResults.AddRange(quantityValidations);
                _logger.LogInformation($"TradeAppService.CreateAsync - Validation : Invalid Quantity");
            }

            var priceValidations = (from s in input.Details
                                    join p in products on s.ProductId equals p.Id
                                    where s.Price <= 0
                                    select new ValidationResult($"Invalid Price for {p.DisplayName} product", new[] { "price" })
                                     ).ToList();
            if (priceValidations.Any())
            {
                valResults.AddRange(priceValidations);
                _logger.LogInformation($"TradeAppService.CreateAsync - Validation : Invalid Price");
            }

            if (valResults.Any())
            {
                _logger.LogInformation($"TradeAppService.CreateAsync - Throw Validation exception");
                throw new AbpValidationException(valTitle, valResults);
            }
            #endregion

            var transaction = ObjectMapper.Map<CreateTransactionDto, Transaction>(input);
            var transactionDetails = ObjectMapper.Map<List<CreateTransactionDetailDto>, List<TransactionDetail>>(input.Details);

            await _transactionRepository.InsertAsync(transaction);
            _logger.LogInformation($"TradeAppService.CreateAsync - Inserted Transaction");

            foreach (var transactionDetail in transactionDetails)
            {
                transactionDetail.Transaction = transaction;
            }
            await _transactionDetailRepository.InsertManyAsync(transactionDetails);
            _logger.LogInformation($"TradeAppService.CreateAsync - Inserted Transaction Details");

            _logger.LogInformation($"TradeAppService.CreateAsync - Ended");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"TradeAppService.CreateAsync - Exception : {ex}");
            throw;
        }
    }
    public async Task<List<DropdownDto>> GetTradeTypes()
    {
        try
        {
            _logger.LogInformation($"TradeAppService.GetTradeTypes - Started");

            var transactionTypes = await _transactionTypeRepository.GetQueryableAsync();
            var data = transactionTypes.Where(s => s.Description == Constants.Global.TRANSACTION_TYPE_TRADE)
                                       .Select(s => new DropdownDto(s.Id.ToString(), s.DisplayName)).ToList();

            _logger.LogInformation($"TradeAppService.GetTradeTypes - Ended");
            return data;
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"TradeAppService.GetTradeTypes - Exception : {ex}");
            throw;
        }
    }

    public async Task<PagedResultDto<TradeDto>> GetListByFilterAsync(PagedAndSortedResultRequestDto input, TradeFilter filter)
    {
        try
        {
            _logger.LogInformation($"TradeAppService.GetListByFilterAsync - Started");

            if (string.IsNullOrWhiteSpace(input.Sorting))
            {
                input.Sorting = "TranDate desc";
            }

            var data = await GetListDataByFilterAsync(filter);
            var dataCount = data.Count();
            var items = data.OrderBy(input.Sorting).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var result = new PagedResultDto<TradeDto>(dataCount, items);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"TradeAppService.GetListByFilterAsync - ExceptionError - {ex}");
            throw;
        }
        finally
        {
            _logger.LogInformation($"InventoryReportAppService.GetListByFilterAsync - Ended");
        }
    }

    public  async Task<FileBlobDto> ExportExcelAsync(TradeFilter filter)
    {
        try
        {
            _logger.LogInformation($"TradeAppService.ExportExcelAsync - Started");

            var trades = await GetListDataByFilterAsync(filter);
            var content = await _excelService.ExportAsync(trades.ToList(), _mapconfig);
            var filename = string.Format(ExcelFileNames.TRADE, $"_{DateTime.Now:yyyy/MM/dd HH:mm}");

            return new FileBlobDto(content, filename);
        }
        catch (Exception ex)
        {
            _logger.LogError($"TradeAppService.ExportExcelAsync - ExceptionError - {ex}");
            throw;
        }
        finally
        {
            _logger.LogInformation($"TradeAppService.ExportExcelAsync - Ended");
        }
    }
    private async Task<IQueryable<TradeDto>> GetListDataByFilterAsync(TradeFilter filter)
    {
        try
        {
            _logger.LogInformation($"TradeAppService.GetListDataByFilterAsync - Started");

            // Trim
            filter.VoucherNo = filter.VoucherNo?.Trim()?.ToLower();

            var customers = await _customerRepository.GetQueryableAsync();
            var transactions = await _transactionRepository.GetQueryableAsync();
            var transactionTypes = await _transactionTypeRepository.GetQueryableAsync();

            var data = (from t in transactions
                        join tt in transactionTypes on t.TransactionTypeId equals tt.Id
                        join c in customers on t.CustomerId equals c.Id
                        where t.TranDate.Date >= filter.FromTranDate.Date
                        && t.TranDate.Date <= filter.ToTranDate.Date
                        select new TradeDto
                        {
                            TransactionId = t.Id,
                            TradeTypeId = tt.Id,
                            TradeTypeName = tt.DisplayName,
                            CustomerId = c.Id,
                            CustomerName = c.DisplayName,
                            DiscountAmount = t.DiscountAmount,
                            TransportCharge = t.TransportCharge,
                            VoucherNo = t.VoucherNo,
                            TranDate = t.TranDate,
                            Amount = t.Amount
                        })
                        .WhereIf(!string.IsNullOrWhiteSpace(filter.VoucherNo), s => s.VoucherNo.ToLower().Contains(filter.VoucherNo))
                        .WhereIf(filter.TradeTypeId.HasValue, s => s.TradeTypeId == filter.TradeTypeId)
                        .WhereIf(filter.CustomerId.HasValue, s => s.CustomerId == filter.CustomerId);

            return data;
        }
        catch (Exception ex)
        {
            _logger.LogError($"TradeAppService.GetListDataByFilterAsync - ExceptionError - {ex}");
            throw;
        }
        finally
        {
            _logger.LogInformation($"TradeAppService.GetListDataByFilterAsync - Ended");
        }
    }

    public async Task<TransactionDto> GetAsync(Guid transactionId)
    {
        try
        {
            _logger.LogInformation($"TradeAppService.GetAsync - Started");

            var transactions = await _transactionRepository.GetQueryableAsync();
            var transactionDetails = await _transactionDetailRepository.GetQueryableAsync();

            var data = (from t in transactions
                        join td in transactionDetails on t.Id equals td.TransactionId into gp
                        where t.Id == transactionId
                        select new TransactionDto
                        {
                            Id = t.Id,
                            TransactionTypeId = t.TransactionTypeId,
                            CustomerId = t.CustomerId,
                            DiscountAmount = t.DiscountAmount,
                            TransportCharge = t.TransportCharge,
                            VoucherNo = t.VoucherNo,
                            TranDate = t.TranDate,
                            Amount = t.Amount,
                            Details = gp.Select(s => new TransactionDetailDto { 
                                Id = s.Id,
                                ProductId = s.ProductId,
                                Cases = s.Cases,
                                Quantity = s.Quantity,
                                Price = s.Price
                            }).ToList()
                        }).FirstOrDefault();

            return data;
        }
        catch (Exception ex)
        {
            _logger.LogError($"TradeAppService.GetAsync - ExceptionError - {ex}");
            throw;
        }
        finally
        {
            _logger.LogInformation($"TradeAppService.GetAsync - Ended");
        }
    }

    public async Task DeleteAsync(Guid transactionId)
    {
        try
        {
            _logger.LogInformation($"TradeAppService.DeleteAsync - Started");

            var hasTransaction = await _transactionRepository.AnyAsync(s => s.Id == transactionId);
            if (!hasTransaction) 
            {
                var msg = "Transaction not found!!";
                _logger.LogInformation($"TradeAppService.DeleteAsync - Validation : {msg}");
                throw new AbpValidationException(msg, new List<ValidationResult>()
                {
                    new ValidationResult(msg, new [] {"transactionId"})
                });
            }

            var transactionDetailQuery = await _transactionDetailRepository.GetQueryableAsync();
            var transactionDetails = transactionDetailQuery.Where(s => s.TransactionId == transactionId)
                                    .Select(s => s.Id).ToList();
            if (transactionDetails.Any()) {
                await _transactionDetailRepository.DeleteManyAsync(transactionDetails);
                _logger.LogInformation($"TradeAppService.DeleteAsync - Deleted Transaction Details");
            }

            await _transactionRepository.DeleteAsync(transactionId);
            _logger.LogInformation($"TradeAppService.DeleteAsync - Deleted Transaction");
        }
        catch (Exception ex)
        {
            _logger.LogError($"TradeAppService.DeleteAsync - ExceptionError - {ex}");
            throw;
        }
        finally
        {
            _logger.LogInformation($"TradeAppService.DeleteAsync - Ended");
        }
    }

    public async Task UpdateAsync(UpdateTransactionDto input)
    {
        try
        {
            _logger.LogInformation($"TradeAppService.UpdateAsync - Started");
            // Trim
            input.VoucherNo = input.VoucherNo?.Trim();

            #region Validations
            var valResults = new List<ValidationResult>();
            var valTitle = "Update Trade Validations";

            var transaction = await _transactionRepository.FindAsync(s => s.Id == input.Id);
            if (transaction == null)
            {
                var msg = "Transaction not Found !!";
                valResults.Add(new ValidationResult(msg, new[] { "customerId" }));
                _logger.LogInformation($"TradeAppService.CreateAsync - Validation : {msg}");
            }

            var isCustomerValidTask = _customerRepository.AnyAsync(s => s.Id == input.CustomerId);
            if (input.CustomerId == null)
            {
                var msg = $"Customer is required !!";
                valResults.Add(new ValidationResult(msg, new[] { "customerId" }));
                _logger.LogInformation($"TradeAppService.UpdateAsync - Validation : {msg}");
            }

            if (input.Amount == null)
            {
                var msg = $"Amount is required !!";
                valResults.Add(new ValidationResult(msg, new[] { "amount" }));
                _logger.LogInformation($"TradeAppService.UpdateAsync - Validation : {msg}");
            }

            if (input.Amount < 0)
            {
                var msg = $"Invalid Amount !!";
                valResults.Add(new ValidationResult(msg, new[] { "amount" }));
                _logger.LogInformation($"TradeAppService.UpdateAsync - Validation : {msg}");
            }

            if (input.TransactionTypeId == null)
            {
                var msg = $"Transaction Type is required !!";
                valResults.Add(new ValidationResult(msg, new[] { "transactionTypeId" }));
                _logger.LogInformation($"TradeAppService.UpdateAsync - Validation : {msg}");
            }

            if (input.TransportCharge < 0)
            {
                var msg = $"Invalid Transport Charge !!";
                valResults.Add(new ValidationResult(msg, new[] { "transportCharge" }));
                _logger.LogInformation($"TradeAppService.UpdateAsync - Validation : {msg}");
            }

            var isCustomerValid = await isCustomerValidTask;
            var isTransactionTypeValidTask = _transactionTypeRepository.AnyAsync(s => s.Id == input.TransactionTypeId);

            if (input.DiscountAmount < 0)
            {
                var msg = $"Invalid Discount Amount !!";
                valResults.Add(new ValidationResult(msg, new[] { "discountCharge" }));
                _logger.LogInformation($"TradeAppService.UpdateAsync - Validation : {msg}");
            }

            if (string.IsNullOrWhiteSpace(input.VoucherNo))
            {
                var msg = $"Voucher No is required !!";
                valResults.Add(new ValidationResult(msg, new[] { "voucherNo" }));
                _logger.LogInformation($"TradeAppService.UpdateAsync - Validation : {msg}");
            }

            if (input.Details == null)
            {
                var msg = $"Transaction Details is required !!";
                valResults.Add(new ValidationResult(msg, new[] { "details" }));
                _logger.LogInformation($"TradeAppService.UpdateAsync - Validation : {msg}");
            }

            if (input.Details?.Count <= 0)
            {
                var msg = $"Invalid Transaction Details !!";
                valResults.Add(new ValidationResult(msg, new[] { "details" }));
                _logger.LogInformation($"TradeAppService.UpdateAsync - Validation : {msg}");
            }

            var duplicateProducts = input.Details?
                .GroupBy(s => s.ProductId)
                .Select(s => new
                {
                    ProductId = s.Key,
                    Count = s.Count()
                }).Where(s => s.Count > 1);
            if (duplicateProducts.Any())
            {
                var msg = $"Duplicate Product !!";
                valResults.Add(new ValidationResult(msg, new[] { "productId" }));
                _logger.LogInformation($"TradeAppService.UpdateAsync - Validation : {msg}");
            }

            if (!isCustomerValid)
            {
                var msg = $"Invalid Customer !!";
                valResults.Add(new ValidationResult(msg, new[] { "customerId" }));
                _logger.LogInformation($"TradeAppService.UpdateAsync - Validation : {msg}");
            }

            var isTransactionTypeValid = await isTransactionTypeValidTask;
            if (!isTransactionTypeValid)
            {
                var msg = $"Invalid Transaction Type !!";
                valResults.Add(new ValidationResult(msg, new[] { "transactionTypeId" }));
                _logger.LogInformation($"TradeAppService.UpdateAsync - Validation : {msg}");
            }

            var productQuery = await _productRepository.GetQueryableAsync();
            var products = productQuery
            .Select(s => new
            {
                s.Id,
                s.DisplayName
            }).ToList();

            if (!products.Any())
            {
                var msg = "Product Not Found.";
                valResults.Add(new ValidationResult(msg, new[] { "productId" }));
                _logger.LogInformation($"TradeAppService.UpdateAsync - Validation : {msg}");
            }

            var productValidations = (from s in input.Details
                                      join p in products on s.ProductId equals p.Id into pg
                                      from pj in pg.DefaultIfEmpty()
                                      where pj == null
                                      select new ValidationResult($"Invalid Product for {s.Quantity} quantity and {s.Price} price.", new[] { "productId" })
                                      ).ToList();
            if (productValidations.Any())
            {
                valResults.AddRange(productValidations);
                _logger.LogInformation($"TradeAppService.UpdateAsync - Validation : Invalid Product");
            }

            var casesValidations = (from s in input.Details
                                    join p in products on s.ProductId equals p.Id
                                    where s.Cases <= 0
                                    select new ValidationResult($"Invalid Cases for {p.DisplayName} product", new[] { "productId" })
                                      ).ToList();
            if (casesValidations.Any())
            {
                valResults.AddRange(casesValidations);
                _logger.LogInformation($"TradeAppService.UpdateAsync - Validation : Invalid Cases");
            }

            var quantityValidations = (from s in input.Details
                                       join p in products on s.ProductId equals p.Id
                                       where s.Quantity <= 0
                                       select new ValidationResult($"Invalid Quantity for {p.DisplayName} product", new[] { "quantity" })
                                     ).ToList();
            if (quantityValidations.Any())
            {
                valResults.AddRange(quantityValidations);
                _logger.LogInformation($"TradeAppService.UpdateAsync - Validation : Invalid Quantity");
            }

            var priceValidations = (from s in input.Details
                                    join p in products on s.ProductId equals p.Id
                                    where s.Price <= 0
                                    select new ValidationResult($"Invalid Price for {p.DisplayName} product", new[] { "price" })
                                     ).ToList();
            if (priceValidations.Any())
            {
                valResults.AddRange(priceValidations);
                _logger.LogInformation($"TradeAppService.UpdateAsync - Validation : Invalid Price");
            }

            if (valResults.Any())
            {
                _logger.LogInformation($"TradeAppService.UpdateAsync - Throw Validation exception");
                throw new AbpValidationException(valTitle, valResults);
            }
            #endregion

            ObjectMapper.Map(input, transaction);
            await _transactionRepository.UpdateAsync(transaction);
            _logger.LogInformation($"TradeAppService.UpdateAsync - Inserted Transaction");

            var transactionDetailQuery = await _transactionDetailRepository.GetQueryableAsync();
            var transactionDetails = transactionDetailQuery.Where(s => s.TransactionId == input.Id).ToList();

            if (!transactionDetails.Any()) 
            {
                var msg = "Transaction Detail data not found.";
                _logger.LogInformation($"TradeAppService.UpdateAsync - Validation : {msg}");
                throw new AbpValidationException(msg, new List<ValidationResult>()
                {
                new ValidationResult(msg, new[] {"transactionDetailId"})
                });
            }

            var transactionDetailsUpdate = (from td in transactionDetails
                                           join inp in input.Details on td.Id equals inp.Id
                                           select UpdateTransactionDetail(td, inp)).ToList();
      
            // filter out new transaction details
            var transactionDetailsInsert = input.Details.Where(inp => inp.Id == null).ToList();

            // map dto details to transaction details
            var mappedTransactionDetailsInsert = ObjectMapper.Map<List<UpdateTransactionDetailDto>, List<TransactionDetail>>(transactionDetailsInsert);
            if (!transactionDetailsUpdate.Any() && !mappedTransactionDetailsInsert.Any())
            {
                var msg = "Transaction Detail Update data not found.";
                _logger.LogInformation($"TradeAppService.UpdateAsync - Validation : {msg}");
                throw new AbpValidationException(msg, new List<ValidationResult>()
                {
                    new ValidationResult(msg, new[] {"transactionDetailId"})
                });
            }

            await _transactionDetailRepository.UpdateManyAsync(transactionDetailsUpdate);
            _logger.LogInformation($"TradeAppService.UpdateAsync -  Updated Transaction Details");

            // insert new transaction details
            await _transactionDetailRepository.InsertManyAsync(mappedTransactionDetailsInsert);

            var transactionDetailsDelete = (from td in transactionDetails
                                            join inp in input.Details on td.Id equals inp.Id into gp
                                            from lj in gp.DefaultIfEmpty()
                                            where lj.Id == null
                                          select td.Id).ToList();

            await _transactionDetailRepository.DeleteManyAsync(transactionDetailsDelete);
            _logger.LogInformation($"TradeAppService.DeleteAsync -  Delete Transaction Details");

        }
        catch (Exception ex)
        {
            _logger.LogError($"TradeAppService.UpdateAsync - ExceptionError - {ex}");
            throw;
        }
        finally
        {
            _logger.LogInformation($"TradeAppService.UpdateAsync - Ended");
        }
    }

    private  static TransactionDetail UpdateTransactionDetail(TransactionDetail td, UpdateTransactionDetailDto inp)
    {
        td.ProductId = inp.ProductId;
        td.Cases = inp.Cases;
        td.Quantity = inp.Quantity;
        td.Price = inp.Price;
        return td;
    }
}