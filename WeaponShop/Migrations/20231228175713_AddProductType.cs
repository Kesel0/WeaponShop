using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeaponShop.Migrations
{
    /// <inheritdoc />
    public partial class AddProductType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:prodtype", "Weapon,Accessory,Ammo");

            migrationBuilder.CreateSequence(
                name: "productid",
                startValue: 1000L);

            migrationBuilder.CreateSequence(
                name: "userid");

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('productid'::regclass)"),
                    product_name = table.Column<string>(type: "character varying", nullable: true),
                    product_description = table.Column<string>(type: "character varying", nullable: true),
                    ProductType = table.Column<int>(type: "integer", nullable: false),
                    product_subtype = table.Column<string>(type: "character varying", nullable: true),
                    product_price = table.Column<double>(type: "double precision", nullable: true),
                    caliber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Image = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("product_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('userid'::regclass)"),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropSequence(
                name: "productid");

            migrationBuilder.DropSequence(
                name: "userid");
        }
    }
}
