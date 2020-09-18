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
            Cleanup cleanup = new Cleanup(_context);

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

            var cmdConfig = new Command("cfg");
            var cmdConfigAnalyze = new Command("analyze");
            cmdConfigAnalyze.Handler = CommandHandler.Create(analysis.AnalyzeCfg);
            var cmdConfigCleanup = new Command("cleanup");
            cmdConfigCleanup.Handler = CommandHandler.Create(cleanup.CleanupDB);

            analyzeCommand.Add(cmdAnalyzeFile);
            analyzeCommand.Add(cmdAnalyzeDir);

            cmdConfig.Add(cmdConfigAnalyze);
            cmdConfig.Add(cmdConfigCleanup);

            rootCommand.Add(analyzeCommand);
            rootCommand.Add(cmdConfig);

            rootCommand.InvokeAsync(args).Wait();
        }
    }
}
