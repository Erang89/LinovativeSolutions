using Microsoft.Playwright;
using POSManagementTest.Bases;
using System.Text.RegularExpressions;

namespace POSManagementTest
{
    public class ItemMasterTest : PageTestBase
    {

        /// <summary>
        /// Given I am a loggedin user,
        /// When I create a new item
        /// Then I should be able to do it, and the new item should appear in the list.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddNewItemTest()
        {
            await Page.GotoAsync($"{RootUri}pos/management/item-master/items");

            // Arrange 1 : Create new item
            await Page.GetByRole(AriaRole.Button, new() { Name = "Add New" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Name*" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Name*" }).FillAsync("Test Add New Item");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Name*" }).PressAsync("Tab");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Code*" }).FillAsync("TestAddNewItem01");
            await Page.GetByRole(AriaRole.Button).Filter(new() { HasTextRegex = new Regex("^$") }).Nth(3).ClickAsync();
            await Page.GetByText("Test", new() { Exact = true }).ClickAsync();
            await Page.GetByRole(AriaRole.Button).Filter(new() { HasTextRegex = new Regex("^$") }).Nth(5).ClickAsync();
            await Page.GetByText("Yeah").ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Sell Price*" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Sell Price*" }).FillAsync("10000");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Sell Price*" }).PressAsync("Enter");


            // Action 1 : Click Save Button
            await Page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();

            // Assert 1 : Success message is shown
            var message = Page.GetByText("New Item has been Created");
            await Expect(message).ToBeVisibleAsync();

            // Arrange 2 : Search created item
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Search" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Search" }).FillAsync("TestAddNewItem01");
            await Page.GetByRole(AriaRole.Cell, new() { Name = "TestAddNewItem01" }).ClickAsync();
            await Page.GetByRole(AriaRole.Row, new() { Name = "TestAddNewItem01 Test Add New" }).GetByLabel("", new() { Exact = true }).CheckAsync();

            // Action 2 : Delete created item
            await Page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Yes" }).ClickAsync();

            // Assert: No record message is shown
            await Page.GetByText("No matching records found").ClickAsync();

        }



        /// <summary>
        /// GIVEN I am a user 
        /// WHEN I create new item with duplicate code
        /// THEN I should get an error message
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateItemWithDuplicateItemCode()
        {

            await Page.GotoAsync($"{RootUri}pos/management/item-master/items");
            const string itemCode = "TestDuplicateCode0001";

            // Arrange 1 : Create new item
            await Page.GetByRole(AriaRole.Button, new() { Name = "Add New" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Name*" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Name*" }).FillAsync("Test Add New Item 1x");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Name*" }).PressAsync("Tab");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Code*" }).FillAsync(itemCode);
            await Page.GetByRole(AriaRole.Button).Filter(new() { HasTextRegex = new Regex("^$") }).Nth(3).ClickAsync();
            await Page.GetByText("Test", new() { Exact = true }).ClickAsync();
            await Page.GetByRole(AriaRole.Button).Filter(new() { HasTextRegex = new Regex("^$") }).Nth(5).ClickAsync();
            await Page.GetByText("Yeah").ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Sell Price*" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Sell Price*" }).FillAsync("10000");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Sell Price*" }).PressAsync("Enter");
            // Action 1 : Click Save Button for item 1
            await Page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();

            // Arrange 2 : Add new item with the same code
            await Page.GetByRole(AriaRole.Button, new() { Name = "Add New" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Name*" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Name*" }).FillAsync("Test Add New Item 2x");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Name*" }).PressAsync("Tab");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Code*" }).FillAsync(itemCode);
            await Page.GetByRole(AriaRole.Button).Filter(new() { HasTextRegex = new Regex("^$") }).Nth(3).ClickAsync();
            await Page.GetByText("Test", new() { Exact = true }).ClickAsync();
            await Page.GetByRole(AriaRole.Button).Filter(new() { HasTextRegex = new Regex("^$") }).Nth(5).ClickAsync();
            await Page.GetByText("Yeah").ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Sell Price*" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Sell Price*" }).FillAsync("10000");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Sell Price*" }).PressAsync("Enter");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();

            var message = Page.Locator("form").GetByText("Code must be unique. '");
            await Expect(message).ToBeVisibleAsync();


            // Arrange 3 : Search created item
            await Page.GetByRole(AriaRole.Button, new() { Name = "Cancel" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Search" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Search" }).FillAsync(itemCode);
            await Page.GetByRole(AriaRole.Cell, new() { Name = itemCode }).ClickAsync();
            await Page.GetByRole(AriaRole.Row, new() { Name = $"{itemCode} Test Add New" }).GetByLabel("", new() { Exact = true }).CheckAsync();

            // Action 2 : Delete created item
            await Page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Yes" }).ClickAsync();
        }
    }
}
