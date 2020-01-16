using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;

namespace SqlDbFrameworkNetCore.Linq
{
    public interface IQueryBuilder
    {
        ISelectQueryBuilder<TEntity> Select<TEntity>(params Expression<Func<TEntity, object>>[] columns);
        IUpdateQueryBuilder<TEntity> Update<TEntity>();
        IInsertQueryBuilder<TEntity> InsertInto<TEntity>();
        IDeleteQueryBuilder<TEntity> DeleteFrom<TEntity>();

        int ExecuteNonQuery();
        int ExecuteNonQuery(string rawSql);
        IEnumerable<TEntity> ExecuteQuery<TEntity>(string rawSql);
        void Clear();
    }

    public interface IQueryBuilder<TEntity> : IQueryBuilder
    {
    }
}
