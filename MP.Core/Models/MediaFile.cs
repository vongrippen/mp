using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MP.Core.Models
{
    public class MediaFile
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }

        [MaxLength(1000)]
        public string FilePath { get; set; }
        public long Size { get; set; }
        public string ContentType { get; set; }
        public Analysis Analysis { get; set; }
    }
}
