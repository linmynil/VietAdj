using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace triluatsoft.tls.Dto
{
    public class FileDto
    {
        [Required]
        public string FileName { get; set; }

        [Required]
        public string FileType { get; set; }

        [Required]
        public string FileToken { get; set; }

        public FileDto()
        {
            
        }

        public FileDto(string fileName, string fileType)
        {
            FileName = fileName;
            FileType = fileType;
            FileToken = Guid.NewGuid().ToString("N");
        }
        public FileDto(string fileName, string fileType, string fileToken)
        {
            FileName = fileName;
            FileType = fileType;
            FileToken = fileToken;
        }
    }
}