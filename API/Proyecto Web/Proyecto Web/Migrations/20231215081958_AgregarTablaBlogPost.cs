using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_Web.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTablaBlogPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlHandle",
                table: "BlogPosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlHandle",
                table: "BlogPosts");
        }
    }
}
