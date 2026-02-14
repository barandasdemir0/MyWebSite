using SharedKernel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Extensions;

public static class PagingExtensions
{
    public static PagedResult<T> ToPagedResult<T>(this List<T> items , int page , int size , int totalCount)
    {
        return new PagedResult<T>
        {
            Items = items,
            PageNumber = page,
            PageSize = size,
            TotalCount = totalCount
        };
    }

}
