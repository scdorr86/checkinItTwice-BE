using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace checkinItTwice_BE.Migrations
{
    public partial class initialSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Uid = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChristmasYears",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ListYear = table.Column<string>(type: "text", nullable: false),
                    YearBudget = table.Column<decimal>(type: "numeric", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChristmasYears", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChristmasYears_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Giftees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Giftees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Giftees_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GiftName = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    OrderedFrom = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gifts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChristmasLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ListName = table.Column<string>(type: "text", nullable: false),
                    YearId = table.Column<int>(type: "integer", nullable: false),
                    ChristmasYearId = table.Column<int>(type: "integer", nullable: false),
                    GifteeId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChristmasLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChristmasLists_ChristmasYears_ChristmasYearId",
                        column: x => x.ChristmasYearId,
                        principalTable: "ChristmasYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChristmasLists_Giftees_GifteeId",
                        column: x => x.GifteeId,
                        principalTable: "Giftees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChristmasLists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChristmasListGift",
                columns: table => new
                {
                    ChristmasListsId = table.Column<int>(type: "integer", nullable: false),
                    GiftsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChristmasListGift", x => new { x.ChristmasListsId, x.GiftsId });
                    table.ForeignKey(
                        name: "FK_ChristmasListGift_ChristmasLists_ChristmasListsId",
                        column: x => x.ChristmasListsId,
                        principalTable: "ChristmasLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChristmasListGift_Gifts_GiftsId",
                        column: x => x.GiftsId,
                        principalTable: "Gifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChristmasListGift_GiftsId",
                table: "ChristmasListGift",
                column: "GiftsId");

            migrationBuilder.CreateIndex(
                name: "IX_ChristmasLists_ChristmasYearId",
                table: "ChristmasLists",
                column: "ChristmasYearId");

            migrationBuilder.CreateIndex(
                name: "IX_ChristmasLists_GifteeId",
                table: "ChristmasLists",
                column: "GifteeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChristmasLists_UserId",
                table: "ChristmasLists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChristmasYears_UserId",
                table: "ChristmasYears",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Giftees_UserId",
                table: "Giftees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_UserId",
                table: "Gifts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChristmasListGift");

            migrationBuilder.DropTable(
                name: "ChristmasLists");

            migrationBuilder.DropTable(
                name: "Gifts");

            migrationBuilder.DropTable(
                name: "ChristmasYears");

            migrationBuilder.DropTable(
                name: "Giftees");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
