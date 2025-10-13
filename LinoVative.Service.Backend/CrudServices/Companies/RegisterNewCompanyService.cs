using LinoVative.Service.Backend.AuthServices;
using LinoVative.Service.Backend.Helpers;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Auth;
using LinoVative.Service.Core.Commons;
using LinoVative.Service.Core.Companies;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Companies
{

    public class RegisterNewCompanyServiceCommand : RegisterNewCompanyDto, IRequest<Result>
    {

    }


    public class RegisterNewCompanyService : SaveNewServiceBase<Company, RegisterNewCompanyServiceCommand>, IRequestHandler<RegisterNewCompanyServiceCommand, Result>, IScoopService
    {
        private readonly IStringLocalizer _lang;
        public RegisterNewCompanyService(IAppDbContext dbContext, ISystemAdministrator actor, IMapper mapper, IAppCache appCache, IActionContextAccessor actionContext, IStringLocalizer localizer) : base(dbContext, actor, mapper, appCache, actionContext)
        {
            _lang = localizer;
        }


        public Task<Result> Handle(RegisterNewCompanyServiceCommand req, CancellationToken token) => base.SaveNew(req, token);


        protected override async Task<List<Company>> OnCreatingEntity(RegisterNewCompanyServiceCommand request, CancellationToken token = default)
        {
            var newUserId = Guid.NewGuid();
            var newUser = new AppUser()
            {
                Id = newUserId,
                NikName = request.NickName,
                EmailAddress = request.EmailAddress!,
                Password = PasswordHelper.CreateHashedPassword(request.Password!, newUserId),
                ForceChangePasswordOnLogin = false,
                IsActive = true,
                HasConfirmed = false,
            };

            
            var newCompany = new Company()
            {
                Id = request.Id,
                Name = request.Name,
                CountryId = request.CountryId,
                TimeZone = request.TimeZone,
                OwnByUserId = newUser.Id
            };

            var actor = new ActorService() { UserId = newUserId, ClientId = newCompany.Id };
            newUser.CreateBy(actor);
            newCompany.CreateBy(actor);

            _dbContext.Set<Company>().Add(newCompany);
            _dbContext.Set<AppUser>().Add(newUser);

            await Task.CompletedTask;
            return new() { newCompany };
        }


        protected override async Task<Result> Validate(RegisterNewCompanyServiceCommand request, CancellationToken token)
        {
            var isValid = await base.Validate(request, token);
            
            if (!_dbContext.Countries.Any(x => x.Id == request.CountryId))
                isValid.AddInvalidProperty(nameof(request.CountryId), _lang[Message.Entity.NotFound, _lang[Message.EntityName.Country], request.CountryId!]);

            if (!_dbContext.Currencies.Any(x => x.Id == request.CurrencyId))
                isValid.AddInvalidProperty(nameof(request.CurrencyId), _lang[Message.Entity.NotFound, _lang[Message.EntityName.Currency], request.CurrencyId!]);

            if (PasswordHelper.IsPasswordStrong(request.Password!))
                isValid.AddInvalidProperty(nameof(request.Password), _lang[Message.Password.NotStrong]);

            if (_dbContext.Users.Where(x => x.EmailAddress == request.EmailAddress).Any())
                isValid.AddInvalidProperty(nameof(request.EmailAddress), _lang[Message.User.EmailAlreadyExist, request.EmailAddress!]);

            if (!_dbContext.TimeZones.Any(x => !x.IsDeleted && x.TimeZone == request.TimeZone))
                isValid.AddInvalidProperty(nameof(request.TimeZone), _lang[Message.Entity.NotFound, _lang[Message.EntityName.TimeZone], request.TimeZone!]);

            return isValid;
        }

    }
}
