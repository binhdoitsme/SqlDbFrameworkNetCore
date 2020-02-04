using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SqlDbFrameworkNetCore.Linq
{
    public interface ISelectQueryBuilder<TEntity> : ISortableQueryBuilder<TEntity> where TEntity : class
    {
        ISelectQueryBuilder<TEntity> GroupBy(Expression<Func<TEntity, object>> key);
        ISelectQueryBuilder<TEntity> Having(Expression<Func<TEntity, bool>> predicate);

        IJoinedQueryBuilder<TEntity, T> InnerJoin<T>(string alias);
    }
}
