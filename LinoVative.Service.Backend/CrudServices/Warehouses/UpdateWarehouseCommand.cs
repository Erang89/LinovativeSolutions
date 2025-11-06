using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Warehoses;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Warehouses;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Warehouses
{
    public class UpdateWarehouseCommand : WarehouseDto, IRequest<Result>
    {
    }

    public class UpdateWarehouseHandlerService : SaveUpdateServiceBase<Warehouse, UpdateWarehouseCommand>, IRequestHandler<UpdateWarehouseCommand, Result>
    {
        public UpdateWarehouseHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }


        protected override async Task<Result> ValidateSaveUpdate(UpdateWarehouseCommand request, CancellationToken token)
        {
            var result = await base.ValidateSaveUpdate(request, token);
            
            // Your validations are here

            return result;
        }
    }
}
