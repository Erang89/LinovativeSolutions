using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using POSManagementTest.Constans;

namespace POSManagementTest.Bases
{
    public class PageTestBase : PageTest
    {
        protected Uri RootUri = new Uri(TestConstants.POSWebUrl);
        protected string StoragePath => TestConstants.StoragePath;

        public override BrowserNewContextOptions ContextOptions()
        {
            return new BrowserNewContextOptions
            {
                StorageStatePath = StoragePath
            };
        }
    }
}
