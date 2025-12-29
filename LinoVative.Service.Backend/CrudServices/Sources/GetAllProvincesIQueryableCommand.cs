using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Sources;
using LinoVative.Shared.Dto.Sources;
using Mapster;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.Sources
{
    public class GetAllProvincesIQueryableCommand : IRequest<IQueryable<ProvinceDto>>
    {
        public string? SearchKeyword { get; set; }
        public Guid? CountryId { get; set; }
    }

    public class GetAllProvincesIQueryaHandlerService : QueryServiceBase<Province, GetAllProvincesIQueryableCommand>, IRequestHandler<GetAllProvincesIQueryableCommand, IQueryable<ProvinceDto>>
    {
        public GetAllProvincesIQueryaHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {
        }

        protected override IQueryable<Province> OnGetAllFilter(IQueryable<Province> query, GetAllProvincesIQueryableCommand req)
        {
            var q = base.OnGetAllFilter(query, req).Where(x => string.Concat(x.Name, x.Code)!.Contains(req.SearchKeyword??""));

            if (req.CountryId is not null)
                q = q.Where(x => x.CountryId == req.CountryId);

            return q;
        }

        public Task<IQueryable<ProvinceDto>> Handle(GetAllProvincesIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).ProjectToType<ProvinceDto>(_mapper.Config));

    }
}
