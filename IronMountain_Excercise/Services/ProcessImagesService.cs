using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IronMountain_Excercise.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace IronMountain_Excercise.Services
{
    public interface IProcessImagesService
    {
        byte[] ProcessImages(List<IFormFile> filesData);
        void ProcessZipFile(IFormFile zipDat);
        //FileContentResult DownloadZip(byte[] zipFile);
    }

    public class ProcessImagesService : IProcessImagesService
    {
        private readonly IWebHostEnvironment _host;
        private readonly AppDBContext _context;

        public ProcessImagesService(IWebHostEnvironment host, AppDBContext context)
        {
            _host = host;
            _context = context;
        }

        //public FileContentResult DownloadZip(byte[] zipFile)
        //{
        //    return new FileContentResult(zipFile, new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/octet"))
        //    {
        //        FileDownloadName = DateTime.Now.DayOfWeek.ToString()
        //    };
        //}

        public byte[] ProcessImages(List<IFormFile> filesData)
        {
            var uploadFilesPath = Path.Combine("Uploads", DateTime.Now.DayOfWeek.ToString());
            var zipFile = uploadFilesPath + ".zip";

            if (File.Exists(uploadFilesPath)) { File.Delete(uploadFilesPath); }

            if (File.Exists(zipFile)) { File.Delete(zipFile); }

            if (!Directory.Exists(uploadFilesPath)) { Directory.CreateDirectory(uploadFilesPath); }

            if (filesData.Count > 0)
            {
                for (int i = 0; i < filesData.Count; i++)
                { 
                    var filePath = Path.Combine(uploadFilesPath, filesData[i].FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        filesData[i].CopyTo(stream);
                    }

                    var currentDate = DateTime.Today;
                    var formatedDate = string.Format("{0}{1}", currentDate.ToString("yy"), currentDate.DayOfYear);
                    Random randomIntGenerator = new Random();
                    string randomInt = randomIntGenerator.Next(0, 99999).ToString("D5");
                    int.TryParse(formatedDate + randomInt, out int julianDate);
                    byte[] content = File.ReadAllBytes(filePath);
                    string contentToString = Convert.ToBase64String(content);

                    ImageFile image = new ImageFile
                    {
                        Id = julianDate,
                        CreationDate = DateTime.Now,
                        FileName = filesData[i].FileName,
                        Content = contentToString
                    };

                    _context.Add(image);
                    _context.SaveChanges();
                }

                ZipFile.CreateFromDirectory(uploadFilesPath, zipFile);
                var readZip = File.ReadAllBytes(zipFile);

                return readZip;
            }
            else
            {
                return null;
            }
        }

        public void ProcessZipFile(IFormFile zipDat)
        {
            throw new NotImplementedException();
        }
    }
}
