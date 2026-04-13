using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.EntityFramework;
using triluatsoft.tls.OldTable.dto;

namespace triluatsoft.tls.OldTable
{
    public class ContributionAdjustmentAppService : tlsAppServiceBase, IContributionAdjustmentAppService
    {
        private readonly ISqlExecuter _sqlExecuter;
        private readonly IRepository<ContributionAdjustment> _contributionAdjustmentRepository;

        public ContributionAdjustmentAppService(ISqlExecuter sqlExecuter,
            IRepository<ContributionAdjustment> contributionAdjustmentRepository)
        {
            _sqlExecuter = sqlExecuter;
            _contributionAdjustmentRepository = contributionAdjustmentRepository;
        }

        public string DeleteById(int taskId)
        {
            ContributionAdjustment contributionAdjustment = _contributionAdjustmentRepository.FirstOrDefault(taskId);
            _contributionAdjustmentRepository.Delete(contributionAdjustment);
            return "ok";
        }

        public PagedResultDto<ContributionAdjustmentSearchOptions> GetAll(ContributionSearchOption option)
        {
            //TODO ADD PERMISSION                        
            var curr_user = GetCurrentUser();
            var curr_role = "";
            var context = _sqlExecuter.GetTLSDBContext();

            var role_query = from r in context.Roles
                             join ur in context.UserRoles on r.Id equals ur.RoleId
                             where ur.UserId == curr_user.Id
                             select new RoleView
                             {
                                 ID = r.Id,
                                 Name = r.Name,
                                 DisplayName = r.DisplayName
                             };
            var roles = role_query.ToList();
            foreach (var role in roles)
            {
                curr_role = role.Name;
            }

            option.SearchAll = true;            
            var query = from con in context.ContributionAdjustments
                        join emp in context.Employees on con.EmployeeID equals emp.Id
                        where (option.DebitOrCredit == true ? con.AdjustAMT >= 0: option.DebitOrCredit == null ? con.AdjustAMT > 0 ||  con.AdjustAMT <= 0 : con.AdjustAMT <= 0)
                           && (option.StartDate == null || con.AdjustDate >= option.StartDate)
                           && (option.EndDate == null || con.AdjustDate <= option.EndDate)
                           && (((option.EmployeeID == null) && ((curr_role=="Admin") || (con.EmployeeID.Value == curr_user.EmployeeId))) || con.EmployeeID == option.EmployeeID)
                        select new ContributionAdjustmentSearchOptions
                        {
                            ID = con.Id,
                            EmployeeID = con.EmployeeID,
                            EmployeeName = emp.Name,
                            AdjustDate = con.AdjustDate,
                            CreateDate = con.CreateDate,
                            CreateBy = con.CreateBy,
                            Description = con.Description,
                            AdjustAMT = con.AdjustAMT,
                        };
            query = query.OrderByDescending(t => t.CreateDate);

            //phan trang
            var total = query.Count();
            var page = option.Page;
            var page1 = page - 1;
            query = query.Skip(option.PageSize * page1)
                .Take(option.PageSize);
            var list = query.ToList();
            var result = new PagedResultDto<ContributionAdjustmentSearchOptions>()
            {
                TotalCount = total,
                Items = list
            };

            return result;
        }

        public ContributionAdjustmentSearchOptions GetById(int taskId)
        {
            ContributionAdjustment contributionAdjustment = _contributionAdjustmentRepository.FirstOrDefault(taskId);
            if (contributionAdjustment == null) return null;
            ContributionAdjustmentSearchOptions list = new ContributionAdjustmentSearchOptions
            {
                ID = contributionAdjustment.Id,
                EmployeeID = contributionAdjustment.EmployeeID,
                AdjustAMT = contributionAdjustment.AdjustAMT,
                AdjustDate = contributionAdjustment.AdjustDate,
                CreateBy = contributionAdjustment.CreateBy,
                CreateDate = contributionAdjustment.CreateDate,
                Description = contributionAdjustment.Description
            };
            return list;
        }

        public string Save(CreateOrUpdateContributionAdjustmentInput input)
        {
            ContributionAdjustment contributionAdjustment;
            if (input.ID.HasValue)
            {
                contributionAdjustment = _contributionAdjustmentRepository.FirstOrDefault(input.ID.Value);
            }
            else
            {
                contributionAdjustment = new ContributionAdjustment();
            }
            var currUser = GetCurrentUser();
            if (!input.EmployeeID.HasValue)
                contributionAdjustment.EmployeeID = (int)currUser.EmployeeId;
            else
                contributionAdjustment.EmployeeID = input.EmployeeID.Value;
            
            contributionAdjustment.AdjustAMT = input.AdjustAMT;
            contributionAdjustment.AdjustDate = input.AdjustDate;
            contributionAdjustment.CreateBy = (int)currUser.EmployeeId;
            contributionAdjustment.Description = input.Description;
            contributionAdjustment.CreateDate = DateTime.Now;
            if (!input.ID.HasValue)
                _contributionAdjustmentRepository.InsertAndGetId(contributionAdjustment);
            else
                _contributionAdjustmentRepository.Update(contributionAdjustment);
            return "ok";
        }
    }
}
