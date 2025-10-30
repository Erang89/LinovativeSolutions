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
    public class CreateWarehouseCommand : WarehouseDto, IRequest<Result>
    {
    }

    public class CreateWarehouseHandlerService : SaveNewServiceBase<Warehouse, CreateWarehouseCommand>, IRequestHandler<CreateWarehouseCommand, Result>
    {
        
        public CreateWarehouseHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : base(dbContext, actor, mapper, appCache, localizer)
        {
          
        }

        public Task<Result> Handle(CreateWarehouseCommand request, CancellationToken ct) => base.SaveNew(request, ct);


        protected override async Task<Result> Validate(CreateWarehouseCommand request, CancellationToken token)
        {
            var result = await base.Validate(request, token);
            if (!result) return result;

            return result;
        }
    }
}
