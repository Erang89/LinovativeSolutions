using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Payments;
using LinoVative.Shared.Dto.MasterData.Payments;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Payments.PaymentMethods
{
    public class GetAllPaymentMethodIQueryableCommand : IRequest<IQueryable<PaymentMethodViewDto>>
    {
        public string? SearchKeyword { get; set; }
    }

    public class GetAllPaymentMethodQueryableHandlerService : QueryServiceBase<PaymentMethod, GetAllPaymentMethodIQueryableCommand>, IRequestHandler<GetAllPaymentMethodIQueryableCommand, IQueryable<PaymentMethodViewDto>>
    {
        public GetAllPaymentMethodQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<PaymentMethod> OnGetAllFilter(IQueryable<PaymentMethod> query, GetAllPaymentMethodIQueryableCommand req)
        {
            return base.OnGetAllFilter(query, req).Where(x => x.Name.Contains(req.SearchKeyword??""));
        }

        public Task<IQueryable<PaymentMethodViewDto>> Handle(GetAllPaymentMethodIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).AsNoTracking().ProjectToType<PaymentMethodViewDto>(_mapper.Config));

    }
}
