using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bikamovie.Migrations
{
    public partial class AddUserNameToFavorites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Favorites",
                newName: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Favorites",
                newName: "Username");
        }
    }
}
