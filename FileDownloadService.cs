using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading.Tasks;
using App.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Core.Services
{
    public class FileDownloadService : IFileDownloadService
    {

        public async Task<FileStreamResult> DownloadFile(string filePath)
        {
            try
            {
                var memory = new MemoryStream();

                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                   await stream.CopyToAsync(memory);
                }

                memory.Position = 0;
                var ext = Path.GetExtension(filePath).ToLowerInvariant();
                FileStreamResult fileStreamResult = new FileStreamResult(memory, ContentType()[ext]);
                fileStreamResult.FileStream = memory;
                fileStreamResult.FileDownloadName = Path.GetFileName(filePath);
                return fileStreamResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private Dictionary<string, string> ContentType()
        {
            return new Dictionary<string, string>
          {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                {".csv", "text/csv"}
          };
        }

    }
}
