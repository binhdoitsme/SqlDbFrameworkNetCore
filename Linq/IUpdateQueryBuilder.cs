using System;
using System.Collections.Generic;
using System.Text;

namespace SqlDbFrameworkNetCore.Linq
{
    public interface IUpdateQueryBuilder<TEntity> : IFilterableQueryBuilder<TEntity>
    {
        IFilterableQueryBuilder<TEntity> Set(dynamic newValues);
    }
}
