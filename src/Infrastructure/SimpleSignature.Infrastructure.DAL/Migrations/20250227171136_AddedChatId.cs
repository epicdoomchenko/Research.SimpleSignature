using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleSignature.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedChatId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Userame",
                table: "Users",
                newName: "Username");

            migrationBuilder.AddColumn<long>(
                name: "ChatId",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "Userame");
        }
    }
}
