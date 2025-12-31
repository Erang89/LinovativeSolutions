using Linovative.BackendTest.Models;
using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Configurations;
using LinoVative.Service.Backend.CrudServices;
using LinoVative.Service.Backend.DatabaseService;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Backend.LocalizerServices;
using LinoVative.Service.Core.Auth;
using LinoVative.Service.Core.Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Linovative.BackendTest.Bases
{
    public class UseDatabaseTestBase : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<AppDbContext> _options;
        protected IActor _actor;
        protected IMapper _mapper;
        protected IAppCache _appCache = new AppCacheService();
        protected IStringLocalizer _localizer;
        protected ILangueageService _langService;


        protected Guid _testCompanyId = new Guid("f7c1b0f3-9e2e-4c89-91ef-bc387cb6349a");
        protected Guid _testUserId = new Guid("7e5a55df-2f6b-4f0f-8c9c-0fb94b3f4a93");

        public UseDatabaseTestBase()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            using (var ctx = new AppDbContext(_options))
            {
                ctx.Database.EnsureCreated();
                _actor = CreateActor(ctx);
            };
           
            _mapper = CreateMapper();
            _localizer = CreateStringLocalizer();
            _langService = CreateLanguaService();
        }

        IActor CreateActor(AppDbContext ctx)
        {
            var user = new AppUser()
            {
                Id = _testUserId,
                NikName = "Test",
                EmailAddress = "admin@test.com",
                IsActive = true,
                HasConfirmed = true,
            };
            ctx.Users.Add(user);
            ctx.SaveChanges();

            return new TestActor()
            {
                CompanyId = _testCompanyId,
                UserId = user.Id,
                EmailAddress = user.EmailAddress
            };
        }

        IMapper CreateMapper()
        {
            var config = new TypeAdapterConfig();
            new MapsterConfigs().Register(config);
           return new Mapper(config);
        }

        IStringLocalizer CreateStringLocalizer()
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "Resources");
            return new JsonStringLocalizer(filePath, "validation");
        }

        ILangueageService CreateLanguaService()
        {
            var filePathValidation = Path.Combine(AppContext.BaseDirectory, "Resources", "en");
            _langService = new LangueageService();

            foreach (var jsonFile in Directory.GetFiles(filePathValidation, "*.json", SearchOption.TopDirectoryOnly))
            {
                var fileName = Path.GetFileNameWithoutExtension(jsonFile);
                _langService.EnsureLoad(fileName);
            }

            return _langService;
        }

        protected IAppDbContext CreateContext()
        {
            return new AppDbContext(_options);
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
