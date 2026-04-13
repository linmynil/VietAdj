using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using triluatsoft.tls.EntityFramework;
using triluatsoft.tls.OldTable.dto;
using triluatsoft.tls.Dto;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Abp.AutoMapper;
using System.Security.AccessControl;
using System.Configuration;
using triluatsoft.tls.Net.MimeTypes;

namespace triluatsoft.tls.OldTable
{
    public class ClaimFolderService : tlsAppServiceBase, IClaimFolderService
    {
        private readonly ISqlExecuter _sqlExecuter;
        private readonly IRepository<F> _tblFolderRepository;
        private readonly IAppFolders _appFolders;

        public ClaimFolderService(ISqlExecuter sqlExecuter,
            IRepository<F> tblFolderRepository, IAppFolders appFolders)
        {
            _sqlExecuter = sqlExecuter;
            _tblFolderRepository = tblFolderRepository;
            _appFolders = appFolders;
        }

        public PagedResultDto<ClaimFolderInfo> GetAll(FolderSearchOption input)
        {
            var context = _sqlExecuter.GetTLSDBContext();
            var query = from con in context.Fs
                        select new ClaimFolderInfo
                        {
                            ID = con.Id,
                            FName = con.FName,
                            FPath = con.FPath,
                            FSecurity = con.FSecurity,
                            CreatedDate = con.CreatedDate
                        };
            query = query.OrderByDescending(t => t.CreatedDate);
            if (!string.IsNullOrEmpty(input.ClaimID))
                query = query.Where(p => p.FName.Contains(input.ClaimID));
            
            //phan trang
            var total = query.Count();
            var page = input.Page;
            var page1 = page - 1;
            query = query.Skip(input.PageSize * page1)
                .Take(input.PageSize);
            var list = query.ToList();
            var result = new PagedResultDto<ClaimFolderInfo>()
            {
                TotalCount = total,
                Items = list
            };

            return result;
        }

        public FolderDetails GetFolderDetails(string virtualPath)
        {
            FolderDetails returnList = new FolderDetails();
            returnList.FolderList = new List<ListFolder>();
            returnList.FileList = new List<ListFiles>();
            var rootFolder = ConfigurationManager.AppSettings["RootFolder"];
            var fullPath = rootFolder + virtualPath;            
            try
            {
                DirectoryInfo di = new DirectoryInfo(fullPath);
                foreach (var fi in di.GetDirectories("*", SearchOption.TopDirectoryOnly))
                {
                    var temp = new ListFolder();
                    temp.FName = fi.Name;
                    temp.CreationTime = fi.CreationTime;
                    temp.LastAccessTime = fi.LastAccessTime;
                    returnList.FolderList.Add(temp);
                }
            }
            catch
            {
                
            }

            try
            {
                DirectoryInfo di = new DirectoryInfo(fullPath);
                foreach (var fi in di.GetFiles("*", SearchOption.TopDirectoryOnly))
                {
                    var temp = new ListFiles();
                    temp.FName = fi.Name;
                    temp.Extension = fi.Extension;
                    temp.Length = fi.Length;
                    temp.CreationTime = fi.CreationTime;
                    temp.LastAccessTime = fi.LastAccessTime;
                    returnList.FileList.Add(temp);
                }
            }
            catch
            {

            }

            return returnList;
        }

        public FileDto DownloadClaimFile(string fName, string virtualPath)
        {
            var rootFolder = ConfigurationManager.AppSettings["RootFolder"];
            var fullPath = rootFolder + virtualPath;
            var file = new FileDto(fullPath, MimeTypeNames.ApplicationOctetStream, fName);
            var filePath = Path.Combine(_appFolders.TempFileDownloadFolder, fName);
            if (File.Exists(fullPath))
            {
                File.Copy(fullPath, filePath);
            }
            return file;
        }

        public List<ListFolder> GetFolderList(string virtualPath)
        {
            var rootFolder = ConfigurationManager.AppSettings["RootFolder"];
            var fullPath = rootFolder + virtualPath;
            List<ListFolder> list = new List<ListFolder>();
            try
            {
                DirectoryInfo di = new DirectoryInfo(fullPath);
                foreach (var fi in di.GetDirectories("*", SearchOption.TopDirectoryOnly))
                {
                    var temp = new ListFolder();
                    temp.FName = fi.Name;
                    temp.CreationTime = fi.CreationTime;
                    temp.LastAccessTime = fi.LastAccessTime;
                    list.Add(temp);
                }
            } catch
            {

            }
            
            return list;
        }

        public List<ListFiles> GetFilesList(string virtualPath)
        {
            var rootFolder = ConfigurationManager.AppSettings["RootFolder"];
            var fullPath = rootFolder + virtualPath;
            List<ListFiles> list = new List<ListFiles>();
            DirectoryInfo di = new DirectoryInfo(fullPath);
            foreach (var fi in di.GetFiles("*", SearchOption.TopDirectoryOnly))
            {
                var temp = new ListFiles();
                temp.FName = fi.Name;
                temp.Extension = fi.Extension;
                temp.Length = fi.Length;
                temp.CreationTime = fi.CreationTime;
                temp.LastAccessTime = fi.LastAccessTime;
                list.Add(temp);
            }
            return list;
        }

        public List<ClaimFolderInfo> GetUploadFolderList()
        {
            var context = _sqlExecuter.GetTLSDBContext();
            var query = from fu in context.FileUpLoad
                        select new ClaimFolderInfo
                        {                            
                            FName = fu.FName,
                            FPath = fu.FPath,                            
                            catalog = fu.Catagory,
                            CreatedDate = fu.CreatedDate
                        };
            query = query.OrderByDescending(t => t.CreatedDate);

            return query.ToList();            
        }

        public List<ClaimFolderInfo> SearchUploadFolderList(LibreryUploadSearchOptions opt)
        {
            var context = _sqlExecuter.GetTLSDBContext();
            var query = from fu in context.FileUpLoad
                        where (string.IsNullOrEmpty(opt.Content) || fu.FName.Contains(opt.Content)) &&
                              (string.IsNullOrEmpty(opt.Category) || (fu.Catagory == opt.Category))
                        select new ClaimFolderInfo
                        {
                            FName = fu.FName,
                            FPath = fu.FPath,
                            catalog = fu.Catagory,
                            CreatedDate = fu.CreatedDate
                        };
            query = query.OrderByDescending(t => t.CreatedDate);

            return query.ToList();
        }

        public FileDto DownloadLabFile(string fName)
        {
            var context = _sqlExecuter.GetTLSDBContext();
            var query = from fu in context.FileUpLoad where fu.FName == fName
                        select new ClaimFolderInfo
                        {                            
                            FName = fu.FName,
                            FPath = fu.FPath,
                            //FName = fu.FName.Trim().Replace(" ", "%20"),
                            //FPath = fu.FPath.Trim().Replace(" ", "%20"),
                            catalog = fu.Catagory,
                            CreatedDate = fu.CreatedDate
                        };
            query = query.OrderByDescending(t => t.CreatedDate);
            var selFile = query.FirstOrDefault();            
            var file = new FileDto(selFile.FPath, MimeTypeNames.ApplicationOctetStream, selFile.FName);
            var filePath = Path.Combine(_appFolders.TempFileDownloadFolder, selFile.FName);
            if (File.Exists(selFile.FPath))
            {
                File.Copy(selFile.FPath, filePath);
            }            
            return file;
        }
    }
}
