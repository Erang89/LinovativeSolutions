using Linovative.Shared.Interface;
using LinoVative.Service.Backend.Interfaces;
using LinoVative.Service.Core.Interfaces;

namespace LinoVative.Service.Backend.Extensions
{
    public static class ActorExtension
    {
        public static async Task<bool> CanDeleteEntity<T>(this IActor actor, T entity, IAppDbContext dbContext)
        {
            if (typeof(IsEntityManageByCompany).IsAssignableFrom(typeof(T)))
            {
                if(actor.CompanyId != ((IsEntityManageByCompany)entity!).CompanyId!.Value)
                    return false;
            }

            await Task.CompletedTask;
            return true;
        }


        public static async Task<bool> CanCreateEntity<T>(this IActor actor, IAppDbContext dbContext)
        {
            await Task.CompletedTask;
            return true;
        }

       
        public static async Task<bool> CanUpdateEntity<T>(this IActor actor, T entity, IAppDbContext dbContext)
        {
            if (typeof(IsEntityManageByCompany).IsAssignableFrom(typeof(T)))
            {
                if (actor.CompanyId != ((IsEntityManageByCompany)entity!).CompanyId!.Value)
                    return false;
            }

            await Task.CompletedTask;
            return true;
        }


        public static async Task<bool> IsAdministrator(this IActor actor, IAppDbContext? dbContext = default)
        {
            await Task.CompletedTask;
            return true;
        }

        
    }
}
