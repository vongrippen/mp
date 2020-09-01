using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Core.Models
{
    public class VideoStream : FFMpegCore.VideoStream
    {
        public Guid Id { get; set; }
        public Guid AnalysisId { get; set; }
        public Analysis Analysis { get; set; }
    }
}
