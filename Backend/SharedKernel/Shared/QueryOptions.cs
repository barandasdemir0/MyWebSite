using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SharedKernel.Shareds;

public class QueryOptions<T> where T :class
{
    public Expression<Func<T,object>>? OrderBy { get; set; }

    public bool Descending { get; set; } = true;
    public Expression<Func<T,object>>? ThenBy { get; set; }

    public bool ThenByDescending { get; set; } = false;

    public int? Take { get; set; }
    public int? Skip { get; set; }


}
