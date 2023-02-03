using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using IntegrationTests.Configs;

namespace IntegrationTests.Factories
{
    internal class ContainerFactory
    {
        private static string _connectionString;

        private static TestcontainerDatabase _testDatabase { get; set; }

        public static string ConnectionString
        {
            get
            {
                if (_connectionString is null)
                {
                    _testDatabase = new ContainerBuilder<MsSqlTestcontainer>()
                        .WithDatabase(
                            new MsSqlTestcontainerConfiguration()
                            {
                                Password = Guid.NewGuid().ToString(),
                                Database = Guid.NewGuid().ToString()
                            })
                        .WithImage(Configurations.Appsettings.SqlServerImage)
                        .WithCleanUp(true)
                        .WithPortBinding(1432, false)
                        .WithExposedPort(1432)
                        .WithName("SqlServer")
                        .Build();

                    _testDatabase!.StartAsync().Wait();

                    _connectionString = $"{_testDatabase.ConnectionString}TrustServerCertificate=True;";

                }

                return _connectionString;
            }

            private set => _connectionString = value;
        }
    }
}
