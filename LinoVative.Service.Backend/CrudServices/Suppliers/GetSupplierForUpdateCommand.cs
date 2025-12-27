using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Suppliers;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;


namespace LinoVative.Service.Backend.CrudServices.Suppliers
{

    public class GetSupplierForUpdateCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
    }


    public class GetSupplierForUpdateHandlerService : IRequestHandler<GetSupplierForUpdateCommand, Result>
    {

        private readonly IAppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IActor _actor;


        public GetSupplierForUpdateHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _actor = actor;
        }


        public async Task<Result> Handle(GetSupplierForUpdateCommand request, CancellationToken ct)
        {
            var data = await _dbContext.Suppliers.GetAll(_actor).ProjectToType<SupplierInputDto>(_mapper.Config).FirstOrDefaultAsync(x => x.Id == request.Id);
            if (data == null) return Result.Failed($"No Data with ID: {request.Id}");

            data.Contacts = await _dbContext.SupplierContact.Where(x => x.SupplierId == data.Id).ProjectToType<SupplierContactDto>(_mapper.Config).ToListAsync();
            data.Addresses = await _dbContext.SupplierAddress.Where(x => x.SupplierId == data.Id).ProjectToType<SupplierAddressDto>(_mapper.Config).ToListAsync();

            return Result.OK(data);
        }

    }
}
