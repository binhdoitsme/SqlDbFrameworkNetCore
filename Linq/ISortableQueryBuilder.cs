﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SqlDbFrameworkNetCore.Linq
{
    public interface ISortableQueryBuilder<TEntity> : IFilterableQueryBuilder<TEntity>
    {
        ISortableQueryBuilder<TEntity> OrderBy(Expression<Func<TEntity, object>> key);
        ISortableQueryBuilder<TEntity> OrderByDescending(Expression<Func<TEntity, object>> key);
        ISortableQueryBuilder<TEntity> ThenBy(Expression<Func<TEntity, object>> key);
        ISortableQueryBuilder<TEntity> ThenByDescending(Expression<Func<TEntity, object>> key);

        new ISortableQueryBuilder<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        new ISortableQueryBuilder<TEntity> Limit(int count);
        new ISortableQueryBuilder<TEntity> Offset(int count);

        IEnumerable<TEntity> ExecuteQuery();
    }
}