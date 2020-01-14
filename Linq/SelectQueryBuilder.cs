using Dapper;
using SqlDbFrameworkNetCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SqlDbFrameworkNetCore.Linq
{
    internal class SelectQueryBuilder<TEntity> : SortableQueryBuilder<TEntity>, ISelectQueryBuilder<TEntity>
    {
        internal SelectQueryBuilder(QueryBuilder queryStringBuilder) : base(queryStringBuilder) { }

        public ISelectQueryBuilder<TEntity> GroupBy(Expression<Func<TEntity, object>> key)
        {
            return this;
        }

        public ISelectQueryBuilder<TEntity> Having(Expression<Func<TEntity, bool>> predicate)
        {
            string havingStr = ExpressionEvaluator.BuildWhereQueryString(predicate).Replace("WHERE", "HAVING");
            QueryStringBuilder.Append($" {havingStr}");
            return this;
        }

        public IJoinedQueryBuilder<TEntity, T> InnerJoin<T>(string alias)
        {
            string typeName = typeof(T).Name;
            QueryStringBuilder.Append($" INNER JOIN {StringToolkit.PascalToUnderscore(typeName)} {alias} ");
            return new JoinedQueryBuilder<TEntity, T>(this);
        }
    }
}
