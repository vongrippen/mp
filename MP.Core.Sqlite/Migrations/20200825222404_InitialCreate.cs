using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MP.Core.Sqlite.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MediaFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    FileExt = table.Column<string>(nullable: true),
                    FilePath = table.Column<string>(maxLength: 1000, nullable: true),
                    Size = table.Column<long>(nullable: false),
                    ContentType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Analyses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analyses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Analyses_MediaFiles_FileId",
                        column: x => x.FileId,
                        principalTable: "MediaFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaFormats",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Duration = table.Column<TimeSpan>(nullable: false),
                    FormatName = table.Column<string>(nullable: false),
                    FormatLongName = table.Column<string>(nullable: false),
                    StreamCount = table.Column<int>(nullable: false),
                    ProbeScore = table.Column<double>(nullable: false),
                    BitRate = table.Column<double>(nullable: false),
                    AnalysisId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFormats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaFormats_Analyses_AnalysisId",
                        column: x => x.AnalysisId,
                        principalTable: "Analyses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AudioStreams",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Index = table.Column<int>(nullable: false),
                    CodecName = table.Column<string>(nullable: false),
                    CodecLongName = table.Column<string>(nullable: false),
                    BitRate = table.Column<int>(nullable: false),
                    Duration = table.Column<TimeSpan>(nullable: false),
                    Language = table.Column<string>(nullable: true),
                    Channels = table.Column<int>(nullable: false),
                    ChannelLayout = table.Column<string>(nullable: false),
                    SampleRateHz = table.Column<int>(nullable: false),
                    Profile = table.Column<string>(nullable: false),
                    AnalysisId = table.Column<Guid>(nullable: false),
                    MediaFormatId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AudioStreams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AudioStreams_Analyses_AnalysisId",
                        column: x => x.AnalysisId,
                        principalTable: "Analyses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AudioStreams_MediaFormats_MediaFormatId",
                        column: x => x.MediaFormatId,
                        principalTable: "MediaFormats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VideoStreams",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Index = table.Column<int>(nullable: false),
                    CodecName = table.Column<string>(nullable: false),
                    CodecLongName = table.Column<string>(nullable: false),
                    BitRate = table.Column<int>(nullable: false),
                    Duration = table.Column<TimeSpan>(nullable: false),
                    Language = table.Column<string>(nullable: true),
                    AvgFrameRate = table.Column<double>(nullable: false),
                    BitsPerRawSample = table.Column<int>(nullable: false),
                    DisplayAspectRatio = table.Column<string>(nullable: false),
                    Profile = table.Column<string>(nullable: false),
                    Width = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    FrameRate = table.Column<double>(nullable: false),
                    PixelFormat = table.Column<string>(nullable: false),
                    Rotation = table.Column<int>(nullable: false),
                    AnalysisId = table.Column<Guid>(nullable: false),
                    MediaFormatId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoStreams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoStreams_Analyses_AnalysisId",
                        column: x => x.AnalysisId,
                        principalTable: "Analyses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VideoStreams_MediaFormats_MediaFormatId",
                        column: x => x.MediaFormatId,
                        principalTable: "MediaFormats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Analyses_FileId",
                table: "Analyses",
                column: "FileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AudioStreams_AnalysisId",
                table: "AudioStreams",
                column: "AnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_AudioStreams_MediaFormatId",
                table: "AudioStreams",
                column: "MediaFormatId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFormats_AnalysisId",
                table: "MediaFormats",
                column: "AnalysisId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VideoStreams_AnalysisId",
                table: "VideoStreams",
                column: "AnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoStreams_MediaFormatId",
                table: "VideoStreams",
                column: "MediaFormatId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AudioStreams");

            migrationBuilder.DropTable(
                name: "VideoStreams");

            migrationBuilder.DropTable(
                name: "MediaFormats");

            migrationBuilder.DropTable(
                name: "Analyses");

            migrationBuilder.DropTable(
                name: "MediaFiles");
        }
    }
}
