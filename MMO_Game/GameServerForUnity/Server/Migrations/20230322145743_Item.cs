using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Migrations
{
    public partial class Item : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Account_AccountDBId",
                table: "Player");

            migrationBuilder.RenameColumn(
                name: "AccountDBId",
                table: "Player",
                newName: "AccountDbId");

            migrationBuilder.RenameIndex(
                name: "IX_Player_AccountDBId",
                table: "Player",
                newName: "IX_Player_AccountDbId");

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    ItemDbId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Slot = table.Column<int>(nullable: false),
                    OwnerDbId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.ItemDbId);
                    table.ForeignKey(
                        name: "FK_Item_Player_OwnerDbId",
                        column: x => x.OwnerDbId,
                        principalTable: "Player",
                        principalColumn: "PlayerDbId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Item_OwnerDbId",
                table: "Item",
                column: "OwnerDbId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Account_AccountDbId",
                table: "Player",
                column: "AccountDbId",
                principalTable: "Account",
                principalColumn: "AccountDbId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Account_AccountDbId",
                table: "Player");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.RenameColumn(
                name: "AccountDbId",
                table: "Player",
                newName: "AccountDBId");

            migrationBuilder.RenameIndex(
                name: "IX_Player_AccountDbId",
                table: "Player",
                newName: "IX_Player_AccountDBId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Account_AccountDBId",
                table: "Player",
                column: "AccountDBId",
                principalTable: "Account",
                principalColumn: "AccountDbId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
