using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTS.API.ApiServices
{
    public enum FolderType
    {
        Images,
        Documents,
        Other
    }

    public class FileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string SaveFileAsync(byte[] fileBytes, FolderType folder, string extension)
        {
            string wwwRootPath = _env.WebRootPath;
            string folderPath = Path.Combine(wwwRootPath, folder.ToString());

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fileName = $"{Guid.NewGuid()}{extension}";
            string fullPath = Path.Combine(folderPath, fileName);

            File.WriteAllBytes(fullPath, fileBytes);

            return Path.Combine(folder.ToString(), fileName);
        }
    }

}
