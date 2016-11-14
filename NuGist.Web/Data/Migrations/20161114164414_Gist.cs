using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NuGist.Web.Data.Migrations
{
    public partial class Gist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gists",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    InternalVersion = table.Column<int>(nullable: false),
                    Metadata = table.Column<string>(nullable: true),
                    ModifiedById = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Version = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gists_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Gists_AspNetUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GistFile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: true),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    FileName = table.Column<string>(maxLength: 64, nullable: false),
                    GistId = table.Column<int>(nullable: false),
                    InternalVersion = table.Column<int>(nullable: false),
                    ModifiedById = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    Type = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GistFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GistFile_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GistFile_Gists_GistId",
                        column: x => x.GistId,
                        principalTable: "Gists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GistFile_AspNetUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gists_CreatedById",
                table: "Gists",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Gists_ModifiedById",
                table: "Gists",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_GistFile_CreatedById",
                table: "GistFile",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_GistFile_GistId",
                table: "GistFile",
                column: "GistId");

            migrationBuilder.CreateIndex(
                name: "IX_GistFile_ModifiedById",
                table: "GistFile",
                column: "ModifiedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GistFile");

            migrationBuilder.DropTable(
                name: "Gists");
        }
    }
}
