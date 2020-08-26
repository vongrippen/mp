using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.CommandLine;
using System.CommandLine.Invocation;
using MP.Core;
using MP.Core.Context;
using Microsoft.EntityFrameworkCore;

namespace MP.Cli
{
    class App
    {
        private readonly IConfiguration _config;
        private MPContext _context;

        public App(IConfiguration config, MPContext ctx)
        {
            _config = config;
            _context = ctx;
        }

        public void Run(string[] args)
        {
            _context.Database.Migrate();
            Console.WriteLine("Media Processor");

            Analysis analysis = new Analysis(_context);

            var rootCommand = new RootCommand();
            var processCommand = new Command("process");

            var cmdProcessFile = new Command("file");
            cmdProcessFile.AddOption(new Option<string>("--filename"));
            cmdProcessFile.AddOption(new Option<string>("--content_type"));
            cmdProcessFile.Handler = CommandHandler.Create<string, string>(analysis.ProcessFile);

            processCommand.Add(cmdProcessFile);

            rootCommand.Add(processCommand);

            rootCommand.InvokeAsync(args).Wait();
        }
    }
}
