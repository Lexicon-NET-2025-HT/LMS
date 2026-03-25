using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Extensions;

internal static class QueryableExtensions
{
    /// <summary>
    /// Asynchronously retrieves a single page of items from the query and the total number of items available.
    /// </summary>
    /// <remarks>The method executes the query twice: once to count the total items and once to retrieve the
    /// page. The returned collection may be empty if the page is beyond the end of the results.</remarks>
    /// <typeparam name="T">The type of the elements in the query.</typeparam>
    /// <param name="query">The source query to paginate. Must not be null.</param>
    /// <param name="page">The one-based index of the page to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of items to include in each page. Must be greater than 0.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with the items for the
    /// specified page and the total count of items in the query.</returns>
    public static async Task<(IEnumerable<T> items, int totalCount)> PagedResult<T>(
        this IQueryable<T> query,
        int page,
        int pageSize)
    {
        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
