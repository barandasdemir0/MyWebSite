using System;
using System.Collections.Generic;
using System.Text;

namespace SharedKernel.Shared;

public class PaginationQuery
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public Guid? TopicId { get; set; }
}
