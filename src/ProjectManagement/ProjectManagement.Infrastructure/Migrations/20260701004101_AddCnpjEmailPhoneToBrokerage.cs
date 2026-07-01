using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCnpjEmailPhoneToBrokerage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContributorsCount",
                table: "OrganizationDataModel");

            migrationBuilder.RenameColumn(
                name: "Secret",
                table: "OrganizationDataModel",
                newName: "Phone");

            migrationBuilder.AddColumn<string>(
                name: "CNPJ",
                table: "OrganizationDataModel",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "OrganizationDataModel",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CNPJ",
                table: "OrganizationDataModel");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "OrganizationDataModel");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "OrganizationDataModel",
                newName: "Secret");

            migrationBuilder.AddColumn<int>(
                name: "ContributorsCount",
                table: "OrganizationDataModel",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
