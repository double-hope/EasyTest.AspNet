using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyTest.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Add_QuestionsAttempted_To_Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionsAttempted",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionsAttempted",
                table: "Tests");
        }
    }
}
