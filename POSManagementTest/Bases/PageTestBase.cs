using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

namespace POSManagementTest
{
    public class PageTestBase : PageTest
    {
        protected Uri RootUri => new Uri("https://localhost:7081");
    }
}
