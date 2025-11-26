using Linovative.BackendTest.Bases;
using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.ValidatorServices;
using LinoVative.Service.Core.Accountings;
using LinoVative.Service.Core.Items;
using LinoVative.Service.Core.OrderTypes;
using LinoVative.Service.Core.Outlets;
using LinoVative.Service.Core.Payments;
using LinoVative.Service.Core.Shifts;

namespace Linovative.BackendTest.Test.Service.Backend.ValidatorServices
{
    public class EntityIDValidationServiceTest : UseDatabaseTestBase
    {

        [Fact]
        public async Task CheckIfItemExit()
        {
            // Arrange
            var dbContext = CreateContext();
            var item = new Item() { Id = Guid.NewGuid(), Name = "Test Exist Item", CompanyId = _actor.CompanyId };
            var otherItem = new Item() { Id = Guid.NewGuid(), Name = "Test Exist Item", CompanyId = Guid.NewGuid() };
            dbContext.Items.Add(item);
            dbContext.Items.Add(otherItem);
            await dbContext.SaveAsync(_actor);
            IEntityIDValidatorService service = new EntityIDValidationService(dbContext);

            // Act
            var result1 = service.IsValid(EntityTypes.Item, item.Id, _actor);
            var result2 = service.IsValid(EntityTypes.Item, otherItem.Id, _actor);

            // Assert
            Assert.True(result1);
            Assert.False(result2);

            await Task.CompletedTask;
        }

        [Fact]
        public async Task CheckIfItemUnitExit()
        {
            // Arrange
            var dbContext = CreateContext();
            var unit = new ItemUnit() { Id = Guid.NewGuid(), Name = "Item Unit", CompanyId = _actor.CompanyId };
            var otherUnit = new ItemUnit() { Id = Guid.NewGuid(), Name = "Other Item Unit", CompanyId = Guid.NewGuid() };
            dbContext.ItemUnits.Add(unit);
            dbContext.ItemUnits.Add(otherUnit);
            await dbContext.SaveAsync(_actor);
            IEntityIDValidatorService service = new EntityIDValidationService(dbContext);

            // Act
            var result1 = service.IsValid(EntityTypes.ItemUnit, unit.Id, _actor);
            var result2 = service.IsValid(EntityTypes.ItemUnit, otherUnit.Id, _actor);

            // Assert
            Assert.True(result1);
            Assert.False(result2);

            await Task.CompletedTask;
        }


        [Fact]
        public async Task CheckIfItemGroupExit()
        {
            // Arrange
            var dbContext = CreateContext();
            var group = new ItemGroup() { Id = Guid.NewGuid(), Name = "Item Group", CompanyId = _actor.CompanyId };
            var otherGroup = new ItemGroup() { Id = Guid.NewGuid(), Name = "Other Item Group", CompanyId = Guid.NewGuid() };
            dbContext.ItemGroups.Add(group);
            dbContext.ItemGroups.Add(otherGroup);
            await dbContext.SaveAsync(_actor);
            IEntityIDValidatorService service = new EntityIDValidationService(dbContext);

            // Act
            var result1 = service.IsValid(EntityTypes.ItemGroup, group.Id, _actor);
            var result2 = service.IsValid(EntityTypes.ItemGroup, otherGroup.Id, _actor);

            // Assert
            Assert.True(result1);
            Assert.False(result2);

            await Task.CompletedTask;
        }


        [Fact]
        public async Task CheckIfItemCategoryExit()
        {
            // Arrange
            var dbContext = CreateContext();
            var group = new ItemGroup() { Name = "Test Group", CompanyId = _actor.CompanyId };
            var otherGroup = new ItemGroup() { Name = "Other Group", CompanyId = Guid.NewGuid() };
            var category = new ItemCategory() { GroupId = group.Id, Id = Guid.NewGuid(), Name = "Item Category", CompanyId = _actor.CompanyId };
            var otherCategory = new ItemCategory() {GroupId = otherGroup.Id, Id = Guid.NewGuid(), Name = "Other Item Category", CompanyId = Guid.NewGuid() };
            dbContext.ItemCategories.Add(category);
            dbContext.ItemCategories.Add(otherCategory);
            dbContext.ItemGroups.Add(group);
            dbContext.ItemGroups.Add(otherGroup);
            await dbContext.SaveAsync(_actor);
            IEntityIDValidatorService service = new EntityIDValidationService(dbContext);

            // Act
            var result1 = service.IsValid(EntityTypes.ItemCategory, category.Id, _actor);
            var result2 = service.IsValid(EntityTypes.ItemCategory, otherCategory.Id, _actor);

            // Assert
            Assert.True(result1);
            Assert.False(result2);

            await Task.CompletedTask;
        }


        [Fact]
        public async Task CheckIfOutletExit()
        {
            // Arrange
            var dbContext = CreateContext();
            var outlet = new Outlet() { Id = Guid.NewGuid(), Name = "Outlet Test", CompanyId = _actor.CompanyId };
            var otherOutlet = new Outlet() { Id = Guid.NewGuid(), Name = "Other Outlet", CompanyId = Guid.NewGuid() };
            dbContext.Outlets.Add(outlet);
            dbContext.Outlets.Add(otherOutlet);
            await dbContext.SaveAsync(_actor);
            IEntityIDValidatorService service = new EntityIDValidationService(dbContext);

            // Act
            var result1 = service.IsValid(EntityTypes.Outlet, outlet.Id, _actor);
            var result2 = service.IsValid(EntityTypes.Outlet, otherOutlet.Id, _actor);

            // Assert
            Assert.True(result1);
            Assert.False(result2);

            await Task.CompletedTask;
        }


        [Fact]
        public async Task CheckIfOutletAreaExit()
        {
            // Arrange
            var dbContext = CreateContext();
            var outlet = new Outlet() { Name = "Test Outlet", CompanyId = _actor.CompanyId };
            var otherOutlet = new Outlet() { Name = "Other Outlet", CompanyId = Guid.NewGuid()};
            var area = new OutletArea() {OutletId = outlet.Id, Id = Guid.NewGuid(), Name = "Outlet Test"};
            var otherArea = new OutletArea() {OutletId = otherOutlet.Id, Id = Guid.NewGuid(), Name = "Other Outlet"};
            dbContext.OutletAreas.Add(area);
            dbContext.OutletAreas.Add(otherArea);
            dbContext.Outlets.Add(outlet);
            dbContext.Outlets.Add(otherOutlet);

            await dbContext.SaveAsync(_actor);
            IEntityIDValidatorService service = new EntityIDValidationService(dbContext);

            // Act
            var result1 = service.IsValid(EntityTypes.OutletArea, area.Id, _actor);
            var result2 = service.IsValid(EntityTypes.OutletArea, otherArea.Id, _actor);

            // Assert
            Assert.True(result1);
            Assert.False(result2);

            await Task.CompletedTask;
        }


        [Fact]
        public async Task PaymentMethod()
        {
            // Arrange
            var dbContext = CreateContext();

            
            var group = new PaymentMethodGroup() { Name = "Test Group", CompanyId = _actor.CompanyId };
            var otherGroup = new PaymentMethodGroup() { Name = "Other Group", CompanyId = Guid.NewGuid() };
            dbContext.PaymentMethodGroups.Add(group);
            dbContext.PaymentMethodGroups.Add(otherGroup);

            var method = new PaymentMethod() { Name = "Test Method", PaymentMethodGroupId = group.Id, CompanyId = _actor.CompanyId };
            var otherMethod = new PaymentMethod() { Name = "Other Method", PaymentMethodGroupId = otherGroup.Id, CompanyId = otherGroup.CompanyId };
            dbContext.PaymentMethods.Add(method);
            dbContext.PaymentMethods.Add(otherMethod);

            await dbContext.SaveAsync(_actor);
            IEntityIDValidatorService service = new EntityIDValidationService(dbContext);

            // Act
            var result1 = service.IsValid(EntityTypes.PaymentMethodGroup, group.Id, _actor);
            var result2 = service.IsValid(EntityTypes.PaymentMethodGroup, otherGroup.Id, _actor);
            var result3 = service.IsValid(EntityTypes.PaymentMethod, method.Id, _actor);
            var result4 = service.IsValid(EntityTypes.PaymentMethod, otherMethod.Id, _actor);

            // Assert
            Assert.True(result1);
            Assert.False(result2);
            Assert.True(result3);
            Assert.False(result4);

            await Task.CompletedTask;
        }


        [Fact]
        public async Task CheckIfOrderTypeExit()
        {
            // Arrange
            var dbContext = CreateContext();
            var orderType = new OrderType() { Id = Guid.NewGuid(), Name = "Order Type Test", CompanyId = _actor.CompanyId };
            var otherOrderType = new OrderType() { Id = Guid.NewGuid(), Name = "Other Type Outlet", CompanyId = Guid.NewGuid() };
            dbContext.OrderTypes.Add(orderType);
            dbContext.OrderTypes.Add(otherOrderType);
            await dbContext.SaveAsync(_actor);
            IEntityIDValidatorService service = new EntityIDValidationService(dbContext);

            // Act
            var result1 = service.IsValid(EntityTypes.OrderType, orderType.Id, _actor);
            var result2 = service.IsValid(EntityTypes.OrderType, otherOrderType.Id, _actor);

            // Assert
            Assert.True(result1);
            Assert.False(result2);

            await Task.CompletedTask;
        }


        [Fact]
        public async Task CheckIfShiftExit()
        {
            // Arrange
            var dbContext = CreateContext();
            var startTime = new TimeSpan(1, 0, 0);
            var endTime = new TimeSpan(10, 0, 0);

            var shift = new Shift() { Id = Guid.NewGuid(), Name = "Shift Test", CompanyId = _actor.CompanyId, StartTime = startTime, EndTime = endTime };
            var otherShift = new Shift() { Id = Guid.NewGuid(), Name = "Other Shift", CompanyId = Guid.NewGuid(), StartTime = startTime, EndTime = endTime };
            dbContext.Shifts.Add(shift);
            dbContext.Shifts.Add(otherShift);
            await dbContext.SaveAsync(_actor);
            IEntityIDValidatorService service = new EntityIDValidationService(dbContext);

            // Act
            var result1 = service.IsValid(EntityTypes.Shift, shift.Id, _actor);
            var result2 = service.IsValid(EntityTypes.Shift, otherShift.Id, _actor);

            // Assert
            Assert.True(result1);
            Assert.False(result2);

            await Task.CompletedTask;
        }


        [Fact]
        public async Task CheckIfAccountExit()
        {
            // Arrange
            var dbContext = CreateContext();
            var group = new COAGroup() { Name = "COA Test", CompanyId = _actor.CompanyId, Type = COATypes.Asset };
            var otherGroup = new COAGroup() { Name = "Other COA", CompanyId = _actor.CompanyId, Type = COATypes.Asset };

            var account = new Account() {GroupId = group.Id, Id = Guid.NewGuid(), Name = "Account Test", CompanyId = _actor.CompanyId };
            var otherAccount = new Account() {GroupId = otherGroup.Id, Id = Guid.NewGuid(), Name = "Other Account", CompanyId = Guid.NewGuid() };
            dbContext.Accounts.Add(account);
            dbContext.Accounts.Add(otherAccount);
            dbContext.CoaGroups.Add(group);
            dbContext.CoaGroups.Add(otherGroup);
            await dbContext.SaveAsync(_actor);
            IEntityIDValidatorService service = new EntityIDValidationService(dbContext);

            // Act
            var result1 = service.IsValid(EntityTypes.Account, account.Id, _actor);
            var result2 = service.IsValid(EntityTypes.Account, otherAccount.Id, _actor);
            var result3 = service.IsValid(EntityTypes.Account, Guid.NewGuid(), _actor);

            // Assert
            Assert.True(result1);
            Assert.False(result2);
            Assert.False(result3);

            await Task.CompletedTask;
        }

    }
}
