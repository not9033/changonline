using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChangOnline.Migrations
{
    /// <inheritdoc />
    public partial class editProfileProviderModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Service",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ProviderProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Service = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: true),
                    DistrictId = table.Column<int>(type: "int", nullable: true),
                    SubdistrictId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProviderProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProviderProfiles_District_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "District",
                        principalColumn: "DistrictId");
                    table.ForeignKey(
                        name: "FK_ProviderProfiles_Province_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Province",
                        principalColumn: "ProvinceId");
                    table.ForeignKey(
                        name: "FK_ProviderProfiles_subdistrict_SubdistrictId",
                        column: x => x.SubdistrictId,
                        principalTable: "subdistrict",
                        principalColumn: "subdistrict_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProviderProfiles_DistrictId",
                table: "ProviderProfiles",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderProfiles_ProvinceId",
                table: "ProviderProfiles",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderProfiles_SubdistrictId",
                table: "ProviderProfiles",
                column: "SubdistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderProfiles_UserId",
                table: "ProviderProfiles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProviderProfiles");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Service",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
