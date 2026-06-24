namespace Aoe2BuildOrders.Api.Models;

public class BuildOrder
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Civilization { get; set; }
    public required string StrategyType { get; set; }
    public required string Difficulty { get; set; }
    public string? Description { get; set; }

    public List<BuildOrderStep> Steps { get; set; } = [];
}