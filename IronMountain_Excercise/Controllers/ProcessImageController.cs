using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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

            var filesList = _processImagesService.ProcessImages(images);
            return Ok(filesList);
        }

        [HttpPost("download")]
        public IActionResult DownloadZip(List<ImageFile> imagesList)
        {
            _processImagesService.DownloadZip(imagesList);
            return Ok();
        }
    }
}