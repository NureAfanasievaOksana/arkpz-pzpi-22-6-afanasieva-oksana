using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SortGarbageAPI.Migrations
{
    /// <inheritdoc />
    public partial class MakeUserIdNullableInContainers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Containers_Users_UserId",
                table: "Containers");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Containers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Containers_Users_UserId",
                table: "Containers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Containers_Users_UserId",
                table: "Containers");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Containers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Containers_Users_UserId",
                table: "Containers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
