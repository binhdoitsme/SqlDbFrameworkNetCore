using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SqlDbFrameworkNetCore.Linq
{
    public interface IQueryBuilder
    {
        ISelectQueryBuilder<TEntity> Select<TEntity>(params Expression<Func<TEntity, object>>[] columns) where TEntity : class;
        ISelectQueryBuilder<TEntity> SelectCount<TEntity>(Expression<Func<TEntity, object>> columns = null) where TEntity : class;
        IUpdateQueryBuilder<TEntity> Update<TEntity>() where TEntity : class;
        IInsertQueryBuilder<TEntity> InsertInto<TEntity>() where TEntity : class;
        IDeleteQueryBuilder<TEntity> DeleteFrom<TEntity>() where TEntity : class;

        object ExecuteScalar();
        object ExecuteScalar(string sql);
        Task<object> ExecuteScalarAsync();
        Task<object> ExecuteScalarAsync(string sql);
        int ExecuteNonQuery();
        Task<int> ExecuteNonQueryAsync();
        int ExecuteNonQuery(string rawSql);
        Task<int> ExecuteNonQueryAsync(string rawSql);
        IEnumerable<TEntity> ExecuteQuery<TEntity>(string rawSql) where TEntity : class;
        Task<IEnumerable<TEntity>> ExecuteQueryAsync<TEntity>(string rawSql) where TEntity : class;

        void Clear();
    }

    public interface IQueryBuilder<TEntity> : IQueryBuilder where TEntity : class
    {
    }
}
