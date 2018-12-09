using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SmarthomeAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://127.0.0.1:8000")
                .UseStartup<Startup>();
    }
}