using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linovative.Frontend.Services.Interfaces
{
    public interface ILanguageProvider
    {
        Task<string?> GetLanguage();
        Task SetLanguage(string cultureName);
        string DefaultCulture => "en";
    }
}
