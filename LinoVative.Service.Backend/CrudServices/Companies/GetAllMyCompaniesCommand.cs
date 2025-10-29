using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Companies;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.CompanyDtos;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Companies
{
    public class GetAllMyCompaniesCommand : IRequest<Result>
    {
        public string? SearchKeyword { get; set; }
    }

    public class GetAllMyCompaniesService : PaginationQueryServiceBase<Company, GetAllMyCompaniesCommand>, IRequestHandler<GetAllMyCompaniesCommand, Result>
    {
        public GetAllMyCompaniesService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache)
            : base(dbContext, actor, mapper, appCache)
        {
            
        }

        protected override IQueryable<Company> OnPaginationQueryFilter(IQueryable<Company> query, GetAllMyCompaniesCommand request)
        {
            var mainCompanyId = _dbContext.Companies.Where(x => !x.IsDeleted && x.OwnByUserId == _actor.UserId).FirstOrDefault()?.Id;

            var companyIds = new List<Guid>();

            if (mainCompanyId is not null)
                companyIds.Add(mainCompanyId.Value);

            var userCompanies = _dbContext.UserCompanies.Where(x => !x.IsDeleted && x.UserId == _actor.UserId).Select(x => x.CompanyId);

            if (!string.IsNullOrWhiteSpace(request.SearchKeyword))
                query = query.Where(x => x.Name!.Contains(request.SearchKeyword??""));
            
            return query.Where(x => x.Id == mainCompanyId || userCompanies.Contains(x.Id));
        }

        public Task<Result> Handle(GetAllMyCompaniesCommand request, CancellationToken ct)
            => base.GetPaginationResult<CompanyDto>(request);
    }

}
