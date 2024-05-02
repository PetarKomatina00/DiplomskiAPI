using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiplomskiAPI.Migrations
{
    /// <inheritdoc />
    public partial class removedSideCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SideCategory",
                table: "Lekovi");

            migrationBuilder.UpdateData(
                table: "Lekovi",
                keyColumn: "LekID",
                keyValue: 1,
                column: "BestSeller",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SideCategory",
                table: "Lekovi",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Lekovi",
                keyColumn: "LekID",
                keyValue: 1,
                columns: new[] { "BestSeller", "SideCategory" },
                values: new object[] { false, "Side" });
        }
    }
}
