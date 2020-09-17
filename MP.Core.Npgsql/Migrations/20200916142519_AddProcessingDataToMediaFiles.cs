using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MP.Core.Npgsql.Migrations
{
    public partial class AddProcessingDataToMediaFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BytesPerSecond",
                table: "MediaFiles",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastProcessingUpdate",
                table: "MediaFiles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_BytesPerSecond",
                table: "MediaFiles",
                column: "BytesPerSecond");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_LastProcessingUpdate",
                table: "MediaFiles",
                column: "LastProcessingUpdate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MediaFiles_BytesPerSecond",
                table: "MediaFiles");

            migrationBuilder.DropIndex(
                name: "IX_MediaFiles_LastProcessingUpdate",
                table: "MediaFiles");

            migrationBuilder.DropColumn(
                name: "BytesPerSecond",
                table: "MediaFiles");

            migrationBuilder.DropColumn(
                name: "LastProcessingUpdate",
                table: "MediaFiles");
        }
    }
}
