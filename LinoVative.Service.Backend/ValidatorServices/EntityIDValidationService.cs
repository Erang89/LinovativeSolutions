using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Shifts;

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
                // outlets
                {EntityTypes.Outlet, _appDbContext.Outlets.Where(x => !x.IsDeleted && x.Id == id && x.CompanyId == actor.CompanyId) },
                {EntityTypes.PaymentMethod, _appDbContext.PaymentMethods.Where(x => !x.IsDeleted && x.Id == id && x.CompanyId == actor.CompanyId) },
                {EntityTypes.PaymentMethodGroup, _appDbContext.PaymentMethodGroups.Where(x => !x.IsDeleted && x.Id == id && x.CompanyId == actor.CompanyId) },
                {EntityTypes.OrderType, _appDbContext.OrderTypes.Where(x => !x.IsDeleted && x.Id == id && x.CompanyId == actor.CompanyId) },
                {EntityTypes.Shift, _appDbContext.Shifts.Where(x => !x.IsDeleted && x.Id == id && x.CompanyId == actor.CompanyId) },
                {EntityTypes.OutletArea, _appDbContext.OutletAreas.Where(x => !x.IsDeleted && x.Id == id && x.Outlet!.CompanyId == actor.CompanyId) },
                {EntityTypes.BankNote, _appDbContext.BankNotes.Where(x => !x.IsDeleted && x.Id == id && x.CompanyId == actor.CompanyId) },
                {EntityTypes.Account, _appDbContext.Accounts.Where(x => !x.IsDeleted && x.Id == id && x.CompanyId == actor.CompanyId) },
                {EntityTypes.COAGroup, _appDbContext.CoaGroups.Where(x => !x.IsDeleted && x.Id == id && x.CompanyId == actor.CompanyId) },
                {EntityTypes.SalesCOAMapping, _appDbContext.SalesCOAMappings.Where(x => !x.IsDeleted && x.Id == id && x.CompanyId == actor.CompanyId) },
            };

            if (!queryDictionary.ContainsKey(entityType.Value))
                throw new NotImplementedException($"{entityType} not in the dictionary");

            var query = queryDictionary[entityType.Value];

            var isAny = query.AnyDynamic();
            return isAny;
        }
    }
}
