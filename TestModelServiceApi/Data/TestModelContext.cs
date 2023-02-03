using ApiIntegrationTests.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiIntegrationTests.Data
{
    public class TestModelContext : DbContext
    {
        public TestModelContext(DbContextOptions<TestModelContext> opt) : base(opt) 
        { 
        }

        public DbSet<TestModel> TestModels => Set<TestModel>();
    }
}
