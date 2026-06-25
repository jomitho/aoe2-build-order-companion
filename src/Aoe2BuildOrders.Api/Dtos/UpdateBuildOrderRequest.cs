namespace Aoe2BuildOrders.Api.Dtos;

public class UpdateBuildOrderRequest
{
    public required string Name { get; set; }
    public required string Civilization { get; set; }
    public required string StrategyType { get; set; }
    public required string Difficulty { get; set; }
    public string? Description { get; set; }

    public List<UpdateBuildOrderStepRequest> Steps { get; set; } = [];
}