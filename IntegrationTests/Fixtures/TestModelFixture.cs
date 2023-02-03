using ApiIntegrationTests.Models;

namespace IntegrationTests.Fixtures
{
    public class TestModelFixture
    {
        public TestModel DynamicTestModel
        {
            get
            {
                return new TestModel()
                {
                    Id = Guid.NewGuid(),
                    Name = Guid.NewGuid().ToString(),
                    LastName = Guid.NewGuid().ToString()
                };
            }
        }
    }
}
