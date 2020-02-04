using System;
using System.Collections.Generic;
using System.Text;

namespace SqlDbFrameworkNetCore.Linq
{
    public interface IUpdateQueryBuilder<TEntity> : IFilterableQueryBuilder<TEntity> where TEntity : class
    {
        IFilterableQueryBuilder<TEntity> Set(dynamic newValues);
    }
}
