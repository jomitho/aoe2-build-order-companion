using Aoe2BuildOrders.Api.Data;

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

app.Run();
