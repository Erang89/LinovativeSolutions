using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.Commons;
using LinoVative.Shared.Dto.ItemDtos;
using LinoVative.Shared.Dto.Outlets;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Items.ItemCategories
{
    public class GetItemCategoryForUpdateCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
    }

    public class GetItemCategoryForUpdateCommandHandlerService :  IRequestHandler<GetItemCategoryForUpdateCommand, Result>
    {
        private readonly IAppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IActor _actor;

        public GetItemCategoryForUpdateCommandHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _actor = actor;
        }


        public async Task<Result> Handle(GetItemCategoryForUpdateCommand request, CancellationToken ct)
        {
            var itemCategory = await _dbContext.ItemCategories.GetAll(_actor)
                .Where(x => x.Id == request.Id)
                .ProjectToType<ItemCategoryInputDto>(_mapper.Config)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            
            if (itemCategory == null) return Result.Failed($"No Data with ID: {request.Id}");

            itemCategory.ItemGroup = await
                _dbContext.ItemGroups
                .Where(x => x.Id == itemCategory.GroupId)
                .Select(x => new IdWithNameDto { Id = x.Id, Name = x.Name })
                .FirstOrDefaultAsync();

            itemCategory.OutletCategories = await
                _dbContext.OutletItemCategories.GetAll(_actor)
                .Where(x => x.ItemCategoryId == request.Id)
                .ProjectToType<OutletItemCategoryDto>(_mapper.Config)
                .ToListAsync();
           
            return Result.OK(itemCategory);
        }

    }
}
