using System;
using System.Linq.Expressions;

namespace SqlDbFrameworkNetCore.Linq
{
    public interface IFilterableQueryBuilder<TEntity> : IQueryBuilder<TEntity> where TEntity : class
    {
        IFilterableQueryBuilder<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        IFilterableQueryBuilder<TEntity> Limit(int count);
        IFilterableQueryBuilder<TEntity> Offset(int count);
    }
}
