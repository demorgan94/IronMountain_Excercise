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

        [Required]
        public int Identifier { get; set; }
        
        [Required]
        public DateTime CreationDate { get; set; }
        
        [Required]
        public string FileName { get; set; }
        
        [Required]
        public string Content { get; set; }
    }
}
