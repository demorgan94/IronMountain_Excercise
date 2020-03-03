using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace IronMountain_Excercise.Data
{
    [Table("ImageFile")]
    public class ImageFile
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreationDate { get; set; }

        public string FileName { get; set; }

        public string Content { get; set; }
    }
}
