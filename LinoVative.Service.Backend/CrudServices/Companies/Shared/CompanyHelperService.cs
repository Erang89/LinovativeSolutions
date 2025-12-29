using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Companies;
using Microsoft.EntityFrameworkCore;

namespace LinoVative.Service.Backend.CrudServices.Companies.Shared
{
    public interface ICompanyHelperService
    {
        Task<CompanySetting?> GetCurrenctCompanySettings();
    }

    public class CompanyHelperService : ICompanyHelperService
    {
        private readonly IActor _actor;
        private readonly IAppDbContext _dbContext;

        public CompanyHelperService(IActor actor, IAppDbContext dbContext)
        {
            _dbContext = dbContext;
            _actor = actor;
        }


        public async Task<CompanySetting?> GetCurrenctCompanySettings() => await _dbContext.CompanySettings.Where(x => x.CompanyId == _actor.CompanyId).FirstOrDefaultAsync();
    }
}
