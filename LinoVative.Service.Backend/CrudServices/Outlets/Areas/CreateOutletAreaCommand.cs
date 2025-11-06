using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Outlets;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.Outlets;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Outlets.Areas
{

    public class CreateOutletAreaCommand : IRequest<Result>
    {
        public List<OutletAreaCreateDto> Areas { get; set; } = new();
    }

    public class CreateOutletAreaHandlerService : SaveNewServiceBase<OutletArea, CreateOutletAreaCommand>, IRequestHandler<CreateOutletAreaCommand, Result>
    {
        ILangueageService _lang;
        public CreateOutletAreaHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer, ILangueageService lang) : base(dbContext, actor, mapper, appCache, localizer)
        {
            lang.EnsureLoad(x => x.CreateOutletAreaCommand);
            _lang = lang;
        }


        protected override Task<List<OutletArea>> OnCreatingEntity(CreateOutletAreaCommand request, CancellationToken token = default)
        {
            return base.OnCreatingEntity(request, token);
        }

        protected override async Task<Result> Validate(CreateOutletAreaCommand request, CancellationToken token)
        {
            var areas = MappingAreas(request);
            var result = Result.OK();

            foreach (var area in request.Areas)
            {
                var index = request.Areas.IndexOf(area);
                if (areas.Any(x => x.Name!.ToLower().Equals(area.Name!.ToLower()) && x.Id != area.Id))
                    result.AddInvalidProperty($"Name_{index}", _lang.Format($"{nameof(CreateOutletAreaCommand)}.AreaName.Duplicate", area.Name!));

                foreach(var table in area.Tables)
                {
                    var tableIndex = area.Tables.IndexOf(table);
                    if(area.Tables.Any(x => x.Name!.ToLower().Equals(table.Name!.ToLower()) && x.Id != table.Id))
                        result.AddInvalidProperty($"Name_{index}.table_{tableIndex}", _lang.Format($"{nameof(CreateOutletAreaCommand)}.Table.Duplicate", table.Name!));
                }
            }
            
            await Task.CompletedTask;

            return Result.OK();
        }

        List<OutletArea> MappingAreas(CreateOutletAreaCommand request)
        {
            var areas = GetAreas(request);
            foreach(var areaDto in request.Areas)
            {
                var existArea = areas.FirstOrDefault(x => x.Id == areaDto.Id);
                if (existArea is null)
                {
                    var newArea = _mapper.Map<OutletArea>(areaDto);
                    _dbContext.OutletAreas.Add(newArea);
                    areas.Add(newArea);
                    MappingTable(newArea, areaDto.Tables);
                    continue;
                }
                
                existArea = _mapper.Map(areaDto, existArea);
                MappingTable(existArea, areaDto.Tables);
            }

            return areas;
        }

        void MappingTable(OutletArea area, List<OutletTableDto> tablesDto)
        {
            foreach(var tableDto in tablesDto)
            {
                var existingTable = area.Tables.FirstOrDefault(x => x.Id == tableDto.Id);
                if(existingTable is null)
                {
                    var newTable = _mapper!.Map<OutletTable>(tableDto);
                    newTable.AreaId = area.Id;
                    area.Tables.Add(newTable);
                    _dbContext.OutletTables.Add(newTable);
                    continue;
                }

                existingTable = _mapper.Map(tableDto, existingTable);
                existingTable.AreaId = area.Id;
            }
        }

        private List<OutletArea>? _areas;
        List<OutletArea> GetAreas(CreateOutletAreaCommand request)
        {
            if (_areas is not null)
                return _areas;

            _areas = GetAll().Include(x => x.Tables).ToList();
            return _areas;
        }

    }
}
