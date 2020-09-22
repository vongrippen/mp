using System;
using System.Collections.Generic;
using MP.Core.Models;
using System.IO;
using System.Linq;
using MP.Core.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace MP.Core
{
    public class Cleanup
    {
        private MPContext context;
        public Cleanup(MPContext ctx)
        {
            context = ctx;
        }
        public async Task CleanupDB()
        {
            Console.Out.WriteLine("--== Starting Cleanup ==--");
            var files = context.MediaFiles.ToList();
            foreach (var file in files)
            {
                var exists = File.Exists(file.FilePath + "/" + file.FileName);
                if (!exists)
                {
                    context.RemoveRange(file);
                }
            }
            var filesWError = context.FilesWithErrors.ToList();
            foreach (var file in filesWError)
            {
                var exists = File.Exists(file.FilePath);
                if (!exists)
                {
                    context.RemoveRange(file);
                }
            }
            await context.SaveChangesAsync();
            Console.Out.WriteLine("--== Finished Cleanup ==--");
        }
    }
}
