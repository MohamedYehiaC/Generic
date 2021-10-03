using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace App.Core.Interfaces.Services
{
    public interface IFileDownloadService
    {
        public Task<FileStreamResult> DownloadFile(string filePath);

    }
}
