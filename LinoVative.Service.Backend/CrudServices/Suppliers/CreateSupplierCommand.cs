using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Suppliers;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Suppliers;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Suppliers
{
    public class CreateSupplierCommand : SupplierInputDto, IRequest<Result>
    {
    }

    public class CreateSupplierService : SaveNewServiceBase<Supplier, CreateSupplierCommand>
    {

        public CreateSupplierService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) :
            base(dbContext, actor, mapper, appCache, localizer)
        {

        }


        public override async Task BeforeSave(CreateSupplierCommand request, Supplier entity, CancellationToken token)
        {
            foreach (var dto in request.Addresses)
            {
                var newAddress = _mapper.Map<SupplierAddress>(dto);
                newAddress.SupplierId = dto.SupplierId; 
                _dbContext.SupplierAddress.Add(newAddress);
            }

            foreach(var dto in request.Contacts)
            {
                var newContact = _mapper.Map<SupplierContact>(dto);
                newContact.SupplierId = dto.SupplierId;
                _dbContext.SupplierContact.Add(newContact);
            }

            await Task.CompletedTask;
        }

    }
}
