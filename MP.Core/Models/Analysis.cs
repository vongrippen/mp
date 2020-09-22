using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Transactions;

namespace MP.Core.Models
{
    public class Analysis
    {
        public Guid Id { get; set; }
        public Guid FileId { get; set; }
        public MediaFile File { get; set; }
        public AudioStream PrimaryAudioStream { get => AudioStreams.First(); }
        public VideoStream PrimaryVideoStream { get => VideoStreams.Where(v => v.Duration > TimeSpan.Zero).First(); }
        public MediaFormat Format { get; set; }
        public List<VideoStream> VideoStreams { get; set; }
        public List<AudioStream> AudioStreams { get; set; }
    }
}
