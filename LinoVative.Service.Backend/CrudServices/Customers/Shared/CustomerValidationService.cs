using Linovative.Shared.Interface;
using LinoVative.Service.Backend.CrudServices.Companies.Shared;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Customers;
using Microsoft.EntityFrameworkCore;

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
        private readonly ICompanyHelperService _companyHelper;

        public CustomerValidationService(IAppDbContext dbContext, IActor actor, ILangueageService lang, ICompanyHelperService helper)
        {
            _dbContext  = dbContext;
            _actor = actor;
            _lang = lang;
            _lang.EnsureLoad(nameof(CustomerInputDto));
            _companyHelper = helper;
        }


        public async Task<Result> Validate(CustomerInputDto request)
        {
            var countryId = await _dbContext.Companies.Where(x => x.Id == _actor.CompanyId!.Value).Select(x => x.CountryId).FirstOrDefaultAsync();
            var result = Result.OK();
            var index = 0;
            var settings = await _companyHelper.GetCurrenctCompanySettings();
            var provinceRequired = settings?.RequiredProvinceForCustomerAddress ?? false;
            var cityRequired = settings?.RequiredCityForCustomerAddress ?? false;
            var postalCodeRequired = settings?.RequiredPostalCodeForCustomerAddress ?? false;
            var regencyRequired = settings?.RequiredRegencyForCustomerAddress ?? false;
            var regencyIds = request.Address.Where(x => x.RegencyId is not null).Select(x => x.RegencyId).ToList();
            var regencies = await _dbContext.Regencies.Where(x => regencyIds.Contains(x.Id)).Select(x => new { x.Id, x.ProvinceId }).ToListAsync();

            foreach (var dto in request.Address)
            {

                if(dto.RegencyId is not null)
                {
                    var regency = regencies.FirstOrDefault(x => x.Id == dto.RegencyId);
                    if (regency!.ProvinceId != dto.ProvinceId)
                        return Result.Failed("Regency and Province doesn't match");
                }


                if (countryId == dto.CountryId && provinceRequired && dto.ProvinceId is null)
                {
                    result.AddInvalidProperty($"{nameof(request.Address)}[{index}].{nameof(CustomerAddressDto.ProvinceId)}", _lang[$"{ResourceKey}.Required.ProvinceId.Message"]);
                }

                if(countryId == dto.CountryId && regencyRequired && dto.RegencyId is null)
                {
                    result.AddInvalidProperty($"{nameof(request.Address)}[{index}].{nameof(CustomerAddressDto.ProvinceId)}", _lang[$"{ResourceKey}.Required.RegencyId.Message"]);
                }

                if(countryId != dto.CountryId && provinceRequired &&  dto.ProvinceName is null)
                {
                    result.AddInvalidProperty($"{nameof(request.Address)}[{index}].{nameof(CustomerAddressDto.ProvinceName)}", _lang[$"{ResourceKey}.Required.ProvinceName.Message"]);
                }

                if (cityRequired && dto.City is null)
                {
                    result.AddInvalidProperty($"{nameof(request.Address)}[{index}].{nameof(CustomerAddressDto.City)}", _lang[$"{ResourceKey}.Required.City.Message"]);
                }

                if(postalCodeRequired && dto.PostalCode is null)
                {
                    result.AddInvalidProperty($"{nameof(request.Address)}[{index}].{nameof(CustomerAddressDto.PostalCode)}", _lang[$"{ResourceKey}.Required.PostalCode.Message"]);
                }

                index++;    
            }

            return result;
        }
    }
}
