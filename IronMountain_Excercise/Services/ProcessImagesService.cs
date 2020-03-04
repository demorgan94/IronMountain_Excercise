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
        List<ImageFile> ProcessImages(List<IFormFile> filesData);
        byte[] DownloadZip(List<ImageFile> imagesIdList);
    }

    public class ProcessImagesService : IProcessImagesService
    {
        private readonly AppDBContext _context;

        public ProcessImagesService(AppDBContext context)
        {
            _context = context;
        }

        public byte[] DownloadZip(List<ImageFile> imagesList)
        {
            var formatedDateZipName = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss");
            var uploadFilesPath = DateTime.Now.DayOfWeek.ToString();
            var txtFileName = new Guid() + "_" + DateTime.Now.ToString("dd/MM/yy_hh:mm:ss") + ".meta";
            byte[] txtBytes = null;
            byte[] zipBytes = null;

            using (var stream = new MemoryStream())
            {
                foreach (var image in imagesList)
                {
                    var result = _context.ImageFiles.FirstOrDefault(x => x.Id == image.Id);

                    if (result != null)
                    {
                        using (var txtStream = new MemoryStream())
                        {
                            var writer = new StreamWriter(txtStream);
                            writer.WriteLine("\tID\t\t" + "|\tCreation Date\t|" + "\t\t\t\tImage Path\t");
                            writer.WriteLine($"{image.Id}\t" + $"|{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}|" + $"\t{image.FileName}\t");

                            using (var ms = new MemoryStream())
                            {
                                using (var resStream = new FileStream(Path.Combine(Path.GetTempPath(), uploadFilesPath), FileMode.Open, FileAccess.Read))
                                {
                                    ms.CopyTo(resStream);
                                    txtBytes = ms.ToArray();
                                }
                            }
                        }

                        using (var zip = new ZipArchive(stream, ZipArchiveMode.Create, true))
                        {
                            var zipEntry = zip.CreateEntry(image.FileName, CompressionLevel.Fastest);

                            using (var zipStream = zipEntry.Open())
                            {
                                zipStream.Write(Convert.FromBase64String(image.Content), 0, image.Content.Length);
                            }

                            zipEntry = zip.CreateEntry(txtFileName, CompressionLevel.Fastest);

                            if (zip.GetEntry(txtFileName).ToString() != txtFileName)
                            {
                                using (var zipStream = zipEntry.Open())
                                {
                                    zipStream.Write(txtBytes, 0, txtBytes.Length);
                                }
                            }
                        }
                    }
                }

                zipBytes = stream.ToArray();
            }

            return zipBytes;
        }

        public List<ImageFile> ProcessImages(List<IFormFile> filesData)
        {
            List<ImageFile> imagesList = new List<ImageFile>();

            foreach (var imageFile in filesData)
            {
                using (var stream = new MemoryStream())
                {
                    imageFile.CopyTo(stream);
                    byte[] content = stream.ToArray();

                    var currentDate = DateTime.Today;
                    var formatedDate = string.Format("{0}{1}", currentDate.ToString("yy"), currentDate.DayOfYear);
                    Random randomIntGenerator = new Random();
                    string randomInt = randomIntGenerator.Next(0, 99999).ToString("D5");
                    int.TryParse(formatedDate + randomInt, out int julianDate);

                    ImageFile image = new ImageFile
                    {
                        Id = julianDate,
                        CreationDate = DateTime.Now,
                        FileName = imageFile.FileName,
                        Content = Convert.ToBase64String(content)
                    };

                    imagesList.Add(image);

                    _context.Add(image);
                    _context.SaveChanges();
                }
            }

            return imagesList;
        }
    }
}
