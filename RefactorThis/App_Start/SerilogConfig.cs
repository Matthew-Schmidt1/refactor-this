using Serilog;
using Serilog.Formatting.Json;
using System.Web;

namespace refactor_me.App_Start
{
    public static class SerilogConfig
    {
        public static void Configer()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(new JsonFormatter())
                .WriteTo.File(new JsonFormatter(), HttpContext.Current.Server.MapPath("~/logs/log-.txt"), rollingInterval: RollingInterval.Day)
                .ReadFrom.AppSettings()
                .CreateLogger();
        }
    }
}