using Linovative.BackendTest.Bases;
using LinoVative.Service.Backend.CrudServices.Items.Units;

namespace Linovative.BackendTest.Test.Service.Backend.CrudService.Items.Units
{
    public class UnitCRUDTest : UseDatabaseTestBase
    {
        [Fact]
        public async Task CRUD_Success()
        {
            /*+++++++++++++++++++++++++++++++++++++++++++++++++++
             *+ INSERT ITEM UNIT
             *++++++++++++++++++++++++++++++++++++++++++++++++++*/

            // Arrange
            var dbContext = CreateContext();
            var cts = new CancellationTokenSource();
            var createHandler = new CreateItemUnitHandlerService(dbContext, _actor, _mapper, _appCache, _localizer);
            var createCommand = new CreateItemUnitCommand()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
            };


            // Action
            await createHandler.Handle(createCommand, cts.Token);
            var unit = dbContext.ItemUnits.Where(x => x.Id == createCommand.Id).FirstOrDefault();
            
            // Assert
            Assert.NotNull(unit);
            Assert.Equal(createCommand.Name, unit.Name);
            Assert.False(unit.IsDeleted);
            Assert.Equal(unit.CreatedBy, _actor.UserId);
            Assert.NotNull(unit.CreatedAtUtcTime);
            Assert.Null(unit.LastModifiedAtUtcTime);
            Assert.Null(unit.LastModifiedBy);
            Assert.Equal(unit.CompanyId, _actor.CompanyId);


            /*+++++++++++++++++++++++++++++++++++++++++++++++++++
             *+ UPDATE ITEM UNIT
             *++++++++++++++++++++++++++++++++++++++++++++++++++*/

            // Arrange
            var updateHandler = new UpdateItemUnitHandlerService(dbContext, _actor, _mapper, _appCache, _localizer);
            var updateCommand = new UpdateItemUnitCommand() { Id = createCommand.Id, Name = "Test Update" };
            
            // Action
            var updateResult = await updateHandler.Handle(updateCommand, cts.Token);
            unit = dbContext.ItemUnits.FirstOrDefault(x => x.Id == updateCommand.Id);

            // Assert
            Assert.True(updateResult);
            Assert.NotNull(unit);
            Assert.Equal(unit.Name, updateCommand.Name);
            Assert.NotNull(unit.LastModifiedAtUtcTime);
            Assert.NotNull(unit.LastModifiedBy);
            Assert.Equal(unit.LastModifiedBy, _actor.UserId);




            /*+++++++++++++++++++++++++++++++++++++++++++++++++++
             *+ DELETE ITEM UNIT
             *++++++++++++++++++++++++++++++++++++++++++++++++++*/

            //Arrange
            var deleteHandler = new DeleteItemUnitHandlerService(dbContext, _actor, _mapper, _appCache, _localizer);
            var deleteCommand = new DeleteItemUnitCommand() { Id = createCommand.Id };

            // Action
            await deleteHandler.Handle(deleteCommand, cts.Token);
            unit = dbContext.ItemUnits.FirstOrDefault(x => x.Id == updateCommand.Id);

            // Assert
            Assert.True(unit!.IsDeleted);
            Assert.NotNull(unit.LastModifiedAtUtcTime);
            Assert.NotNull(unit.LastModifiedBy);
            Assert.Equal(unit.LastModifiedBy, _actor.UserId);
        }

    }
}
