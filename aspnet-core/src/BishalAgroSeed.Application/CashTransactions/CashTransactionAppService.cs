using AutoMapper.Internal.Mappers;
using BishalAgroSeed.Customers;
using BishalAgroSeed.Dtos;
using BishalAgroSeed.PaymentTypes;
using BishalAgroSeed.Permissions;
using BishalAgroSeed.Products;
using BishalAgroSeed.Trades;
using BishalAgroSeed.TransactionDetails;
using BishalAgroSeed.TransactionPayments;
using BishalAgroSeed.Transactions;
using BishalAgroSeed.TranscationTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using Volo.Abp.Validation;

namespace BishalAgroSeed.CashTransactions;
public class CashTransactionAppService : ApplicationService, ICashTransactionAppService
{
    private readonly IRepository<Transaction, Guid> _transactionRepository;
    private readonly IRepository<TransactionDetail, Guid> _transactionDetailRepository;
    private readonly IRepository<Product, Guid> _productRepository;
    private readonly IRepository<Customer, Guid> _customerRepository;
    private readonly ILogger<TradeAppService> _logger;
    private readonly IRepository<TransactionType, Guid> _transactionTypeRepository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IRepository<TransactionPayment, Guid> _transactionPaymentRepository;

    public CashTransactionAppService(
      IRepository<Transaction, Guid> transactionRepository,
      IRepository<TransactionDetail, Guid> transactionDetailRepository,
      IRepository<Product, Guid> productRepository,
      IRepository<Customer, Guid> customerRepository,
       ILogger<TradeAppService> logger,
        IRepository<TransactionType, Guid> transactionTypeRepository,
        IUnitOfWorkManager unitOfWorkManager,
        IRepository<TransactionPayment, Guid> transactionPaymentRepository
        )
    {
        _transactionRepository = transactionRepository;
        _transactionDetailRepository = transactionDetailRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _logger = logger;
        _transactionTypeRepository = transactionTypeRepository;
        _unitOfWorkManager = unitOfWorkManager;
        _transactionPaymentRepository = transactionPaymentRepository;
    }
    [Authorize(BishalAgroSeedPermissions.CashTransactions.Default)]
    public async Task<List<DropdownDto>> GetCashTransactionTypesAsync()
    {
        try
        {
            _logger.LogInformation($"CashTransactionAppService.GetCashTransactionTypesAsync - Started");

            var transactionTypes = await _transactionTypeRepository.GetQueryableAsync();
            var data = transactionTypes.Where(s => s.Description == Constants.Global.TRANSACTION_TYPE_CASH_TRANSACTION)
                                       .Select(s => new DropdownDto(s.Id.ToString(), s.DisplayName)).ToList();

            _logger.LogInformation($"CashTransactionAppService.GetCashTransactionTypesAsync - Ended");
            return data;
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"CashTransactionAppService.GetCashTransactionTypesAsync - Exception : {ex}");
            throw;
        }
    }

    [Authorize(BishalAgroSeedPermissions.CashTransactions.Default)]
    public async Task<List<DropdownDto>> GetPaymentTypesAsync()
    {
        try
        {
            _logger.LogInformation($"CashTransactionAppService.GetPaymentTypesAsync - Started");

            var data = await Task.Run(() =>
            {
                return Enum.GetValues<PaymentType>()
                .Select(s => new DropdownDto(((int)s).ToString(), s.ToString())).ToList();
            });

            _logger.LogInformation($"CashTransactionAppService.GetPaymentTypesAsync - Ended");
            return data;
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"CashTransactionAppService.GetPaymentTypesAsync - Exception : {ex}");
            throw;
        }
    }

    [Authorize(BishalAgroSeedPermissions.CashTransactions.Create)]
    public async Task SaveTransactionAsync(CreateTransactionDto input)
    {
        try
        {
            _logger.LogInformation($"CashTransactionAppService.SaveTransactionAsync - Started");

            // Trim
            input.VoucherNo = input.VoucherNo?.Trim();

            #region Validations
            var valResults = new List<ValidationResult>();
            var valTitle = "Save Cash Transaction Validations";

            var isCustomerValidTask = _customerRepository.AnyAsync(s => s.Id == input.CustomerId);

            if (input.CustomerId == null)
            {
                var msg = $"Customer is required !!";
                valResults.Add(new ValidationResult(msg, new[] { "customerId" }));
                _logger.LogInformation($"CashTransactionAppService.SaveTransactionAsync - Validation : {msg}");
            }

            if (input.Amount == null)
            {
                var msg = $"Amount is required !!";
                valResults.Add(new ValidationResult(msg, new[] { "amount" }));
                _logger.LogInformation($"CashTransactionAppService.SaveTransactionAsync - Validation : {msg}");
            }

            if (input.Amount < 0)
            {
                var msg = $"Invalid Amount !!";
                valResults.Add(new ValidationResult(msg, new[] { "amount" }));
                _logger.LogInformation($"CashTransactionAppService.SaveTransactionAsync - Validation : {msg}");
            }

            if (input.TransactionTypeId == null)
            {
                var msg = $"Transaction Type is required !!";
                valResults.Add(new ValidationResult(msg, new[] { "transactionTypeId" }));
                _logger.LogInformation($"CashTransactionAppService.SaveTransactionAsync - Validation : {msg}");
            }

            if (input.TransportCharge < 0)
            {
                var msg = $"Invalid Transport Charge !!";
                valResults.Add(new ValidationResult(msg, new[] { "transportCharge" }));
                _logger.LogInformation($"CashTransactionAppService.SaveTransactionAsync - Validation : {msg}");
            }

            var isCustomerValid = await isCustomerValidTask;
            var isTransactionTypeValidTask = _transactionTypeRepository.AnyAsync(s => s.Id == input.TransactionTypeId);

            if (input.DiscountAmount < 0)
            {
                var msg = $"Invalid Discount Amount !!";
                valResults.Add(new ValidationResult(msg, new[] { "discountCharge" }));
                _logger.LogInformation($"CashTransactionAppService.SaveTransactionAsync - Validation : {msg}");
            }

            if (string.IsNullOrWhiteSpace(input.VoucherNo))
            {
                var msg = $"Voucher No is required !!";
                valResults.Add(new ValidationResult(msg, new[] { "voucherNo" }));
                _logger.LogInformation($"CashTransactionAppService.SaveTransactionAsync - Validation : {msg}");
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

            if (valResults.Any())
            {
                _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Throw Validation exception");
                throw new AbpValidationException(valTitle, valResults);
            }

            if (input.Payment == null)
            {
                var msg = $"Empty Payment Data !!";
                valResults.Add(new ValidationResult(msg, new[] { "payment" }));
                _logger.LogInformation($"CashTransactionAppService.SaveTransactionAsync - Validation : {msg}");
            }
            else
            {
                if (input.Payment.PaymentTypeId == null)
                {
                    var msg = $"Payment Type is required !!";
                    valResults.Add(new ValidationResult(msg, new[] { "paymentTypeId" }));
                    _logger.LogInformation($"CashTransactionAppService.SaveTransactionAsync - Validation : {msg}");
                }
                else if (input.Payment.PaymentTypeId == PaymentType.Banking)
                {
                    if (string.IsNullOrWhiteSpace(input.Payment.BankName))
                    {
                        var msg = $"Bank Name is required for Banking Payment Type !!";
                        valResults.Add(new ValidationResult(msg, new[] { "bankName" }));
                        _logger.LogInformation($"CashTransactionAppService.SaveTransactionAsync - Validation : {msg}");
                    }
                    if (string.IsNullOrWhiteSpace(input.Payment.ChequeNo))
                    {
                        var msg = $"Cheque No is required for Banking Payment Type !!";
                        valResults.Add(new ValidationResult(msg, new[] { "chequeNo" }));
                        _logger.LogInformation($"CashTransactionAppService.SaveTransactionAsync - Validation : {msg}");
                    }
                }
            }
            #endregion

            var transaction = ObjectMapper.Map<CreateTransactionDto, Transaction>(input);
            var transactionPayment = ObjectMapper.Map<CreateTransactionPaymentDto, TransactionPayment>(input.Payment);

            await _transactionRepository.InsertAsync(transaction);
            _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Inserted Transaction");

            transactionPayment.Transaction = transaction;
            await _transactionPaymentRepository.InsertAsync(transactionPayment);
            _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Inserted Transaction Payment");

            _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Ended");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"TradeAppService.SaveTransactionAsync - Exception : {ex}");
            throw;
        }
    }
}