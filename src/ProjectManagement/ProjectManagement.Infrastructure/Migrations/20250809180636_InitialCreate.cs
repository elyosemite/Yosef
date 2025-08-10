using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrganizationName = table.Column<string>(type: "text", nullable: false),
                    ContributorsCount = table.Column<int>(type: "integer", nullable: false),
                    Secret = table.Column<string>(type: "text", nullable: true),
                    Identifier = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationDataModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectDataModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    StarsCount = table.Column<int>(type: "integer", nullable: false),
                    ForksCount = table.Column<int>(type: "integer", nullable: false),
                    ContributorsCount = table.Column<int>(type: "integer", nullable: false),
                    Identifier = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationIdentifier = table.Column<Guid>(type: "uuid", nullable: false)
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
