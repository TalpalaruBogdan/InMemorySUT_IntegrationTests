using ApiIntegrationTests.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;

namespace IntegrationTests.Factories
{
    public class CustomWebApplicationFactory<ITestMarker> 
        : WebApplicationFactory<ITestMarker> where ITestMarker : class
    {
        public TestModelContext TestModelContext { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<TestModelContext>));

                services.Remove(dbContextDescriptor);

                var dbConnectionDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbConnection));

                services.Remove(dbConnectionDescriptor);

                services.AddDbContext<TestModelContext>(opt =>
                {
                    opt.UseSqlServer(ContainerFactory.ConnectionString);
                });

                var sp = services.BuildServiceProvider();

                var context = sp.GetService<TestModelContext>();

                context.Database.EnsureCreated();

                TestModelContext = context;
            });

            builder.UseEnvironment("Development");
        }

    }
}
