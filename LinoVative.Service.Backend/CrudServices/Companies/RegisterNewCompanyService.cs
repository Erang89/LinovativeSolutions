using LinoVative.Service.Backend.AuthServices;
using LinoVative.Service.Backend.Helpers;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Auth;
using LinoVative.Service.Core.Companies;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Companies
{

    public class RegisterNewCompanyServiceCommand : RegisterNewCompanyDto, IRequest<Result>
    {

    }


    public class RegisterNewCompanyService : SaveNewServiceBase<Company, RegisterNewCompanyServiceCommand>, IRequestHandler<RegisterNewCompanyServiceCommand, Result>, IScoopService
    {
        private readonly IHttpContextAccessor _httpContext;
        public RegisterNewCompanyService(IAppDbContext dbContext, IMapper mapper, IAppCache appCache, IStringLocalizer localizer, IHttpContextAccessor httpContextAccessor) 
            : base(dbContext, new SystemAdministrator(), mapper, appCache, localizer)
        {
            _httpContext = httpContextAccessor;
        }


        public Task<Result> Handle(RegisterNewCompanyServiceCommand req, CancellationToken token) => base.Handle(req, token);

        

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

            var timezone = _dbContext.TimeZones.FirstOrDefault(x => x.Id == request.TimeZoneId);
            
            var newCompany = new Company()
            {
                Id = request.Id,
                Name = request.Name,
                Address = request.Address,
                CountryId = request.CountryId,
                TimeZoneId = request.TimeZoneId,
                CurrencyId = request.CurrencyId,
                OwnByUserId = newUser.Id
            };

            var actor = new ActorService(_httpContext) { UserId = newUserId, CompanyId = newCompany.Id };
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
            var (isPassStrong, passErrors) = PasswordHelper.IsPasswordStrong(request.Password!, _localizer);
            if (!isPassStrong)
                AddError(result, x => x.Password!, passErrors);

            if (!EmailHelper.IsValidEmailAddress(request.EmailAddress ?? ""))
                AddError(result, x => x.EmailAddress!, _localizer["Property.InvalidEmailFormat"]);

            if (_dbContext.Users.Where(x => x.EmailAddress == request.EmailAddress).Any())
                AddError(result, x => x.EmailAddress!, _localizer["User.EmailAlreadyExist"]);

        }

    }
}
