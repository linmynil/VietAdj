using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using triluatsoft.tls.EntityFramework;
using triluatsoft.tls.OldTable.dto;
using triluatsoft.tls.OldTable.OldViewClass;

namespace triluatsoft.tls.OldTable
{
    public class BorderauxStatusAppService : tlsAppServiceBase, IBorderauxStatusAppService
    {
        private readonly ISqlExecuter _sqlExecuter;
        public BorderauxStatusAppService(ISqlExecuter sqlExecuter)
        {
            _sqlExecuter = sqlExecuter;
        }
        public List<BordereauxStatusDBObj> GetAll()
        {
            List<BordereauxStatusDBObj> list = _sqlExecuter.GetDatabase().SqlQuery<BordereauxStatusDBObj>("Select * from BordereauxStatus").ToList();
            
            return list;
        }

        private int Create(BordereauxStatusDBObj input)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("INSERT INTO BordereauxStatus(ID, Name, isActive) VALUES(@p0, @p1, @p2)", input.ID, input.Name, input.isActive);
        }

        public int Delete(string ID)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("DELETE FROM BordereauxStatus WHERE ID = @p0", ID);
        }
        private int Update(BordereauxStatusDBObj input)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("UPDATE BordereauxStatus SET isActive=@p0 WHERE ID = @p1", input.isActive, input.ID);
        }

        public int CreateOrUpdate(CreateOrUpdateBorderauxStatusInput input)
        {
            BordereauxStatusDBObj s = new BordereauxStatusDBObj(){ ID = input.ID, Name = input.Name, isActive = input.isActive };

            BordereauxStatusDBObj exist = _sqlExecuter.GetDatabase().SqlQuery<BordereauxStatusDBObj>("Select * from BordereauxStatus Where ID = @p0", input.ID).FirstOrDefault();

            if (exist!=null)
            {
                return Update(s);
            }
            else
            {
                return Create(s);
            }
        }

        /// <summary>
        /// hvtam-19032016: Get activeBordereauxStatus
        /// </summary>
        /// <returns></returns>
        public List<BordereauxStatusView> GetActiveList()
        {
            var context = _sqlExecuter.GetTLSDBContext();
            {
                var query = (from bstatus in context.BordereauxStatuses
                             where bstatus.isActive == true
                             select new BordereauxStatusView
                             {
                                 StatusID = bstatus.Id,
                                 Name = bstatus.Name,
                                 IsActive = bstatus.isActive,   //hvtam-19032016
                                 IsUsed = bstatus.Bordereaux.Any()
                             });

                return query.OrderBy(s => s.StatusID).ToList();
            }
        }
    }
}