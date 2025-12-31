using Linovative.Dto.MasterData.People;
using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.Commons;
using LinoVative.Shared.Dto.MasterData.Customers;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;


namespace LinoVative.Service.Backend.CrudServices.Customers
{

    public class GetCustomerForUpdateCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
    }


    public class GetCusotomerForUpdateHandlerService : IRequestHandler<GetCustomerForUpdateCommand, Result>
    {

        private readonly IAppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IActor _actor;


        public GetCusotomerForUpdateHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _actor = actor;
        }




        public async Task<Result> Handle(GetCustomerForUpdateCommand request, CancellationToken ct)
        {
            var data = await _dbContext.Customers.GetAll(_actor).ProjectToType<CustomerInputDto>(_mapper.Config).FirstOrDefaultAsync(x => x.Id == request.Id);
            if (data == null) return Result.Failed($"No Data with ID: {request.Id}");

            data.Contacts = await _dbContext.CustomerContacts.Where(x => x.CustomerId == data.Id).ProjectToType<CustomerContactDto>(_mapper.Config).ToListAsync();
            data.Address = await _dbContext.CustomerAddress.Where(x => x.CustomerId == data.Id).ProjectToType<CustomerAddressInputDto>(_mapper.Config).ToListAsync();
            data.Person = await _dbContext.People.Where(x => x.Id == data.PersonId).ProjectToType<PersonDto>(_mapper.Config).FirstOrDefaultAsync();

            if(data.Address.Count == 0)
                return Result.OK(data);

            var provinceIds = data.Address.Where(x => x.ProvinceId is not null).Select(x => x.ProvinceId).ToList();
            var regencyIds = data.Address.Where(x => x.RegencyId is not null).Select(x => x.RegencyId).ToList();
            var provinces = await _dbContext.Provinces.Where(x => provinceIds.Contains(x.Id)).ProjectToType<IdWithNameDto>(_mapper.Config).ToListAsync();
            var regencies = await _dbContext.Regencies.Where(x => regencyIds.Contains(x.Id)).ProjectToType<IdWithNameDto>(_mapper.Config).ToListAsync();
            
            data.Address = data.Address.Select(x => {
                x.Province = provinces.FirstOrDefault(p => p.Id == x.ProvinceId);
                x.Regency = regencies.FirstOrDefault(p => p.Id == x.RegencyId);
                return x;
            }).ToList();

            return Result.OK(data);
        }

    }
}
