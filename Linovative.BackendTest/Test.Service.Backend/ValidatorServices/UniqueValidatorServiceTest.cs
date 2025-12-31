using Linovative.BackendTest.Bases;
using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.ValidatorServices;
using LinoVative.Service.Core.Items;
using LinoVative.Service.Core.OrderTypes;
using LinoVative.Service.Core.Outlets;
using LinoVative.Service.Core.Payments;
using LinoVative.Shared.Dto.ItemDtos;
using LinoVative.Shared.Dto.MasterData.Payments;
using LinoVative.Shared.Dto.OrderTypes;
using LinoVative.Shared.Dto.Outlets;

namespace Linovative.BackendTest.Test.Service.Backend.ValidatorServices
{
    public class UniqueValidatorServiceTest : UseDatabaseTestBase
    {
        [Fact]
        public async Task ValidateDuplicate_ItemUnit()
        {
            // Arrange Database
            var dbContext = CreateContext();
            var itemUnit = new ItemUnit() { Name = "Test Unit", CompanyId = _actor.CompanyId};
            var itemUnitX = new ItemUnit() { Name = "Test Unit Other", CompanyId = Guid.NewGuid()};
            dbContext.ItemUnits.Add(itemUnit);
            dbContext.ItemUnits.Add(itemUnitX);
            await dbContext.SaveAsync(_actor);
            IUniqueFieldValidatorService service = new UniqueValidatorService(dbContext);

            // Action
            var dto1 = new ItemUnitDto() {Id = Guid.NewGuid(), Name = "Test Unit"};
            var result1 = service.IsValid(EntityTypes.ItemUnit, Guid.NewGuid(), nameof(ItemUnitDto.Name), dto1.Name,  _actor, dto1);
            var dto2 = new ItemUnitDto() { Id = Guid.NewGuid(), Name = "Test Unit-X" };
            var result2 = service.IsValid(EntityTypes.ItemUnit, Guid.NewGuid(), nameof(ItemUnitDto.Name), dto2.Name, _actor, dto2);
            var dto3 = new ItemUnitDto() { Id = Guid.NewGuid(), Name = "Test Unit Other" };
            var result3 = service.IsValid(EntityTypes.ItemUnit, Guid.NewGuid(), nameof(ItemUnitDto.Name), dto3.Name, _actor, dto3);

            // Assert
            Assert.False(result1);
            Assert.True(result2);
            Assert.True(result3);
        }

        [Fact]
        public async Task ValidateDuplicate_ItemGroup()
        {
            // Arrange Database
            var dbContext = CreateContext();
            var itemGroup = new ItemGroup() { Name = "Test Group", CompanyId = _actor.CompanyId };
            var itemGroupX = new ItemGroup() { Name = "Test Group Other", CompanyId = Guid.NewGuid() };
            dbContext.ItemGroups.Add(itemGroup);
            dbContext.ItemGroups.Add(itemGroupX);
            await dbContext.SaveAsync(_actor);
            IUniqueFieldValidatorService service = new UniqueValidatorService(dbContext);

            // Action
            var dto1 = new ItemGroupDto() { Id = Guid.NewGuid(), Name = "Test Group" };
            var result1 = service.IsValid(EntityTypes.ItemGroup, Guid.NewGuid(), nameof(ItemGroupDto.Name), dto1.Name, _actor, dto1);
            var dto2 = new ItemGroupDto() { Id = Guid.NewGuid(), Name = "Test Group-X" };
            var result2 = service.IsValid(EntityTypes.ItemGroup, Guid.NewGuid(), nameof(ItemGroupDto.Name), dto2.Name, _actor, dto2);
            var dto3 = new ItemGroupDto() { Id = Guid.NewGuid(), Name = "Test Group Other" };
            var result3 = service.IsValid(EntityTypes.ItemGroup, Guid.NewGuid(), nameof(ItemGroupDto.Name), dto3.Name, _actor, dto3);

            // Assert
            Assert.False(result1);
            Assert.True(result2);
            Assert.True(result3);
        }


        [Fact]
        public async Task ValidateDuplicate_ItemCategory()
        {
            // Arrange Database
            var dbContext = CreateContext();
            var group = new ItemGroup() { Name = "Test Group", CompanyId = _actor.CompanyId};
            var otherGroup = new ItemGroup() { Name = "Test Group", CompanyId = Guid.NewGuid()};
            var itemCategory = new ItemCategory() { GroupId = group.Id, Name = "Test Category", CompanyId = _actor.CompanyId };
            var itemCategoryX = new ItemCategory() { GroupId = otherGroup.Id,  Name = "Test Category Other", CompanyId = Guid.NewGuid() };
            dbContext.ItemCategories.Add(itemCategory);
            dbContext.ItemCategories.Add(itemCategoryX);
            dbContext.ItemGroups.Add(group);
            dbContext.ItemGroups.Add(otherGroup);
            await dbContext.SaveAsync(_actor);
            IUniqueFieldValidatorService service = new UniqueValidatorService(dbContext);

            // Action
            var dto1 = new ItemCategoryDto() { Id = Guid.NewGuid(), GroupId = group.Id, Name = "Test Category" };
            var result1 = service.IsValid(EntityTypes.ItemCategory, Guid.NewGuid(), nameof(ItemCategoryDto.Name), dto1.Name, _actor, dto1);
            var dto2 = new ItemCategoryDto() { Id = Guid.NewGuid(), Name = "Test Category-X" };
            var result2 = service.IsValid(EntityTypes.ItemCategory, Guid.NewGuid(), nameof(ItemCategoryDto.Name), dto2.Name, _actor, dto2);
            var dto3 = new ItemCategoryDto() { Id = Guid.NewGuid(), GroupId = group.Id, Name = "Test Category Other" };
            var result3 = service.IsValid(EntityTypes.ItemCategory, Guid.NewGuid(), nameof(ItemCategoryDto.Name), dto3.Name, _actor, dto3);

            // Assert
            Assert.False(result1);
            Assert.True(result2);
            Assert.True(result3);
        }


        [Fact]
        public async Task ValidateDuplicate_Item()
        {
            // Arrange Database
            var dbContext = CreateContext();
            var item = new Item() { Name = "Test Item", CompanyId = _actor.CompanyId };
            var itemx = new Item() { Name = "Test Item Other", CompanyId = Guid.NewGuid() };
            dbContext.Items.Add(item);
            dbContext.Items.Add(itemx);
            await dbContext.SaveAsync(_actor);
            IUniqueFieldValidatorService service = new UniqueValidatorService(dbContext);

            // Action
            var dto1 = new ItemDto() { Id = Guid.NewGuid(), Name = "Test Item" };
            var result1 = service.IsValid(EntityTypes.Item, Guid.NewGuid(), nameof(ItemDto.Name), dto1.Name, _actor, dto1);
            var dto2 = new ItemDto() { Id = Guid.NewGuid(), Name = "Test Item-X" };
            var result2 = service.IsValid(EntityTypes.Item, Guid.NewGuid(), nameof(ItemDto.Name), dto2.Name, _actor, dto2);
            var dto3 = new ItemDto() { Id = Guid.NewGuid(), Name = "Test Item Other" };
            var result3 = service.IsValid(EntityTypes.Item, Guid.NewGuid(), nameof(ItemDto.Name), dto3.Name, _actor, dto3);

            // Assert
            Assert.False(result1);
            Assert.True(result2);
            Assert.True(result3);
        }


        [Fact]
        public async Task ValidateDuplicate_Outlet()
        {
            // Arrange Database
            var dbContext = CreateContext();
            var outlet = new Outlet() { Name = "Test Outlet", CompanyId = _actor.CompanyId };
            var outletX = new Outlet() { Name = "Test Outlet Other", CompanyId = Guid.NewGuid() };
            dbContext.Outlets.Add(outlet);
            dbContext.Outlets.Add(outletX);
            await dbContext.SaveAsync(_actor);
            IUniqueFieldValidatorService service = new UniqueValidatorService(dbContext);

            // Action
            var dto1 = new OutletDto() { Id = Guid.NewGuid(), Name = "Test Outlet" };
            var result1 = service.IsValid(EntityTypes.Outlet, Guid.NewGuid(), nameof(OutletDto.Name), dto1.Name, _actor, dto1);
            var dto2 = new OutletDto() { Id = Guid.NewGuid(), Name = "Test Outlet-X" };
            var result2 = service.IsValid(EntityTypes.Outlet, Guid.NewGuid(), nameof(OutletDto.Name), dto2.Name, _actor, dto2);
            var dto3 = new OutletDto() { Id = Guid.NewGuid(), Name = "Test Outlet Other" };
            var result3 = service.IsValid(EntityTypes.Outlet, Guid.NewGuid(), nameof(OutletDto.Name), dto3.Name, _actor, dto3);

            // Assert
            Assert.False(result1);
            Assert.True(result2);
            Assert.True(result3);
        }

        [Fact]
        public async Task ValidateDuplicate_OutletArea()
        {
            // Arrange Database
            var dbContext = CreateContext();
            var outlet = new Outlet() { CompanyId = _actor.CompanyId, Name = "Test Outlet" };
            var otherOutlet = new Outlet() { CompanyId = Guid.NewGuid(), Name = "Test Outlet-X" };
            var outletArea = new OutletArea() { Name = "Test Area", OutletId = outlet.Id };
            var outletAreasX = new OutletArea() { Name = "Test Area Other", OutletId = otherOutlet.Id };
            dbContext.OutletAreas.Add(outletArea);
            dbContext.OutletAreas.Add(outletAreasX);
            dbContext.Outlets.Add(outlet);
            dbContext.Outlets.Add(otherOutlet);
            await dbContext.SaveAsync(_actor);
            IUniqueFieldValidatorService service = new UniqueValidatorService(dbContext);

            // Action
            var dto1 = new OutletAreaDto() {OutletId = outlet.Id, Id = Guid.NewGuid(), Name = "Test Area" };
            var result1 = service.IsValid(EntityTypes.OutletArea, Guid.NewGuid(), nameof(OutletAreaDto.Name), dto1.Name, _actor, dto1);
            var dto2 = new OutletAreaDto() { OutletId = outlet.Id, Id = Guid.NewGuid(), Name = "Test Area-X" };
            var result2 = service.IsValid(EntityTypes.OutletArea, Guid.NewGuid(), nameof(OutletAreaDto.Name), dto2.Name, _actor, dto2);
            var dto3 = new OutletAreaDto() {OutletId = otherOutlet.Id, Id = Guid.NewGuid(), Name = "Test Area Other" };
            var result3 = service.IsValid(EntityTypes.OutletArea, Guid.NewGuid(), nameof(OutletAreaDto.Name), dto3.Name, _actor, dto3);

            // Assert
            Assert.False(result1);
            Assert.True(result2);
            Assert.True(result3);
        }


        [Fact]
        public async Task ValidateDuplicate_PaymentMethodGroup()
        {
            // Arrange Database
            var dbContext = CreateContext();
            var item = new PaymentMethodGroup() { Name = "Test PMG", CompanyId = _actor.CompanyId };
            var itemx = new PaymentMethodGroup() { Name = "Test PMG Other", CompanyId = Guid.NewGuid() };
            dbContext.PaymentMethodGroups.Add(item);
            dbContext.PaymentMethodGroups.Add(itemx);
            await dbContext.SaveAsync(_actor);
            IUniqueFieldValidatorService service = new UniqueValidatorService(dbContext);

            // Action
            var dto1 = new PaymentMethodGroupDto() { Id = Guid.NewGuid(), Name = "Test PMG" };
            var result1 = service.IsValid(EntityTypes.PaymentMethodGroup, Guid.NewGuid(), nameof(PaymentMethodGroupDto.Name), dto1.Name, _actor, dto1);
            var dto2 = new PaymentMethodGroupDto() { Id = Guid.NewGuid(), Name = "Test PMG-X" };
            var result2 = service.IsValid(EntityTypes.PaymentMethodGroup, Guid.NewGuid(), nameof(PaymentMethodGroupDto.Name), dto2.Name, _actor, dto2);
            var dto3 = new PaymentMethodGroupDto() { Id = Guid.NewGuid(), Name = "Test PMG Other" };
            var result3 = service.IsValid(EntityTypes.PaymentMethodGroup, Guid.NewGuid(), nameof(PaymentMethodGroupDto.Name), dto3.Name, _actor, dto3);

            // Assert
            Assert.False(result1);
            Assert.True(result2);
            Assert.True(result3);
        }


        [Fact]
        public async Task ValidateDuplicate_PaymentMethod()
        {
            // Arrange Database
            var dbContext = CreateContext();
            var group = new PaymentMethodGroup() { Name = "Test PMG", CompanyId = _actor.CompanyId };
            var otherGroup = new PaymentMethodGroup() { Name = "Test PMG Other", CompanyId = Guid.NewGuid() };
            var method = new PaymentMethod() { PaymentMethodGroupId = group.Id, Name = "Test PMed", CompanyId = _actor.CompanyId };
            var otherMethod = new PaymentMethod() { PaymentMethodGroupId = otherGroup.Id, Name = "Test PMPMedG Other", CompanyId = otherGroup.CompanyId };
            dbContext.PaymentMethods.Add(method);
            dbContext.PaymentMethods.Add(otherMethod);
            dbContext.PaymentMethodGroups.Add(group);
            dbContext.PaymentMethodGroups.Add(otherGroup);
            await dbContext.SaveAsync(_actor);
            IUniqueFieldValidatorService service = new UniqueValidatorService(dbContext);

            // Action
            var dto1 = new PaymentMethodDto() { PaymentMethodGroupId = group.Id, Id = Guid.NewGuid(), Name = "Test PMed" };
            var result1 = service.IsValid(EntityTypes.PaymentMethod, Guid.NewGuid(), nameof(PaymentMethodDto.Name), dto1.Name, _actor, dto1);
            var dto2 = new PaymentMethodDto() { PaymentMethodGroupId = group.Id, Id = Guid.NewGuid(), Name = "Test PMed-X" };
            var result2 = service.IsValid(EntityTypes.PaymentMethod, Guid.NewGuid(), nameof(PaymentMethodDto.Name), dto2.Name, _actor, dto2);
            var dto3 = new PaymentMethodDto() { PaymentMethodGroupId = otherGroup.Id, Id = Guid.NewGuid(), Name = "Test PMed Other" };
            var result3 = service.IsValid(EntityTypes.PaymentMethod, Guid.NewGuid(), nameof(PaymentMethodDto.Name), dto3.Name, _actor, dto3);

            // Assert
            Assert.False(result1);
            Assert.True(result2);
            Assert.True(result3);
        }


        [Fact]
        public async Task ValidateDuplicate_OrderType()
        {
            // Arrange Database
            var dbContext = CreateContext();
            var orderType = new OrderType() { Name = "Test Order Type", CompanyId = _actor.CompanyId };
            var orderTypex = new OrderType() { Name = "Test Order Type Other", CompanyId = Guid.NewGuid() };
            dbContext.OrderTypes.Add(orderType);
            dbContext.OrderTypes.Add(orderTypex);
            await dbContext.SaveAsync(_actor);
            IUniqueFieldValidatorService service = new UniqueValidatorService(dbContext);

            // Action
            var dto1 = new OrderTypeDto() { Id = Guid.NewGuid(), Name = "Test Order Type" };
            var result1 = service.IsValid(EntityTypes.OrderType, Guid.NewGuid(), nameof(OrderTypeDto.Name), dto1.Name, _actor, dto1);
            var dto2 = new OrderTypeDto() { Id = Guid.NewGuid(), Name = "Test Order Type-X" };
            var result2 = service.IsValid(EntityTypes.OrderType, Guid.NewGuid(), nameof(OrderTypeDto.Name), dto2.Name, _actor, dto2);
            var dto3 = new OrderTypeDto() { Id = Guid.NewGuid(), Name = "Test Order Type Other" };
            var result3 = service.IsValid(EntityTypes.OrderType, Guid.NewGuid(), nameof(OrderTypeDto.Name), dto3.Name, _actor, dto3);

            // Assert
            Assert.False(result1);
            Assert.True(result2);
            Assert.True(result3);
        }
    }
}
