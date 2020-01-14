using System;
using System.Collections.Generic;
using System.Text;

namespace SqlDbFrameworkNetCore.Linq
{
    public interface IInsertQueryBuilder<TEntity> : IQueryBuilder<TEntity>
    {
        IQueryBuilder<TEntity> Values(TEntity[] items);
    }
}
