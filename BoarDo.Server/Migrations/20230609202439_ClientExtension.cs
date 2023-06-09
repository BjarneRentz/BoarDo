using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoarDo.Server.Migrations
{
    /// <inheritdoc />
    public partial class ClientExtension : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "OAuthClients",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClientSecret",
                table: "OAuthClients",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "OAuthClients");

            migrationBuilder.DropColumn(
                name: "ClientSecret",
                table: "OAuthClients");
        }
    }
}
