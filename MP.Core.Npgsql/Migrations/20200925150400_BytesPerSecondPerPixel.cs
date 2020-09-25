using Microsoft.EntityFrameworkCore.Migrations;

namespace MP.Core.Npgsql.Migrations
{
    public partial class BytesPerSecondPerPixel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BytesPerSecondPerPixel",
                table: "MediaFiles",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BytesPerSecondPerPixel",
                table: "MediaFiles");
        }
    }
}
