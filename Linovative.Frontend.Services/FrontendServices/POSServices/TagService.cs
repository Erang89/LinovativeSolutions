using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.ItemDtos;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface ITagService : IReadOnlyService<TagDto>, ICrudInterfaces
    {

    }

    public class TagService : CrudServiceAbstract<TagDto>, ITagService
    {
        public TagService(IHttpClientFactory httpFactory, ILogger<TagService> logger) : base(httpFactory, logger, "Tags")
        {
        }
    }
}
