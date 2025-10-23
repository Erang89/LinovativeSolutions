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

        public bool IsValid(EntityTypes? entityType, string fieldName, object fieldValue, IActor actor)
        {
            if(entityType == null) 
                throw new ArgumentNullException($"{nameof(entityType)} is null");

            var queryDictionary = new Dictionary<EntityTypes, IQueryable>()
            {
                {EntityTypes.ItemUnit, _appDbContext.ItemUnits.Where(x => !x.IsDeleted && x.CompanyId == actor.CompanyId) }
            };

            if(!queryDictionary.ContainsKey(entityType.Value))
                return false;

            var query = queryDictionary[entityType.Value]!.WhereEquals(fieldName, fieldValue);

            return !query.AnyDynamic();
        }


    }
}
