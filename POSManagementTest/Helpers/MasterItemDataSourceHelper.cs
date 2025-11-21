using Microsoft.Playwright;
using POSManagementTest.Constans;

namespace POSManagementTest.Helpers
{
    public struct ItemDropdownItemSource
    {
        public string UnitName { get; set; }
        public string CategoryName { get; set; }
        public string GroupName { get; set; }
    }

    public static class MasterItemDataSourceHelper
    {
        public static async Task<ItemDropdownItemSource> EnsureDataSourceAvailable(this IPage page)
        {
            await page.PauseAsync();

            Uri uri = new Uri(TestConstants.POSWebUrl);
            string groupsPageLink = $"{uri}pos/management/item-master/groups";
            string categoriesPageLink = $"{uri}pos/management/item-master/categories";
            string unitsPageLink = $"{uri}pos/management/item-master/units";
            string groupName = "Automated Test Group";
            string categoryName = "Automated Test Category";
            string unitName = "Automated Test Unit";

            await page.GotoAsync(groupsPageLink);
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Search" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Search" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Search" }).FillAsync(groupName);
            var noGroupFound = await page.GetByText("No matching records found").IsVisibleAsync();
            if (noGroupFound)
            {

                await page.GetByRole(AriaRole.Button, new() { Name = "Add New" }).ClickAsync();
                await page.GetByRole(AriaRole.Textbox, new() { Name = "Name*" }).ClickAsync();
                await page.GetByRole(AriaRole.Textbox, new() { Name = "Name*" }).FillAsync(groupName);
                await page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
            }


            await page.GotoAsync(categoriesPageLink);
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Search" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Search" }).FillAsync(categoryName);
            var noCategoryFound = await page.GetByText("No matching records found").IsVisibleAsync();
            if(noCategoryFound)
            {
                await page.GetByRole(AriaRole.Button, new() { Name = "Add New" }).ClickAsync();
                await page.GetByRole(AriaRole.Textbox, new() { Name = "Name*" }).ClickAsync();
                await page.GetByRole(AriaRole.Textbox, new() { Name = "Name*" }).FillAsync(categoryName);
                await page.GetByRole(AriaRole.Textbox, new() { Name = "Group*" }).ClickAsync();
                await page.GetByText(groupName).ClickAsync();
                await page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
            }


            await page.GotoAsync(unitsPageLink);
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Search" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Search" }).FillAsync(unitName);
            var noUnitFound = await page.GetByText("No matching records found").IsVisibleAsync();
            if (noUnitFound)
            {
                await page.GetByRole(AriaRole.Button, new() { Name = "Add New" }).ClickAsync();
                await page.GetByRole(AriaRole.Textbox, new() { Name = "Name*" }).ClickAsync();
                await page.GetByRole(AriaRole.Textbox, new() { Name = "Name*" }).FillAsync(unitName);
                await page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
            }

            return new()
            {
                UnitName = unitName,
                CategoryName = categoryName,
                GroupName = groupName,
            };

        }
    }
}
