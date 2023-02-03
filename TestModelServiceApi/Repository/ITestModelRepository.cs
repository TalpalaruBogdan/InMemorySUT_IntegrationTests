using ApiIntegrationTests.Data;
using ApiIntegrationTests.DTOs;
using ApiIntegrationTests.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ApiIntegrationTests.Repository
{
    public interface ITestModelRepository
    {
        public Task<TestModel?> GetTestModelByIdAsync(Guid id);
        public Task<IList<TestModel>> GetTestModelsAsync();
        public Task<Guid> CreateTestModelAsync(TestModel testModel);
        public Task DeleteTestModelAsync(Guid id);
        public Task UpdateTestModelAsync(Guid id, TestModelDTO testModel);
    }
}
