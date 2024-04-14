using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppleStore.Migrations
{
    /// <inheritdoc />
    public partial class updataaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderStatuses_Status_Id",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "OrderStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Status_Id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Status_Id",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Orders",
                newName: "PhoneNumber_Order");

            migrationBuilder.AddColumn<string>(
                name: "Email_Order",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName_Order",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email_Order",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FullName_Order",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber_Order",
                table: "Orders",
                newName: "PhoneNumber");

            migrationBuilder.AddColumn<int>(
                name: "Status_Id",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OrderStatuses",
                columns: table => new
                {
                    Status_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status_Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatuses", x => x.Status_Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Status_Id",
                table: "Orders",
                column: "Status_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderStatuses_Status_Id",
                table: "Orders",
                column: "Status_Id",
                principalTable: "OrderStatuses",
                principalColumn: "Status_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
