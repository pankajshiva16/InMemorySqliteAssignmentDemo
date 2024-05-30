using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InMemorySqliteAssignmentApp.Infrastructure_Data_.Migrations
{
    /// <inheritdoc />
    public partial class CustomerAvtar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerAvtar",
                table: "Customers",
                type: "TEXT",
                nullable: true,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerAvtar",
                table: "Customers");
        }
    }
}
