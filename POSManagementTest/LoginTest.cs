using Microsoft.Playwright;
using POSManagementTest.Bases;

namespace POSManagementTest
{
    public class LoginTest : PageTestBase
    {

        [Fact]
        public async Task ValidLogin()
        {
            await Page.GotoAsync($"{RootUri}login");

            await Page.GetByRole(AriaRole.Textbox, new() { Name = "User Name*" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "User Name*" }).FillAsync("vincent@linovative.com");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Password*" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Password*" }).FillAsync("NotSecure@1");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
            await Expect(Page).ToHaveURLAsync($"{RootUri}");
            await Context.StorageStateAsync(new BrowserContextStorageStateOptions { Path = StoragePath });
        }

        [Fact]
        public async Task InvalidLoginTest()
        {
            await Page.GotoAsync($"{RootUri}login");
            
            // Set 1
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "User Name*" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "User Name*" }).FillAsync("user@linovative.com");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Password*" }).FillAsync("1");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
            var errorLocator = Page.Locator(".mud-alert-message");
            // Assert 1
            await Expect(errorLocator).ToContainTextAsync("Incorect username or password");
            await Expect(Page).ToHaveURLAsync($"{RootUri}login");
           

            // Set 2
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Password*" }).FillAsync("NotSecure@1");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "User Name*" }).FillAsync("");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
            await Page.GetByText("User Name is required").ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "User Name*" }).FillAsync("xxx");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
            errorLocator = Page.Locator(".mud-alert-message");
            // Assert 2
            await Expect(errorLocator).ToContainTextAsync("Incorect username or password");
            await Expect(Page).ToHaveURLAsync($"{RootUri}login");
        }
    }
}
