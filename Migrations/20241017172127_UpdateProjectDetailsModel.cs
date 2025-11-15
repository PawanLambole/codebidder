using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeBidder.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectDetailsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CoreFeatures",
                table: "ProjectDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Competitors",
                table: "ProjectDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IntendedAudience",
                table: "ProjectDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectScope",
                table: "ProjectDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Competitors",
                table: "ProjectDetails");

            migrationBuilder.DropColumn(
                name: "IntendedAudience",
                table: "ProjectDetails");

            migrationBuilder.DropColumn(
                name: "ProjectScope",
                table: "ProjectDetails");

            migrationBuilder.AlterColumn<string>(
                name: "CoreFeatures",
                table: "ProjectDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
