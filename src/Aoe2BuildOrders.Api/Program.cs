using Aoe2BuildOrders.Api.Data;
using Aoe2BuildOrders.Api.Dtos;
using Aoe2BuildOrders.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/", () => Results.Ok(new
{
    name = "AoE2 Build Order Companion API",
    status = "running"
}));

app.MapGet("/api/buildorders", () =>
{
    var buildOrders = SeedData.BuildOrders
        .Select(buildOrder => new
        {
            buildOrder.Id,
            buildOrder.Name,
            buildOrder.Civilization,
            buildOrder.StrategyType,
            buildOrder.Difficulty,
            buildOrder.Description,
            StepCount = buildOrder.Steps.Count
        });

    return Results.Ok(buildOrders);
});

app.MapGet("/api/buildorders/{id:int}", (int id) =>
{
    var buildOrder = SeedData.BuildOrders.FirstOrDefault(buildOrder => buildOrder.Id == id);

    return buildOrder is null
        ? Results.NotFound()
        : Results.Ok(buildOrder);
});

app.MapPost("/api/buildorders", (CreateBuildOrderRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Name))
    {
        return Results.BadRequest("Name is required.");
    }

    if (string.IsNullOrWhiteSpace(request.Civilization))
    {
        return Results.BadRequest("Civilization is required.");
    }

    if (string.IsNullOrWhiteSpace(request.StrategyType))
    {
        return Results.BadRequest("Strategy type is required.");
    }

    if (string.IsNullOrWhiteSpace(request.Difficulty))
    {
        return Results.BadRequest("Difficulty is required.");
    }

    var nextBuildOrderId = SeedData.BuildOrders.Count == 0
        ? 1
        : SeedData.BuildOrders.Max(buildOrder => buildOrder.Id) + 1;

    var nextStepId = SeedData.BuildOrders
        .SelectMany(buildOrder => buildOrder.Steps)
        .DefaultIfEmpty()
        .Max(step => step?.Id ?? 0) + 1;

    var buildOrder = new BuildOrder
    {
        Id = nextBuildOrderId,
        Name = request.Name,
        Civilization = request.Civilization,
        StrategyType = request.StrategyType,
        Difficulty = request.Difficulty,
        Description = request.Description,
        Steps = request.Steps
            .OrderBy(step => step.StepNumber)
            .Select(step => new BuildOrderStep
            {
                Id = nextStepId++,
                BuildOrderId = nextBuildOrderId,
                StepNumber = step.StepNumber,
                Population = step.Population,
                Age = step.Age,
                Instruction = step.Instruction,
                ResourceFocus = step.ResourceFocus,
                Notes = step.Notes
            })
            .ToList()
    };

    SeedData.BuildOrders.Add(buildOrder);

    return Results.Created($"/api/buildorders/{buildOrder.Id}", buildOrder);
});

app.MapPut("/api/buildorders/{id:int}", (int id, UpdateBuildOrderRequest request) =>
{
    var existingBuildOrder = SeedData.BuildOrders.FirstOrDefault(buildOrder => buildOrder.Id == id);

    if (existingBuildOrder is null)
    {
        return Results.NotFound();
    }

    if (string.IsNullOrWhiteSpace(request.Name))
    {
        return Results.BadRequest("Name is required.");
    }

    if (string.IsNullOrWhiteSpace(request.Civilization))
    {
        return Results.BadRequest("Civilization is required.");
    }

    if (string.IsNullOrWhiteSpace(request.StrategyType))
    {
        return Results.BadRequest("Strategy type is required.");
    }

    if (string.IsNullOrWhiteSpace(request.Difficulty))
    {
        return Results.BadRequest("Difficulty is required.");
    }

    var nextStepId = SeedData.BuildOrders
        .SelectMany(buildOrder => buildOrder.Steps)
        .DefaultIfEmpty()
        .Max(step => step?.Id ?? 0) + 1;

    existingBuildOrder.Name = request.Name;
    existingBuildOrder.Civilization = request.Civilization;
    existingBuildOrder.StrategyType = request.StrategyType;
    existingBuildOrder.Difficulty = request.Difficulty;
    existingBuildOrder.Description = request.Description;
    existingBuildOrder.Steps = request.Steps
        .OrderBy(step => step.StepNumber)
        .Select(step => new BuildOrderStep
        {
            Id = nextStepId++,
            BuildOrderId = existingBuildOrder.Id,
            StepNumber = step.StepNumber,
            Population = step.Population,
            Age = step.Age,
            Instruction = step.Instruction,
            ResourceFocus = step.ResourceFocus,
            Notes = step.Notes
        })
        .ToList();

    return Results.Ok(existingBuildOrder);
});

app.MapDelete("/api/buildorders/{id:int}", (int id) =>
{
    var buildOrder = SeedData.BuildOrders.FirstOrDefault(buildOrder => buildOrder.Id == id);

    if (buildOrder is null)
    {
        return Results.NotFound();
    }

    SeedData.BuildOrders.Remove(buildOrder);

    return Results.NoContent();
});

app.Run();
