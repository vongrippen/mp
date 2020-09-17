using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MP.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MP.Core.Context
{
    public class MPContext : DbContext
    {
        public MPContext(DbContextOptions<MPContext> options) : base(options)
        {
        }

        public DbSet<MediaFile> MediaFiles { get; set; }
        public DbSet<Models.Analysis> Analyses { get; set; }
        public DbSet<MediaFormat> MediaFormats { get; set; }
        public DbSet<VideoStream> VideoStreams { get; set; }
        public DbSet<AudioStream> AudioStreams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VideoStream>()
                .Property(e => e.DisplayAspectRatio)
                .HasConversion(
                    v => $"{v.Width},{v.Height}",
                    v => parseDimensions(v)
                );
            modelBuilder.Entity<VideoStream>()
                .Property(e => e.Profile)
                .IsRequired(false);
            modelBuilder.Entity<VideoStream>()
                .Property(e => e.PixelFormat)
                .IsRequired(false);
            modelBuilder.Entity<AudioStream>()
                .Property(e => e.Profile)
                .IsRequired(false);
            modelBuilder.Entity<AudioStream>()
                .Property(e => e.ChannelLayout)
                .IsRequired(false);
            modelBuilder.Entity<MediaFile>()
                .Property(e => e.FilenameData)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                    v => JsonConvert.DeserializeObject<Dictionary<string, string>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
                );
            modelBuilder.Entity<MediaFile>()
                .HasIndex(b => b.BytesPerSecond);
            modelBuilder.Entity<MediaFile>()
                .HasIndex(b => b.LastProcessingUpdate);
        }

        private (int Width, int Height) parseDimensions(string v)
        {
            var strings = v.Split(",");
            return (Int32.Parse(strings[0]), Int32.Parse(strings[1]));
        }
    }
}
