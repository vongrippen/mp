using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Core.Models
{
    public class AudioStream : FFMpegCore.AudioStream
    {
        public Guid Id { get; set; }
        public Guid AnalysisId { get; set; }
        public Analysis Analysis { get; set; }
    }
}
