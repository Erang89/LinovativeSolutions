using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Service.Core.Outlets;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.ItemDtos;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Items.ItemGroups
{
    public class CreateItemGroupCommand : ItemGroupInputDto, IRequest<Result>
    {

    }

    public class CreateItemGroupHandlerService : SaveNewServiceBase<ItemGroup, CreateItemGroupCommand>
    {
        public CreateItemGroupHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : base(dbContext, actor, mapper, appCache, localizer)
        {
        }


        public override async Task BeforeSave(CreateItemGroupCommand request, ItemGroup entity, CancellationToken token)
        {
            var maxSequence = await _dbContext.OutletItemGroups
                .GetAll(_actor)
                .Where(x => x.Outlet!.CompanyId == _actor.CompanyId)
                .GroupBy(x => x.OutletId).Select(x => new { Id = x.Key, Max = x.Max(s => s.Sequence) }).ToListAsync();

            foreach (var os in request.OutletGroups)
            {
                var group = _mapper.Map<OutletItemGroup>(os);
                group.ItemGroupId = entity.Id;
                group.CreateBy(_actor);
                group.Sequence = (maxSequence.FirstOrDefault(x => x.Id == os.OutletId)?.Max ?? 0) + 1;
                _dbContext.OutletItemGroups.Add(group);
            }
        }


        protected override async Task<Result> Validate(CreateItemGroupCommand request, CancellationToken token)
        {
            var result = await base.Validate(request, token);

            var isNameExist = await GetAll().Where(x => x.Name!.Contains(request.Name!)).AnyAsync();
            if (isNameExist) AddError(result, x => x.Name!, _localizer["Property.AreadyExist", request.Name!]);


            var anyDuplicateOutlet = request.OutletGroups.GroupBy(x => x.OutletId).Any(x => x.Count() > 1);
            if (anyDuplicateOutlet)
                return Result.Failed("Duplicate Outlet ID is not allowed");

            var outletIds = request.OutletGroups.Select(x => x.OutletId).Distinct();
            var outletIdsCount = await _dbContext.Outlets.CountAsync(x => outletIds.Contains(x.Id));
            if (outletIds.Count() != outletIdsCount)
                return Result.Failed("Some outlet ID are not in the system");

            return result;
        }
    }
}
