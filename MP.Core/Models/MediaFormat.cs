using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Transactions;

namespace MP.Core.Models
{
    public class MediaFormat : FFMpegCore.MediaFormat
    {
        public Guid Id { get; set; }
        public Guid AnalysisId { get; set; }
        public Analysis Analysis { get; set; }
        public List<VideoStream> VideoStreams { get; set; }
        public List<AudioStream> AudioStreams { get; set; }
    }
}
