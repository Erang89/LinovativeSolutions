using LinoVative.Service.Backend.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Suppliers;

namespace LinoVative.Service.Backend.CrudServices.Suppliers.Shared
{
    public interface ISupplierValidatorService
    {
        public Task<Result> Validate(SupplierInputDto supplier);
    }

    public class SupplierValidatorService : ISupplierValidatorService
    {
        private readonly ILangueageService _lang;
        const string ResourceName = "SupplierInputCommand";

        public SupplierValidatorService(ILangueageService lang)
        {
            _lang = lang;
            _lang.EnsureLoad(ResourceName);
        }


        public async Task<Result> Validate(SupplierInputDto supplier)
        {
            var result = Result.OK();

            // Validate Primary Contact
            if(supplier.Contacts.Count > 0 && supplier.Contacts.Count(x => x.IsPrimary) != 1)
            {
                result.AddInvalidProperty($"{nameof(supplier.Contacts)}[all].{nameof(SupplierContactDto.IsPrimary)}", _lang[$"{ResourceName}.NoPrimaryContact.Message"]);
            }

            // Contact grouping
            var contacts = supplier.Contacts.GroupBy(x => x.ContactName.ToLower()).Select(x => new { Name = x.Key, contacts = x}).ToList();
            foreach (var contact in contacts.Where(x => x.contacts.Count() > 1).SelectMany(x => x.contacts))
            {
                result.AddInvalidProperty($"{nameof(supplier.Contacts)}", _lang[$"{ResourceName}.ContactNameDuplicated.Message"]);
            }

            return result;
        }
    }
}
