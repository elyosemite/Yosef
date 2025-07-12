using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrganizationDataModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrganizationName = table.Column<string>(type: "TEXT", nullable: false),
                    ContributorsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Secret = table.Column<string>(type: "TEXT", nullable: true),
                    Identifier = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationDataModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectDataModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    StarsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    ForksCount = table.Column<int>(type: "INTEGER", nullable: false),
                    ContributorsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Identifier = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrganizationIdentifier = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDataModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectDataModel_OrganizationDataModel_Id",
                        column: x => x.Id,
                        principalTable: "OrganizationDataModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectDataModel");

            migrationBuilder.DropTable(
                name: "OrganizationDataModel");
        }
    }
}
