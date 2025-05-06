using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CasinoConsoleApp.Migrations
{
    /// <inheritdoc />
    public partial class FixClientGamesJoin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientGames_Clients_PlayersId",
                table: "ClientGames");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientGames_Games_GamesId",
                table: "ClientGames");

            migrationBuilder.RenameColumn(
                name: "PlayersId",
                table: "ClientGames",
                newName: "GameId");

            migrationBuilder.RenameColumn(
                name: "GamesId",
                table: "ClientGames",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientGames_PlayersId",
                table: "ClientGames",
                newName: "IX_ClientGames_GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientGames_Clients_ClientId",
                table: "ClientGames",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientGames_Games_GameId",
                table: "ClientGames",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientGames_Clients_ClientId",
                table: "ClientGames");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientGames_Games_GameId",
                table: "ClientGames");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "ClientGames",
                newName: "PlayersId");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "ClientGames",
                newName: "GamesId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientGames_GameId",
                table: "ClientGames",
                newName: "IX_ClientGames_PlayersId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientGames_Clients_PlayersId",
                table: "ClientGames",
                column: "PlayersId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientGames_Games_GamesId",
                table: "ClientGames",
                column: "GamesId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
