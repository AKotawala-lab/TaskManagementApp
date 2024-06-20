
using Microsoft.Extensions.Configuration;

namespace TaskManagementApp.Application.Helpers
{
    public static class ConfigurationHelper
    {
        public static IConfiguration config;
        public static void Initialize(IConfiguration Configuration)
        {
            config = Configuration;
        }
    }
}