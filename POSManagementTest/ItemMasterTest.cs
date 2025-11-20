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

            // ===============================================================================================
            // Arrange 1 : Create new Item
            await Page.GetByRole(AriaRole.Button, new() { Name = "Add New" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Name*" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Name*" }).FillAsync("Testxxx123");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Code*" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Code*" }).FillAsync("Testxx123");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Description" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Unit*" }).ClickAsync();
            await Page.GetByText("Test", new() { Exact = true }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Category*" }).ClickAsync();
            await Page.GetByText("Yeah").ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Sell Price*" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Sell Price*" }).FillAsync("10000");

            // Action 1 : Save Item
            await Page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();

            // Assert 1 : 
            var message =  Page.Locator("div").Filter(new() { HasText = "New Item has been Created" }).Nth(5);
            await Expect(message).ToBeVisibleAsync(); 
            

            // ===============================================================================================
            // Arrange 2 : Search the created item
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Search" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Search" }).FillAsync("Testxx123");
            
            // Assert 2 : Item should exist
            var celName =  Page.GetByRole(AriaRole.Cell, new() { Name = "Testxx123" });
            await Expect(celName).ToBeVisibleAsync();


            // ===============================================================================================
            // Arrange 3 : Delete the Item
            await Page.GetByRole(AriaRole.Row, new() { Name = "Code Name Unit Group Category" }).GetByLabel("", new() { Exact = true }).CheckAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Yes" }).ClickAsync();
            
            // Assert 3 : Should found no item
            var norecord = Page.GetByText("No matching records found");
            await Expect(norecord).ToBeVisibleAsync();
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
            //const string itemCode = "TestDuplicateCode0001";

        }
    }
}
