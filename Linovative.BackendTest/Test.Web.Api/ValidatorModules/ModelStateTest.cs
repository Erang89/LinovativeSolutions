using Linovative.Shared.Interface.Enums;
using Linovative.Shared.Interface;
using LinoVative.Service.Core.Interfaces;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LinoVative.Web.Api.Areas.Admin.Controllers.Items;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using LinoVative.Service.Backend.CrudServices.Items.Units;
using System.ComponentModel.DataAnnotations;
using Linovative.BackendTest.Bases;

namespace Linovative.BackendTest.Test.Web.Api.ValidatorModules
{
    public class ModelStateTest : UseDatabaseTestBase
    {

        private static ItemUnitsController CreateController(IMediator mediator, ILogger<ItemUnitsController>? logger = null)
        {
            logger ??= Mock.Of<ILogger<ItemUnitsController>>();
            var controller = new ItemUnitsController(mediator, logger);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    TraceIdentifier = "test-trace-id"
                }
            };

            return controller;
        }

        [Fact]
        public async Task UniqueFieldTest_UniqueValidatorReturnFalse()
        {
            // Arrange
            var isValidIsCalled = false;
            var uniqueMock = new Mock<IUniqueFieldValidatorService>();
            uniqueMock
                .Setup(s => s.IsValid(
                    It.IsAny<EntityTypes>(),
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IActor>(),
                    It.IsAny<object>()))
                .Returns(() =>
                {
                    isValidIsCalled = true;
                    return false;
                });

            var actorMock = new Mock<IActor>();

            // Build a service provider for ValidationContext
            var services = new ServiceCollection();
            services.AddSingleton(uniqueMock.Object);
            services.AddSingleton(_localizer);
            services.AddSingleton(actorMock.Object);

            var provider = services.BuildServiceProvider();

            // Prepare the command to validate
            var command = new CreateItemUnitCommand
            {
                Name = "DUPLICATE-001"
            };

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(command, provider, items: null);

            // --- Act ---
            Validator.TryValidateObject(command, validationContext, validationResults, validateAllProperties: true);

            // --- Assert ---
            Assert.NotEmpty(validationResults); // should contain validation errors
            Assert.Contains(validationResults, vr => vr.ErrorMessage!.Contains("Unit name must be unique. 'DUPLICATE-001' already exist"));
            Assert.True(isValidIsCalled);

            await Task.CompletedTask;
        }
    }
}
