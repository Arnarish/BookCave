using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BookCave.Migrations
{
    public partial class initialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Author = table.Column<string>(nullable: true),
                    Discount = table.Column<int>(nullable: false),
                    Genre = table.Column<string>(nullable: true),
                    ISBN = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    OnSale = table.Column<bool>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    ReleaseYear = table.Column<int>(nullable: false),
                    Stock = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    TopSeller = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
