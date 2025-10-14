using LinoVative.Service.Backend.AuthServices;
using LinoVative.Service.Backend.Helpers;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Backend.LocalizerServices;
using LinoVative.Service.Core.Auth;
using LinoVative.Service.Core.Commons;
using LinoVative.Service.Core.Companies;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Sources;
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
        private readonly ILanguageService _lang;
        public RegisterNewCompanyService(IAppDbContext dbContext, ISystemAdministrator actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer, ILanguageService lang) 
            : base(dbContext, actor, mapper, appCache, localizer)
        {
            _lang = lang;
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
            var result = (await base.Validate(request, token))!;
            if (!result) return result;

            ValidateData(request, result);
            if (!result) return result;

            // Validate your bussiness rules here
            // ...

            return result;

        }


        void ValidateData(RegisterNewCompanyServiceCommand request, Result result)
        {

            if (!_dbContext.Countries.Any(x => x.Id == request.CountryId))
                result.AddInvalidProperty(nameof(request.CountryId), _lang.EntityNotFound<Country>(request.CountryId));

            if (!_dbContext.Currencies.Any(x => x.Id == request.CurrencyId))
                result.AddInvalidProperty(nameof(request.CurrencyId), _lang.EntityNotFound<Currency>(request.CurrencyId));

            if (PasswordHelper.IsPasswordStrong(request.Password!))
                result.AddInvalidProperty(nameof(request.Password), _localizer["Password.NotStrong"]);

            if (EmailHelper.IsValidEmailAddress(request.EmailAddress ?? ""))
                result.AddInvalidProperty(nameof(request.EmailAddress), _localizer["Property.InvalidEmailFormat"]);

            if (_dbContext.Users.Where(x => x.EmailAddress == request.EmailAddress).Any())
                result.AddInvalidProperty(nameof(request.EmailAddress), _localizer["User.EmailAlreadyExist"]);

            if (!_dbContext.TimeZones.Any(x => !x.IsDeleted && x.TimeZone == request.TimeZone))
                result.AddInvalidProperty(nameof(request.TimeZone), _lang.EntityNotFound<AppTimeZone>(request.TimeZone));

        }

    }
}
