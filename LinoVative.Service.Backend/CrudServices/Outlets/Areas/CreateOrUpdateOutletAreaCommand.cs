using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Outlets;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.Outlets;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Outlets.Areas
{

    public class CreateOrUpdateOutletAreaCommand : IRequest<Result>
    {
        [IgnoreEntityIDValidation(new string[] { $"{nameof(OutletTableDto)}.{nameof(OutletTableDto.AreaId)}" })]
        public List<OutletAreaCreateDto> Areas { get; set; } = new();
    }

    public class CreateOutletAreaHandlerService : SaveNewServiceBase<OutletArea, CreateOrUpdateOutletAreaCommand>, IRequestHandler<CreateOrUpdateOutletAreaCommand, Result>
    {
        ILangueageService _lang;
        public CreateOutletAreaHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer, ILangueageService lang) : base(dbContext, actor, mapper, appCache, localizer)
        {
            lang.EnsureLoad(x => x.CreateOrUpdateOutletAreaCommand);
            _lang = lang;
        }


        public override async Task<Result> Handle(CreateOrUpdateOutletAreaCommand request, CancellationToken token)
        {
            var validate = await Validate(request, token);
            if(!validate) return validate;


            await _dbContext.SaveAsync(_actor);
            return Result.OK();
        }


        protected override async Task<List<OutletArea>> OnCreatingEntity(CreateOrUpdateOutletAreaCommand request, CancellationToken token = default)
        {
            await Task.CompletedTask;
            return MappingAreas(request);
        }

        protected override async Task<Result> Validate(CreateOrUpdateOutletAreaCommand request, CancellationToken token)
        {
            var areas = MappingAreas(request);
            var result = Result.OK();
            var duplicateAreaName = new List<string>();

            foreach (var area in request.Areas)
            {
                var index = request.Areas.IndexOf(area);
                if (!duplicateAreaName.Contains(area.Name!) && areas.Any(x => x.Name!.ToLower().Equals(area.Name!.ToLower()) && x.Id != area.Id))
                {
                    duplicateAreaName.Add(area.Name!);
                    result.AddInvalidProperty($"Areas[{index}].Name", _lang.Format($"{nameof(CreateOrUpdateOutletAreaCommand)}.AreaName.Duplicate", area.Name!));
                }
                
                var duplicateTableName = new List<string>();
                foreach(var table in area.Tables)
                {
                    var tableIndex = area.Tables.IndexOf(table);
                    if(!duplicateTableName.Contains(table.Name!) && area.Tables.Any(x => x.Name!.ToLower().Equals(table.Name!.ToLower()) && x.Id != table.Id))
                    {
                        duplicateTableName.Add(table.Name!);
                        result.AddInvalidProperty($"Areas[{index}].Tables[{tableIndex}].Name", _lang.Format($"{nameof(CreateOrUpdateOutletAreaCommand)}.Table.Duplicate", table.Name!));
                    }
                }
            }
            
            await Task.CompletedTask;
            return result;
        }

        List<OutletArea> MappingAreas(CreateOrUpdateOutletAreaCommand request)
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
        List<OutletArea> GetAreas(CreateOrUpdateOutletAreaCommand request)
        {
            if (_areas is not null)
                return _areas;

            _areas = GetAll().Include(x => x.Tables).ToList();
            return _areas;
        }

    }
}
