using System.Net;
using System.Net.Http.Json;
using Aoe2BuildOrders.Api.Data;
using Aoe2BuildOrders.Api.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Storage;

namespace Aoe2BuildOrders.Tests;

public class BuildOrdersApiTests
{
    [Fact]
    public async Task GetBuildOrders_ReturnsSeededBuildOrders()
    {
        await using var application = new BuildOrdersApiApplication();

        var client = application.CreateClient();

        var buildOrders = await client.GetFromJsonAsync<List<BuildOrderSummaryResponse>>(
            "/api/buildorders");

        Assert.NotNull(buildOrders);
        Assert.NotEmpty(buildOrders);
    }

    [Fact]
    public async Task GetBuildOrderById_ReturnsBuildOrderDetails()
    {
        await using var application = new BuildOrdersApiApplication();

        var client = application.CreateClient();

        var buildOrders = await client.GetFromJsonAsync<List<BuildOrderSummaryResponse>>(
            "/api/buildorders");

        var firstBuildOrderId = buildOrders![0].Id;

        var buildOrder = await client.GetFromJsonAsync<BuildOrderDetailResponse>(
            $"/api/buildorders/{firstBuildOrderId}");

        Assert.NotNull(buildOrder);
        Assert.Equal(firstBuildOrderId, buildOrder.Id);
        Assert.NotEmpty(buildOrder.Steps);
    }

    [Fact]
    public async Task CreateBuildOrder_ReturnsCreatedBuildOrder()
    {
        await using var application = new BuildOrdersApiApplication();

        var client = application.CreateClient();

        var request = new CreateBuildOrderRequest
        {
            Name = "Test Scouts",
            Civilization = "Franks",
            StrategyType = "Scouts",
            Difficulty = "Beginner",
            Description = "A test build order created from an integration test.",
            Steps =
            [
                new CreateBuildOrderStepRequest
                {
                    StepNumber = 1,
                    Population = 6,
                    Age = "Dark Age",
                    Instruction = "Send starting villagers to sheep.",
                    ResourceFocus = "Food",
                    Notes = null
                }
            ]
        };

        var response = await client.PostAsJsonAsync("/api/buildorders", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdBuildOrder =
            await response.Content.ReadFromJsonAsync<BuildOrderDetailResponse>();

        Assert.NotNull(createdBuildOrder);
        Assert.Equal("Test Scouts", createdBuildOrder.Name);
        Assert.Single(createdBuildOrder.Steps);
    }

    [Fact]
    public async Task DeleteBuildOrder_RemovesBuildOrder()
    {
        await using var application = new BuildOrdersApiApplication();

        var client = application.CreateClient();

        var request = new CreateBuildOrderRequest
        {
            Name = "Build order to delete",
            Civilization = "Generic",
            StrategyType = "Test",
            Difficulty = "Beginner",
            Description = "Temporary test build order.",
            Steps =
            [
                new CreateBuildOrderStepRequest
                {
                    StepNumber = 1,
                    Population = 6,
                    Age = "Dark Age",
                    Instruction = "Send starting villagers to sheep.",
                    ResourceFocus = "Food",
                    Notes = null
                }
            ]
        };

        var createResponse = await client.PostAsJsonAsync("/api/buildorders", request);
        var createdBuildOrder =
            await createResponse.Content.ReadFromJsonAsync<BuildOrderDetailResponse>();

        var deleteResponse = await client.DeleteAsync(
            $"/api/buildorders/{createdBuildOrder!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getDeletedResponse = await client.GetAsync(
            $"/api/buildorders/{createdBuildOrder.Id}");

        Assert.Equal(HttpStatusCode.NotFound, getDeletedResponse.StatusCode);
    }

private sealed class BuildOrdersApiApplication : WebApplicationFactory<Program>
{
    private readonly InMemoryDatabaseRoot _databaseRoot = new();

    public BuildOrdersApiApplication()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureTestServices(services =>
        {
            var dbContextDescriptors = services
                .Where(descriptor =>
                    descriptor.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                    descriptor.ServiceType == typeof(DbContextOptions))
                .ToList();

            foreach (var descriptor in dbContextDescriptors)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("BuildOrdersTestDb", _databaseRoot);
            });
        });
    }
}

    private sealed record BuildOrderSummaryResponse(
        int Id,
        string Name,
        string Civilization,
        string StrategyType,
        string Difficulty,
        string? Description,
        int StepCount);

    private sealed record BuildOrderDetailResponse(
        int Id,
        string Name,
        string Civilization,
        string StrategyType,
        string Difficulty,
        string? Description,
        List<BuildOrderStepResponse> Steps);

    private sealed record BuildOrderStepResponse(
        int Id,
        int BuildOrderId,
        int StepNumber,
        int? Population,
        string Age,
        string Instruction,
        string? ResourceFocus,
        string? Notes);
}
