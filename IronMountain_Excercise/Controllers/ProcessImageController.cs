using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpPost]
        [Route("images")]
        public IActionResult ProcessImages()
        {
            List<IFormFile> images = new List<IFormFile>();
            var files = Request.Form.Files;
            for (int i = 0; i < files.Count; i++)
            {
                images.Add(files[i]);
            }


            if (images == null) return BadRequest("No Image(s)");
            if (images.Count == 0)
            {
                return BadRequest("Empty Image(s)");
            }

            return _processImagesService.ProcessImages(images);
        }

        [HttpPost]
        [Route("zip")]
        public IActionResult ProcessZip()
        {
            var zip = Request.Form.Files[0];

            if (zip == null) return BadRequest("No Image(s)");
            if (zip.Length == 0)
            {
                return BadRequest("Empty Image(s)");
            }

            _processImagesService.ProcessZipFile(zip);

            return Ok();
        }
    }
}