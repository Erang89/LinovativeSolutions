using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Backend.LocalizerServices;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto;
using MapsterMapper;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.CrudServices.Items.ItemCategries
{
    public class DeleteItemCategoryCommand : IRequest<Result>, IEntityId
    {
        public Guid Id { get; set; }
    }

    public class DeleteItemCategoryHandlerService : SaveDeleteServiceBase<ItemCategory, DeleteItemCategoryCommand>, IRequestHandler<DeleteItemCategoryCommand, Result>
    {
        public DeleteItemCategoryHandlerService(IAppDbContext dbContext, IActor actor, IMapper mapper, IAppCache appCache, IStringLocalizer localizer) : 
            base(dbContext, actor, mapper, appCache, localizer)
        {
        }


        public Task<Result> Handle(DeleteItemCategoryCommand request, CancellationToken ct) => base.Handle(request, ct);
    }
}
