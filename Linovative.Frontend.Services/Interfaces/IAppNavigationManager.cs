using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linovative.Frontend.Services.Interfaces
{
    public interface IAppNavigationService
    {
        string Uri { get; }
        void NavigateTo(string url, bool forceLoad = default);
    }
}
