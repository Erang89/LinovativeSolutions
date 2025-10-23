using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;

namespace LinoVative.Service.Backend.ValidatorServices
{
    public class EntityIDValidationService : IEntityIDValidatorService
    {
        readonly IAppDbContext _appDbContext;

        public EntityIDValidationService(IAppDbContext dbContext)
        {
            _appDbContext = dbContext;
        }


        public bool IsValid(EntityTypes? entityType, Guid id, IActor actor)
        {
            if(id == Guid.Empty) 
                throw new ArgumentNullException(nameof(id));

            if(entityType is null)
                throw new ArgumentNullException(nameof(entityType));

            var queryDictionary = new Dictionary<EntityTypes, IQueryable>()
            {
                {EntityTypes.Item, _appDbContext.Items.Where(x => !x.IsDeleted && x.CompanyId == actor.CompanyId && x.Id == id) },
                {EntityTypes.ItemUnit, _appDbContext.ItemUnits.Where(x => !x.IsDeleted && x.CompanyId == actor.CompanyId && x.Id == id) },
                {EntityTypes.ItemGroup, _appDbContext.ItemGroups.Where(x => !x.IsDeleted && x.CompanyId == actor.CompanyId && x.Id == id) },
                {EntityTypes.ItemCategory, _appDbContext.ItemCategories.Where(x => !x.IsDeleted && x.CompanyId == actor.CompanyId && x.Id == id) },
                {EntityTypes.Currency, _appDbContext.Currencies.Where(x => !x.IsDeleted && x.Id == id) },
                {EntityTypes.Country, _appDbContext.Countries.Where(x => !x.IsDeleted && x.Id == id) },
                {EntityTypes.AppTimeZone, _appDbContext.TimeZones.Where(x => !x.IsDeleted && x.Id == id) },
                {EntityTypes.Company, _appDbContext.Companies.Where(x => !x.IsDeleted && x.Id == id) },
                {EntityTypes.CountryRegion, _appDbContext.CountryRegions.Where(x => !x.IsDeleted && x.Id == id) },
            };

            if (!queryDictionary.ContainsKey(entityType.Value))
                throw new NotImplementedException($"{entityType} not in the dictionary");

            var isAny = queryDictionary[entityType.Value].AnyDynamic();
            return isAny;
        }
    }
}
