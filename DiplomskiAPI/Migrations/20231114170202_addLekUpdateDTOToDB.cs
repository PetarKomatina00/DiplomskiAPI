using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiplomskiAPI.Migrations
{
    /// <inheritdoc />
    public partial class addLekUpdateDTOToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Lekovi",
                columns: new[] { "LekID", "Description", "ISBN", "Image", "NazivLeka", "Price" },
                values: new object[] { 1, "Vitamin", "12345", "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi5.walmartimages.com%2Fasr%2F5eeea1fc-e2aa-4ff3-b4af-727c43b82143.bf48df129ffc63fe459af913a3ae973a.jpeg&f=1&nofb=1&ipt=25c48e641f608ddf0de24a2363f208954b3f7883d349c03e8ec4365c9d0a0b9d&ipo=images", "Mg", 1200.0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Lekovi",
                keyColumn: "LekID",
                keyValue: 1);
        }
    }
}
