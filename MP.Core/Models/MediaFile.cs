using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Column(TypeName = "TEXT")]
        public Dictionary<string,string> FilenameData { get; set; }
        public long BytesPerSecond { get; set; }
        public DateTime LastProcessingUpdate { get; set; }
        public string ProcessedFormat { get; set; }
    }
}
