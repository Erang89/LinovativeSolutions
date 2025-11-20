using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

namespace POSManagementTest.Bases
{
    public class PageTestBase : PageTest
    {
        protected Uri RootUri => new Uri("https://localhost:7081");
        protected const string StoragePath = "../../../playwright/.auth/state.json";

        public override BrowserNewContextOptions ContextOptions()
        {
            return new BrowserNewContextOptions
            {
                StorageStatePath = StoragePath
            };
        }
    }
}
