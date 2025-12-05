using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChangOnline.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryToProviderCategoryModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "ProviderCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "ProviderCategories");
        }
    }
}
