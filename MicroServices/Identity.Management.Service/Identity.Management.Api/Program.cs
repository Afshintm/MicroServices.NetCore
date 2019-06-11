using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Identity.Management.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {


            //Running Database Creation Migration and seeding data
            //SeedData.EnsureSeedData(host.Services);

            var seed = args.Contains("/seed");
            if (seed)
            {
                args = args.Except(new[] { "/seed" }).ToArray();
            }

            var host = CreateWebHostBuilder(args).Build();
            if (seed)
            {
                SeedData.EnsureSeedData(host.Services);
                return;
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
