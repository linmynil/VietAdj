using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using System.Data;
using System.Linq;
using System;
using triluatsoft.tls.EntityFramework;
using triluatsoft.tls.OldTable.dto;
using triluatsoft.tls.ClaimManagement;

namespace triluatsoft.tls.OldTable
{
    public class ClaimProcessAppService : tlsAppServiceBase, IClaimProcessAppService
    {
        private readonly IRepository<ClaimProcess> _claimProcessRepo;
        private readonly ISqlExecuter _sqlExecuter;
        private readonly IRepository<Claim, string> _claimTableRepo;
        private readonly IRepository<Customer> _customerRepo;
        private readonly IRepository<EmployeeClaim> _employeeClaimRepo;
        private readonly IEmployeeAppService _employeeService;        
        private readonly ITasMailAppService _emailSenderVAJ;


        public ClaimProcessAppService(ISqlExecuter sqlExecuter, IRepository<Claim, string> claimTableRepo             
            , IRepository<EmployeeClaim> employeeClaimRepo
            , IEmployeeAppService employeeService
            , ITasMailAppService emailSenderVAJ
            , IRepository<ClaimProcess> claimProcessRepo
            , IRepository<Customer> customerRepo
            )
        {
            _sqlExecuter = sqlExecuter;
            _claimTableRepo = claimTableRepo;            
            _employeeClaimRepo = employeeClaimRepo;            
            _employeeService = employeeService;            
            _emailSenderVAJ = emailSenderVAJ;
            _claimProcessRepo = claimProcessRepo;
            _customerRepo = customerRepo;
        }
        public List<ClaimProcessDto> GetAll(string ClaimID)
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

            List<string> myClaims = new List<string>();
            var list_claim = _employeeClaimRepo.GetAll()
                .Where(ec => ec.EmployeeID == curr_user.EmployeeId)
                .Select(ec => ec.ClaimID)
                .ToList();

            var claimList = _claimProcessRepo.GetAll()
                .Where(c => (curr_role == "Admin" || list_claim.Contains(c.ClaimID))
                && (string.IsNullOrEmpty(ClaimID) || ClaimID == "All" || c.ClaimID == ClaimID))
                .ToList();

            List<ClaimProcessDto> returnClaims = new List<ClaimProcessDto>();

            foreach (ClaimProcess clp in claimList)
            {
                ClaimProcessDto newItem = new ClaimProcessDto();
                newItem.Id = clp.Id;
                newItem.ClaimID = clp.ClaimID;
                newItem.WaitDocRequest = false;
                newItem.EnPREL2Request = false;
                newItem.EnINTL2Request = false;
                newItem.EnFINL2Request = false;
                //if (!clp.IsAck)
                if (!clp.IsAck.HasValue)
                {                    
                    newItem.LastStatus = "Waiting for Acknowledgement";
                    newItem.NextAction = 1;
                }

                if (clp.IsAck.HasValue)
                {
                    if (clp.IsAck.Value)
                    {
                        newItem.LastStatus = "Waiting for First Site Survey";
                        newItem.NextAction = 16;
                    } else
                    {
                        newItem.LastStatus = "Waiting for Acknowledgement";
                        newItem.NextAction = 1;
                    }                    
                }

                if (clp.IsFirstSurvey.HasValue)
                {
                    if (clp.IsFirstSurvey.Value)
                    {
                        newItem.LastStatus = "Waiting for Meeting Note";
                        newItem.NextAction = 2;
                    }
                }

                if (clp.IsMeetingNote.HasValue)
                {
                    if (clp.IsMeetingNote.Value)
                    {
                        newItem.LastStatus = "Waiting for Completing Site Survey";
                        newItem.NextAction = 17;
                    }                    
                }

                if (clp.IsCompleteSurvey.HasValue)
                {
                    if (clp.IsCompleteSurvey.Value)
                    {
                        newItem.LastStatus = "Waiting for ILA";
                        newItem.NextAction = 3;
                    }
                }
                
                if (clp.IsILA.HasValue)
                {
                    if (clp.IsILA.Value)
                    {
                        newItem.LastStatus = "Waiting for PRE L1";
                        newItem.NextAction = 4;
                        newItem.EnPREL2Request = true;
                        if (!clp.IsDocRequest.HasValue)
                        {
                            newItem.WaitDocRequest = true;                            
                        }
                        if (clp.IsRequestPREL2.HasValue)
                        {
                            if (clp.IsRequestPREL2.Value)
                            {
                                newItem.EnPREL2Request = false;
                            }
                        }
                    }                    
                }                

                if (clp.IsPREL1.HasValue)
                {
                    if (clp.IsPREL1.Value)
                    {
                        if (clp.IsRequestPREL2.HasValue)
                        {
                            if (clp.IsRequestPREL2.Value)
                            {
                                if (clp.IsPREL2.HasValue)
                                {
                                    if (clp.IsPREL2.Value)
                                    {
                                        newItem.LastStatus = "Waiting for confirm PRE report";
                                        newItem.NextAction = 19;
                                    } else
                                    {
                                        newItem.LastStatus = "Waiting for PRE L2";
                                        newItem.NextAction = 5;
                                    }
                                } else
                                {
                                    newItem.LastStatus = "Waiting for PRE L2";
                                    newItem.NextAction = 5;
                                }                                
                            } else
                            {
                                newItem.LastStatus = "Waiting for confirm PRE report";
                                newItem.NextAction = 19;
                            }
                        } else
                        {
                            newItem.LastStatus = "Waiting for confirm PRE report";
                            newItem.NextAction = 19;
                        }                        
                    }                    
                }

                if (clp.IsConfirmPRE.HasValue)
                {
                    if (clp.IsConfirmPRE.Value)
                    {
                        newItem.EnPREL2Request = false;
                        newItem.LastStatus = "Waiting for PRE hard copy";
                        newItem.NextAction = 6;
                    }
                }
                

                if (clp.IsPREHC.HasValue)
                {
                    if (clp.IsPREHC.Value)
                    {
                        newItem.EnPREL2Request = false;
                        newItem.LastStatus = "Waiting for Insured’s request for payment on account/Insurer’s instructions on writing Interim report";
                        newItem.NextAction = 20;
                    }                        
                }

                if (clp.IsRequestINTL1.HasValue)
                {
                    if (clp.IsRequestINTL1.Value)
                    {
                        newItem.EnINTL2Request = true;
                        newItem.LastStatus = "Waiting for INT L1";
                        newItem.NextAction = 7;
                        if (clp.IsRequestINTL2.HasValue)
                        {
                            if (clp.IsRequestINTL2.Value)
                            {
                                newItem.EnINTL2Request = false;
                            }
                        }
                    }
                }

                if (clp.IsINTL1.HasValue)
                {
                    if (clp.IsRequestINTL2.HasValue)
                    {
                        if (clp.IsRequestINTL2.Value)
                        {
                            if (clp.IsINTL2.HasValue)
                            {
                                if (clp.IsINTL2.Value)
                                {
                                    newItem.LastStatus = "Waiting for confirm INT report";
                                    newItem.NextAction = 22;
                                }
                                else
                                {
                                    newItem.LastStatus = "Waiting for INT L2";
                                    newItem.NextAction = 8;
                                }
                            }
                            else
                            {
                                newItem.LastStatus = "Waiting for INT L2";
                                newItem.NextAction = 8;
                            }
                        }
                        else
                        {
                            newItem.LastStatus = "Waiting for confirm INT report";
                            newItem.NextAction = 22;
                        }
                    }
                    else
                    {
                        newItem.LastStatus = "Waiting for confirm INT report";
                        newItem.NextAction = 22;
                    }                    
                }

                if (clp.IsConfirmINT.HasValue)
                {
                    if (clp.IsConfirmINT.Value)
                    {
                        newItem.EnINTL2Request = false;
                        newItem.LastStatus = "Waiting for INT hard copy";
                        newItem.NextAction = 9;
                    }
                }                

                if (clp.IsINTHC.HasValue)
                {
                    if (clp.IsINTHC.Value)
                    {
                        newItem.EnINTL2Request = false;
                        newItem.LastStatus = "Waiting for Insured fully provides claim documentation";
                        newItem.NextAction = 23;                        
                    }                        
                }

                if (clp.IsRequestFINL1.HasValue)
                {
                    if (clp.IsRequestFINL1.Value)
                    {
                        newItem.EnFINL2Request = true;
                        newItem.LastStatus = "Waiting for FIN L1";
                        newItem.NextAction = 10;
                        if (clp.IsRequestFINL2.HasValue)
                        {
                            if (clp.IsRequestFINL2.Value)
                            {
                                newItem.EnFINL2Request = false;
                            }
                        }
                    }
                }

                if (clp.IsFINL1.HasValue)
                {
                    if (clp.IsFINL1.Value)
                    {
                        if (clp.IsRequestFINL2.HasValue)
                        {
                            if (clp.IsRequestFINL2.Value)
                            {
                                if (clp.IsFINL2.HasValue)
                                {
                                    if (clp.IsFINL2.Value)
                                    {
                                        newItem.LastStatus = "Waiting for confirm FIN report";
                                        newItem.NextAction = 25;
                                    }
                                    else
                                    {
                                        newItem.LastStatus = "Waiting for FIN L2";
                                        newItem.NextAction = 11;
                                    }
                                }
                                else
                                {
                                    newItem.LastStatus = "Waiting for FIN L2";
                                    newItem.NextAction = 11;
                                }
                            }
                            else
                            {
                                newItem.LastStatus = "Waiting for confirm FIN report";
                                newItem.NextAction = 25;
                            }
                        }
                        else
                        {
                            newItem.LastStatus = "Waiting for confirm FIN report";
                            newItem.NextAction = 25;
                        }
                        
                    }                        
                }

                if (clp.IsConfirmFIN.HasValue)
                {
                    if (clp.IsConfirmFIN.Value)
                    {
                        newItem.EnFINL2Request = false;
                        newItem.LastStatus = "Waiting for FIN hard copy";
                        newItem.NextAction = 12;
                    }
                }                
                
                if (clp.IsFINHC.HasValue)
                {
                    if (clp.IsFINHC.Value)
                    {
                        newItem.LastStatus = "Waiting for submitting";
                        newItem.NextAction = 13;
                    }                        
                }

                if (clp.IsSubmit.HasValue)
                {
                    if (clp.IsSubmit.Value)
                    {
                        newItem.LastStatus = "Submitted";
                        newItem.NextAction = 14;
                    }                        
                }

                if (newItem.WaitDocRequest)
                {
                    newItem.LastStatus += " and Document Request";
                }

                returnClaims.Add(newItem);
            }

            return returnClaims;
        }

        public List<string> GetProcessClaim()
        {
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
                if (curr_role != "Admin")
                {
                    curr_role = role.Name;
                }                
            }

            List<string> myClaims = new List<string>();
            var list_claim = _employeeClaimRepo.GetAll()
                .Where(ec => ec.EmployeeID == curr_user.EmployeeId)
                .Select(ec => ec.ClaimID)
                .ToList();
            return _claimProcessRepo.GetAll()                       
                       .Where(c => (curr_role == "Admin" || list_claim.Contains(c.ClaimID))) //open
                       .OrderBy(c => c.ClaimID) 
                       .Select(c => c.ClaimID)                        
                        .ToList();            
        }

        public string UpdateClaimProcess(int ClaimProcessID, int UpdateStatus)
        {
            var claimp = _claimProcessRepo.Get(ClaimProcessID);

            var user = GetCurrentUser();

            var claim = _claimTableRepo.Get(claimp.ClaimID);
            var insurer = _customerRepo.Get(claim.InsurerID.Value);

            var AMClaim = _employeeService.GetUserInfo(claim.AccountManagerID.Value);

            List<EmployeeView> adj_list = GetEmpByClaim(claimp.ClaimID);
            VAJMailList mailList = new VAJMailList();            
            foreach (EmployeeView adj in adj_list)
            {
                if (adj.Email != null)
                {
                    mailList.toList.Add(adj.Email);
                }
            }
            mailList.ccList.Add("bod@vietadjusters.com");
            mailList.ccList.Add("managers@vietadjusters.com");
            
            switch (UpdateStatus)
            {
                case 1:
                    claimp.IsAck = true;
                    claimp.AckTime = DateTime.Now;
                    claimp.AckBy = (int)user.EmployeeId;                    
                    _claimProcessRepo.UpdateAsync(claimp);
                    break;
                case 2:
                    claimp.IsMeetingNote = true;
                    claimp.MeetingNoteTime = DateTime.Now;
                    claimp.MeetingNoteBy = (int)user.EmployeeId;                   
                    _claimProcessRepo.UpdateAsync(claimp);
                    break;
                case 3:
                    claimp.IsILA = true;
                    claimp.ILATime = DateTime.Now;
                    claimp.ILABy = (int)user.EmployeeId;                    
                    claimp.DocRequestDeadline = DateTime.Now.AddHours(24);
                    claimp.PREL1Deadline = claimp.ILATime.Value.AddHours(120);
                    _claimProcessRepo.UpdateAsync(claimp);

                    VAJMailList pre_mailList = new VAJMailList();
                    pre_mailList.toList.Add(AMClaim.Email);                    
                    foreach (string cc in mailList.toList)
                    {
                        if (cc != AMClaim.Email)
                        {
                            pre_mailList.ccList.Add(cc);
                        }
                    }
                    pre_mailList.ccList.Add("bod@vietadjusters.com");
                    pre_mailList.ccList.Add("managers@vietadjusters.com");

                    _emailSenderVAJ.SendDocumentRequest(claim.Id, insurer.BrandName, claimp.DocRequestDeadline?.ToString("HH:mm, dd-MM-yyyy"), mailList);
                    _emailSenderVAJ.SendPRE(claim.Id, insurer.BrandName, claimp.PREL1Deadline?.ToString("HH:mm, dd-MM-yyyy"), pre_mailList);
                    break;
                case 4:
                    claimp.IsPREL1 = true;
                    claimp.PREL1Time = DateTime.Now;
                    claimp.PREL1By = (int)user.EmployeeId;                    

                    if (claimp.IsRequestPREL2.HasValue)
                    {
                        if (claimp.RequestPREL2Time.HasValue)
                        {
                            if (claimp.RequestPREL2Time.Value.Subtract(claimp.PREL1Time.Value).Seconds < 0)
                            {
                                claimp.PREL2Deadline = DateTime.Now.AddHours(120);

                                VAJMailList pre2_mailList = new VAJMailList();
                                pre2_mailList.toList.Add(AMClaim.Email);
                                foreach (string cc in mailList.toList)
                                {
                                    if (cc != AMClaim.Email)
                                    {
                                        pre2_mailList.ccList.Add(cc);
                                    }
                                }
                                pre2_mailList.ccList.Add("bod@vietadjusters.com");
                                pre2_mailList.ccList.Add("managers@vietadjusters.com");
                                _emailSenderVAJ.SendPRE(claim.Id, insurer.BrandName, claimp.PREL2Deadline?.ToString("HH:mm, dd-MM-yyyy"), pre2_mailList);
                            }
                        }
                    }                                        
                    _claimProcessRepo.UpdateAsync(claimp);
                    
                    break;
                case 5:
                    claimp.IsPREL2 = true;
                    claimp.PREL2Time = DateTime.Now;
                    claimp.PREL2By = (int)user.EmployeeId;
                    //claimp.PREHCDeadline = DateTime.Now.AddHours(48);
                    _claimProcessRepo.UpdateAsync(claimp);

                    //_emailSenderVAJ.SendPREHardCopy(claim.Id, claimp.PREHCDeadline?.ToString("HH:mm, dd-MM-yyyy"), cc_list);
                    break;
                case 6:
                    claimp.IsPREHC = true;
                    claimp.PREHCTime = DateTime.Now;
                    claimp.PREHCBy = (int)user.EmployeeId;
                    //claimp.INTL1Deadline = DateTime.Now.AddHours(120);
                    _claimProcessRepo.UpdateAsync(claimp);

                    //_emailSenderVAJ.SendINT(claim.Id, claimp.INTL1Deadline?.ToString("HH:mm, dd-MM-yyyy"), cc_list);
                    break;
                case 7:
                    claimp.IsINTL1 = true;
                    claimp.INTL1Time = DateTime.Now;
                    claimp.INTL1By = (int)user.EmployeeId;                    

                    if (claimp.IsRequestINTL2.HasValue)
                    {
                        if (claimp.RequestINTL2Time.HasValue)
                        {
                            if (claimp.RequestINTL2Time.Value.Subtract(claimp.INTL1Time.Value).Seconds < 0)
                            {
                                claimp.INTL2Deadline = DateTime.Now.AddHours(120);

                                VAJMailList int_mailList = new VAJMailList();
                                int_mailList.toList.Add(AMClaim.Email);
                                foreach (string cc in mailList.toList)
                                {
                                    if (cc != AMClaim.Email)
                                    {
                                        int_mailList.ccList.Add(cc);
                                    }
                                }
                                int_mailList.ccList.Add("bod@vietadjusters.com");
                                int_mailList.ccList.Add("managers@vietadjusters.com");
                                _emailSenderVAJ.SendINT(claim.Id, insurer.BrandName, claimp.INTL2Deadline?.ToString("HH:mm, dd-MM-yyyy"), int_mailList);
                            }
                        }
                    }                    
                    _claimProcessRepo.UpdateAsync(claimp);                    
                    break;
                case 8:
                    claimp.IsINTL2 = true;
                    claimp.INTL2Time = DateTime.Now;
                    claimp.INTL2By = (int)user.EmployeeId;
                    //claimp.INTHCDeadline = DateTime.Now.AddHours(48);
                    _claimProcessRepo.UpdateAsync(claimp);

                    //_emailSenderVAJ.SendINTHardCopy(claim.Id, claimp.INTHCDeadline?.ToString("HH:mm, dd-MM-yyyy"), cc_list);
                    break;
                case 9:
                    claimp.IsINTHC = true;
                    claimp.INTHCTime = DateTime.Now;
                    claimp.INTHCBy = (int)user.EmployeeId;
                    //claimp.FINL1Deadline = DateTime.Now.AddHours(120);
                    _claimProcessRepo.UpdateAsync(claimp);

                    //_emailSenderVAJ.SendFIN(claim.Id, claimp.FINL1Deadline?.ToString("HH:mm, dd-MM-yyyy"), cc_list);
                    break;
                case 10:
                    claimp.IsFINL1 = true;
                    claimp.FINL1Time = DateTime.Now;
                    claimp.FINL1By = (int)user.EmployeeId;
                    if (claimp.IsRequestFINL2.HasValue)
                    {
                        if (claimp.RequestFINL2Time.HasValue)
                        {
                            if (claimp.RequestFINL2Time.Value.Subtract(claimp.FINL1Time.Value).Seconds < 0)
                            {
                                claimp.FINL2Deadline = DateTime.Now.AddHours(120);

                                VAJMailList fin_mailList = new VAJMailList();
                                fin_mailList.toList.Add(AMClaim.Email);
                                foreach (string cc in mailList.toList)
                                {
                                    if (cc != AMClaim.Email)
                                    {
                                        fin_mailList.ccList.Add(cc);
                                    }
                                }
                                fin_mailList.ccList.Add("bod@vietadjusters.com");
                                fin_mailList.ccList.Add("managers@vietadjusters.com");
                                _emailSenderVAJ.SendFIN(claim.Id, insurer.BrandName, claimp.FINL2Deadline?.ToString("HH:mm, dd-MM-yyyy"), fin_mailList);
                            }
                        }
                    }                    
                    _claimProcessRepo.UpdateAsync(claimp);                    
                    break;
                case 11:
                    claimp.IsFINL2 = true;
                    claimp.FINL2Time = DateTime.Now;
                    claimp.FINL2By = (int)user.EmployeeId;
                    //claimp.FINHCDeadline = DateTime.Now.AddHours(48);
                    _claimProcessRepo.UpdateAsync(claimp);

                    //_emailSenderVAJ.SendFINHardCopy(claim.Id, claimp.FINHCDeadline?.ToString("HH:mm, dd-MM-yyyy"), cc_list);
                    break;
                case 12:
                    claimp.IsFINHC = true;
                    claimp.FINHCTime = DateTime.Now;
                    claimp.FINHCBy = (int)user.EmployeeId;
                    //claimp.SubmitDeadline = DateTime.Now.AddHours(72);
                    _claimProcessRepo.UpdateAsync(claimp);

                    //_emailSenderVAJ.SendTimeSheet(claim.Id, claimp.SubmitDeadline?.ToString("HH:mm, dd-MM-yyyy"), cc_list);
                    break;
                case 13:
                    claimp.IsSubmit = true;
                    claimp.SubmitTime = DateTime.Now;
                    claimp.SubmitBy = (int)user.EmployeeId;
                    _claimProcessRepo.UpdateAsync(claimp);
                    break;
                case 15:
                    claimp.IsDocRequest = true;
                    claimp.DocRequestTime = DateTime.Now;
                    claimp.DocRequestBy = (int)user.EmployeeId;                    
                    _claimProcessRepo.UpdateAsync(claimp);                    
                    break;
                case 16:
                    claimp.IsFirstSurvey = true;
                    claimp.FirstSurveyTime = DateTime.Now;
                    claimp.FirstSurveyBy = (int)user.EmployeeId;
                    claimp.MeetingNoteDeadline = DateTime.Now.AddHours(24);
                    _claimProcessRepo.UpdateAsync(claimp);
                    
                    VAJMailList mn_mailList = new VAJMailList();
                    foreach (string t in mailList.toList)
                    {
                        mn_mailList.toList.Add(t);
                    }
                    foreach (string c in mailList.ccList)
                    {
                        mn_mailList.ccList.Add(c);                        
                    }
                    mailList.ccList.Add("bod@vietadjusters.com");
                    mailList.ccList.Add("managers@vietadjusters.com");
                    _emailSenderVAJ.SendMeetingNote(claim.Id, insurer.BrandName, claimp.MeetingNoteDeadline?.ToString("HH:mm, dd-MM-yyyy"), mn_mailList);
                    break;
                case 17:
                    claimp.IsCompleteSurvey = true;
                    claimp.CompleteSurveyTime = DateTime.Now;
                    claimp.CompleteSurveyBy = (int)user.EmployeeId;
                    claimp.ILADeadline = DateTime.Now.AddHours(24);
                    _claimProcessRepo.UpdateAsync(claimp);

                    var claimila = _claimTableRepo.Get(claimp.ClaimID);
                    var insurerila = _customerRepo.Get(claimila.InsurerID.Value);

                    VAJMailList ila_mailList = new VAJMailList();
                    ila_mailList.toList.Add(AMClaim.Email);
                    foreach(string cc in mailList.toList)
                    {
                        if (cc != AMClaim.Email)
                        {
                            ila_mailList.ccList.Add(cc);
                        }
                    }
                    ila_mailList.ccList.Add("bod@vietadjusters.com");
                    ila_mailList.ccList.Add("managers@vietadjusters.com");
                    _emailSenderVAJ.SendILA(claim.Id, insurerila.BrandName, claimp.ILADeadline?.ToString("HH:mm, dd-MM-yyyy"), ila_mailList);
                    break;
                case 18:
                    claimp.IsRequestPREL2 = true;
                    claimp.RequestPREL2Time = DateTime.Now;
                    claimp.RequestPREL2By = (int)user.EmployeeId;
                    if (claimp.PREL1Time.HasValue)
                    {
                        if (claimp.RequestPREL2Time.Value.Subtract(claimp.PREL1Time.Value).Seconds > 0)
                        {
                            claimp.PREL2Deadline = claimp.RequestPREL2Time.Value.AddHours(120);

                            VAJMailList pre3_mailList = new VAJMailList();
                            pre3_mailList.toList.Add(AMClaim.Email);
                            foreach (string cc in mailList.toList)
                            {
                                if (cc != AMClaim.Email)
                                {
                                    pre3_mailList.ccList.Add(cc);
                                }
                            }
                            pre3_mailList.ccList.Add("bod@vietadjusters.com");
                            pre3_mailList.ccList.Add("managers@vietadjusters.com");
                            _emailSenderVAJ.SendPRE(claim.Id, insurer.BrandName, claimp.PREL2Deadline?.ToString("HH:mm, dd-MM-yyyy"), pre3_mailList);
                        }
                    }
                    _claimProcessRepo.UpdateAsync(claimp);
                    break;
                case 19:
                    claimp.IsConfirmPRE = true;
                    claimp.ConfirmPRETime = DateTime.Now;
                    claimp.ConfirmPREBy = (int)user.EmployeeId;
                    claimp.PREHCDeadline = DateTime.Now.AddHours(48);
                    _claimProcessRepo.UpdateAsync(claimp);

                    VAJMailList pre_admin_mailList = new VAJMailList();
                    pre_admin_mailList.toList.Add("admin@vietadjusters.com");
                    pre_admin_mailList.ccList.Add("bod@vietadjusters.com");
                    pre_admin_mailList.ccList.Add("managers@vietadjusters.com");
                    foreach (string t in mailList.toList)
                    {
                        pre_admin_mailList.ccList.Add(t);
                    }
                    foreach(string c in mailList.ccList)
                    {
                        pre_admin_mailList.ccList.Add(c);
                    }
                    _emailSenderVAJ.SendPREHardCopy(claim.Id, insurer.BrandName, claimp.PREHCDeadline?.ToString("HH:mm, dd-MM-yyyy"), pre_admin_mailList);
                    break;
                case 20:
                    claimp.IsRequestINTL1 = true;
                    claimp.RequestINTL1Time = DateTime.Now;
                    claimp.RequestINTL1By = (int)user.EmployeeId;
                    claimp.INTL1Deadline = DateTime.Now.AddHours(120);
                    _claimProcessRepo.UpdateAsync(claimp);

                    VAJMailList int2_mailList = new VAJMailList();
                    int2_mailList.toList.Add(AMClaim.Email);
                    foreach (string cc in mailList.toList)
                    {
                        if (cc != AMClaim.Email)
                        {
                            int2_mailList.ccList.Add(cc);
                        }
                    }
                    int2_mailList.ccList.Add("bod@vietadjusters.com");
                    int2_mailList.ccList.Add("managers@vietadjusters.com");
                    _emailSenderVAJ.SendINT(claim.Id, insurer.BrandName, claimp.INTL1Deadline?.ToString("HH:mm, dd-MM-yyyy"), int2_mailList);
                    break;
                case 21:
                    claimp.IsRequestINTL2 = true;
                    claimp.RequestINTL2Time = DateTime.Now;
                    claimp.RequestINTL2By = (int)user.EmployeeId;
                    if (claimp.INTL1Time.HasValue)
                    {
                        if (claimp.RequestINTL2Time.Value.Subtract(claimp.INTL1Time.Value).Seconds > 0)
                        {
                            claimp.INTL2Deadline = claimp.RequestINTL2Time.Value.AddHours(120);

                            VAJMailList int3_mailList = new VAJMailList();
                            int3_mailList.toList.Add(AMClaim.Email);
                            foreach (string cc in mailList.toList)
                            {
                                if (cc != AMClaim.Email)
                                {
                                    int3_mailList.ccList.Add(cc);
                                }
                            }
                            int3_mailList.ccList.Add("bod@vietadjusters.com");
                            int3_mailList.ccList.Add("managers@vietadjusters.com");
                            _emailSenderVAJ.SendINT(claim.Id, insurer.BrandName, claimp.INTL2Deadline?.ToString("HH:mm, dd-MM-yyyy"), int3_mailList);
                        }
                    }
                    _claimProcessRepo.UpdateAsync(claimp);
                    break;
                case 22:
                    claimp.IsConfirmINT = true;
                    claimp.ConfirmINTTime = DateTime.Now;
                    claimp.ConfirmINTBy = (int)user.EmployeeId;
                    claimp.INTHCDeadline = DateTime.Now.AddHours(48);
                    _claimProcessRepo.UpdateAsync(claimp);

                    VAJMailList int_admin_mailList = new VAJMailList();
                    int_admin_mailList.toList.Add("admin@vietadjusters.com");
                    foreach(string t in mailList.toList)
                    {
                        int_admin_mailList.ccList.Add(t);
                    }
                    int_admin_mailList.ccList.Add("bod@vietadjusters.com");
                    int_admin_mailList.ccList.Add("managers@vietadjusters.com");
                    _emailSenderVAJ.SendINTHardCopy(claim.Id, insurer.BrandName, claimp.INTHCDeadline?.ToString("HH:mm, dd-MM-yyyy"), int_admin_mailList);
                    break;
                case 23:
                    claimp.IsRequestFINL1 = true;
                    claimp.RequestFINL1Time = DateTime.Now;
                    claimp.RequestFINL1By = (int)user.EmployeeId;
                    claimp.FINL1Deadline = DateTime.Now.AddHours(120);
                    _claimProcessRepo.UpdateAsync(claimp);

                    VAJMailList fin2_mailList = new VAJMailList();
                    fin2_mailList.toList.Add(AMClaim.Email);
                    foreach (string cc in mailList.toList)
                    {
                        if (cc != AMClaim.Email)
                        {
                            fin2_mailList.ccList.Add(cc);
                        }
                    }
                    fin2_mailList.ccList.Add("bod@vietadjusters.com");
                    fin2_mailList.ccList.Add("managers@vietadjusters.com");

                    _emailSenderVAJ.SendFIN(claim.Id, insurer.BrandName, claimp.FINL1Deadline?.ToString("HH:mm, dd-MM-yyyy"), fin2_mailList);
                    break;
                case 24:
                    claimp.IsRequestFINL2 = true;
                    claimp.RequestFINL2Time = DateTime.Now;
                    claimp.RequestFINL2By = (int)user.EmployeeId;
                    if (claimp.FINL1Time.HasValue)
                    {
                        if (claimp.RequestFINL2Time.Value.Subtract(claimp.FINL1Time.Value).Seconds > 0)
                        {
                            claimp.FINL2Deadline = claimp.RequestFINL2Time.Value.AddHours(120);

                            VAJMailList fin3_mailList = new VAJMailList();
                            fin3_mailList.toList.Add(AMClaim.Email);
                            foreach (string cc in mailList.toList)
                            {
                                if (cc != AMClaim.Email)
                                {
                                    fin3_mailList.ccList.Add(cc);
                                }
                            }
                            fin3_mailList.ccList.Add("bod@vietadjusters.com");
                            fin3_mailList.ccList.Add("managers@vietadjusters.com");
                            _emailSenderVAJ.SendFIN(claim.Id, insurer.BrandName, claimp.FINL2Deadline?.ToString("HH:mm, dd-MM-yyyy"), fin3_mailList);
                        }
                    }
                    _claimProcessRepo.UpdateAsync(claimp);
                    break;
                case 25:
                    claimp.IsConfirmFIN = true;
                    claimp.ConfirmFINTime = DateTime.Now;
                    claimp.ConfirmFINBy = (int)user.EmployeeId;
                    claimp.FINHCDeadline = DateTime.Now.AddHours(48);
                    claimp.SubmitDeadline = DateTime.Now.AddHours(72);
                    _claimProcessRepo.UpdateAsync(claimp);

                    VAJMailList fin_admin_mailList = new VAJMailList();
                    fin_admin_mailList.toList.Add("admin@vietadjusters.com");

                    VAJMailList submit_mailList = new VAJMailList();
                    submit_mailList.toList.Add("admin@vietadjusters.com");
                    foreach(string c in mailList.toList)
                    {
                        if ((c != "bod@vietadjusters.com") && c != "managers@vietadjusters.com")
                        {
                            fin_admin_mailList.ccList.Add(c);
                            submit_mailList.ccList.Add(c);
                        }
                    }
                    fin_admin_mailList.ccList.Add("bod@vietadjusters.com");
                    fin_admin_mailList.ccList.Add("managers@vietadjusters.com");
                    submit_mailList.ccList.Add("bod@vietadjusters.com");
                    submit_mailList.ccList.Add("managers@vietadjusters.com");
                    _emailSenderVAJ.SendFINHardCopy(claim.Id, insurer.BrandName, claimp.FINHCDeadline?.ToString("HH:mm, dd-MM-yyyy"), fin_admin_mailList);
                    _emailSenderVAJ.SendTimeSheet(claim.Id, insurer.BrandName, claimp.SubmitDeadline?.ToString("HH:mm, dd-MM-yyyy"), submit_mailList);
                    break;
                case 30:
                    claimp.IsPREL1 = true;
                    claimp.IsPREL2 = true;
                    claimp.IsPREHC = true;
                    claimp.IsINTL1 = true;
                    claimp.IsINTL2 = true;
                    claimp.IsINTHC = true;
                    claimp.IsFINL1 = true;
                    claimp.IsFINL2 = true;
                    claimp.IsFINHC = true;
                    claimp.SubmitDeadline = DateTime.Now.AddHours(72);
                    _claimProcessRepo.UpdateAsync(claimp);                    

                    VAJMailList submit_frr_mailList = new VAJMailList();
                    submit_frr_mailList.toList.Add("admin@vietadjusters.com");
                    foreach (string c in mailList.toList)
                    {
                        if ((c != "bod@vietadjusters.com") && c != "managers@vietadjusters.com")
                        {                            
                            submit_frr_mailList.ccList.Add(c);
                        }
                    }                    
                    submit_frr_mailList.ccList.Add("bod@vietadjusters.com");
                    submit_frr_mailList.ccList.Add("managers@vietadjusters.com");                    
                    _emailSenderVAJ.SendTimeSheet(claim.Id, insurer.BrandName, claimp.SubmitDeadline?.ToString("HH:mm, dd-MM-yyyy"), submit_frr_mailList);                    
                    break;
            }

            return "ok";
        }

        public List<EmployeeView> GetEmpByClaim(string claimID)
        {
            var users = UserManager.Users.ToList();
            var list = _employeeClaimRepo.GetAll()
                .Where(x => x.ClaimID == claimID)
                .Select(x => new EmployeeView { EmployeeID = x.EmployeeID })
                .ToList();
            list.ForEach(x =>
            {
                //x.Name = users.FirstOrDefault(y => y.EmployeeId == x.EmployeeID).Name;
                x.Name = (users.FirstOrDefault(y => y.EmployeeId == x.EmployeeID) != null) ? users.FirstOrDefault(y => y.EmployeeId == x.EmployeeID).Name : "";
                x.Email = (users.FirstOrDefault(y => y.EmployeeId == x.EmployeeID) != null) ? users.FirstOrDefault(y => y.EmployeeId == x.EmployeeID).EmailAddress : "";
            });

            return list;
        }

        public async System.Threading.Tasks.Task SendReminder()
        {            
            Logger.Debug(">>>>>>> Sending reminder mail at " + DateTime.Now.ToString());            
            //List<string> emaillist = new List<string>();
            //emaillist.Add("dpctam@triluatsoft.vn");
            //emaillist.Add("info@triluatsoft.vn");
            //await _emailSenderVAJ.SendClaimConfirmRemind("56.19.SG", "Test Insurer", "12-06-2019 09:50:00", "1", emaillist);            
            
            var curtime = DateTime.Now;

            var open_claim = _claimTableRepo.GetAll().Where(x => x.RefStatusID != 2).Select(x => x.Id).ToList();

            //Check Ack
            Logger.Debug(">>>>>>> Sending Ack reminder");
            var acklist = _claimProcessRepo.GetAll()
                .Where(c => (c.IsAck == null || c.IsAck == false) 
                && (c.AckDeadline.Value.Hour == curtime.Hour) && (open_claim.Contains(c.ClaimID))
                //&& (c.AckDeadline.Value.Minute == curtime.Minute)
                && (c.AckDeadline.Value.Day < curtime.Day)).ToList();

            if (acklist.Count > 0)
            {
                foreach(ClaimProcess ack in acklist)
                {
                    Logger.Debug(">>>>>>> Sending Ack reminder for " + ack.ClaimID);

                    var claimack = _claimTableRepo.Get(ack.ClaimID);
                    var insurerack = _customerRepo.Get(claimack.InsurerID.Value);

                    List<EmployeeView> adj_list = GetEmpByClaim(ack.ClaimID);
                    VAJMailList mailList = new VAJMailList();                    
                    foreach (EmployeeView adj in adj_list)
                    {
                        if (adj.Email != null)
                        {
                            mailList.toList.Add(adj.Email);
                        }
                    }                    
                    mailList.ccList.Add("bod@vietadjusters.com");
                    mailList.ccList.Add("managers@vietadjusters.com");

                    var reminderNo = Math.Ceiling(curtime.Subtract(ack.AckDeadline.Value).TotalDays).ToString();                    
                    await _emailSenderVAJ.SendClaimConfirmRemind(ack.ClaimID, insurerack.BrandName, ack.AckDeadline.Value.ToString("HH:mm, dd-MM-yyyy"), reminderNo, mailList);
                    
                }
            }


            //Check Meeting Note
            Logger.Debug(">>>>>>> Sending Meeting Note reminder");
            var mnlist = _claimProcessRepo.GetAll()
                .Where(c => (c.IsMeetingNote == null || c.IsMeetingNote == false) && (open_claim.Contains(c.ClaimID))
                && (c.MeetingNoteDeadline.Value.Hour == curtime.Hour)
                && (c.MeetingNoteDeadline.Value.Minute == curtime.Minute)
                && (c.MeetingNoteDeadline.Value.Day < curtime.Day)).ToList();

            if (mnlist.Count > 0)
            {
                foreach (ClaimProcess mn in mnlist)
                {
                    Logger.Debug(">>>>>>> Sending Meeting Note reminder for " + mn.ClaimID);

                    var claimmn = _claimTableRepo.Get(mn.ClaimID);
                    var insurermn = _customerRepo.Get(claimmn.InsurerID.Value);

                    List<EmployeeView> adj_list = GetEmpByClaim(mn.ClaimID);
                    VAJMailList mailList = new VAJMailList();
                    foreach (EmployeeView adj in adj_list)
                    {
                        if (adj.Email != null)
                        {
                            mailList.toList.Add(adj.Email);
                        }
                    }
                    mailList.toList.Add("bod@vietadjusters.com");
                    mailList.toList.Add("managers@vietadjusters.com");

                    var reminderNo = Math.Ceiling(curtime.Subtract(mn.MeetingNoteDeadline.Value).TotalDays).ToString();
                    await _emailSenderVAJ.SendMeetingNoteRemind(mn.ClaimID, insurermn.BrandName, mn.MeetingNoteDeadline.Value.ToString("HH:mm, dd-MM-yyyy"), reminderNo, mailList);

                }
            }

            //Check ILA
            Logger.Debug(">>>>>>> Sending ILA reminder");
            var ilalist = _claimProcessRepo.GetAll()
                .Where(c => (c.IsILA == null || c.IsILA == false) && (open_claim.Contains(c.ClaimID))
                && (c.ILADeadline.Value.Hour == curtime.Hour)
                && (c.ILADeadline.Value.Minute == curtime.Minute)
                && (c.ILADeadline.Value.Day < curtime.Day)).ToList();

            if (ilalist.Count > 0)
            {
                foreach (ClaimProcess ila in ilalist)
                {
                    Logger.Debug(">>>>>>> Sending ILA reminder for " + ila.ClaimID);

                    var claimila = _claimTableRepo.Get(ila.ClaimID);
                    var insurerila = _customerRepo.Get(claimila.InsurerID.Value);

                    var AMClaim = _employeeService.GetEmpInfo(claimila.AccountManagerID.Value);
                    VAJMailList mailList = new VAJMailList();                    
                    mailList.toList.Add(AMClaim.Email);

                    List<EmployeeView> adj_list = GetEmpByClaim(ila.ClaimID);                    
                    foreach (EmployeeView adj in adj_list)
                    {
                        if (adj.Email != null)
                        {
                            if (adj.Email != AMClaim.Email)
                            {
                                mailList.ccList.Add(adj.Email);
                            }                            
                        }
                    }                    
                    mailList.ccList.Add("bod@vietadjusters.com");
                    mailList.ccList.Add("managers@vietadjusters.com");

                    var reminderNo = Math.Ceiling(curtime.Subtract(ila.ILADeadline.Value).TotalDays).ToString();
                    await _emailSenderVAJ.SendILARemind(ila.ClaimID, insurerila.BrandName, ila.ILADeadline.Value.ToString("HH:mm, dd-MM-yyyy"), reminderNo, mailList);

                }
            }


            //Check Document Request
            Logger.Debug(">>>>>>> Sending Document Request reminder");
            var doclist = _claimProcessRepo.GetAll()
                .Where(c => (c.IsDocRequest == null || c.IsDocRequest == false) && (open_claim.Contains(c.ClaimID))
                && (c.DocRequestDeadline.Value.Hour == curtime.Hour)
                && (c.DocRequestDeadline.Value.Minute == curtime.Minute)
                && (c.DocRequestDeadline.Value.Day < curtime.Day)).ToList();

            if (doclist.Count > 0)
            {
                foreach (ClaimProcess doc in doclist)
                {
                    Logger.Debug(">>>>>>> Sending Document Request reminder for " + doc.ClaimID);

                    var claimdoc = _claimTableRepo.Get(doc.ClaimID);
                    var insurerdoc = _customerRepo.Get(claimdoc.InsurerID.Value);

                    List<EmployeeView> adj_list = GetEmpByClaim(doc.ClaimID);
                    VAJMailList mailList = new VAJMailList();
                    foreach (EmployeeView adj in adj_list)
                    {
                        if (adj.Email != null)
                        {
                            mailList.toList.Add(adj.Email);
                        }
                    }                    
                    mailList.ccList.Add("bod@vietadjusters.com");
                    mailList.ccList.Add("managers@vietadjusters.com");

                    var reminderNo = Math.Ceiling(curtime.Subtract(doc.DocRequestDeadline.Value).TotalDays).ToString();

                    await _emailSenderVAJ.SendDocumentRequestRemind(doc.ClaimID, insurerdoc.BrandName, doc.DocRequestDeadline.Value.ToString("HH:mm, dd-MM-yyyy"), reminderNo, mailList);

                }
            }

            //Check PRE L1
            Logger.Debug(">>>>>>> Sending PRE L1 reminder");
            var prel1list = _claimProcessRepo.GetAll()
                .Where(c => (c.IsPREL1 == null || c.IsPREL1 == false) && (open_claim.Contains(c.ClaimID))
                && (c.PREL1Deadline.Value.Hour == curtime.Hour)
                && (c.PREL1Deadline.Value.Minute == curtime.Minute)
                && (c.PREL1Deadline.Value.Day < curtime.Day)).ToList();

            if (prel1list.Count > 0)
            {
                foreach (ClaimProcess prel1 in prel1list)
                {
                    Logger.Debug(">>>>>>> Sending PRE L1 reminder for " + prel1.ClaimID);

                    var claimprel1 = _claimTableRepo.Get(prel1.ClaimID);
                    var insurerprel1 = _customerRepo.Get(claimprel1.InsurerID.Value);

                    var AMClaim = _employeeService.GetEmpInfo(claimprel1.AccountManagerID.Value);
                    VAJMailList mailList = new VAJMailList();                    
                    mailList.toList.Add(AMClaim.Email);

                    List<EmployeeView> adj_list = GetEmpByClaim(prel1.ClaimID);                    
                    foreach (EmployeeView adj in adj_list)
                    {
                        if (adj.Email != null)
                        {
                            if (adj.Email != AMClaim.Email)
                            {
                                mailList.ccList.Add(adj.Email);
                            }                            
                        }
                    }
                    mailList.ccList.Add("bod@vietadjusters.com");
                    mailList.ccList.Add("managers@vietadjusters.com");

                    var reminderNo = Math.Ceiling(curtime.Subtract(prel1.PREL1Deadline.Value).TotalDays).ToString();

                    await _emailSenderVAJ.SendPRERemind(prel1.ClaimID, insurerprel1.BrandName, prel1.PREL1Deadline.Value.ToString("HH:mm, dd-MM-yyyy"), reminderNo, mailList);

                }
            }

            //Check PRE L2
            Logger.Debug(">>>>>>> Sending PRE L2 reminder");
            var prel2list = _claimProcessRepo.GetAll()
                .Where(c => (c.IsPREL2 == null || c.IsPREL2 == false) && (open_claim.Contains(c.ClaimID))
                && (c.PREL2Deadline.Value.Hour == curtime.Hour)
                && (c.PREL2Deadline.Value.Minute == curtime.Minute)
                && (c.PREL2Deadline.Value.Day < curtime.Day)).ToList();

            if (prel2list.Count > 0)
            {
                foreach (ClaimProcess prel2 in prel2list)
                {
                    Logger.Debug(">>>>>>> Sending PRE L2 reminder for " + prel2.ClaimID);

                    var claimprel2 = _claimTableRepo.Get(prel2.ClaimID);
                    var insurerprel2 = _customerRepo.Get(claimprel2.InsurerID.Value);

                    var AMClaim = _employeeService.GetEmpInfo(claimprel2.AccountManagerID.Value);
                    VAJMailList mailList = new VAJMailList();
                    mailList.toList.Add(AMClaim.Email);

                    List<EmployeeView> adj_list = GetEmpByClaim(prel2.ClaimID);                    
                    foreach (EmployeeView adj in adj_list)
                    {
                        if (adj.Email != null)
                        {
                            if (adj.Email != AMClaim.Email)
                            {
                                mailList.ccList.Add(adj.Email);
                            }                            
                        }
                    }
                    mailList.ccList.Add("bod@vietadjusters.com");
                    mailList.ccList.Add("managers@vietadjusters.com");

                    var reminderNo = Math.Ceiling(curtime.Subtract(prel2.PREL2Deadline.Value).TotalDays).ToString();

                    await _emailSenderVAJ.SendPRERemind(prel2.ClaimID, insurerprel2.BrandName, prel2.PREL2Deadline.Value.ToString("HH:mm, dd-MM-yyyy"), reminderNo, mailList);

                }
            }

            //Check PRE HC
            Logger.Debug(">>>>>>> Sending PRE hard copy reminder");
            var prehclist = _claimProcessRepo.GetAll()
                .Where(c => (c.IsPREHC == null || c.IsPREHC == false) && (open_claim.Contains(c.ClaimID))
                && (c.PREHCDeadline.Value.Hour == curtime.Hour)
                && (c.PREHCDeadline.Value.Minute == curtime.Minute)
                && (c.PREHCDeadline.Value.Day < curtime.Day)).ToList();

            if (prehclist.Count > 0)
            {
                foreach (ClaimProcess prehc in prehclist)
                {
                    Logger.Debug(">>>>>>> Sending PRE hard copy reminder for " + prehc.ClaimID);

                    var claimprehc = _claimTableRepo.Get(prehc.ClaimID);
                    var insurerprehc = _customerRepo.Get(claimprehc.InsurerID.Value);

                    VAJMailList mailList = new VAJMailList();
                    mailList.toList.Add("admin@vietadjusters.com");
                    
                    List<EmployeeView> adj_list = GetEmpByClaim(prehc.ClaimID);                    
                    foreach (EmployeeView adj in adj_list)
                    {
                        if (adj.Email != null)
                        {
                            mailList.ccList.Add(adj.Email);
                        }
                    }
                    mailList.ccList.Add("bod@vietadjusters.com");
                    mailList.ccList.Add("managers@vietadjusters.com");

                    var reminderNo = Math.Ceiling(curtime.Subtract(prehc.PREHCDeadline.Value).TotalDays).ToString();

                    await _emailSenderVAJ.SendPREHardCopyRemind(prehc.ClaimID, insurerprehc.BrandName, prehc.PREHCDeadline.Value.ToString("HH:mm, dd-MM-yyyy"), reminderNo, mailList);

                }
            }

            //Check INT L1
            Logger.Debug(">>>>>>> Sending INT L1 reminder");
            var intl1list = _claimProcessRepo.GetAll()
                .Where(c => (c.IsINTL1 == null || c.IsINTL1 == false) && (open_claim.Contains(c.ClaimID))
                && (c.INTL1Deadline.Value.Hour == curtime.Hour)
                && (c.INTL1Deadline.Value.Minute == curtime.Minute)
                && (c.INTL1Deadline.Value.Day < curtime.Day)).ToList();

            if (intl1list.Count > 0)
            {
                foreach (ClaimProcess intl1 in intl1list)
                {
                    Logger.Debug(">>>>>>> Sending INT L1 reminder for " + intl1.ClaimID);
                    
                    var claimintl1 = _claimTableRepo.Get(intl1.ClaimID);
                    var insurerintl1 = _customerRepo.Get(claimintl1.InsurerID.Value);

                    var AMClaim = _employeeService.GetEmpInfo(claimintl1.AccountManagerID.Value);

                    VAJMailList mailList = new VAJMailList();
                    mailList.toList.Add(AMClaim.Email);

                    List<EmployeeView> adj_list = GetEmpByClaim(intl1.ClaimID);                    
                    foreach (EmployeeView adj in adj_list)
                    {
                        if (adj.Email != null)
                        {
                            mailList.ccList.Add(adj.Email);
                        }
                    }
                    mailList.ccList.Add("bod@vietadjusters.com");
                    mailList.ccList.Add("managers@vietadjusters.com");

                    var reminderNo = Math.Ceiling(curtime.Subtract(intl1.INTL1Deadline.Value).TotalDays).ToString();

                    await _emailSenderVAJ.SendINTRemind(intl1.ClaimID, insurerintl1.BrandName, intl1.INTL1Deadline.Value.ToString("HH:mm, dd-MM-yyyy"), reminderNo, mailList);

                }
            }

            //Check INT L2
            Logger.Debug(">>>>>>> Sending INT L2 reminder");
            var intl2list = _claimProcessRepo.GetAll()
                .Where(c => (c.IsINTL2 == null || c.IsINTL2 == false) && (open_claim.Contains(c.ClaimID))
                && (c.INTL2Deadline.Value.Hour == curtime.Hour)
                && (c.INTL2Deadline.Value.Minute == curtime.Minute)
                && (c.INTL2Deadline.Value.Day < curtime.Day)).ToList();

            if (intl2list.Count > 0)
            {
                foreach (ClaimProcess intl2 in intl2list)
                {
                    Logger.Debug(">>>>>>> Sending INT L2 reminder for " + intl2.ClaimID);
                    
                    var claimintl2 = _claimTableRepo.Get(intl2.ClaimID);
                    var insurerintl2 = _customerRepo.Get(claimintl2.InsurerID.Value);

                    var AMClaim = _employeeService.GetEmpInfo(claimintl2.AccountManagerID.Value);

                    VAJMailList mailList = new VAJMailList();
                    mailList.toList.Add(AMClaim.Email);

                    List<EmployeeView> adj_list = GetEmpByClaim(intl2.ClaimID);                    
                    foreach (EmployeeView adj in adj_list)
                    {
                        if (adj.Email != null)
                        {
                            mailList.ccList.Add(adj.Email);
                        }
                    }
                    mailList.ccList.Add("bod@vietadjusters.com");
                    mailList.ccList.Add("managers@vietadjusters.com");

                    var reminderNo = Math.Ceiling(curtime.Subtract(intl2.INTL2Deadline.Value).TotalDays).ToString();

                    await _emailSenderVAJ.SendINTRemind(intl2.ClaimID, insurerintl2.BrandName, intl2.INTL2Deadline.Value.ToString("HH:mm, dd-MM-yyyy"), reminderNo, mailList);

                }
            }

            //Check INT HC
            Logger.Debug(">>>>>>> Sending INT hard copy reminder");
            var inthclist = _claimProcessRepo.GetAll()
                .Where(c => (c.IsINTHC == null || c.IsINTHC == false) && (open_claim.Contains(c.ClaimID))
                && (c.INTHCDeadline.Value.Hour == curtime.Hour)
                && (c.INTHCDeadline.Value.Minute == curtime.Minute)
                && (c.INTHCDeadline.Value.Day < curtime.Day)).ToList();

            if (inthclist.Count > 0)
            {
                foreach (ClaimProcess inthc in inthclist)
                {
                    Logger.Debug(">>>>>>> Sending INT hard copy reminder for " + inthc.ClaimID);
                                                            
                    var claiminthc = _claimTableRepo.Get(inthc.ClaimID);
                    var insurerinthc = _customerRepo.Get(claiminthc.InsurerID.Value);

                    VAJMailList mailList = new VAJMailList();
                    mailList.toList.Add("admin@vietadjusters.com");

                    List<EmployeeView> adj_list = GetEmpByClaim(inthc.ClaimID);                    
                    foreach (EmployeeView adj in adj_list)
                    {
                        if (adj.Email != null)
                        {
                            mailList.ccList.Add(adj.Email);
                        }
                    }
                    mailList.ccList.Add("bod@vietadjusters.com");
                    mailList.ccList.Add("managers@vietadjusters.com");

                    var reminderNo = Math.Ceiling(curtime.Subtract(inthc.INTHCDeadline.Value).TotalDays).ToString();

                    await _emailSenderVAJ.SendINTHardCopyRemind(inthc.ClaimID, insurerinthc.BrandName, inthc.INTHCDeadline.Value.ToString("HH:mm, dd-MM-yyyy"), reminderNo, mailList);

                }
            }

            //Check FIN L1
            Logger.Debug(">>>>>>> Sending FIN L1 reminder");
            var finl1list = _claimProcessRepo.GetAll()
                .Where(c => (c.IsFINL1 == null || c.IsFINL1 == false) && (open_claim.Contains(c.ClaimID))
                && (c.FINL1Deadline.Value.Hour == curtime.Hour)
                && (c.FINL1Deadline.Value.Minute == curtime.Minute)
                && (c.FINL1Deadline.Value.Day < curtime.Day)).ToList();

            if (finl1list.Count > 0)
            {
                foreach (ClaimProcess finl1 in finl1list)
                {
                    Logger.Debug(">>>>>>> Sending FIN L1 reminder for " + finl1.ClaimID);

                    var claimfinl1 = _claimTableRepo.Get(finl1.ClaimID);
                    var insurerfinl1 = _customerRepo.Get(claimfinl1.InsurerID.Value);

                    var AMClaim = _employeeService.GetEmpInfo(claimfinl1.AccountManagerID.Value);

                    VAJMailList mailList = new VAJMailList();
                    mailList.toList.Add(AMClaim.Email);

                    List<EmployeeView> adj_list = GetEmpByClaim(finl1.ClaimID);                    
                    foreach (EmployeeView adj in adj_list)
                    {
                        if (adj.Email != null)
                        {
                            mailList.ccList.Add(adj.Email);
                        }
                    }
                    mailList.ccList.Add("bod@vietadjusters.com");
                    mailList.ccList.Add("managers@vietadjusters.com");

                    var reminderNo = Math.Ceiling(curtime.Subtract(finl1.FINL1Deadline.Value).TotalDays).ToString();

                    await _emailSenderVAJ.SendFINRemind(finl1.ClaimID, insurerfinl1.BrandName, finl1.FINL1Deadline.Value.ToString("HH:mm, dd-MM-yyyy"), reminderNo, mailList);

                }
            }

            //Check FIN L2
            Logger.Debug(">>>>>>> Sending FIN L2 reminder");
            var finl2list = _claimProcessRepo.GetAll()
                .Where(c => (c.IsFINL2 == null || c.IsFINL2 == false) && (open_claim.Contains(c.ClaimID))
                && (c.FINL2Deadline.Value.Hour == curtime.Hour)
                && (c.FINL2Deadline.Value.Minute == curtime.Minute)
                && (c.FINL2Deadline.Value.Day < curtime.Day)).ToList();

            if (finl2list.Count > 0)
            {
                foreach (ClaimProcess finl2 in finl2list)
                {
                    Logger.Debug(">>>>>>> Sending FIN L2 reminder for " + finl2.ClaimID);

                    var claimfinl2 = _claimTableRepo.Get(finl2.ClaimID);
                    var insurerfinl2 = _customerRepo.Get(claimfinl2.InsurerID.Value);

                    var AMClaim = _employeeService.GetEmpInfo(claimfinl2.AccountManagerID.Value);

                    VAJMailList mailList = new VAJMailList();
                    mailList.toList.Add(AMClaim.Email);

                    List<EmployeeView> adj_list = GetEmpByClaim(finl2.ClaimID);                    
                    foreach (EmployeeView adj in adj_list)
                    {
                        if (adj.Email != null)
                        {
                            mailList.ccList.Add(adj.Email);
                        }
                    }
                    mailList.ccList.Add("bod@vietadjusters.com");
                    mailList.ccList.Add("managers@vietadjusters.com");

                    var reminderNo = Math.Ceiling(curtime.Subtract(finl2.FINL2Deadline.Value).TotalDays).ToString();

                    await _emailSenderVAJ.SendFINRemind(finl2.ClaimID, insurerfinl2.BrandName, finl2.FINL2Deadline.Value.ToString("HH:mm, dd-MM-yyyy"), reminderNo, mailList);

                }
            }

            //Check FIN HC
            Logger.Debug(">>>>>>> Sending FIN hard copy reminder");
            var finhclist = _claimProcessRepo.GetAll()
                .Where(c => (c.IsFINHC == null || c.IsFINHC == false) && (open_claim.Contains(c.ClaimID))
                && (c.FINHCDeadline.Value.Hour == curtime.Hour)
                && (c.FINHCDeadline.Value.Minute == curtime.Minute)
                && (c.FINHCDeadline.Value.Day < curtime.Day)).ToList();

            if (finhclist.Count > 0)
            {
                foreach (ClaimProcess finhc in finhclist)
                {
                    Logger.Debug(">>>>>>> Sending FIN hard copy reminder for " + finhc.ClaimID);

                    var claimfinhc = _claimTableRepo.Get(finhc.ClaimID);
                    var insurerfinhc = _customerRepo.Get(claimfinhc.InsurerID.Value);

                    VAJMailList mailList = new VAJMailList();
                    mailList.toList.Add("admin@vietadjusters.com");

                    List<EmployeeView> adj_list = GetEmpByClaim(finhc.ClaimID);                    
                    foreach (EmployeeView adj in adj_list)
                    {
                        if (adj.Email != null)
                        {
                            mailList.ccList.Add(adj.Email);
                        }
                    }
                    mailList.ccList.Add("bod@vietadjusters.com");
                    mailList.ccList.Add("managers@vietadjusters.com");

                    var reminderNo = Math.Ceiling(curtime.Subtract(finhc.FINHCDeadline.Value).TotalDays).ToString();

                    await _emailSenderVAJ.SendFINHardCopyRemind(finhc.ClaimID, insurerfinhc.BrandName, finhc.FINHCDeadline.Value.ToString("HH:mm, dd-MM-yyyy"), reminderNo, mailList);
                }
            }

            //Check Submit Timesheet
            Logger.Debug(">>>>>>> Sending Submit Timesheet reminder");
            var tslist = _claimProcessRepo.GetAll()
                .Where(c => (c.IsSubmit == null || c.IsSubmit == false) && (open_claim.Contains(c.ClaimID))
                && (c.SubmitDeadline.Value.Hour == curtime.Hour)
                && (c.SubmitDeadline.Value.Minute == curtime.Minute)
                && (c.SubmitDeadline.Value.Day < curtime.Day)).ToList();

            if (tslist.Count > 0)
            {
                foreach (ClaimProcess ts in tslist)
                {
                    Logger.Debug(">>>>>>> Sending Submit Timesheet reminder for " + ts.ClaimID);

                    var claimts = _claimTableRepo.Get(ts.ClaimID);
                    var insurerts = _customerRepo.Get(claimts.InsurerID.Value);

                    List<EmployeeView> adj_list = GetEmpByClaim(ts.ClaimID);
                    VAJMailList mailList = new VAJMailList();
                    mailList.toList.Add("admin@vietadjusters.com");
                    foreach (EmployeeView adj in adj_list)
                    {
                        if (adj.Email != null)
                        {
                            mailList.toList.Add(adj.Email);
                        }
                    }                    
                    mailList.ccList.Add("bod@vietadjusters.com");
                    mailList.ccList.Add("managers@vietadjusters.com");

                    var reminderNo = Math.Ceiling(curtime.Subtract(ts.SubmitDeadline.Value).TotalDays).ToString();

                    await _emailSenderVAJ.SendTimeSheetRemind(ts.ClaimID, insurerts.BrandName, ts.SubmitDeadline.Value.ToString("HH:mm, dd-MM-yyyy"), reminderNo, mailList);

                }
            }

            Logger.Debug(">>>>>>> Sending remider mail done ");
        }
    }
}
