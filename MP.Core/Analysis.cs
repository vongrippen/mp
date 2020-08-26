using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Text;
using FFMpegCore;
using MP.Core.Models;
using System.IO;
using System.Linq;
using MP.Core.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;

namespace MP.Core
{
    public class Analysis
    {
        private MPContext context;
        public Analysis(MPContext ctx)
        {
            context = ctx;
        }
        public void WatchDir(string directory, string contentType) { }
        public void ProcessDir(string directory, string contentType) { }
        public void ProcessFile(string filename, string content_type)
        {
            var fileInfo = new FileInfo(filename);
            var ffprobe = FFProbe.Analyse(fileInfo.FullName);

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FFMpegCore.AudioStream, Models.AudioStream>();
                cfg.CreateMap<FFMpegCore.VideoStream, Models.VideoStream>();
                cfg.CreateMap<FFMpegCore.MediaFormat, Models.MediaFormat>();
            }).CreateMapper();

            MP.Core.Models.Analysis analysis = new MP.Core.Models.Analysis();
            analysis.AudioStreams = mapper.Map<List<Models.AudioStream>>(ffprobe.AudioStreams);
            analysis.VideoStreams = mapper.Map<List<Models.VideoStream>>(ffprobe.VideoStreams);
            analysis.Format = mapper.Map<Models.MediaFormat>(ffprobe.Format);

            MP.Core.Models.MediaFile mediaFile = new MP.Core.Models.MediaFile();

            mediaFile.Analysis = analysis;
            mediaFile.FileName = fileInfo.Name;
            mediaFile.FileExt = fileInfo.Extension;
            mediaFile.FilePath = fileInfo.DirectoryName;
            mediaFile.Size = fileInfo.Length;
            mediaFile.ContentType = content_type;

            
            context.MediaFiles.Add(mediaFile);
            context.SaveChanges();
        }
    }
}
