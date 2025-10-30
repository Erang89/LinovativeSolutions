using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Accountings;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Accountings;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.SalesCOAMappings
{
    public class UpdateSalesCOAMappingCommand : SalesCOAMappingDto, IRequest<Result>
    {
    }

    public class UpdateSalesCOAMappingHandlerService : SaveUpdateServiceBase<SalesCOAMapping, UpdateSalesCOAMappingCommand>, IRequestHandler<UpdateSalesCOAMappingCommand, Result>
    {
        public UpdateSalesCOAMappingHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }

        public Task<Result> Handle(UpdateSalesCOAMappingCommand request, CancellationToken ct) => base.SaveUpdate(request, ct);

        protected override async Task<SalesCOAMapping> OnMapping(UpdateSalesCOAMappingCommand request)
        {
            var mapping = GetAll().Where(x => x.OutletId == request.OutletId && x.AccountId == request.AccountId).FirstOrDefault();
            if(mapping is not null && mapping.Id != request.Id)
            {
                var oldMapping = GetAll().Where(x => x.Id == request.Id).FirstOrDefault();
                if(oldMapping is not null) oldMapping.Delete(_actor);
            }

            await Task.CompletedTask;
            return mapping!;
        }

    }
}
