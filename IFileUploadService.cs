using App.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Core.Interfaces.Services
{
    public interface IFileUploadService
    {
        
        public void FolderCreate(string path);

        public List<FileUploadModel> PostFile(IFormFileCollection uploadedFiles , string folderName);
        string RemoveSpecialCharachter(string folderName);
    }
}
