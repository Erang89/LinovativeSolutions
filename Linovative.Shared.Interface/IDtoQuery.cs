namespace Linovative.Shared.Interface
{
    public interface IDtoQuery
    {
        public IQueryable GetAll<T>() where T : IDto;
    }
}
