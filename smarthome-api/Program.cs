using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SmarthomeAPI
{
    /// <summary>
    /// Entry point to the app (`Program.Main`).
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method of program (currently app uses no arguments).
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// As method name says, it creates web host builder with some set up parameters such as listener address and
        /// port.
        /// </summary>
        /// <param name="args">Passed from Main</param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://0.0.0.0:8000")
                .UseStartup<Startup>();
    }
}