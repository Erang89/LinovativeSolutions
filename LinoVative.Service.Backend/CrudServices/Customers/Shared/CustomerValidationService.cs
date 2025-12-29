using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Customers;

namespace LinoVative.Service.Backend.CrudServices.Customers.Shared
{
    public interface ICustomerValidationService
    {
        public Task<Result> Validate(CustomerInputDto request);
    }


    public class CustomerValidationService : ICustomerValidationService
    {
        private readonly IAppDbContext _dbContext;
        private readonly IActor _actor;
        private readonly ILangueageService _lang;
        private const string ResourceKey = nameof(CustomerInputDto);

        public CustomerValidationService(IAppDbContext dbContext, IActor actor, ILangueageService lang)
        {
            _dbContext  = dbContext;
            _actor = actor;
            _lang = lang;
            _lang.EnsureLoad(nameof(CustomerInputDto));
        }


        public async Task<Result> Validate(CustomerInputDto request)
        {
            var countryId = _dbContext.Companies.Where(x => x.Id == _actor.CompanyId!.Value).Select(x => x.CountryId).FirstOrDefault();
            var result = Result.OK();
            var index = 0;

            foreach(var dto in request.Address)
            {
                if(countryId == dto.CountryId && dto.ProvinceId is null)
                {
                    result.AddInvalidProperty($"{nameof(request.Address)}[{index}].{nameof(CustomerAddressDto.ProvinceId)}", _lang[$"{ResourceKey}.Required.ProvinceId.Message"]);
                }

                index++;    
            }

            return result;
        }
    }
}
