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
    public class CreateSalesCOAMappingCommand : SalesCOAMappingDto, IRequest<Result>
    {
    }

    public class CreateSalesCOAMappingHandlerService : SaveNewServiceBase<SalesCOAMapping, CreateSalesCOAMappingCommand>, IRequestHandler<CreateSalesCOAMappingCommand, Result>
    {
        
        public CreateSalesCOAMappingHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : base(dbContext, actor, mapper, appCache, localizer)
        {
          
        }        

        public async Task<Result> Handle(CreateSalesCOAMappingCommand request, CancellationToken ct)
        {
            var mapping = GetAll().Where(x => x.AccountId == request.AccountId && x.OutletId == request.OutletId).FirstOrDefault();
            if (mapping is not null)
            {
                mapping.AutoPushWhenPOSClosing = request.AutoPushWhenPOSClosing;
            }                
            else
            {
                mapping = _mapper.Map<SalesCOAMapping>(request);
                mapping.CompanyId = _actor.CompanyId;
                _dbContext.SalesCOAMappings.Add(mapping);
            }

            await _dbContext.SaveAsync(_actor);
            return Result.OK(mapping.Id);
        }



        protected override async Task<Result> Validate(CreateSalesCOAMappingCommand request, CancellationToken token)
        {
            var result = await base.Validate(request, token);
            if (!result) return result;

            return result;
        }
    }
}
