using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.Pkcs;
using System.Threading.Tasks;
using IronMountain_Excercise.Data;
using IronMountain_Excercise.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IronMountain_Excercise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessImageController : ControllerBase
    {
        private readonly IProcessImagesService _processImagesService;

        public ProcessImageController(IProcessImagesService processImagesService)
        {
            _processImagesService = processImagesService;
        }

        [HttpPost("upload")]
        public IActionResult ProcessImages()
        {
            List<IFormFile> images = new List<IFormFile>();

            var files = Request.Form.Files;
            for (int i = 0; i < files.Count; i++)
            {
                images.Add(files[i]);
            }

            if (images == null) return NoContent();
            if (images.Count == 0)
            {
                return BadRequest();
            }

            var txtFile = _processImagesService.ProcessImages(images);
            return new FileContentResult(txtFile, new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/octet"));
        }

        [HttpPost("textfile")]
        public IActionResult ProcessTxt()
        {
            IFormFile file = Request.Form.Files[0];

            if (file == null) return NoContent();
            if (file.Length == 0)
            {
                return BadRequest();
            }

            var zipFile = _processImagesService.ProcessTxt(file);
            return new FileContentResult(zipFile, new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/octet"));
        }
    }
}