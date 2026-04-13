using System.IO;
using System.Web.Mvc;
using Abp.Auditing;
using Abp.UI;
using Abp.Web.Models;
using Abp.Web.Mvc.Authorization;
using triluatsoft.tls.Dto;
using System.Configuration;
using System;

namespace triluatsoft.tls.Web.Controllers
{
    public class FileController : tlsControllerBase
    {
        private readonly IAppFolders _appFolders;

        public FileController(IAppFolders appFolders)
        {
            _appFolders = appFolders;
        }

        [AbpMvcAuthorize]
        [DisableAuditing]
        public ActionResult DownloadTempFile(FileDto file)
        {
            //var filePath = file.FileName;
            var filePath = Path.Combine(_appFolders.TempFileDownloadFolder, file.FileToken);            
            if (!System.IO.File.Exists(filePath))
            {
                throw new UserFriendlyException(L("RequestedFileDoesNotExists"));
            }
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            System.IO.File.Delete(filePath);
            return File(fileBytes, file.FileType, file.FileName);
        }

        [AbpMvcAuthorize]
        public JsonResult UploadFile()
        {
            try
            {
                //Check input
                if (Request.Files.Count <= 0 || Request.Files[0] == null)
                {
                    return Json("Please choose a file.");
                }

                var file = Request.Files[0];

                if (file.ContentLength > 10048576)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Warn_SizeLimit"));
                }

                var tempFileName = $"{file.FileName}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var tempFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    ConfigurationManager.AppSettings["uploaded-folder"], tempFileName);
                file.SaveAs(tempFilePath);

                return Json(new AjaxResponse(new { fileName = tempFileName }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
    }
}