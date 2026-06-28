using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aoe2BuildOrders.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BuildOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Civilization = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    StrategyType = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Difficulty = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BuildOrderSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuildOrderId = table.Column<int>(type: "int", nullable: false),
                    StepNumber = table.Column<int>(type: "int", nullable: false),
                    Population = table.Column<int>(type: "int", nullable: true),
                    Age = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Instruction = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ResourceFocus = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildOrderSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuildOrderSteps_BuildOrders_BuildOrderId",
                        column: x => x.BuildOrderId,
                        principalTable: "BuildOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BuildOrderSteps_BuildOrderId",
                table: "BuildOrderSteps",
                column: "BuildOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuildOrderSteps");

            migrationBuilder.DropTable(
                name: "BuildOrders");
        }
    }
}
