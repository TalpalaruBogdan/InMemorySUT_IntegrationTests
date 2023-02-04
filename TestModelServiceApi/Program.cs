using ApiIntegrationTests.Data;
using ApiIntegrationTests.DTOs;
using ApiIntegrationTests.Maps;
using ApiIntegrationTests.Models;
using ApiIntegrationTests.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMapper>(TestModelMap.Instance);
builder.Services.AddScoped<ITestModelRepository, TestModelRepository>();
builder.Services.AddDbContext<TestModelContext>(opt => opt.UseInMemoryDatabase("TestModelDatabase"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/testmodels", async ([FromBody] TestModel testModel, ITestModelRepository repo) =>
{
    if (testModel is not TestModel)
    {
        return Results.UnprocessableEntity("Invalid data");
    }
    var existingEntry = await repo.GetTestModelByIdAsync(testModel.Id);
    if (existingEntry is not null)
    {
        return Results.Conflict($"TestModel with id {testModel.Id} already exists");
    }
    await repo.CreateTestModelAsync(testModel);
    return Results.Created($"/testmodels/{testModel.Id}", testModel);

})
.WithName("CreateTestModel")
.WithOpenApi();

app.MapGet("/testmodels", async (ITestModelRepository repo) =>
{
    return Results.Ok(await repo.GetTestModelsAsync());
})
.WithName("GetTestModels")
.WithOpenApi();

app.MapGet("/testmodels/{id}", async (Guid id, ITestModelRepository repo) =>
{
    var model = await repo.GetTestModelByIdAsync(id);
    return model is null ?
         Results.NotFound() :
         Results.Ok(model);
})
.WithName("GetTestModelById")
.WithOpenApi();

app.MapPut("/testmodels/{id}", async (Guid id, [FromBody] TestModelDTO testModelDto, ITestModelRepository repo) =>
{
    try
    {
        await repo.UpdateTestModelAsync(id, testModelDto);
        return Results.NoContent();
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound();
    }

})
.WithName("UpdateTestModel")
.WithOpenApi();

app.MapDelete("/testmodels/{id}", async (Guid id, ITestModelRepository repo) =>
{
    try
    {
        await repo.DeleteTestModelAsync(id);
        return Results.NoContent();
    }
        catch (KeyNotFoundException) 
    { 
        return Results.NotFound();
    }
})
.WithName("DeleteTestModel")
.WithOpenApi();

app.Run();