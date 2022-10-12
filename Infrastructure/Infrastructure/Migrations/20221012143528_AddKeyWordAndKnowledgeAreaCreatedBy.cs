using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddKeyWordAndKnowledgeAreaCreatedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "KnowledgeArea",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedById",
                table: "KnowledgeArea",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "KeyWord",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedById",
                table: "KeyWord",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "KnowledgeArea");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "KnowledgeArea");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "KeyWord");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "KeyWord");
        }
    }
}
