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
            var files = context.MediaFiles.ToList();
            foreach (var file in files)
            {
                var exists = File.Exists(file.FilePath + "/" + file.FileName);
                if (!exists)
                {
                    context.RemoveRange(file);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
