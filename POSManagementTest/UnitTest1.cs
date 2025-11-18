using Microsoft.Playwright;
using System.Text.RegularExpressions;

namespace POSManagementTest;

public class UnitTest1 : PageTestBase
{
    

    [Fact]
    public async Task HasTitle()
    {        
        await Page.GotoAsync("https://playwright.dev");
        await Page.PauseAsync();
        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Playwright"));

        await Page.GetByRole(AriaRole.Heading, new() { Name = "Playwright enables reliable" }).Locator("span").ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Get started" }).ClickAsync();
        await Page.GetByRole(AriaRole.Heading, new() { Name = "Installation" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Writing tests", Exact = true }).ClickAsync();
        await Page.GetByRole(AriaRole.Heading, new() { Name = "First testDirect link to" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Emulation" }).ClickAsync();
    }

    [Fact]
    public async Task GetStartedLink()
    {
        await Page.GotoAsync("https://playwright.dev");

        // Click the get started link.
        await Page.GetByRole(AriaRole.Link, new() { Name = "Get started" }).ClickAsync();

        // Expects page to have a heading with the name of Installation.
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Installation" })).ToBeVisibleAsync();

    }
}