using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Companies;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.Bases;
using LinoVative.Shared.Dto.CompanyDtos;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.Companies
{
    public class GetAllCompanyCommand : PaginationRequestBase, IRequest<Result>
    {
        public string? SearchKeyword { get; set; }
    }

    public class GetAllCompanyHandlerService : PaginationQueryServiceBase<Company, GetAllCompanyCommand>, IRequestHandler<GetAllCompanyCommand, Result>
    {
        public GetAllCompanyHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache)
            : base(dbContext, actor, mapper, appCache)
        {
            
        }

        protected override IQueryable<Company> OnPaginationQueryFilter(IQueryable<Company> query, GetAllCompanyCommand request)
        {
            if (!string.IsNullOrWhiteSpace(request.SearchKeyword))
                query = query.Where(x => x.Name!.Contains(request.SearchKeyword??""));

            return query;
        }

        public Task<Result> Handle(GetAllCompanyCommand request, CancellationToken ct)
            => base.GetPaginationResult<CompanyDto>(request);
    }

}
