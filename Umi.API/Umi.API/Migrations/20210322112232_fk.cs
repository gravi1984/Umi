using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Umi.API.Migrations
{
    public partial class fk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineItems_ShoppingCarts_ShoppingCartId",
                table: "LineItems");

            migrationBuilder.DropIndex(
                name: "IX_LineItems_ShoppingCartId",
                table: "LineItems");

            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "LineItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShoppingCardId",
                table: "LineItems",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "R-000",
                column: "ConcurrencyStamp",
                value: "403e9c22-eda3-487b-af33-a56c2328e91e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "U-OOO",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5e8afe38-53d5-4e3d-8538-2d2adfbf0be5", "AQAAAAEAACcQAAAAEDv1mvShzg9pD7uVDpF5P669+Ip4RC+RTx7q7ym+/Qpc15RF4NMc1lnfxe/QRTS9hw==", "38b6e0a4-f06f-49c2-a186-ffe7d1e2c1ba" });

            migrationBuilder.CreateIndex(
                name: "IX_LineItems_ShoppingCardId",
                table: "LineItems",
                column: "ShoppingCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_LineItems_ShoppingCarts_ShoppingCardId",
                table: "LineItems",
                column: "ShoppingCardId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineItems_ShoppingCarts_ShoppingCardId",
                table: "LineItems");

            migrationBuilder.DropIndex(
                name: "IX_LineItems_ShoppingCardId",
                table: "LineItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShoppingCardId",
                table: "LineItems",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "ShoppingCartId",
                table: "LineItems",
                type: "char(36)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "R-000",
                column: "ConcurrencyStamp",
                value: "864ca886-e874-48fb-8b42-f6c089685f68");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "U-OOO",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "07ad4296-c388-4755-848d-82ab64015f8a", "AQAAAAEAACcQAAAAEHnp2cLB3WxwpkdPL1/TMrb2JKvSY6uq/PtdEtInug8FkXoP7OucVc8J/oUpqrREYg==", "3329ee83-a91d-4d35-818f-d6fd2d0835e7" });

            migrationBuilder.CreateIndex(
                name: "IX_LineItems_ShoppingCartId",
                table: "LineItems",
                column: "ShoppingCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_LineItems_ShoppingCarts_ShoppingCartId",
                table: "LineItems",
                column: "ShoppingCartId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
