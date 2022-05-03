using System.Linq.Expressions;

public interface ISearch<T>
{
    IEnumerable<T>? ApplySearch(IEnumerable<T>? entities, string? searchString);
}