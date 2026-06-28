using Aoe2BuildOrders.Api.Data;
using Aoe2BuildOrders.Api.Dtos;
using Aoe2BuildOrders.Api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactClient", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    dbContext.Database.Migrate();

    if (!dbContext.BuildOrders.Any())
    {
        dbContext.BuildOrders.AddRange(SeedData.BuildOrders);
        dbContext.SaveChanges();
    }
}

app.UseCors("AllowReactClient");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/", () => Results.Ok(new
{
    name = "AoE2 Build Order Companion API",
    status = "running"
}));

app.MapGet("/api/buildorders", async (AppDbContext dbContext) =>
{
    var buildOrders = await dbContext.BuildOrders
        .AsNoTracking()
        .Include(buildOrder => buildOrder.Steps)
        .Select(buildOrder => new
        {
            buildOrder.Id,
            buildOrder.Name,
            buildOrder.Civilization,
            buildOrder.StrategyType,
            buildOrder.Difficulty,
            buildOrder.Description,
            StepCount = buildOrder.Steps.Count
        })
        .ToListAsync();

    return Results.Ok(buildOrders);
});

app.MapGet("/api/buildorders/{id:int}", async (int id, AppDbContext dbContext) =>
{
    var buildOrder = await dbContext.BuildOrders
        .AsNoTracking()
        .Include(buildOrder => buildOrder.Steps)
        .FirstOrDefaultAsync(buildOrder => buildOrder.Id == id);

        if (buildOrder is null)
    {
        return Results.NotFound();
    }

    buildOrder.Steps = buildOrder.Steps
        .OrderBy(step => step.StepNumber)
        .ToList();

    return Results.Ok(buildOrder);
});

app.MapPost("/api/buildorders", async (CreateBuildOrderRequest request, AppDbContext dbContext) =>
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

    var buildOrder = new BuildOrder
    {
        Name = request.Name,
        Civilization = request.Civilization,
        StrategyType = request.StrategyType,
        Difficulty = request.Difficulty,
        Description = request.Description,
        Steps = request.Steps
            .OrderBy(step => step.StepNumber)
            .Select(step => new BuildOrderStep
            {
                StepNumber = step.StepNumber,
                Population = step.Population,
                Age = step.Age,
                Instruction = step.Instruction,
                ResourceFocus = step.ResourceFocus,
                Notes = step.Notes
            })
            .ToList()
    };

    dbContext.BuildOrders.Add(buildOrder);
    await dbContext.SaveChangesAsync();

    return Results.Created($"/api/buildorders/{buildOrder.Id}", buildOrder);
});

app.MapPut("/api/buildorders/{id:int}", async (int id, UpdateBuildOrderRequest request, AppDbContext dbContext) =>
{
    var existingBuildOrder = await dbContext.BuildOrders
        .Include(buildOrder => buildOrder.Steps)
        .FirstOrDefaultAsync(buildOrder => buildOrder.Id == id);

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

    existingBuildOrder.Name = request.Name;
    existingBuildOrder.Civilization = request.Civilization;
    existingBuildOrder.StrategyType = request.StrategyType;
    existingBuildOrder.Difficulty = request.Difficulty;
    existingBuildOrder.Description = request.Description;

    dbContext.BuildOrderSteps.RemoveRange(existingBuildOrder.Steps);

    existingBuildOrder.Steps = request.Steps
        .OrderBy(step => step.StepNumber)
        .Select(step => new BuildOrderStep
        {
            BuildOrderId = existingBuildOrder.Id,
            StepNumber = step.StepNumber,
            Population = step.Population,
            Age = step.Age,
            Instruction = step.Instruction,
            ResourceFocus = step.ResourceFocus,
            Notes = step.Notes
        })
        .ToList();

    await dbContext.SaveChangesAsync();

    return Results.Ok(existingBuildOrder);
});

app.MapDelete("/api/buildorders/{id:int}", async (int id, AppDbContext dbContext) =>
{
    var buildOrder = await dbContext.BuildOrders
        .FirstOrDefaultAsync(buildOrder => buildOrder.Id == id);

    if (buildOrder is null)
    {
        return Results.NotFound();
    }

    dbContext.BuildOrders.Remove(buildOrder);
    await dbContext.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();
