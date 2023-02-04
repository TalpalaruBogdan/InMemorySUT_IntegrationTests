using ApiIntegrationTests.Data;
using ApiIntegrationTests.Models;
using FluentAssertions;
using FluentAssertions.Execution;
using IntegrationTests.Factories;
using IntegrationTests.Fixtures;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using TestModelServiceApi;

namespace IntegrationTests.Tests
{
    public class TestModelServiceApiTests : 
        IClassFixture<CustomWebApplicationFactory<ITestMarker>>,
        IClassFixture<TestModelFixture>
    {
        private readonly CustomWebApplicationFactory<ITestMarker> _factory;
        private readonly TestModelFixture _fixture;
        private readonly HttpClient _client;

        public TestModelServiceApiTests(
            CustomWebApplicationFactory<ITestMarker> customWebApplicationFactory, 
            TestModelFixture fixture)
        {
            _factory = customWebApplicationFactory;
            _client = _factory.CreateDefaultClient();
            _fixture = fixture;
        }

        [Fact]
        public async Task Creates_TestModel()
        {
            // Arrange
            var testData = _fixture.DynamicTestModel;

            // Act
            var result = await _client.PostAsJsonAsync("/testmodels", testData);

            // Assert
            using (new AssertionScope())
            {
                result.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

                var databaseEntry = _factory.TestModelContext.TestModels.Where(x => x.Id == testData.Id).FirstOrDefault();
                databaseEntry.Should().BeEquivalentTo(testData);
            }
        }

        [Fact]
        public async Task Create_Returns_UnprocessableEntity_For_Duplicated_Entry()
        {
            // Arrange
            var testData = "{\"inexistentKey\":\"inexistentValue\"}";

            // Act
            var result = await _client.PostAsJsonAsync("/testmodels", testData);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Gets_TestModels()
        {
            // Arrange
            _factory.TestModelContext.AddRange(_fixture.DynamicTestModel, _fixture.DynamicTestModel);
            await _factory.TestModelContext.SaveChangesAsync();

            // Act
            var result = await _client.GetFromJsonAsync<List<TestModel>>("/testmodels");

            // Assert
            using (new AssertionScope())
            {
                result!.Should().HaveCount(_factory.TestModelContext.TestModels.Count());
                result!.ToList().Should().BeEquivalentTo(_factory.TestModelContext.TestModels.ToList());
            }
        }

        [Fact]
        public async Task Get_Returns_NotFound_For_Inexistent_Entry()
        {
            // Arrange

            // Act
            var result = await _client.GetAsync($"/testmodels/{Guid.NewGuid()}");

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Gets_TestModel()
        {
            // Arrange
            var testData = _fixture.DynamicTestModel;

            _factory.TestModelContext.Add(testData);
            await _factory.TestModelContext.SaveChangesAsync();

            // Act
            var result = await _client.GetFromJsonAsync<TestModel>($"/testmodels/{testData.Id}");

            // Assert
            using (new AssertionScope())
            {
                result!.Should().BeEquivalentTo(testData);
            }
        }

        [Fact]
        public async Task Deletes_TestModel()
        {
            // Arrange
            var testData = _fixture.DynamicTestModel;

            _factory.TestModelContext.Add(testData);
            await _factory.TestModelContext.SaveChangesAsync();

            // Act
            var result = await _client.DeleteAsync($"/testmodels/{testData.Id}");

            // Assert
            using (new AssertionScope())
            {
                result!.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
                _factory.TestModelContext.TestModels.FirstOrDefault(x => x.Id == testData.Id).Should().BeNull();
            }
        }

        [Fact]
        public async Task Delete_Returns_NotFound_For_Inexistent_Entry()
        {
            // Arrange

            // Act
            var result = await _client.DeleteAsync($"/testmodels/{Guid.NewGuid()}");

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_Returns_NotFound_For_Inexistent_Entry()
        {
            // Arrange
            var testData = _fixture.DynamicTestModel;
            var httpContent = new StringContent(JsonConvert.SerializeObject(testData), Encoding.UTF8, "application/json");

            // Act
            var result = await _client.PutAsync($"/testmodels/{testData.Id}", httpContent);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
