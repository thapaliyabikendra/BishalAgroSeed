using BishalAgroSeed.Customers;
using BishalAgroSeed.OpeningBalances;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace BishalAgroSeed.LedgerAccounts;
public class LedgerAccountAppService : ApplicationService, ILedgerAccountAppService
{
    private readonly IRepository<Transaction, Guid> _transactionRepository;
    private readonly IRepository<Customer, Guid> _customerRepository;
    private readonly IRepository<DateTimes.DateTime, Guid> _dateTimeRepository;
    private readonly ILogger<LedgerAccountAppService> _logger;
    private readonly IRepository<TransactionType, Guid> _transactionTypeRepository;
    private readonly IRepository<OpeningBalance, Guid> _openingBalanceRepository;

    public LedgerAccountAppService(
      IRepository<Transaction, Guid> transactionRepository,
       IRepository<Customer, Guid> customerRepository,
       IRepository<DateTimes.DateTime, Guid> dateTimeRepository,
       ILogger<LedgerAccountAppService> logger,
        IRepository<TransactionType, Guid> transactionTypeRepository,
        IRepository<OpeningBalance, Guid> openingBalanceRepository
        )
    {
        _transactionRepository = transactionRepository;
        _customerRepository = customerRepository;
        _dateTimeRepository = dateTimeRepository;
        _logger = logger;
        _transactionTypeRepository = transactionTypeRepository;
        _openingBalanceRepository = openingBalanceRepository;
    }

    [Authorize(BishalAgroSeedPermissions.LedgerAccounts.Default)]
    public async Task<PagedResultDto<LedgerAccountDto>> GetListByFilterAsync(PagedAndSortedResultRequestDto input, LedgerAccountFilter filter)
    {
        try
        {
            _logger.LogInformation($"LedgerAccountAppService.GetListByFilterAsync - Started");

            var transactions = await _transactionRepository.GetQueryableAsync();
            var transactionTypes = await _transactionTypeRepository.GetQueryableAsync();
            var dateTimes = await _dateTimeRepository.GetQueryableAsync();
            var openingBalances = await _openingBalanceRepository.GetQueryableAsync();

            var data = (from ob in openingBalances
                        join dt in dateTimes on ob.TranDate.Date equals dt.Datetime.Date
                        where ob.CustomerId == filter.CustomerId
                          && ob.TranDate.Date >= filter.FromTranDate.Date
                          && ob.TranDate.Date <= filter.ToTranDate.Date
                          && ob.IsReceivable
                        select new LedgerAccountDto
                        {
                            Miti = dt.DatetimeNepali,
                            Date = ob.TranDate.Date,
                            Particulars = "Opening Balance",
                            VchType = "",
                            VchNo = "",
                            Debit = ob.Amount,
                            Credit = 0,
                            Balance = 0
                        }).Union(from t in transactions
                                 join tt in transactionTypes on t.TransactionTypeId equals tt.Id
                                 join dt in dateTimes on t.TranDate.Date equals dt.Datetime.Date
                                 where tt.DisplayName == Constants.TransactionTypes.SALES
                                 && t.CustomerId == filter.CustomerId
                                 && t.TranDate.Date >= filter.FromTranDate.Date
                                 && t.TranDate.Date <= filter.ToTranDate.Date
                                 select new LedgerAccountDto
                                 {
                                     Miti = dt.DatetimeNepali,
                                     Date = t.TranDate.Date,
                                     Particulars = "Sales of Goods",
                                     VchType = "Sales Invoice",
                                     VchNo = t.VoucherNo,
                                     Debit = t.Amount,
                                     Credit = 0,
                                     Balance = 0
                                 }).Union(from t in transactions
                                          join tt in transactionTypes on t.TransactionTypeId equals tt.Id
                                          join dt in dateTimes on t.TranDate.Date equals dt.Datetime.Date
                                          where tt.DisplayName == Constants.TransactionTypes.CASH_RECEIPT
                                          && t.CustomerId == filter.CustomerId
                                          && t.TranDate.Date >= filter.FromTranDate.Date
                                          && t.TranDate.Date <= filter.ToTranDate.Date
                                          select new LedgerAccountDto
                                          {
                                              Miti = dt.DatetimeNepali,
                                              Date = t.TranDate.Date,
                                              Particulars = "Cash",
                                              VchType = "Receipt",
                                              VchNo = t.VoucherNo,
                                              Debit = 0,
                                              Credit = t.Amount,
                                              Balance = 0
                                          }).OrderBy(s => s.Date);

            var dataCount = data.Count();
            var items = data.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            decimal obal = 0;
            foreach (var item in items) {
                obal = obal + item.Debit - item.Credit;
                item.Balance = obal;
            }

            var result = new PagedResultDto<LedgerAccountDto>(dataCount, items);

            _logger.LogInformation($"LedgerAccountAppService.GetListByFilterAsync - Ended");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"LedgerAccountAppService.GetListByFilterAsync - Exception : {ex}");
            throw;
        }
    }
}
