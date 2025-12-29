using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Customers;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto.Commons;
using LinoVative.Shared.Dto.MasterData.Customers;
using Mapster;
using MapsterMapper;

namespace LinoVative.Service.Backend.CrudServices.Customers
{
    public class GetAllCustomerIQueryableCommand : IRequest<IQueryable<CustomerDto>>
    {
        public string? SearchKeyword { get; set; }
        public List<FilterCondition> Filter { get; set; } = new();
    }

    public class GetAllCustomerQueryableHandlerService : QueryServiceBase<Customer, GetAllCustomerIQueryableCommand>, IRequestHandler<GetAllCustomerIQueryableCommand, IQueryable<CustomerDto>>
    {
        public GetAllCustomerQueryableHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache) : base(dbContext, actor, mapper, appCache)
        {

        }



        protected override IQueryable<Customer> OnGetAllFilter(IQueryable<Customer> query, GetAllCustomerIQueryableCommand req)
        {
            var q = base.OnGetAllFilter(query, req);

            if(!string.IsNullOrWhiteSpace(req.SearchKeyword))
                q = q.Where(x => string.Concat(x.Person!.Firstname, x.Person!.Lastname, x.Person.Nikname).Contains(req.SearchKeyword ?? ""));

            return q;
        }



        public Task<IQueryable<CustomerDto>> Handle(GetAllCustomerIQueryableCommand request, CancellationToken ct) 
            => Task.FromResult(base.GetAll(request).ProjectToType<CustomerDto>(_mapper.Config).ApplyFilters(request.Filter));
    }
}
