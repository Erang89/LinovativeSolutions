using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Enums;
using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Category;
using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Group;
using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Item;
using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings.Unit;
using Microsoft.Extensions.DependencyInjection;

namespace LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Mappings
{
    public interface IBulkOperationProcessFactory
    {
        public IBulkOperationProcess GetBulkOperationProvider(BulkOperationTypes type);
    }

    public class BulkOperationProcessFactoryService : IBulkOperationProcessFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public BulkOperationProcessFactoryService(IServiceProvider sp)
        {
            _serviceProvider = sp;
        }

        public IBulkOperationProcess GetBulkOperationProvider(BulkOperationTypes type)
        {
            return type switch
            {
                BulkOperationTypes.ItemCreate => ActivatorUtilities.CreateInstance<BulkCreateItemService>(_serviceProvider),
                BulkOperationTypes.ItemUpdate => ActivatorUtilities.CreateInstance<BulkUpdateItemService>(_serviceProvider),
                BulkOperationTypes.ItemDelete => ActivatorUtilities.CreateInstance<BulkDeleteItemService>(_serviceProvider),

                BulkOperationTypes.GroupCreate => ActivatorUtilities.CreateInstance<BulkCreateGroupService>(_serviceProvider),
                BulkOperationTypes.GroupUpdate => ActivatorUtilities.CreateInstance<BulkUpdateGroupService>(_serviceProvider),
                BulkOperationTypes.GroupDelete => ActivatorUtilities.CreateInstance<BulkDeleteItemGroupService>(_serviceProvider),

                BulkOperationTypes.CategoryCreate => ActivatorUtilities.CreateInstance<BulkCreateCategoryService>(_serviceProvider),
                BulkOperationTypes.CategoryUpdate => ActivatorUtilities.CreateInstance<BulkUpdateCategoryService>(_serviceProvider),
                BulkOperationTypes.CategoryDelete => ActivatorUtilities.CreateInstance<BulkCreateCategoryService>(_serviceProvider),

                BulkOperationTypes.UnitCreate => ActivatorUtilities.CreateInstance<BulkCreateUnitService>(_serviceProvider),
                BulkOperationTypes.UnitUpdate => ActivatorUtilities.CreateInstance<BulkUpdateUnitService>(_serviceProvider),
                BulkOperationTypes.UnitDelete => ActivatorUtilities.CreateInstance<BulkDeleteUnitService>(_serviceProvider),

                _ => throw new ArgumentException($"Unknown provider type: {type}")
            };
        }
    }
}
