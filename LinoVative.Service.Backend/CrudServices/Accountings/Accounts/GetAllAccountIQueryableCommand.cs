using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Accountings;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto.MasterData.Accountings;
using Mapster;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.Accounts
{
    public class GetAllAccountIQueryableCommand : IRequest<IQueryable<AccountViewDto>>
    {
        public string? SearchKeyword { get; set; }
    }

    public class GetAllAccountQueryableHandlerService : QueryServiceBase<Account, GetAllAccountIQueryableCommand>, IRequestHandler<GetAllAccountIQueryableCommand, IQueryable<AccountViewDto>>
    {
        public GetAllAccountQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<Account> OnGetAllFilter(IQueryable<Account> query, GetAllAccountIQueryableCommand req)
        {
            return base.OnGetAllFilter(query, req).Where(x => x.Name!.Contains(req.SearchKeyword??""));
        }

        public Task<IQueryable<AccountViewDto>> Handle(GetAllAccountIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).ProjectToType<AccountViewDto>(_mapper.Config));

    }
}
