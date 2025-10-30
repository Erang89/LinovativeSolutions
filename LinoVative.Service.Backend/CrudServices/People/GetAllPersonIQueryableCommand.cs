using Linovative.Dto.MasterData.People;
using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.People;
using Mapster;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.People
{
    public class GetAllPersonIQueryableCommand : IRequest<IQueryable<PersonViewDto>>
    {
        public string? SearchKeyword { get; set; }
    }

    public class GetAllPersonQueryableHandlerService : QueryServiceBase<Person, GetAllPersonIQueryableCommand>, IRequestHandler<GetAllPersonIQueryableCommand, IQueryable<PersonViewDto>>
    {
        public GetAllPersonQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<Person> OnGetAllFilter(IQueryable<Person> query, GetAllPersonIQueryableCommand req)
        {
            return base.OnGetAllFilter(query, req).Where(x => string.Concat(x.Firstname, x.Lastname)!.Contains(req.SearchKeyword??""));
        }

        public Task<IQueryable<PersonViewDto>> Handle(GetAllPersonIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).ProjectToType<PersonViewDto>(_mapper.Config));

    }
}
