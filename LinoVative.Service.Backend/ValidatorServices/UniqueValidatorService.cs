using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;

namespace LinoVative.Service.Backend.ValidatorServices
{
    public class UniqueValidatorService : IUniqueFieldValidatorService
    {
        readonly IAppDbContext _appDbContext;
        public UniqueValidatorService(IAppDbContext dbContext)
        {
            _appDbContext = dbContext;
        }

        public bool IsValid(EntityTypes? entityType, Guid id, string fieldName, object fieldValue, IActor actor)
        {
            if(entityType == null) 
                throw new ArgumentNullException($"{nameof(entityType)} is null");

            var queryDictionary = new Dictionary<EntityTypes, IQueryable>()
            {
                {EntityTypes.ItemUnit, _appDbContext.ItemUnits.Where(x => x.Id != id && !x.IsDeleted && x.CompanyId == actor.CompanyId) },
                {EntityTypes.ItemCategory, _appDbContext.ItemCategories.Where(x => x.Id != id && !x.IsDeleted && x.CompanyId == actor.CompanyId) },
                {EntityTypes.ItemGroup, _appDbContext.ItemGroups.Where(x => x.Id != id && !x.IsDeleted && x.CompanyId == actor.CompanyId) },
                {EntityTypes.Item, _appDbContext.Items.Where(x => x.Id != id && !x.IsDeleted && x.CompanyId == actor.CompanyId) },
                {EntityTypes.Currency, _appDbContext.Currencies.Where(x => x.Id != id && !x.IsDeleted )},
                {EntityTypes.AppTimeZone, _appDbContext.TimeZones.Where(x => x.Id != id && !x.IsDeleted) },
                {EntityTypes.Company, _appDbContext.Companies.Where(x => x.Id != id && !x.IsDeleted) },
                {EntityTypes.CountryRegion, _appDbContext.CountryRegions.Where(x => x.Id != id && !x.IsDeleted) },
            };

            if(!queryDictionary.ContainsKey(entityType.Value))
                throw new NotImplementedException($"{entityType} is not in the dictionary yet");

            var query = queryDictionary[entityType.Value]!.WhereEquals(fieldName, fieldValue);

            return !query.AnyDynamic();
        }


    }
}
