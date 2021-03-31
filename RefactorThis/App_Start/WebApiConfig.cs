using refactor_me.App_Start;
using System.Web.Http;

namespace refactor_this
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var formatters = GlobalConfiguration.Configuration.Formatters;
            formatters.Remove(formatters.XmlFormatter);
            formatters.JsonFormatter.Indent = true;
            SerilogConfig.Configer();
            // Web API routes
            config.MapHttpAttributeRoutes();
            
        }
    }
}