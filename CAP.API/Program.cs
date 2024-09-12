using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CAP.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
#if !DEBUG
            // Prevents config reload on change when in prod, this can break kube deployments  
            Environment.SetEnvironmentVariable("DOTNET_hostBuilder:reloadConfigOnChange", "false");
#endif
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSentry(o =>
                    {
                        // Replace me with your Sentry DSN
                        o.Dsn = "https://5b6ff9ec0eb14169acbb6e811c191030@sentry-test.ls.byu.edu/2";
                        o.TracesSampleRate = 0.5;
                    });
                });
    }
}