using Microsoft.EntityFrameworkCore.Migrations;

namespace MP.Core.Npgsql.Migrations
{
    public partial class NullablePixelFormat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PixelFormat",
                table: "VideoStreams",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PixelFormat",
                table: "VideoStreams",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
