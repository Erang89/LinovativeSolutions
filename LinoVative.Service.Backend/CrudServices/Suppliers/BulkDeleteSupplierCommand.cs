using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Suppliers;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Suppliers
{
    public class BulkDeleteSupplierCommand : IRequest<Result>, IBulkDeleteDto
    {
        public List<Guid> Ids { get; set; } = new();
    }


    public class BulkDeleteSupplierService : SaveDeleteServiceBase<Supplier, BulkDeleteSupplierCommand>, IRequestHandler<BulkDeleteSupplierCommand, Result>
    {
        public BulkDeleteSupplierService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) :
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }
    }

}
