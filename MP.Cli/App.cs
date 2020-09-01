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

            Analysis analysis = new Analysis(_context, _config);

            var rootCommand = new RootCommand();
            var analyzeCommand = new Command("analyze");

            var cmdAnalyzeFile = new Command("file");
            cmdAnalyzeFile.AddOption(new Option<string>("--filename"));
            cmdAnalyzeFile.AddOption(new Option<string>("--content_type"));
            cmdAnalyzeFile.Handler = CommandHandler.Create<string, string>(analysis.AnalyzeFile);

            var cmdAnalyzeDir = new Command("dir");
            cmdAnalyzeDir.AddOption(new Option<string>("--path"));
            cmdAnalyzeDir.AddOption(new Option<string>("--content_type"));
            cmdAnalyzeDir.Handler = CommandHandler.Create<string, string>(analysis.AnalyzeDir);

            var cmdAnalyzeConfig = new Command("cfg");
            cmdAnalyzeConfig.Handler = CommandHandler.Create(analysis.AnalyzeCfg);

            analyzeCommand.Add(cmdAnalyzeFile);
            analyzeCommand.Add(cmdAnalyzeDir);
            analyzeCommand.Add(cmdAnalyzeConfig);

            rootCommand.Add(analyzeCommand);

            rootCommand.InvokeAsync(args).Wait();
        }
    }
}
