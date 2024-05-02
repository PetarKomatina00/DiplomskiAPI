using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DiplomskiAPI.Migrations
{
    /// <inheritdoc />
    public partial class addStepsToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Steps",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Lekovi",
                keyColumn: "LekID",
                keyValue: 1,
                columns: new[] { "Description", "ISBN", "MainCategory", "NazivLeka", "Price" },
                values: new object[] { "High Absorption Magnesium", "11111", "Magnezijum", "Magnezijum", 9.8000000000000007 });

            migrationBuilder.InsertData(
                table: "Lekovi",
                columns: new[] { "LekID", "BestSeller", "Description", "ISBN", "Image", "MainCategory", "NazivLeka", "Price", "TimesBought" },
                values: new object[,]
                {
                    { 2, true, "500 mg 120 Veggie Capsules", "11112", "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi5.walmartimages.com%2Fasr%2F5eeea1fc-e2aa-4ff3-b4af-727c43b82143.bf48df129ffc63fe459af913a3ae973a.jpeg&f=1&nofb=1&ipt=25c48e641f608ddf0de24a2363f208954b3f7883d349c03e8ec4365c9d0a0b9d&ipo=images", "Ashwagandha", "Ashwagandha", 28.420000000000002, 0 },
                    { 3, true, "100mg 120 Veggie Capsules", "11113", "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi5.walmartimages.com%2Fasr%2F5eeea1fc-e2aa-4ff3-b4af-727c43b82143.bf48df129ffc63fe459af913a3ae973a.jpeg&f=1&nofb=1&ipt=25c48e641f608ddf0de24a2363f208954b3f7883d349c03e8ec4365c9d0a0b9d&ipo=images", "5-HTP", "5-HTP", 19.969999999999999, 0 },
                    { 4, true, "90 Tablets", "11114", "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi5.walmartimages.com%2Fasr%2F5eeea1fc-e2aa-4ff3-b4af-727c43b82143.bf48df129ffc63fe459af913a3ae973a.jpeg&f=1&nofb=1&ipt=25c48e641f608ddf0de24a2363f208954b3f7883d349c03e8ec4365c9d0a0b9d&ipo=images", "Minerals", "Calcium, Mg, Zinc + D3", 4.1600000000000001, 0 },
                    { 5, true, "120 DHA per Softgel, 100 SoftGels", "11115", "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi5.walmartimages.com%2Fasr%2F5eeea1fc-e2aa-4ff3-b4af-727c43b82143.bf48df129ffc63fe459af913a3ae973a.jpeg&f=1&nofb=1&ipt=25c48e641f608ddf0de24a2363f208954b3f7883d349c03e8ec4365c9d0a0b9d&ipo=images", "Omega-3", "Omega-3", 6.7199999999999998, 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Lekovi",
                keyColumn: "LekID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Lekovi",
                keyColumn: "LekID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Lekovi",
                keyColumn: "LekID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Lekovi",
                keyColumn: "LekID",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "Steps",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "Lekovi",
                keyColumn: "LekID",
                keyValue: 1,
                columns: new[] { "Description", "ISBN", "MainCategory", "NazivLeka", "Price" },
                values: new object[] { "Vitamin", "12345", "Main", "Mg", 1200.0 });
        }
    }
}
