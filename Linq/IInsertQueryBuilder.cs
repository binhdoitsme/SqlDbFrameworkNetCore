using System;
using System.Collections.Generic;
using System.Text;

namespace SqlDbFrameworkNetCore.Linq
{
    public interface IInsertQueryBuilder<TEntity> : IQueryBuilder<TEntity> where TEntity : class
    {
        IQueryBuilder<TEntity> Values(TEntity[] items);
    }
}
