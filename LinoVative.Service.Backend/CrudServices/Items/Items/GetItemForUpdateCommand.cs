using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.Commons;
using LinoVative.Shared.Dto.ItemDtos;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Items.Items
{
    public class GetItemForUpdateCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
    }

    public class GetItemForUpdateCommandHandlerService :  IRequestHandler<GetItemForUpdateCommand, Result>
    {
        private readonly IAppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IActor _actor;

        public GetItemForUpdateCommandHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _actor = actor;
        }


        public async Task<Result> Handle(GetItemForUpdateCommand request, CancellationToken ct)
        {
            var item = await _dbContext.Items.GetAll(_actor)
                .ProjectToType<ItemInputDto>(_mapper.Config)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            
            if (item == null) return Result.Failed($"No Data with ID: {request.Id}");

            item.SKUItems = await _dbContext.SKUItems.GetAll(_actor).Where(x => x.ItemId == item.Id)
                .Include(x => x.Unit)
                .ProjectToType<SKUItemInputDto>(_mapper.Config).ToListAsync();

            var skuIds = item.SKUItems.Select(x => x.Id).ToList();
            var costumePrices = await _dbContext.ItemPriceTypes.Where(x => skuIds.Contains(x.Id)).ProjectToType<ItemPriceTypeDto>(_mapper.Config).ToListAsync();

            var unitIds = item.SKUItems.Select(x => x.UnitId).ToList();
            var units = await _dbContext.ItemUnits.Where(x => unitIds.Contains(x.Id)).ProjectToType<ItemUnitDto>(_mapper.Config).ToListAsync();

            item.SKUItems = item.SKUItems.Select(x =>
            {
                x.CostumePrices = costumePrices.Where(x => x.SKUItemId == x.Id).ToList();
                x.Unit = units.FirstOrDefault(u => u.Id == x.UnitId!.Value);
                return x;

            }).ToList();

            return Result.OK(item);
        }

    }
}
