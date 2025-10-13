using LinoVative.Service.Backend.AuthServices;
using LinoVative.Service.Backend.Helpers;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Auth;
using LinoVative.Service.Core.Commons;
using LinoVative.Service.Core.Companies;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Sources;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace LinoVative.Service.Backend.CrudServices.Companies
{

    public class RegisterNewCompanyServiceCommand : RegisterNewCompanyDto, IRequest<Result>
    {

    }


    public class RegisterNewCompanyService : SaveNewServiceBase<Company, RegisterNewCompanyServiceCommand>, IRequestHandler<RegisterNewCompanyServiceCommand, Result>, IScoopService
    {
        public RegisterNewCompanyService(IAppDbContext dbContext, ISystemAdministrator actor, IMapper mapper, IAppCache appCache, IActionContextAccessor actionContext) : base(dbContext, actor, mapper, appCache, actionContext)
        {

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
                Password = PasswordHelper.CreateHashedPassword(request.Password!, newUserId)
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
            
            if (!_dbContext.Set<Country>().Any(x => x.Id == request.CountryId))
                isValid.AddInvalidProperty(nameof(request.CountryId), $"Country with ID: {request.CountryId} not found");

            if (!_dbContext.Set<Currency>().Any(x => x.Id == request.CurrencyId))
                isValid.AddInvalidProperty(nameof(request.CurrencyId), $"Currency with ID: {request.CountryId} not found");

            if (PasswordHelper.IsPasswordStrong(request.Password!))
                isValid.AddInvalidProperty(nameof(request.Password), "Password is not strong enough");

            if (!_dbContext.Set<AppUser>().Where(x => x.EmailAddress == request.EmailAddress).Any())
                isValid.AddInvalidProperty(nameof(request.EmailAddress), $"An account with email address: {request.EmailAddress} is exist");

            if (!_dbContext.Set<AppTimeZone>().Any(x => !x.IsDeleted && x.TimeZone == request.TimeZone))
                isValid.AddInvalidProperty(nameof(request.TimeZone), "Invalid TimeZone");

            return isValid;
        }

    }
}
