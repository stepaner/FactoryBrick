using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoryBrick.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsumerTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dependence = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumerTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Consumers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsumerTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consumers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consumers_ConsumerTypes_ConsumerTypeId",
                        column: x => x.ConsumerTypeId,
                        principalTable: "ConsumerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.InsertData(
                table: "ConsumerTypes",
                columns: new[] { "Id", "Dependence", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "weather", "дом", "house" },
                    { 2, "price", "завод", "plant" }
                });

            migrationBuilder.CreateTable(
                name: "ConsumptionDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConsumerId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Consumption = table.Column<decimal>(type: "decimal(26,16)", nullable: false),
                    Dependence = table.Column<decimal>(type: "decimal(26,16)", nullable: false),
                    ImportDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumptionDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsumptionDatas_Consumers_ConsumerId",
                        column: x => x.ConsumerId,
                        principalTable: "Consumers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Consumers_ConsumerTypeId",
                table: "Consumers",
                column: "ConsumerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumptionDatas_ConsumerId",
                table: "ConsumptionDatas",
                column: "ConsumerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsumptionDatas");

            migrationBuilder.DropTable(
                name: "Consumers");

            migrationBuilder.DropTable(
                name: "ConsumerTypes");
        }
    }
}
