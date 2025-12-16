using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Base;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Category;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Group;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Item;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Unit;
using Microsoft.Extensions.DependencyInjection;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Factory
{
    public enum BulkOperationTemplateType
    {
        Item_Create,
        Item_Update,
        Item_Delete,

        Unit_Create,
        Unit_Update,
        Unit_Delete,

        Category_Create,
        Category_Update,
        Category_Delete,

        Group_Create,
        Group_Update,
        Group_Delete,
    }

    public interface IBulkOperationTemplateFactory
    {
        public IExcelTemplateService GetService(BulkOperationTemplateType type);
    }


    public class BulkOperationTemplateFactoryService : IBulkOperationTemplateFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public BulkOperationTemplateFactoryService(IServiceProvider sp)
        {
            _serviceProvider = sp;
        }
        IExcelTemplateService IBulkOperationTemplateFactory.GetService(BulkOperationTemplateType type)
        {
            return type switch
            {
                BulkOperationTemplateType.Item_Create => ActivatorUtilities.CreateInstance<ItemCreateTemplateService>(_serviceProvider),
                BulkOperationTemplateType.Item_Update => ActivatorUtilities.CreateInstance<ItemUpdateTemplateService>(_serviceProvider),
                BulkOperationTemplateType.Item_Delete => ActivatorUtilities.CreateInstance<ItemDeleteTemplateService>(_serviceProvider),

                BulkOperationTemplateType.Group_Create => ActivatorUtilities.CreateInstance<GroupCreateTemplateService>(_serviceProvider),
                BulkOperationTemplateType.Group_Update => ActivatorUtilities.CreateInstance<GroupUpdateTemplateService>(_serviceProvider),
                BulkOperationTemplateType.Group_Delete => ActivatorUtilities.CreateInstance<GroupDeleteTemplateService>(_serviceProvider),

                BulkOperationTemplateType.Category_Create => ActivatorUtilities.CreateInstance<CategoryCreateTemplateService>(_serviceProvider),
                BulkOperationTemplateType.Category_Update => ActivatorUtilities.CreateInstance<CategoryUpdateTemplateService>(_serviceProvider),
                BulkOperationTemplateType.Category_Delete => ActivatorUtilities.CreateInstance<CategoryDeleteTemplateService>(_serviceProvider),

                BulkOperationTemplateType.Unit_Create => ActivatorUtilities.CreateInstance<UnitCreateTemplateService>(_serviceProvider),
                BulkOperationTemplateType.Unit_Update => ActivatorUtilities.CreateInstance<UnitUpdateTemplateService>(_serviceProvider),
                BulkOperationTemplateType.Unit_Delete => ActivatorUtilities.CreateInstance<UnitDeleteTemplateService>(_serviceProvider),

                _ => throw new ArgumentException($"Unknown provider type: {type}")
            };
        }
    }
}
