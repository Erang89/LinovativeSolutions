using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Payments;
using LinoVative.Shared.Dto.MasterData.Payments;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Payments.PaymentMethodGroups
{
    public class GetAllPaymentMethodGroupIQueryableCommand : IRequest<IQueryable<PaymentMethodGroupViewDto>>
    {
        public string? SearchKeyword { get; set; }
    }

    public class GetAllPaymentMethodGroupQueryableHandlerService : QueryServiceBase<PaymentMethodGroup, GetAllPaymentMethodGroupIQueryableCommand>, IRequestHandler<GetAllPaymentMethodGroupIQueryableCommand, IQueryable<PaymentMethodGroupViewDto>>
    {
        public GetAllPaymentMethodGroupQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<PaymentMethodGroup> OnGetAllFilter(IQueryable<PaymentMethodGroup> query, GetAllPaymentMethodGroupIQueryableCommand req)
        {
            return base.OnGetAllFilter(query, req).Where(x => x.Name.Contains(req.SearchKeyword??""));
        }

        public Task<IQueryable<PaymentMethodGroupViewDto>> Handle(GetAllPaymentMethodGroupIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).AsNoTracking().ProjectToType<PaymentMethodGroupViewDto>(_mapper.Config));

    }
}
