using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.ItemDtos;
using LinoVative.Shared.Dto.Outlets;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Items.ItemGroups
{
    public class GetItemGroupForUpdateCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
    }

    public class GetItemGroupForUpdateCommandHandlerService :  IRequestHandler<GetItemGroupForUpdateCommand, Result>
    {
        private readonly IAppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IActor _actor;

        public GetItemGroupForUpdateCommandHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _actor = actor;
        }


        public async Task<Result> Handle(GetItemGroupForUpdateCommand request, CancellationToken ct)
        {
            var itemGroup = await _dbContext.ItemGroups.GetAll(_actor)
                .ProjectToType<ItemGroupInputDto>(_mapper.Config)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            
            if (itemGroup == null) return Result.Failed($"No Data with ID: {request.Id}");

            itemGroup.OutletGroups = await
                _dbContext.OutletItemGroups.GetAll(_actor)
                .Where(x => x.ItemGroupId == request.Id)
                .ProjectToType<OutletItemGroupDto>(_mapper.Config)
                .ToListAsync();
           
            return Result.OK(itemGroup);
        }

    }
}
