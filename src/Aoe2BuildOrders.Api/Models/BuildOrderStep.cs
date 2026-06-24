namespace Aoe2BuildOrders.Api.Models;

public class BuildOrderStep
{
    public int Id { get; set; }
    public int BuildOrderId { get; set; }
    public int StepNumber { get; set; }
    public int? Population { get; set; }
    public required string Age { get; set; }
    public required string Instruction { get; set; }
    public string? ResourceFocus { get; set; }
    public string? Notes { get; set; }
}