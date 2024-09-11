using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShopSolution.Data.Migrations
{
    /// <inheritdoc />
    public partial class edtFileLengthType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isDefault",
                table: "ProductImages",
                newName: "IsDefault");

            migrationBuilder.AlterColumn<long>(
                name: "FileSize",
                table: "ProductImages",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDefault",
                table: "ProductImages",
                newName: "isDefault");

            migrationBuilder.AlterColumn<int>(
                name: "FileSize",
                table: "ProductImages",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2c442ccf-9d80-41fd-87b4-dfd6aef2082b", "AQAAAAIAAYagAAAAEEUmLkK70DI33ImFZZLiqwebMfxz0sTGcn+skC9iqMxJTTbatORtc1kS3y6pxGkkXQ==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2024, 8, 22, 23, 34, 48, 889, DateTimeKind.Local).AddTicks(8659));
        }
    }
}
