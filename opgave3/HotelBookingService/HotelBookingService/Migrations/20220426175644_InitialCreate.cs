using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Consumer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    orderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    hotelId = table.Column<int>(type: "int", nullable: false),
                    checkIn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    checkOut = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    roomNo = table.Column<int>(type: "int", nullable: false),
                    customerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    customerEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    customerAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.orderId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");
        }
    }
}
