namespace Aoe2BuildOrders.Api.Dtos;

public class UpdateBuildOrderStepRequest
{
    public int StepNumber { get; set; }
    public int? Population { get; set; }
    public required string Age { get; set; }
    public required string Instruction { get; set; }
    public string? ResourceFocus { get; set; }
    public string? Notes { get; set; }
}