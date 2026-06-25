using Aoe2BuildOrders.Api.Models;

namespace Aoe2BuildOrders.Api.Data;

public static class SeedData
{
    public static List<BuildOrder> BuildOrders { get; } =
    [
        new BuildOrder
        {
            Id = 1,
            Name = "21 Pop Scouts",
            Civilization = "Generic",
            StrategyType = "Scouts",
            Difficulty = "Beginner",
            Description = "A simple Feudal Age scout opening focused on early mobility and map control.",
            Steps =
            [
                new BuildOrderStep
                {
                    Id = 1,
                    BuildOrderId = 1,
                    StepNumber = 1,
                    Population = 6,
                    Age = "Dark Age",
                    Instruction = "Send starting villagers to sheep.",
                    ResourceFocus = "Food"
                },
                new BuildOrderStep
                {
                    Id = 2,
                    BuildOrderId = 1,
                    StepNumber = 2,
                    Population = 7,
                    Age = "Dark Age",
                    Instruction = "Send the next villager to build a lumber camp.",
                    ResourceFocus = "Wood"
                },
                new BuildOrderStep
                {
                    Id = 3,
                    BuildOrderId = 1,
                    StepNumber = 3,
                    Population = 10,
                    Age = "Dark Age",
                    Instruction = "Keep adding villagers to wood until you have four on wood.",
                    ResourceFocus = "Wood"
                },
                new BuildOrderStep
                {
                    Id = 4,
                    BuildOrderId = 1,
                    StepNumber = 4,
                    Population = 11,
                    Age = "Dark Age",
                    Instruction = "Lure the first boar.",
                    ResourceFocus = "Food"
                },
                new BuildOrderStep
                {
                    Id = 5,
                    BuildOrderId = 1,
                    StepNumber = 5,
                    Population = 15,
                    Age = "Dark Age",
                    Instruction = "Send new villagers to berries and build a mill.",
                    ResourceFocus = "Food"
                },
                new BuildOrderStep
                {
                    Id = 6,
                    BuildOrderId = 1,
                    StepNumber = 6,
                    Population = 18,
                    Age = "Dark Age",
                    Instruction = "Send new villagers to food under the town center.",
                    ResourceFocus = "Food"
                },
                new BuildOrderStep
                {
                    Id = 7,
                    BuildOrderId = 1,
                    StepNumber = 7,
                    Population = 21,
                    Age = "Dark Age",
                    Instruction = "Click up to Feudal Age.",
                    ResourceFocus = "Food"
                },
                new BuildOrderStep
                {
                    Id = 8,
                    BuildOrderId = 1,
                    StepNumber = 8,
                    Population = 21,
                    Age = "Feudal Age",
                    Instruction = "Build a stable and start producing scouts.",
                    ResourceFocus = "Food/Wood"
                }
            ]
        },
        new BuildOrder
        {
            Id = 2,
            Name = "Fast Castle",
            Civilization = "Generic",
            StrategyType = "Boom",
            Difficulty = "Intermediate",
            Description = "A standard economic opening aimed at reaching Castle Age quickly.",
            Steps =
            [
                new BuildOrderStep
                {
                    Id = 9,
                    BuildOrderId = 2,
                    StepNumber = 1,
                    Population = 6,
                    Age = "Dark Age",
                    Instruction = "Send starting villagers to sheep.",
                    ResourceFocus = "Food"
                },
                new BuildOrderStep
                {
                    Id = 10,
                    BuildOrderId = 2,
                    StepNumber = 2,
                    Population = 10,
                    Age = "Dark Age",
                    Instruction = "Send villagers to wood and build a lumber camp.",
                    ResourceFocus = "Wood"
                },
                new BuildOrderStep
                {
                    Id = 11,
                    BuildOrderId = 2,
                    StepNumber = 3,
                    Population = 12,
                    Age = "Dark Age",
                    Instruction = "Lure boar and continue food production.",
                    ResourceFocus = "Food"
                },
                new BuildOrderStep
                {
                    Id = 12,
                    BuildOrderId = 2,
                    StepNumber = 4,
                    Population = 16,
                    Age = "Dark Age",
                    Instruction = "Build mill on berries.",
                    ResourceFocus = "Food"
                },
                new BuildOrderStep
                {
                    Id = 13,
                    BuildOrderId = 2,
                    StepNumber = 5,
                    Population = 25,
                    Age = "Dark Age",
                    Instruction = "Click up to Feudal Age.",
                    ResourceFocus = "Food/Wood"
                },
                new BuildOrderStep
                {
                    Id = 14,
                    BuildOrderId = 2,
                    StepNumber = 6,
                    Population = 27,
                    Age = "Feudal Age",
                    Instruction = "Build market and blacksmith.",
                    ResourceFocus = "Wood/Gold"
                },
                new BuildOrderStep
                {
                    Id = 15,
                    BuildOrderId = 2,
                    StepNumber = 7,
                    Population = 28,
                    Age = "Feudal Age",
                    Instruction = "Click up to Castle Age.",
                    ResourceFocus = "Food/Gold"
                }
            ]
        }
    ];
}