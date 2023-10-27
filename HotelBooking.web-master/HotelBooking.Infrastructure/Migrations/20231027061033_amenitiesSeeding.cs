using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class amenitiesSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Amenities",
                columns: table => new
                {
                    AmenityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VillaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenities", x => x.AmenityId);
                    table.ForeignKey(
                        name: "FK_Amenities_Villas_VillaId",
                        column: x => x.VillaId,
                        principalTable: "Villas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Amenities",
                columns: new[] { "AmenityId", "Description", "Name", "VillaId" },
                values: new object[,]
                {
                    { 1, "Fully Air Conditioned", "AC", 1 },
                    { 2, "Unlimited Wifi", "Wifi", 1 },
                    { 3, null, "Jaccuzi", 1 },
                    { 4, null, "Swimming Pool", 1 },
                    { 5, "Fully Air Conditioned", "AC", 9 },
                    { 6, "Unlimited Wifi", "Wifi", 9 },
                    { 7, null, "Jaccuzi", 9 },
                    { 8, null, "Swimming Pool", 9 },
                    { 9, "Fully Air Conditioned", "AC", 10 },
                    { 10, "Unlimited Wifi", "Wifi", 10 },
                    { 11, null, "Jaccuzi", 10 },
                    { 12, null, "Swimming Pool", 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Amenities_VillaId",
                table: "Amenities",
                column: "VillaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amenities");
        }
    }
}
