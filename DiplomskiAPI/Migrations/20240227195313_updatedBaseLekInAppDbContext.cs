using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiplomskiAPI.Migrations
{
    /// <inheritdoc />
    public partial class updatedBaseLekInAppDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Lekovi",
                keyColumn: "LekID",
                keyValue: 1,
                columns: new[] { "Description", "Image", "MainCategory", "NazivLeka" },
                values: new object[] { "60 Veggie Capsules", "https://diplomskislike.blob.core.windows.net/diplomski/arginine.png", "Supplements", "Arginine" });

            migrationBuilder.UpdateData(
                table: "Lekovi",
                keyColumn: "LekID",
                keyValue: 2,
                columns: new[] { "Description", "Image", "MainCategory", "NazivLeka" },
                values: new object[] { "30 Veggies Capsules", "https://diplomskislike.blob.core.windows.net/diplomski/5HTPMelatonin.png", "5-HTP", "5-HTP Melatonin" });

            migrationBuilder.UpdateData(
                table: "Lekovi",
                keyColumn: "LekID",
                keyValue: 3,
                column: "Image",
                value: "https://diplomskislike.blob.core.windows.net/diplomski/5-htp-.png");

            migrationBuilder.UpdateData(
                table: "Lekovi",
                keyColumn: "LekID",
                keyValue: 4,
                column: "Image",
                value: "https://diplomskislike.blob.core.windows.net/diplomski/Calcium, Mg, Zinc, D3.png");

            migrationBuilder.UpdateData(
                table: "Lekovi",
                keyColumn: "LekID",
                keyValue: 5,
                column: "Image",
                value: "https://diplomskislike.blob.core.windows.net/diplomski/omega-3.png");

            migrationBuilder.InsertData(
                table: "Lekovi",
                columns: new[] { "LekID", "BestSeller", "Description", "ISBN", "Image", "MainCategory", "NazivLeka", "Price", "TimesBought" },
                values: new object[] { 6, true, "120 DHA per Softgel, 100 SoftGels", "11118", "https://diplomskislike.blob.core.windows.net/diplomski/cvitamin.png", "Omega-3", "Vitamin C", 6.7199999999999998, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Lekovi",
                keyColumn: "LekID",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "Lekovi",
                keyColumn: "LekID",
                keyValue: 1,
                columns: new[] { "Description", "Image", "MainCategory", "NazivLeka" },
                values: new object[] { "High Absorption Magnesium", "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi5.walmartimages.com%2Fasr%2F5eeea1fc-e2aa-4ff3-b4af-727c43b82143.bf48df129ffc63fe459af913a3ae973a.jpeg&f=1&nofb=1&ipt=25c48e641f608ddf0de24a2363f208954b3f7883d349c03e8ec4365c9d0a0b9d&ipo=images", "Magnezijum", "Magnezijum" });

            migrationBuilder.UpdateData(
                table: "Lekovi",
                keyColumn: "LekID",
                keyValue: 2,
                columns: new[] { "Description", "Image", "MainCategory", "NazivLeka" },
                values: new object[] { "500 mg 120 Veggie Capsules", "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi5.walmartimages.com%2Fasr%2F5eeea1fc-e2aa-4ff3-b4af-727c43b82143.bf48df129ffc63fe459af913a3ae973a.jpeg&f=1&nofb=1&ipt=25c48e641f608ddf0de24a2363f208954b3f7883d349c03e8ec4365c9d0a0b9d&ipo=images", "Ashwagandha", "Ashwagandha" });

            migrationBuilder.UpdateData(
                table: "Lekovi",
                keyColumn: "LekID",
                keyValue: 3,
                column: "Image",
                value: "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi5.walmartimages.com%2Fasr%2F5eeea1fc-e2aa-4ff3-b4af-727c43b82143.bf48df129ffc63fe459af913a3ae973a.jpeg&f=1&nofb=1&ipt=25c48e641f608ddf0de24a2363f208954b3f7883d349c03e8ec4365c9d0a0b9d&ipo=images");

            migrationBuilder.UpdateData(
                table: "Lekovi",
                keyColumn: "LekID",
                keyValue: 4,
                column: "Image",
                value: "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi5.walmartimages.com%2Fasr%2F5eeea1fc-e2aa-4ff3-b4af-727c43b82143.bf48df129ffc63fe459af913a3ae973a.jpeg&f=1&nofb=1&ipt=25c48e641f608ddf0de24a2363f208954b3f7883d349c03e8ec4365c9d0a0b9d&ipo=images");

            migrationBuilder.UpdateData(
                table: "Lekovi",
                keyColumn: "LekID",
                keyValue: 5,
                column: "Image",
                value: "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi5.walmartimages.com%2Fasr%2F5eeea1fc-e2aa-4ff3-b4af-727c43b82143.bf48df129ffc63fe459af913a3ae973a.jpeg&f=1&nofb=1&ipt=25c48e641f608ddf0de24a2363f208954b3f7883d349c03e8ec4365c9d0a0b9d&ipo=images");
        }
    }
}
