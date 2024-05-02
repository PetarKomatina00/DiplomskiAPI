using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiplomskiAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddMorePropsToLek : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BestSeller",
                table: "Lekovi",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MainCategory",
                table: "Lekovi",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
                columns: new[] { "BestSeller", "MainCategory", "SideCategory" },
                values: new object[] { false, "Main", "Side" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BestSeller",
                table: "Lekovi");

            migrationBuilder.DropColumn(
                name: "MainCategory",
                table: "Lekovi");

            migrationBuilder.DropColumn(
                name: "SideCategory",
                table: "Lekovi");
        }
    }
}
