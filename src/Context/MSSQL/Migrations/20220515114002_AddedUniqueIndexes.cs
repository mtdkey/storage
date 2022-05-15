using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MtdKey.Storage.Context.MSSQL.Migrations
{
    public partial class AddedUniqueIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "uq_all_flag",
                table: "field",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "uq_bunch_flag",
                table: "field",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
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
