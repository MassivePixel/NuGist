using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NuGist.Web.Data.Migrations
{
    public partial class GistFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GistFile_AspNetUsers_CreatedById",
                table: "GistFile");

            migrationBuilder.DropForeignKey(
                name: "FK_GistFile_Gists_GistId",
                table: "GistFile");

            migrationBuilder.DropForeignKey(
                name: "FK_GistFile_AspNetUsers_ModifiedById",
                table: "GistFile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GistFile",
                table: "GistFile");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GistFiles",
                table: "GistFile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GistFiles_AspNetUsers_CreatedById",
                table: "GistFile",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GistFiles_Gists_GistId",
                table: "GistFile",
                column: "GistId",
                principalTable: "Gists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GistFiles_AspNetUsers_ModifiedById",
                table: "GistFile",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameIndex(
                name: "IX_GistFile_ModifiedById",
                table: "GistFile",
                newName: "IX_GistFiles_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_GistFile_GistId",
                table: "GistFile",
                newName: "IX_GistFiles_GistId");

            migrationBuilder.RenameIndex(
                name: "IX_GistFile_CreatedById",
                table: "GistFile",
                newName: "IX_GistFiles_CreatedById");

            migrationBuilder.RenameTable(
                name: "GistFile",
                newName: "GistFiles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GistFiles_AspNetUsers_CreatedById",
                table: "GistFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_GistFiles_Gists_GistId",
                table: "GistFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_GistFiles_AspNetUsers_ModifiedById",
                table: "GistFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GistFiles",
                table: "GistFiles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GistFile",
                table: "GistFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GistFile_AspNetUsers_CreatedById",
                table: "GistFiles",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GistFile_Gists_GistId",
                table: "GistFiles",
                column: "GistId",
                principalTable: "Gists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GistFile_AspNetUsers_ModifiedById",
                table: "GistFiles",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameIndex(
                name: "IX_GistFiles_ModifiedById",
                table: "GistFiles",
                newName: "IX_GistFile_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_GistFiles_GistId",
                table: "GistFiles",
                newName: "IX_GistFile_GistId");

            migrationBuilder.RenameIndex(
                name: "IX_GistFiles_CreatedById",
                table: "GistFiles",
                newName: "IX_GistFile_CreatedById");

            migrationBuilder.RenameTable(
                name: "GistFiles",
                newName: "GistFile");
        }
    }
}
