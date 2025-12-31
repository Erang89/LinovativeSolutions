using Linovative.Shared.Interface;
using LinoVative.Service.Backend.CrudServices.Companies.Shared;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Suppliers;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Customers;
using LinoVative.Shared.Dto.MasterData.Suppliers;
using Microsoft.AspNetCore.Mvc;
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

            return await ValidateContacts(request);
        }


        async Task<Result> ValidateContacts(CustomerInputDto request)
        {
            var result = Result.OK();

            if(request.Contacts.Count > 0 && request.Contacts.Count(x => x.IsPrimary) != 1)
            {
                result.AddInvalidProperty($"{nameof(request.Contacts)}[all].{nameof(SupplierContactDto.IsPrimary)}", _lang[$"{ResourceKey}.NoPrimaryContact.Message"]);
            }

            var index = 0;
            var flagContact = false;
            foreach(var dto in request.Contacts)
            {
                if(string.IsNullOrWhiteSpace(dto.Phone) && string.IsNullOrWhiteSpace(dto.Whatsapp) && string.IsNullOrWhiteSpace(dto.Email))
                {
                    if(!flagContact)
                    {
                        flagContact = true;
                        result.AddInvalidProperty($"{nameof(CustomerInputDto)}[{index}].{nameof(dto.Email)}", _lang[$"{ResourceKey}.EmailPhoneWA.Message"]);
                    }
                    result.AddInvalidProperty($"{nameof(request.Contacts)}[{index}].{nameof(dto.Phone)}", _lang[$"{ResourceKey}.Required.Phone.Message"]);
                    result.AddInvalidProperty($"{nameof(request.Contacts)}[{index}].{nameof(dto.Whatsapp)}", _lang[$"{ResourceKey}.Required.Whatsapp.Message"]);
                    result.AddInvalidProperty($"{nameof(request.Contacts)}[{index}].{nameof(dto.Email)}", _lang[$"{ResourceKey}.Required.Email.Message"]);                    
                }

                index++;
            }

            var groupingContacts = request.Contacts.GroupBy(x => x.ContactName.ToLower()).Select(x => new { Name = x.Key, Contacts = x }).Where(x => x.Contacts.Count() > 1).ToList();
            foreach(var contact in groupingContacts.SelectMany(x => x.Contacts))
            {
                var indexContact = request.Contacts.IndexOf(contact);
                result.AddInvalidProperty($"{nameof(request.Contacts)}[{indexContact}].{nameof(contact.ContactName)}", _lang[$"{ResourceKey}.ContactNameDuplicated.Message"]);
            }

            return result;
        }
    }
}
