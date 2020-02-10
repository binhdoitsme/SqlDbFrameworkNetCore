using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SqlDbFrameworkNetCore.Linq
{
    public interface ISortableQueryBuilder<TEntity> : IFilterableQueryBuilder<TEntity> where TEntity : class
    {
        ISortableQueryBuilder<TEntity> OrderBy(Expression<Func<TEntity, object>> key);
        ISortableQueryBuilder<TEntity> OrderByDescending(Expression<Func<TEntity, object>> key);
        ISortableQueryBuilder<TEntity> ThenBy(Expression<Func<TEntity, object>> key);
        ISortableQueryBuilder<TEntity> ThenByDescending(Expression<Func<TEntity, object>> key);

        ISortableQueryBuilder<TEntity> Where(string whereStr);
        new ISortableQueryBuilder<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        new ISortableQueryBuilder<TEntity> Limit(int count);
        new ISortableQueryBuilder<TEntity> Offset(int count);

        IEnumerable<TEntity> ExecuteQuery();
        Task<IEnumerable<TEntity>> ExecuteQueryAsync();
    }
}
