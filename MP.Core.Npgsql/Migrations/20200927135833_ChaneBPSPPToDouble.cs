using Microsoft.EntityFrameworkCore.Migrations;

namespace MP.Core.Npgsql.Migrations
{
    public partial class ChaneBPSPPToDouble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "BytesPerSecondPerPixel",
                table: "MediaFiles",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "BytesPerSecondPerPixel",
                table: "MediaFiles",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
