using System;
using System.Collections.Generic;
using System.Text;

namespace SqlDbFrameworkNetCore.Linq
{
    public interface IDeleteQueryBuilder<TEntity> : IFilterableQueryBuilder<TEntity>
    {
    }
}
