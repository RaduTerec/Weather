using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Weather.Api.Migrations
{
   /// <inheritdoc />
   public partial class CitiesAndMeasurements : Migration
   {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.CreateTable(
             name: "Cities",
             columns: table => new
             {
                Id = table.Column<int>(type: "int", nullable: false)
                     .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                     .Annotation("MySql:CharSet", "utf8mb4")
             },
             constraints: table =>
             {
                table.PrimaryKey("PK_Cities", x => x.Id);
             })
             .Annotation("MySql:CharSet", "utf8mb4");

         migrationBuilder.CreateTable(
             name: "Measurements",
             columns: table => new
             {
                Id = table.Column<int>(type: "int", nullable: false)
                     .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Temperature = table.Column<float>(type: "float", nullable: false),
                Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                UserId = table.Column<int>(type: "int", nullable: false),
                CityId = table.Column<int>(type: "int", nullable: false)
             },
             constraints: table =>
             {
                table.PrimaryKey("PK_Measurements", x => x.Id);
                table.ForeignKey(
                       name: "FK_Measurements_Cities_CityId",
                       column: x => x.CityId,
                       principalTable: "Cities",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                       name: "FK_Measurements_Users_UserId",
                       column: x => x.UserId,
                       principalTable: "Users",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);
             })
             .Annotation("MySql:CharSet", "utf8mb4");

         migrationBuilder.CreateIndex(
             name: "IX_Measurements_CityId",
             table: "Measurements",
             column: "CityId");

         migrationBuilder.CreateIndex(
             name: "IX_Measurements_UserId",
             table: "Measurements",
             column: "UserId");
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.DropTable(
             name: "Measurements");

         migrationBuilder.DropTable(
             name: "Cities");
      }
   }
}
