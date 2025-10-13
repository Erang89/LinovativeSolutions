using LinoVative.Service.Backend.Helpers;
using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class SystemAdministratorUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var userId = new Guid("7d35452e-5aa9-432e-a091-743c6ce7aacf");
            var haspass = PasswordHelper.CreateHashedPassword("NotSecure!@25", userId);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "EmailAddress", "Password", "HasConfirmed", "CreatedAtUtcTime", "CreatedBy", "ForceChangePasswordOnLogin", "IsActive", "LastModifiedAtUtcTime", "LastModifiedBy", "NikName" },
                values: new object[,]
                {
                    { userId, "system@linovative.com", haspass, true, DateTime.UtcNow, userId, true, true, null, null, "System"},
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
            table: "Users",
            keyColumn: "Id",
            keyValues: new object[]
            {
                new Guid("7d35452e-5aa9-432e-a091-743c6ce7aacf")
            });
        }
    }
}
