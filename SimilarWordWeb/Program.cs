using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SimilarWordWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WordSimilarityLib.WordStudyModel model = new WordSimilarityLib.WordStudyModel();
            model.CreateDb();

            CommTools.Logger.Init(Path.Combine(Directory.GetCurrentDirectory(), @"data\AppLog.txt"), 0);  // level 0 : every thing, level 2: for production
            CommTools.Logger.Log(2, "SimilarWordWeb Main Start  ");

            //CreateWebHostBuilder(args).Build().Run();
            CreateHostBuilder(args).Build().Run();

        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>();
    }
}
