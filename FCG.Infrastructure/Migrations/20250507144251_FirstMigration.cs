using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FCG.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(100)", nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(150)", nullable: false),
                    Password = table.Column<string>(type: "VARCHAR(88)", nullable: false),
                    Role = table.Column<int>(type: "INT", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "DATETIME2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreateDate", "Email", "Name", "Password", "Role" },
                values: new object[,]
                {
                    { new Guid("9f40a332-1f19-4d21-98f5-f354d692f06f"), new DateTime(2025, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "usr@fcg.com", "Usuario Comum", "FEYtYdA4Pp8nOH6mFGLwFxOX2XGBGmUQW9n+Ot8BHzA=", 0 },
                    { new Guid("c412957f-569c-42c1-8223-e648facf210b"), new DateTime(2025, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "adm@fcg.com", "Usuario Adm", "H9i6kZgxyPZ0F4t0a9XxUt8zRMNCwdzktYlJv8EYaWg=", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
