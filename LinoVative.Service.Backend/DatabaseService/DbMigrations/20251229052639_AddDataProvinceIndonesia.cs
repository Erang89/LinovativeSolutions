using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class AddDataProvinceIndonesia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"
                Insert into Provinces (Id, Name, Code) Values('31B046F8-82E9-44CD-8EC0-F26155C54B6B', 'ACEH', '11');
                Insert into Provinces (Id, Name, Code) Values('9AE79938-A93A-471A-B4F4-BA3E5BE303FB', 'SUMATERA UTARA', '12');
                Insert into Provinces (Id, Name, Code) Values('578B2D6D-1E50-4571-9A62-146EAC6A3A5F', 'SUMATERA BARAT', '13');
                Insert into Provinces (Id, Name, Code) Values('FEA56CA7-3534-49B0-922E-D82A1218B918', 'RIAU', '14');
                Insert into Provinces (Id, Name, Code) Values('4FF1ED37-EDD9-444B-86B2-50FEA6056625', 'JAMBI', '15');
                Insert into Provinces (Id, Name, Code) Values('FBD5C2BC-81F3-4A4B-B5C9-CA658661C2FD', 'SUMATERA SELATAN', '16');
                Insert into Provinces (Id, Name, Code) Values('6C8B9402-5131-45C1-9C8B-0A3A2CA78621', 'BENGKULU', '17');
                Insert into Provinces (Id, Name, Code) Values('25A893F3-1DDE-4E24-AAF1-FE7426DE8F35', 'LAMPUNG', '18');
                Insert into Provinces (Id, Name, Code) Values('75ACEF5F-D2BA-4CC0-AE55-C9DC3725446D', 'KEPULAUAN BANGKA BELITUNG', '19');
                Insert into Provinces (Id, Name, Code) Values('FD6E6904-3EBC-4169-BDCC-1C6AB4D38333', 'KEPULAUAN RIAU', '21');
                Insert into Provinces (Id, Name, Code) Values('EB0FAA38-D783-44EF-836A-FEAA36E95B42', 'DKI JAKARTA', '31');
                Insert into Provinces (Id, Name, Code) Values('C3B19936-15D2-4C0B-A84F-7A632F2099A8', 'JAWA BARAT', '32');
                Insert into Provinces (Id, Name, Code) Values('24C11D4E-84F9-4306-9673-5CAD9389E24C', 'JAWA TENGAH', '33');
                Insert into Provinces (Id, Name, Code) Values('18A44E49-AB65-42E1-ACDB-AD38E5C1DB11', 'DAERAH ISTIMEWA YOGYAKARTA', '34');
                Insert into Provinces (Id, Name, Code) Values('5C9B5142-D0F0-4DAE-BA56-768D4FE3D745', 'JAWA TIMUR', '35');
                Insert into Provinces (Id, Name, Code) Values('0A82D9B7-9D57-4848-8D6F-BE2BF09D9A2D', 'BANTEN', '36');
                Insert into Provinces (Id, Name, Code) Values('886D7221-79D5-499D-818A-7DE66B402662', 'BALI', '51');
                Insert into Provinces (Id, Name, Code) Values('97DD6935-010C-4811-BF13-216B63B5BC2A', 'NUSA TENGGARA BARAT', '52');
                Insert into Provinces (Id, Name, Code) Values('8B9CD606-6D83-4FE2-ACDD-043EB6B7B5F4', 'NUSA TENGGARA TIMUR', '53');
                Insert into Provinces (Id, Name, Code) Values('95D3CD6E-4379-4BBB-B02C-40137A194542', 'KALIMANTAN BARAT', '61');
                Insert into Provinces (Id, Name, Code) Values('5E20CD52-A4D0-48A0-9557-13A3D24AA4D6', 'KALIMANTAN TENGAH', '62');
                Insert into Provinces (Id, Name, Code) Values('33B0156E-7D39-44CA-B1C8-EB98561DBD29', 'KALIMANTAN SELATAN', '63');
                Insert into Provinces (Id, Name, Code) Values('DBA9BF3A-A6C5-46D8-99D2-88C83E822C98', 'KALIMANTAN TIMUR', '64');
                Insert into Provinces (Id, Name, Code) Values('A3F05529-23D1-44A2-8DF1-26A1BEABBFE4', 'KALIMANTAN UTARA', '65');
                Insert into Provinces (Id, Name, Code) Values('7E5A82DF-A0C4-43F5-AE0E-59101ED409AD', 'SULAWESI UTARA', '71');
                Insert into Provinces (Id, Name, Code) Values('96E60B2E-05EE-454A-B39B-CD3801D2D5E9', 'SULAWESI TENGAH', '72');
                Insert into Provinces (Id, Name, Code) Values('44048542-ABB4-4D55-A34F-AFF956E8671A', 'SULAWESI SELATAN', '73');
                Insert into Provinces (Id, Name, Code) Values('81251B03-E916-4BF2-A1E8-15BAA01E6682', 'SULAWESI TENGGARA', '74');
                Insert into Provinces (Id, Name, Code) Values('92A058CC-BCBF-42C8-A863-DB0355294B96', 'GORONTALO', '75');
                Insert into Provinces (Id, Name, Code) Values('94DD5BD3-4692-44F4-8ACF-74905D888EFC', 'SULAWESI BARAT', '76');
                Insert into Provinces (Id, Name, Code) Values('30AC76EA-44F5-4566-AE73-A25BA4DA484B', 'MALUKU', '81');
                Insert into Provinces (Id, Name, Code) Values('FA1413EA-2965-4A11-8B4F-9C2F0F58966C', 'MALUKU UTARA', '82');
                Insert into Provinces (Id, Name, Code) Values('E8724A3F-D2BD-4673-8B4A-DBF1EDBF33C2', 'PAPUA', '91');
                Insert into Provinces (Id, Name, Code) Values('D0C4FF68-3687-4E7E-93F8-828FCFA895AF', 'PAPUA BARAT', '92');
                Insert into Provinces (Id, Name, Code) Values('BD2CDE6A-CCC6-40E2-8ECC-A88C05A06E7F', 'PAPUA SELATAN', '93');
                Insert into Provinces (Id, Name, Code) Values('4E9884C1-63CE-4268-AA80-056F280B3CA2', 'PAPUA TENGAH', '94');
                Insert into Provinces (Id, Name, Code) Values('3BA9E72D-A6AD-4C05-AB28-9436567EF3EC', 'PAPUA PEGUNUNGAN', '95');
                Insert into Provinces (Id, Name, Code) Values('8B33BA54-2389-430B-8807-30C781FE77A5', 'PAPUA BARAT DAYA', '96');
                Update Provinces set CountryId = (select top 1 Id from Countries where Name = 'Indonesia');
                ";

            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Provinces");
        }
    }
}
