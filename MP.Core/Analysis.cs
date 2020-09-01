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
using Microsoft.Extensions.Configuration;

namespace MP.Core
{
    class DirConfig
    {
        string path { get; set; }
        string content_type { get; set; }
    }
    public class Analysis
    {
        private MPContext context;
        private readonly IConfiguration config;
        public Analysis(MPContext ctx, IConfiguration cfg)
        {
            context = ctx;
            config = cfg;
        }
        public void AnalyzeCfg()
        {
            var dirCfg = config.GetSection("MP:Directories").GetChildren();
            foreach (IConfigurationSection dir in dirCfg)
            {
                AnalyzeDir(dir["path"], dir["content_type"]);
            }
        }
        public void AnalyzeDir(string path, string content_type)
        {
            var dir = new DirectoryInfo(path);

            var files = dir.EnumerateFiles();
            foreach (var f in files)
            {
                AnalyzeFile(f.FullName, content_type);
            }

            var subdirs = dir.EnumerateDirectories();
            foreach (var d in subdirs)
            {
                AnalyzeDir(d.FullName, content_type);
            }
        }
        public void AnalyzeFile(string filename, string content_type)
        {
            var fileInfo = new FileInfo(filename);
            try
            {
                var ffprobe = FFProbe.Analyse(fileInfo.FullName);
                Console.Out.WriteLine(filename);

                var mapper = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<FFMpegCore.AudioStream, Models.AudioStream>();
                    cfg.CreateMap<FFMpegCore.VideoStream, Models.VideoStream>();
                    cfg.CreateMap<FFMpegCore.MediaFormat, Models.MediaFormat>();
                }).CreateMapper();

                var oldAnalysis = context.MediaFiles
                    .Where(s => s.FileName == fileInfo.Name)
                    .Where(s => s.FilePath == fileInfo.DirectoryName)
                    .ToList();
                context.RemoveRange(oldAnalysis);
                context.SaveChanges();

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
            } catch (System.NullReferenceException) { }
        }
    }
}
