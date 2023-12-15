using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_Web.Migrations
{
    /// <inheritdoc />
    public partial class Basev2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Contet",
                table: "BlogPosts",
                newName: "Content");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "BlogPosts",
                newName: "Contet");
        }
    }
}
