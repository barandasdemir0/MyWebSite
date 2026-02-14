using System;
using System.Collections.Generic;
using System.Text;

namespace SharedKernel.Shared;

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new List<T>();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages  => (int)Math.Ceiling((double)TotalCount / PageSize);
    //total countu pagesize a böl ve onu yuvarla  yukarı doğru

}
