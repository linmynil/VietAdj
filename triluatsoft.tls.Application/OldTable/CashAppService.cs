using Abp.AutoMapper;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.EntityFramework;
using triluatsoft.tls.OldTable.dto;
using triluatsoft.tls.OldTable.View;

namespace triluatsoft.tls.OldTable
{
    public class CashAppService : tlsAppServiceBase, ICashAppService
    {
        private readonly ISqlExecuter _sqlExecuter;
        private readonly IRepository<Cash> _cashRepo;
        private readonly IRepository<CashHistory> _cashHistoryRepo;

        public CashAppService(ISqlExecuter sqlExecuter
            , IRepository<CashHistory> cashHistoryRepo
            , IRepository<Cash> cashRepo)
        {
            _sqlExecuter = sqlExecuter;
            _cashHistoryRepo = cashHistoryRepo;
            _cashRepo = cashRepo;
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="opts">Search options</param>
        /// <returns></returns>
        public CustomPagedResultDto<CashView> Search(CashSearchOptions opts)
        {
            var context = _sqlExecuter.GetTLSDBContext();

            var query = from c in context.Cashs
                        where (string.IsNullOrEmpty(opts.Description) || c.Description.Contains(opts.Description))
                            && (string.IsNullOrEmpty(opts.RefNbr) || c.RefNbr.Contains(opts.RefNbr))
                            && (string.IsNullOrEmpty(opts.PaymentMethod) || c.PaymentMethod == opts.PaymentMethod)
                            && (string.IsNullOrEmpty(opts.VoucherType) || c.VoucherType == opts.VoucherType)
                            && (opts.AmountFrom == null || c.Amount >= opts.AmountFrom.Value)
                            && (opts.AmountTo == null || c.Amount <= opts.AmountTo.Value)
                            && (opts.FromDate == null || c.CreatedDate >= opts.FromDate)
                            && (opts.ToDate == null || c.CreatedDate <= opts.ToDate)
                            && (c.IsDelete != true)   //hvtam-19022016 tam thoi ko cho hien thi nhung cai da xoa
                        orderby c.CreatedDate descending
                        select new CashView
                        {
                            ID = c.Id,
                            CashCode = c.CashCode,
                            Description = c.Description,
                            RefNbr = c.RefNbr,
                            Payment = c.PaymentMethod,
                            VoucherType = c.VoucherType,
                            Amount = c.Amount,
                            CreatedDate = c.CreatedDate,
                            isDelete = c.IsDelete   //hvtam-19022016 isDelete Flag
                        };
            var total = query.Count();
            decimal totalAmount = query.Sum(r => r.Amount) ?? 0;

            query = query.Skip(opts.PageSize * opts.Page)
                .Take(opts.PageSize);

            List<CashView> list = query.ToList();

            var result = new CustomPagedResultDto<CashView>()
            {
                TotalCount = total
                ,
                Items = list
                ,
                SumValue = totalAmount
            };
            return result;

        }
        public CashView GetById(int cashId)
        {
            var query = from c in _cashRepo.GetAll()
                        where c.Id == cashId
                        select new CashView
                        {
                            ID = c.Id,
                            CashCode = c.CashCode,
                            Description = c.Description,
                            RefNbr = c.RefNbr,
                            Payment = c.PaymentMethod,
                            VoucherType = c.VoucherType,
                            Amount = c.Amount,
                            CreatedDate = c.CreatedDate,
                            isDelete = c.IsDelete   //hvtam-19022016 isDelete Flag
                        };
            CashView view = query.FirstOrDefault();
            return view;

        }

        public List<CashBorderauxView> GetHistories(int cashID)
        {
            var context = _sqlExecuter.GetTLSDBContext();

            return context.CashHistorys
                        .Where(ch => ch.CashID == cashID)
                        .Select(ch => new CashBorderauxView
                        {
                            CreatedDate = ch.CreatedDate,
                            InputDate = ch.InputDate,
                            RefNbr = ch.RefNbr,
                            Description = ch.Description,
                            VoucherType = ch.VoucherType,
                            PaymentMethod = ch.PaymentMethod,
                            Amount = ch.Amount,
                            CashCode = ch.CashCode
                        })
                        .OrderByDescending(ch => ch.CreatedDate)
                        .ToList();

        }
        private bool DetectChanges(CreateOrUpdateCashHistoryInput input, Cash cash)
        {
            return cash.RefNbr != input.RefNbr
                || cash.Description != input.Description
                || cash.Notes != input.Notes
                || cash.PaymentMethod != input.PaymentMethod
                || cash.VoucherType != input.VoucherType
                || cash.Amount != input.Amount
                || cash.CreatedDate != input.CreatedDate
                || cash.IsDelete != input.IsDelete;  //hvtam-19022016 using isDelete Flag

        }

        private CashHistory MapCashHist(CreateOrUpdateCashHistoryInput input)
        {
            CashHistory cashHist = new CashHistory();
            if (input.Id.HasValue)
            cashHist.CashID = input.Id.Value;

            cashHist.Amount = input.Amount;
            cashHist.CashCode = string.Format("{0}-{1}", input.VoucherType, DateTime.Now.ToString("yyyyMMddhhmmss"));
            cashHist.CreatedDate = input.DateTimeNow;
            cashHist.Description = input.Description;
            cashHist.PaymentMethod = input.PaymentMethod;
            cashHist.RefNbr = input.RefNbr;
            cashHist.VoucherType = input.VoucherType;
            if (input.IsDelete == true) cashHist.Amount = 0; //hvtam-19022016 neu xoa thi dua so tien ve 0 trong history table


            return cashHist;
        }

        private Cash MapCash(Cash cash, CreateOrUpdateCashHistoryInput input)
        {
            if (cash == null)
            {
                cash = new Cash();
            }
            cash.Amount = input.Amount;
            cash.CashCode = string.Format("{0}-{1}", input.VoucherType, DateTime.Now.ToString("yyyyMMddhhmmss"));
            cash.CreatedDate = input.CreatedDate;
            cash.Description = input.Description;
            cash.PaymentMethod = input.PaymentMethod;
            cash.RefNbr = input.RefNbr;
            cash.VoucherType = input.VoucherType;
            return cash;
        }

        public string SaveCash(CreateOrUpdateCashHistoryInput input)
        {
            if (input.Id.HasValue )
            {
                Cash cash = _cashRepo.FirstOrDefault(input.Id.Value);
                if (cash != null && DetectChanges(input, cash))
                {

                    cash = MapCash(cash, input);
                    _cashRepo.Update(cash);
                    CurrentUnitOfWork.SaveChanges();

                    CashHistory cashHist = MapCashHist(input);
                    cashHist.InputDate = cash.CreatedDate;
                    _cashHistoryRepo.InsertAndGetId(cashHist);
                }
                else
                {
                    return "Nothing changed";
                }
            }
            else
            {
                Cash cash = MapCash(null, input);
                //create new cash row
                var cashID = _cashRepo.InsertAndGetId(cash);
                CurrentUnitOfWork.SaveChanges();

                CashHistory cashHist = MapCashHist(input);
                cashHist.CashID = cash.Id;
                cashHist.InputDate = cash.CreatedDate;
                _cashHistoryRepo.InsertAndGetId(cashHist);

            }
            return "ok";
        }

        public string DeleteCash(int cashId)
        {

            Cash cash = _cashRepo.FirstOrDefault(cashId);
            cash.IsDelete = true;

            CashHistory cashHist = new CashHistory();
            cashHist.CashID = cash.Id;
            cashHist.Amount = cashHist.Amount = 0; //hvtam-19022016 neu xoa thi dua so tien ve 0 trong history table
            cashHist.CashCode = string.Format("{0}-{1}", cash.VoucherType, DateTime.Now.ToString("yyyyMMddhhmmss"));
            cashHist.CreatedDate = cash.CreatedDate;
            cashHist.Description = cash.Description;
            cashHist.PaymentMethod = cash.PaymentMethod;
            cashHist.RefNbr = cash.RefNbr;
            cashHist.VoucherType = cash.VoucherType;
            cashHist.InputDate = cash.CreatedDate;

            _cashHistoryRepo.InsertAndGetId(cashHist);
            return "ok";
        }
    }
}
