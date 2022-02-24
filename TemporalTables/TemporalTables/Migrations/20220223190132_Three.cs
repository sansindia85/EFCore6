using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemporalTables.Migrations
{
    public partial class Three : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "NewProductId",
                table: "Order",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "NewProduct",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewProduct", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_NewProductId",
                table: "Order",
                column: "NewProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_NewProduct_NewProductId",
                table: "Order",
                column: "NewProductId",
                principalTable: "NewProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_NewProduct_NewProductId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "NewProduct");

            migrationBuilder.DropIndex(
                name: "IX_Order_NewProductId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "NewProductId",
                table: "Order");
        }
    }
}
