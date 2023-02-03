using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace IntegrationTests.Configs
{
    internal static class Configurations
    {
        private static AppSettings _appSettings;

        internal static AppSettings Appsettings
        {
            get
            {
                if (_appSettings is null)
                {
                    var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true);
                    var configuration = builder.Build();
                    var appsettings = new AppSettings();
                    configuration.Bind(appsettings);
                    _appSettings = appsettings;
                }

                return _appSettings!;
            }

            private set => _appSettings = value;
        }
    }
}
