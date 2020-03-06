using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
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
        byte[] ProcessTxt(IFormFile txtFile);
    }

    public class ProcessImagesService : IProcessImagesService
    {
        private readonly AppDBContext _context;

        public ProcessImagesService(AppDBContext context)
        {
            _context = context;
        }

        public byte[] ProcessTxt(IFormFile txtFile)
        {
            var formatedDateZipName = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss");
            var uploadFilesPath = DateTime.Now.DayOfWeek.ToString();
            byte[] zipBytes = null;
            List<int> imagesIdentifiers = new List<int>();

            var result = new StringBuilder();
            using (var reader = new StreamReader(txtFile.OpenReadStream()))
            {
                reader.ReadLine();
                while (reader.Peek() >= 0)
                {
                    string line;
                    string[] strArray;
                    line = reader.ReadLine();

                    strArray = line.Split('|');
                    int imageId = Convert.ToInt32(strArray[0]);

                    imagesIdentifiers.Add(imageId);
                }
            }

            using (var stream = new MemoryStream())
            {
                foreach (var imageId in imagesIdentifiers)
                {
                    var image = _context.ImageFiles.FirstOrDefault(x => x.Identifier == imageId);

                    if (result != null)
                    {
                        using (var zip = new ZipArchive(stream, ZipArchiveMode.Update, true))
                        {
                            var zipEntry = zip.CreateEntry(image.FileName, CompressionLevel.Fastest);

                            using (var zipStream = zipEntry.Open())
                            {
                                zipStream.Write(Convert.FromBase64String(image.Content));
                            }
                        }
                    }
                }

                zipBytes = stream.ToArray();
            }

            
            return zipBytes;
        }

        public byte[] ProcessImages(List<IFormFile> filesData)
        {
            List<ImageFile> imagesList = new List<ImageFile>();
            byte[] txtBytes = null;

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
                        Identifier = julianDate,
                        CreationDate = DateTime.Now,
                        FileName = imageFile.FileName,
                        Content = Convert.ToBase64String(content)
                    };

                    imagesList.Add(image);
                }
            }

            _context.AddRange(imagesList);
            _context.SaveChanges();

            using (var txtStream = new MemoryStream())
            {
                TextWriter writer = new StreamWriter(txtStream);
                writer.WriteLine(string.Format("{0}{1}{2}", "    ID   ", "|   Creation Date   |", "   Image Path   "));
                foreach (var image in imagesList)
                {
                    writer.WriteLine(string.Format("{0}{1}{2}", $"{image.Identifier}", $"|{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}|", $"\t{image.FileName}\t"));
                    writer.Flush();
                }

                txtBytes = txtStream.ToArray();
            }

            return txtBytes;
        }
    }
}
