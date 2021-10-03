using App.Core.Interfaces.Services;
using App.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace App.Core.Services
{
    public class FileUploadService : IFileUploadService
    {

        protected readonly IConfiguration configuration;

        public FileUploadService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void FolderCreate(string path)
        {

            if (Directory.Exists(path))
            {
                return ;
            }

            DirectoryInfo di = Directory.CreateDirectory(path);
        }


        public List<FileUploadModel> PostFile(IFormFileCollection uploadedFiles , string folderName)
        {


            List<FileUploadModel> filesList = new List<FileUploadModel>();
            try
            {
                var path = string.Concat(configuration["AppSettings:SharedDirectoryPath"] , folderName);

                if (uploadedFiles.All(c => c.Length > 0))
                {
                    FolderCreate(path);
                    string destinationFolder = Path.Combine(path);
                    string filePath;

                    foreach (var file in uploadedFiles)
                    {
                        filePath = Path.Combine(destinationFolder, file.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        filesList.Add(new FileUploadModel() { FileName = file.FileName, Path = folderName });
                    }

                }

                return filesList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string RemoveSpecialCharachter(string folderName)
        {
            return Regex.Replace(folderName.Trim(), "[^A-Za-z0-9_. ]+", "_");
        }
    }
}
