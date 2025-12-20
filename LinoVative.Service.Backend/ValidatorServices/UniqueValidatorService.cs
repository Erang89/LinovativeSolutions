using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.Extensions;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Items;
using LinoVative.Shared.Dto.ItemDtos;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.ValidatorServices
{
    public class UniqueValidatorService : IUniqueFieldValidatorService
    {
        readonly IAppDbContext _appDbContext;
        public UniqueValidatorService(IAppDbContext dbContext)
        {
            _appDbContext = dbContext;
        }

        Dictionary<EntityTypes, IQueryable>? _queryDic { get; set; } = null;

        public bool IsValid(EntityTypes? entityType, Guid id, string fieldName, object fieldValue, IActor actor, object dto)
        {
            if (entityType == null)
                throw new ArgumentNullException($"{nameof(entityType)} is null");

            if (_queryDic is null)
                _queryDic = new Dictionary<EntityTypes, IQueryable>()
                {
                    {EntityTypes.ItemUnit, _appDbContext.ItemUnits.Where(x => x.Id != id && !x.IsDeleted && x.CompanyId == actor.CompanyId) },
                    {EntityTypes.ItemCategory, _appDbContext.ItemCategories.Where(x => x.Id != id && !x.IsDeleted && x.CompanyId == actor.CompanyId) },
                    {EntityTypes.ItemGroup, _appDbContext.ItemGroups.Where(x => x.Id != id && !x.IsDeleted && x.CompanyId == actor.CompanyId) },
                    {EntityTypes.Item, _appDbContext.Items.Where(x => x.Id != id && !x.IsDeleted && x.CompanyId == actor.CompanyId) },
                    {EntityTypes.Currency, _appDbContext.Currencies.Where(x => x.Id != id && !x.IsDeleted )},
                    {EntityTypes.AppTimeZone, _appDbContext.TimeZones.Where(x => x.Id != id && !x.IsDeleted) },
                    {EntityTypes.Company, _appDbContext.Companies.Where(x => x.Id != id && !x.IsDeleted) },
                    {EntityTypes.CountryRegion, _appDbContext.CountryRegions.Where(x => x.Id != id && !x.IsDeleted) },
                    {EntityTypes.Outlet, _appDbContext.Outlets.Where(x => x.Id != id && !x.IsDeleted && x.CompanyId == actor.CompanyId) },
                    {EntityTypes.OutletArea, _appDbContext.OutletAreas.Where(x => x.Id != id && !x.IsDeleted && x.Outlet!.CompanyId == actor.CompanyId) },
                    {EntityTypes.PaymentMethod, _appDbContext.PaymentMethods.Where(x => x.Id != id && !x.IsDeleted && x.CompanyId == actor.CompanyId) },
                    {EntityTypes.PaymentMethodGroup, _appDbContext.PaymentMethodGroups.Where(x => x.Id != id && !x.IsDeleted && x.CompanyId == actor.CompanyId) },
                    {EntityTypes.OrderType, _appDbContext.OrderTypes.Where(x => x.Id != id && !x.IsDeleted && x.CompanyId == actor.CompanyId) },
                    {EntityTypes.BankNote, _appDbContext.BankNotes.Where(x => x.Id != id && !x.IsDeleted && x.CompanyId == actor.CompanyId) },
                    {EntityTypes.Account, _appDbContext.Accounts.Where(x => x.Id != id && !x.IsDeleted && x.CompanyId == actor.CompanyId) },
                    {EntityTypes.COAGroup, _appDbContext.CoaGroups.Where(x => x.Id != id && !x.IsDeleted && x.CompanyId == actor.CompanyId) },
                    {EntityTypes.Person, _appDbContext.People.Where(x => x.Id != id && !x.IsDeleted && x.CompanyId == actor.CompanyId) },
                    {EntityTypes.Warehouse, _appDbContext.WareHouses.Where(x => x.Id != id && !x.IsDeleted && x.CompanyId == actor.CompanyId) },
                    {EntityTypes.Shift, _appDbContext.Shifts.Where(x => x.Id != id && !x.IsDeleted && x.CompanyId == actor.CompanyId) },
                };

            if (!_queryDic.ContainsKey(entityType.Value))
                throw new NotImplementedException($"{entityType} is not in the dictionary yet");

            var query = _queryDic[entityType.Value]!.WhereEquals(fieldName, fieldValue);

            
            if (dto is ItemCategoryDto categoryDto)
                query = ((IQueryable<ItemCategory>)query).Where(x => x.GroupId == categoryDto.GroupId);

            var querstring = query.ToQueryString();
            var result = !query.AnyDynamic();

            return result;
        }


    }
}
