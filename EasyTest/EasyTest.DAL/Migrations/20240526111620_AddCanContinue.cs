using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyTest.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCanContinue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanContinue",
                table: "UserTests",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanContinue",
                table: "UserTests");
        }
    }
}
