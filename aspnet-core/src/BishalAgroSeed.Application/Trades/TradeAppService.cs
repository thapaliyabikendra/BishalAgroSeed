using BishalAgroSeed.Customers;
using BishalAgroSeed.Dtos;
using BishalAgroSeed.Permissions;
using BishalAgroSeed.Products;
using BishalAgroSeed.TransactionDetails;
using BishalAgroSeed.Transactions;
using BishalAgroSeed.TranscationTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
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

    public TradeAppService(
      IRepository<Transaction, Guid> transactionRepository,
      IRepository<TransactionDetail, Guid> transactionDetailRepository,
      IRepository<Product, Guid> productRepository,
      IRepository<Customer, Guid> customerRepository,
       ILogger<TradeAppService> logger,
        IRepository<TransactionType, Guid> transactionTypeRepository,
        IUnitOfWorkManager unitOfWorkManager
        )
    {
        _transactionRepository = transactionRepository;
        _transactionDetailRepository = transactionDetailRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _logger = logger;
        _transactionTypeRepository = transactionTypeRepository;
        _unitOfWorkManager = unitOfWorkManager;
    }

    [Authorize(BishalAgroSeedPermissions.Trades.Create)]
    [HttpPost]
    public async Task SaveTransactionAsync(CreateTransactionDto input)
    {
        try
        {
            _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Started");

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
                _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Validation : {msg}");
            }

            if (input.Amount == null)
            {
                var msg = $"Amount is required !!";
                valResults.Add(new ValidationResult(msg, new[] { "amount" }));
                _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Validation : {msg}");
            }

            if (input.Amount <= 0)
            {
                var msg = $"Invalid Amount !!";
                valResults.Add(new ValidationResult(msg, new[] { "amount" }));
                _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Validation : {msg}");
            }

            if (input.TransactionTypeId == null)
            {
                var msg = $"Transaction Type is required !!";
                valResults.Add(new ValidationResult(msg, new[] { "transactionTypeId" }));
                _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Validation : {msg}");
            }

            if (input.TransportCharge < 0)
            {
                var msg = $"Invalid Transport Charge !!";
                valResults.Add(new ValidationResult(msg, new[] { "transportCharge" }));
                _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Validation : {msg}");
            }

            var isCustomerValid = await isCustomerValidTask;
            var isTransactionTypeValidTask = _transactionTypeRepository.AnyAsync(s => s.Id == input.TransactionTypeId);

            if (input.DiscountAmount < 0)
            {
                var msg = $"Invalid Discount Amount !!";
                valResults.Add(new ValidationResult(msg, new[] { "discountCharge" }));
                _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Validation : {msg}");
            }

            if (string.IsNullOrWhiteSpace(input.VoucherNo))
            {
                var msg = $"Voucher No is required !!";
                valResults.Add(new ValidationResult(msg, new[] { "voucherNo" }));
                _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Validation : {msg}");
            }

            if (input.Details == null)
            {
                var msg = $"Transaction Details is required !!";
                valResults.Add(new ValidationResult(msg, new[] { "details" }));
                _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Validation : {msg}");
            }

            if (input.Details?.Count <= 0)
            {
                var msg = $"Invalid Transaction Details !!";
                valResults.Add(new ValidationResult(msg, new[] { "details" }));
                _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Validation : {msg}");
            }

            if (!isCustomerValid)
            {
                var msg = $"Invalid Customer !!";
                valResults.Add(new ValidationResult(msg, new[] { "customerId" }));
                _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Validation : {msg}");
            }

            var isTransactionTypeValid = await isTransactionTypeValidTask;
            if (!isTransactionTypeValid)
            {
                var msg = $"Invalid Transaction Type !!";
                valResults.Add(new ValidationResult(msg, new[] { "transactionTypeId" }));
                _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Validation : {msg}");
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
                _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Validation : {msg}");
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
                _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Validation : Invalid Product");
            }

            var casesValidations = (from s in input.Details
                                    join p in products on s.ProductId equals p.Id
                                    where s.Cases <= 0
                                    select new ValidationResult($"Invalid Cases for {p.DisplayName} product", new[] { "productId" })
                                      ).ToList();
            if (casesValidations.Any())
            {
                valResults.AddRange(casesValidations);
                _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Validation : Invalid Cases");
            }

            var quantityValidations = (from s in input.Details
                                       join p in products on s.ProductId equals p.Id
                                       where s.Quantity <= 0
                                       select new ValidationResult($"Invalid Quantity for {p.DisplayName} product", new[] { "quantity" })
                                     ).ToList();
            if (quantityValidations.Any())
            {
                valResults.AddRange(quantityValidations);
                _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Validation : Invalid Quantity");
            }

            var priceValidations = (from s in input.Details
                                    join p in products on s.ProductId equals p.Id
                                    where s.Price <= 0
                                    select new ValidationResult($"Invalid Price for {p.DisplayName} product", new[] { "price" })
                                     ).ToList();
            if (priceValidations.Any())
            {
                valResults.AddRange(priceValidations);
                _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Validation : Invalid Price");
            }

            if (valResults.Any())
            {
                _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Throw Validation exception");
                throw new AbpValidationException(valTitle, valResults);
            }
            #endregion

            var transaction = ObjectMapper.Map<CreateTransactionDto, Transaction>(input);
            transaction.TranDate = DateTime.Now;
            var transactionDetails = ObjectMapper.Map<List<CreateTransactionDetailDto>, List<TransactionDetail>>(input.Details);

            await _transactionRepository.InsertAsync(transaction);
            _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Inserted Transaction");

            foreach (var transactionDetail in transactionDetails)
            {
                transactionDetail.Transaction = transaction;
            }
            await _transactionDetailRepository.InsertManyAsync(transactionDetails);
            _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Inserted Transaction Details");

            _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Ended");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Exception : {ex}");
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
}