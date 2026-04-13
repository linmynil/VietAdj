using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.EntityFramework;
using triluatsoft.tls.OldTable.dto;

namespace triluatsoft.tls.OldTable
{
    public class CustomerAppService : tlsAppServiceBase, ICustomerAppService
    {
        private readonly ISqlExecuter _sqlExecuter;
        public CustomerAppService(ISqlExecuter sqlExecuter)
        {
            _sqlExecuter = sqlExecuter;
        }
        public List<CustomerDBObj> GetAll()
        {
            List<CustomerDBObj> list = _sqlExecuter.GetDatabase().SqlQuery<CustomerDBObj>("Select * from Customer ORDER BY RecordType, Name").ToList();
            return list;
        }
        public List<CustomerDBObj> GetByType(string type)
        {
            List<CustomerDBObj> list = _sqlExecuter.GetDatabase().SqlQuery<CustomerDBObj>("Select * from Customer Where RecordType = @p0 ORDER BY Name", type).ToList();
            return list;
        }

        public List<CustomerDBObj> Search(SearchCustomerInput input)
        {
            string where = "";
            if (!string.IsNullOrEmpty(input.RecordType))
            {
                where += string.Format(" AND RecordType = '{0}' ", input.RecordType);
            }
            if (!string.IsNullOrEmpty(input.Name))
            {
                where += string.Format(" AND Name LIKE '%{0}%' ", input.Name);
            }
            if (!string.IsNullOrEmpty(input.BrandName))
            {
                where += string.Format(" AND BrandName LIKE '%{0}%' ", input.BrandName);
            }
            if (input.Status.HasValue)
            {
                where += string.Format(" AND IsActive = '{0}' ", input.Status);
            }
            List<CustomerDBObj> list = _sqlExecuter.GetDatabase().SqlQuery<CustomerDBObj>("Select * from Customer Where 1 =1 "
                + where
                + " ORDER BY Name").ToList();
            return list;
        }

        public CustomerDBObj GetById(int Id)
        {
            return _sqlExecuter.GetDatabase().SqlQuery<CustomerDBObj>(string.Format("Select * from Customer Where ID = {0}", Id)).FirstOrDefault();
        }
        private int Create(CustomerDBObj input)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("" +
                "INSERT INTO Customer(Name, BrandName, Address, Phone, Email, Website" +
                ", ContactName, ContactTitle, ContactPosition, ContactAddress, ContactPhone, ContactEmail" +
                ", IsActive, RecordType, Balance, TranID, BalanceDebit, BalanceCredit )" +
                " VALUES(@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17)", input.Name, input.BrandName, input.Address, input.Phone, input.Email, input.Website
                , input.ContactName, input.ContactTitle, input.ContactPosition, input.ContactAddress, input.ContactPhone, input.ContactEmail
                , input.IsActive, input.RecordType, input.Balance, input.TranID, input.BalanceDebit, input.BalanceCredit);
        }

        private int Update(CustomerDBObj input)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("UPDATE Customer SET " +
                " Name=@p0" +
                ", BrandName = @p1 " +
                ", Address = @p2" +
                ", Phone = @p3" +
                ", Email = @p4" +
                ", Website = @p5" +
                ", ContactName = @p6 " +
                ", ContactTitle = @p7 " +
                ", ContactPosition = @p8" +
                ", ContactAddress = @p9" +
                ", ContactPhone = @p10" +
                ", ContactEmail = @p11" +
                ", IsActive = @p12" +
                ", RecordType = @p13" +
                ", Balance = @p14" +
                ", TranID = @p15" +
                ", BalanceDebit = @p16" +
                ", BalanceCredit = @p17" +
                " WHERE ID = @p18", input.Name,
                input.BrandName, input.Address, input.Phone, input.Email, input.Website, input.ContactName
                , input.ContactTitle, input.ContactPosition, input.ContactAddress, input.ContactPhone, input.ContactEmail
                , input.IsActive, input.RecordType, input.Balance, input.TranID, input.BalanceDebit, input.BalanceCredit, input.ID);
        }
        public int CreateOrUpdate(CreateOrUpdateCustomerInput input)
        {
            CustomerDBObj c = new CustomerDBObj()
            {
                Name = input.Name,
                BrandName = input.BrandName,
                Address = input.Address,
                Phone = input.Phone,
                Email = input.Email,
                Website = input.Website,
                ContactName = input.ContactName,
                ContactTitle = input.ContactTitle,
                ContactPosition = input.ContactPosition,
                ContactAddress = input.ContactAddress,
                ContactEmail = input.ContactEmail,
                IsActive = input.IsActive,
                RecordType = input.RecordType,
                Balance = input.Balance,
                TranID = input.TranID,
                BalanceDebit = input.BalanceDebit,
                BalanceCredit = input.BalanceCredit
            };

            if (input.ID.HasValue)
            {
                c.ID = input.ID.Value;
                return Update(c);
            }
            else
            {
                return Create(c);
            }
        }


        /// <summary>
        /// copy from old project
        /// select customer to create invoice
        /// </summary>
        /// <param name="claimID"></param>
        /// <returns></returns>
        public List<CustomerView> GetLstCustomerByClaimID(string claimID)
        {
            var context = _sqlExecuter.GetTLSDBContext();

            var query = (from c in context.Customers
                         join cl in context.Claims on c.Id equals cl.InsurerID
                         where cl.Id == claimID

                         select new CustomerView
                         {
                             CustomerID = c.Id,
                             CustomerName = c.Name,
                             BrandName = c.BrandName,
                             Address = c.Address,
                             Phone = c.Phone,
                             Email = c.Email,
                             Website = c.Website,
                             ContactName = c.ContactName,
                             ContactTitle = c.ContactTitle,
                             ContactPosition = c.ContactPosition,
                             ContactAddress = c.ContactAddress,
                             ContactPhone = c.ContactPhone,
                             ContactEmail = c.ContactEmail,
                             IsActive = c.IsActive,
                             RecordType = c.RecordType
                         });
            var query2 = (
                                from c in context.Customers
                                join coCl in context.CoOwnerClaims on c.Id equals coCl.CustomerID
                                where coCl.ClaimID == claimID
                                select new CustomerView
                                {
                                    CustomerID = c.Id,
                                    CustomerName = c.Name,
                                    BrandName = c.BrandName,
                                    Address = c.Address,
                                    Phone = c.Phone,
                                    Email = c.Email,
                                    Website = c.Website,
                                    ContactName = c.ContactName,
                                    ContactTitle = c.ContactTitle,
                                    ContactPosition = c.ContactPosition,
                                    ContactAddress = c.ContactAddress,
                                    ContactPhone = c.ContactPhone,
                                    ContactEmail = c.ContactEmail,
                                    IsActive = c.IsActive,
                                    RecordType = c.RecordType
                                }
                        );
            List<CustomerView> rs = query.OrderBy(c => c.CustomerName).ToList();
            rs.AddRange(query2.OrderBy(c => c.CustomerName).ToList());
            return rs;

        }

        /// <summary>
        /// copy from old project
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public CustomerView GetInfo(int CustomerID)
        {
            CustomerView info = new CustomerView();
            var context = _sqlExecuter.GetTLSDBContext();

            var query = from c in context.Customers
                        where c.Id == CustomerID
                        select new CustomerView
                        {
                            CustomerID = c.Id,
                            CustomerName = c.Name,
                            BrandName = c.BrandName,
                            Address = c.Address,
                            Phone = c.Phone,
                            Email = c.Email,
                            Website = c.Website,
                            ContactName = c.ContactName,
                            ContactTitle = c.ContactTitle,
                            ContactPosition = c.ContactPosition,
                            ContactAddress = c.ContactAddress,
                            ContactPhone = c.ContactPhone,
                            ContactEmail = c.ContactEmail,
                            IsActive = c.IsActive,
                            RecordType = c.RecordType
                        };
            info = query.FirstOrDefault();
            return info;

        }
    }
}
