using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace IronMountain_Excercise.Models
{
    public class TextFile
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime UploadedDate { get; set; }

        public Stream Content { get; set; }
    }
}
