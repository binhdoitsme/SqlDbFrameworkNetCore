using Dapper;
using SqlDbFrameworkNetCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SqlDbFrameworkNetCore.Linq
{
    internal class SortableQueryBuilder<TEntity>
        : FilterableQueryBuilder<TEntity>, ISortableQueryBuilder<TEntity> where TEntity : class
    {
        internal SortableQueryBuilder(QueryBuilder builder) : base(builder) { }

        public ISortableQueryBuilder<TEntity> OrderBy(Expression<Func<TEntity, object>> key)
        {
            string order = ExpressionEvaluator.BuildOrderByQueryString(key, false);
            QueryStringBuilder.Append($" {order}");
            return this;
        }

        public ISortableQueryBuilder<TEntity> OrderByDescending(Expression<Func<TEntity, object>> key)
        {
            string order = ExpressionEvaluator.BuildOrderByQueryString(key, true);
            QueryStringBuilder.Append($" {order}");
            return this;
        }

        public ISortableQueryBuilder<TEntity> ThenBy(Expression<Func<TEntity, object>> key)
        {
            string order = ExpressionEvaluator.BuildOrderByQueryString(key, false)
                            .Replace("ORDER BY", "THEN BY");
            QueryStringBuilder.Append($" {order}");
            return this;
        }

        public ISortableQueryBuilder<TEntity> ThenByDescending(Expression<Func<TEntity, object>> key)
        {
            string order = ExpressionEvaluator.BuildOrderByQueryString(key, true)
                            .Replace("ORDER BY", "THEN BY");
            QueryStringBuilder.Append($" {order}");
            return this;
        }

        ISortableQueryBuilder<TEntity> ISortableQueryBuilder<TEntity>.Where(Expression<Func<TEntity, bool>> predicate)
        {
            string whereString = ExpressionEvaluator.BuildWhereQueryString(predicate);
            RefactorAlias(predicate);
            QueryStringBuilder.Append($" {whereString}");
            return this;
        }

        ISortableQueryBuilder<TEntity> ISortableQueryBuilder<TEntity>.Limit(int count)
        {
            QueryStringBuilder.Append($" LIMIT {count}");
            return this;
        }

        ISortableQueryBuilder<TEntity> ISortableQueryBuilder<TEntity>.Offset(int count)
        {
            QueryStringBuilder.Append($" OFFSET {count}");
            return this;
        }

        public IEnumerable<TEntity> ExecuteQuery()
        {
            Console.WriteLine(this.ToString());
            var rawResult = Connection.Query(this.ToString());
            IEnumerable <TEntity> result = ObjectMapper.ToObjectCollection<TEntity>(rawResult);
            Clear();
            return result;
        }

        public async Task<IEnumerable<TEntity>> ExecuteQueryAsync()
        {
            Console.WriteLine(this.ToString());
            var rawResult = await Connection.QueryAsync(this.ToString());
            IEnumerable<TEntity> result = ObjectMapper.ToObjectCollection<TEntity>(rawResult);
            Clear();
            return result;
        }
    }
}
