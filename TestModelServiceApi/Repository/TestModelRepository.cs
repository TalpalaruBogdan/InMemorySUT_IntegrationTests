using ApiIntegrationTests.Data;
using ApiIntegrationTests.DTOs;
using ApiIntegrationTests.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ApiIntegrationTests.Repository
{
    public class TestModelRepository : ITestModelRepository
    {
        private readonly TestModelContext testModelContext;

        public TestModelRepository(TestModelContext testModelContext)
        {
            this.testModelContext = testModelContext;
        }

        public async Task<TestModel?> GetTestModelByIdAsync(Guid id)
        {
            return await testModelContext.TestModels.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IList<TestModel>> GetTestModelsAsync()
        {
            return await testModelContext.TestModels.ToListAsync();
        }

        public async Task<Guid> CreateTestModelAsync(TestModel testModel)
        {
            await testModelContext.TestModels.AddAsync(testModel);
            await testModelContext.SaveChangesAsync();
            return testModel.Id;
        }

        public async Task UpdateTestModelAsync(Guid id, TestModelDTO testModel)
        {
            var existingEntry = testModelContext.TestModels.FirstOrDefault(x => x.Id == id);
            if (existingEntry is null)
            {
                throw new KeyNotFoundException(nameof(existingEntry));
            }
            existingEntry.Name = testModel.Name;
            existingEntry.LastName = testModel.LastName;
            await testModelContext.SaveChangesAsync();
        }

        public async Task DeleteTestModelAsync(Guid id)
        {
            var existingEntry = testModelContext.TestModels.FirstOrDefault(x => x.Id == id);
            if (existingEntry is null)
            {
                throw new KeyNotFoundException(nameof(existingEntry));
            }
            testModelContext.TestModels.Remove(existingEntry);
            await testModelContext.SaveChangesAsync();
            await testModelContext.SaveChangesAsync();
        }
    }
}
