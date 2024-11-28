using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShopSolution.Data.Migrations
{
    /// <inheritdoc />
    public partial class reconfig_product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "brand",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d5f1d43f-054e-4c95-ad80-a2672373900c", "AQAAAAIAAYagAAAAEBo4OEM+z4FxubnEKD3sxmf1tSfEi7go9/9hdpNN1RHzrdpinObjkS8MFXe+GLCPDw==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "brand" },
                values: new object[] { new DateTime(2024, 11, 25, 22, 46, 0, 509, DateTimeKind.Local).AddTicks(6691), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "brand",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "dce74c6e-9ad4-487b-aa2e-173641b88ec4", "AQAAAAIAAYagAAAAEKieS4Y1E6GCJCirfFWIqoizHuJJrSnon+Wk+dmzvuxYx6feJDzKKG6NfO2HWhk/Eg==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2024, 8, 27, 22, 45, 38, 181, DateTimeKind.Local).AddTicks(7474));
        }
    }
}
