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

            //item.Category = await _dbContext.ItemCategories.GetAll(_actor).Where(x => x.Id == item.CategoryId).ProjectToType<ItemCategoryViewDto>(_mapper.Config).FirstOrDefaultAsync();
            //item.Unit = await _dbContext.ItemUnits.GetAll(_actor).Where(x => x.Id == item.UnitId).ProjectToType<IdWithNameDto>(_mapper.Config).FirstOrDefaultAsync();
            //item.ItemPriceTypes = await _dbContext.ItemPriceTypes.GetAll(_actor).Where(x => x.ItemId == request.Id).ProjectToType<ItemPriceTypeDto>(_mapper.Config).ToListAsync();

            return Result.OK(item);
        }

    }
}
