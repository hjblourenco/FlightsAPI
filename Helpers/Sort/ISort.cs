public interface ISort<T>
{
    IQueryable<T> ApplySort(IQueryable<T> entities, string orderByQueryString);
}