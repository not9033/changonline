using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChangOnline.Migrations
{
    /// <inheritdoc />
    public partial class AddProviderCategoriesAndManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProviderCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProviderCategoryModelProviderProfile",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    ProviderProfilesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderCategoryModelProviderProfile", x => new { x.CategoriesId, x.ProviderProfilesId });
                    table.ForeignKey(
                        name: "FK_ProviderCategoryModelProviderProfile_ProviderCategories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "ProviderCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProviderCategoryModelProviderProfile_ProviderProfiles_ProviderProfilesId",
                        column: x => x.ProviderProfilesId,
                        principalTable: "ProviderProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProviderCategoryModelProviderProfile_ProviderProfilesId",
                table: "ProviderCategoryModelProviderProfile",
                column: "ProviderProfilesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProviderCategoryModelProviderProfile");

            migrationBuilder.DropTable(
                name: "ProviderCategories");
        }
    }
}
