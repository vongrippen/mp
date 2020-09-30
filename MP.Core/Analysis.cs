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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MP.Core
{
    public class Analysis
    {
        private MPContext context;
        private readonly IConfiguration config;
        public Analysis(MPContext ctx, IConfiguration cfg)
        {
            context = ctx;
            config = cfg;
        }
        public async Task AnalyzeCfg()
        {
            var dirCfg = config.GetSection("MP:Directories").GetChildren();
            foreach (IConfigurationSection dir in dirCfg)
            {
                await ProcessDir(dir["path"], dir["content_type"]);
            }
        }
        public async Task ProcessDir(string path, string content_type)
        {
            var dir = new DirectoryInfo(path);

            var files = dir.EnumerateFiles();
            foreach (var f in files)
            {
                await ProcessFile(f.FullName, content_type);
            }

            var subdirs = dir.EnumerateDirectories();
            foreach (var d in subdirs)
            {
                await ProcessDir(d.FullName, content_type);
            }
        }
        public async Task ProcessFile(string filename, string content_type)
        {
            await ProcessFile(filename, content_type, null);
        }
        public async Task ProcessFile(string filename, string content_type, string? processed_format)
        {
            var fileInfo = new FileInfo(filename);
            var filenameRegexString = config[$"MP:FilenameRegex:{content_type}"];
            List<String> ignoreRegex = config.GetSection("MP:Ignore").Get<List<String>>();
            foreach (String regString in ignoreRegex)
            {
                Regex reg = new Regex(regString);
                if (reg.IsMatch(filename))
                {
                    return;
                }
            }
            try
            {
                var oldAnalysis = context.MediaFiles
                    .Where(s => s.FileName == fileInfo.Name)
                    .Where(s => s.FilePath == fileInfo.DirectoryName)
                    .ToList();
                long oldSize = 0;
                if (oldAnalysis.FirstOrDefault() != null)
                {
                    oldSize = oldAnalysis.FirstOrDefault().Size;
                }
                if (oldSize != fileInfo.Length)
                {
                    context.RemoveRange(oldAnalysis);
                    await context.SaveChangesAsync();
                    MediaFile mediaFile = await AnalyzeFile(content_type, fileInfo);
                    mediaFile.ProcessedFormat = processed_format;

                    if (mediaFile != null)
                    {
                        context.MediaFiles.Add(mediaFile);
                        await context.SaveChangesAsync();
                        Console.Out.WriteLine($"[Added] {filename}");
                    }
                }
            }
            catch (System.NullReferenceException e) { await LogFileWithError(e, filename); }
            catch (System.InvalidOperationException e) { await LogFileWithError(e, filename); }
            catch (System.DivideByZeroException e) { await LogFileWithError(e, filename); }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException e) { await LogFileWithError(e, filename); }
        }

        public async Task<MediaFile> AnalyzeFile(string content_type, FileInfo fileInfo)
        {
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
            mediaFile.FilenameData = GetFilenameData(fileInfo.Name, content_type);
            mediaFile.BytesPerSecond = mediaFile.Size / (long)(analysis.Format.Duration.TotalSeconds);
            if (mediaFile.BytesPerSecond < 0)
            {
                mediaFile.BytesPerSecond = 0;
            }
            long pixelsPerFrame = 0;
            int maxWidth = 0;
            MP.Core.Models.VideoStream primaryVideoStream = null;
            foreach (MP.Core.Models.VideoStream v in analysis.VideoStreams)
            {
                if (v.Width > maxWidth)
                {
                    maxWidth = v.Width;
                    primaryVideoStream = v;
                }
            }
            if (primaryVideoStream != null)
            {
                pixelsPerFrame = primaryVideoStream.Width * primaryVideoStream.Height;
            }

            mediaFile.BytesPerSecondPerPixel = 0;
            if (pixelsPerFrame > 0)
            {
                mediaFile.BytesPerSecondPerPixel = (double)mediaFile.BytesPerSecond / (double)pixelsPerFrame;
            }

            return mediaFile;
        }

        private async Task LogFileWithError(Exception e, String filepath)
        {
            var existing = context.FilesWithErrors.Where(fwe => fwe.FilePath == filepath).ToList();
            context.RemoveRange(existing);
            FileWithErrors f = new FileWithErrors();
            f.FilePath = filepath;
            f.Notes = e.ToString() + "\n\n" + e.Message + "\n\n" + e.StackTrace;
            context.FilesWithErrors.Add(f);
            await context.SaveChangesAsync();
            Console.Out.WriteLine($"[Error] {filepath}");
        }

        private Dictionary<string, string> GetFilenameData(string filename, string content_type)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            var filenameRegexString = config[$"MP:FilenameRegex:{content_type}"];
            if (String.IsNullOrEmpty(filenameRegexString))
            {
                return data;
            }
            try
            {
                Regex filenameRegex = new Regex(filenameRegexString);
                var matches = filenameRegex.Matches(filename);
                foreach (Match match in matches)
                {
                    var groups = match.Groups;
                    foreach (Group group in groups)
                    {
                        data[group.Name] = group.Value;
                    }
                }
            }
            catch (ArgumentException)
            {
                Console.Error.WriteLine($"Invalid FilenameRegex for '{content_type}; -- '{filenameRegexString}'");
            }

            return data;
        }
    }
}
