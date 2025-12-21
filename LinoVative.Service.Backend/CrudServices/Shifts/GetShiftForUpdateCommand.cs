using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Shifts;
using LinoVative.Shared.Dto.Outlets;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Shifts
{
    public class GetShiftForUpdateCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
    }

    public class GetShiftForUpdateCommandHandlerService :  IRequestHandler<GetShiftForUpdateCommand, Result>
    {
        private readonly IAppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IActor _actor;

        public GetShiftForUpdateCommandHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _actor = actor;
        }


        public async Task<Result> Handle(GetShiftForUpdateCommand request, CancellationToken ct)
        {
            var shift = await _dbContext.Shifts.GetAll(_actor).ProjectToType<ShiftUpdateDto>(_mapper.Config).FirstOrDefaultAsync(x => x.Id == request.Id);
            if (shift == null) return Result.Failed($"No Data with ID: {request.Id}");

            shift.Outlets = await _dbContext.OutletShifts
                .Where(x => x.ShiftId == shift.Id).ProjectToType<OutletShiftDto>(_mapper.Config)
                .ToListAsync();

            return Result.OK(shift);
        }

    }
}
