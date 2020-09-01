using Microsoft.EntityFrameworkCore.Migrations;

namespace MP.Core.Npgsql.Migrations
{
    public partial class AddFilenameData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FilenameData",
                table: "MediaFiles",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilenameData",
                table: "MediaFiles");
        }
    }
}
