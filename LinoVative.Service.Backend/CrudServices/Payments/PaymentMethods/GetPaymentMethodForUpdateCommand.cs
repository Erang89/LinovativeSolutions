using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Payments;
using LinoVative.Shared.Dto.Outlets;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Payments.PaymentMethods
{
    public class GetPaymentMethodForUpdateCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
    }

    public class GetPaymentMethodForUpdateCommandHandlerService :  IRequestHandler<GetPaymentMethodForUpdateCommand, Result>
    {
        private readonly IAppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IActor _actor;

        public GetPaymentMethodForUpdateCommandHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _actor = actor;
        }


        public async Task<Result> Handle(GetPaymentMethodForUpdateCommand request, CancellationToken ct)
        {
            var paymentMethod = await _dbContext.PaymentMethods.GetAll(_actor)
                .ProjectToType<PaymentMethodUpdateDto>(_mapper.Config)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            
            if (paymentMethod == null) return Result.Failed($"No Data with ID: {request.Id}");

            paymentMethod.OutletPaymentMethods = await
                _dbContext.OutletPaymentMethods.GetAll(_actor)
                .Where(x => x.PaymentMethodId == request.Id)
                .ProjectToType<OutletPaymentMethodDto>(_mapper.Config)
                .ToListAsync();
           
            return Result.OK(paymentMethod);
        }

    }
}
