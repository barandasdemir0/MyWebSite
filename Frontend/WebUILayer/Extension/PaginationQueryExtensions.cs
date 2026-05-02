using Microsoft.AspNetCore.WebUtilities;
using SharedKernel.Shared;

namespace WebUILayer.Extension;

public static class PaginationQueryExtensions
{
    public static string ToQueryString(this PaginationQuery query,string baseUrl)
    {
        var queryParams = new Dictionary<string, string?>
        {
            { "PageNumber", query.PageNumber.ToString() },
            { "PageSize", query.PageSize.ToString() }
        };
        if (query.TopicId.HasValue)
        {
            queryParams.Add("TopicId", query.TopicId.ToString());
        }

        return QueryHelpers.AddQueryString(baseUrl, queryParams);
    }
}
