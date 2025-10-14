using LinoVative.Service.Backend.AuthServices;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Helpers;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Backend.LocalizerServices;
using LinoVative.Service.Core.Auth;
using LinoVative.Service.Core.Companies;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Sources;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;

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
                AddError(result, x => x.CountryId!, _lang.EntityNotFound<Country>(request.CountryId));

            if (!_dbContext.Currencies.Any(x => x.Id == request.CurrencyId))
                AddError(result, x => x.CurrencyId!, _lang.EntityNotFound<Currency>(request.CurrencyId));

            var (isPassStrong, passErrors) = PasswordHelper.IsPasswordStrong(request.Password!, _localizer);
            if (!isPassStrong)
                AddError(result, x => x.Password!, passErrors);

            if (!EmailHelper.IsValidEmailAddress(request.EmailAddress ?? ""))
                AddError(result, x => x.EmailAddress!, _localizer["Property.InvalidEmailFormat"]);

            if (_dbContext.Users.Where(x => x.EmailAddress == request.EmailAddress).Any())
                AddError(result, x => x.EmailAddress!, _localizer["User.EmailAlreadyExist"]);

            if (!_dbContext.TimeZones.Any(x => !x.IsDeleted && x.TimeZone == request.TimeZone))
                AddError(result, x => x.TimeZone!, _lang.EntityNotFound<AppTimeZone>(request.TimeZone));

        }

    }
}
