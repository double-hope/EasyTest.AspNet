using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyTest.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Create_UserTest_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfAttempts",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfAttempts",
                table: "Tests");
        }
    }
}
