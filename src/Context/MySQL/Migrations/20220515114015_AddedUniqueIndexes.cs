using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MtdKey.Storage.Context.MySQL.Migrations
{
    public partial class AddedUniqueIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<sbyte>(
                name: "uq_all_flag",
                table: "field",
                type: "tinyint(2)",
                nullable: false,
                defaultValue: (sbyte)0);

            migrationBuilder.AddColumn<sbyte>(
                name: "uq_bunch_flag",
                table: "field",
                type: "tinyint(2)",
                nullable: false,
                defaultValue: (sbyte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "uq_all_flag",
                table: "field");

            migrationBuilder.DropColumn(
                name: "uq_bunch_flag",
                table: "field");
        }
    }
}
