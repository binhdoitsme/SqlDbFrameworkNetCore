using Dapper;
using Newtonsoft.Json;
using static Newtonsoft.Json.JsonConvert;
using Newtonsoft.Json.Linq;
using SqlDbFrameworkNetCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SqlDbFrameworkNetCore.Linq
{
    internal class JoinedQueryBuilder<TEntity, T1> 
        : SelectQueryBuilder<TEntity>, IJoinedQueryBuilder<TEntity, T1> where TEntity : class
    {
        public JoinedQueryBuilder(QueryBuilder queryStringBuilder) : base(queryStringBuilder) { }

        public IJoinedQueryBuilder<TEntity, T1> On(Expression<Func<TEntity, T1, bool>> predicate)
        {
            string onStr = ExpressionEvaluator.BuildOnConditionString(predicate);
            QueryStringBuilder.Append($"{onStr}");
            return this;
        }

        public IJoinedQueryBuilder<TEntity, T1> Where(Expression<Func<TEntity, T1, bool>> predicate)
        {
            string whereStr = ExpressionEvaluator.BuildWhereQueryString(predicate);
            RefactorAlias(predicate);
            QueryStringBuilder.Append($" {whereStr}");
            return this;
        }

        IJoinedQueryBuilder<TEntity, T1, T2> IJoinedQueryBuilder<TEntity, T1>.InnerJoin<T2>(string alias)
        {
            string typeName = typeof(T2).Name;
            QueryStringBuilder.Append($" INNER JOIN {StringToolkit.PascalToUnderscore(typeName)} {alias}");
            return new JoinedQueryBuilder<TEntity, T1, T2>(this);
        }

        IEnumerable<CompositeModel<TEntity, T1>> IJoinedQueryBuilder<TEntity, T1>.ExecuteQuery()
        {
            // get data from db 
            var queryResult = Connection.Query(this.ToString());

            // and map to attributes
            var result = ObjectMapper.ToCompositeObjectCollection<TEntity, T1>(queryResult);
            return result;
        }
    }

    internal class JoinedQueryBuilder<TEntity, T1, T2> 
        : JoinedQueryBuilder<TEntity, T1>, IJoinedQueryBuilder<TEntity, T1, T2> where TEntity : class
    {
        public JoinedQueryBuilder(QueryBuilder queryStringBuilder) : base(queryStringBuilder) { }

        public IJoinedQueryBuilder<TEntity, T1, T2> On(Expression<Func<TEntity, T1, T2, bool>> predicate)
        {
            string onStr = ExpressionEvaluator.BuildOnConditionString(predicate);
            QueryStringBuilder.Append($"{onStr}");
            return this;
        }

        public IJoinedQueryBuilder<TEntity, T1, T2> Where(Expression<Func<TEntity, T1, T2, bool>> predicate)
        {
            string whereStr = ExpressionEvaluator.BuildWhereQueryString(predicate);
            RefactorAlias(predicate);
            QueryStringBuilder.Append($" {whereStr}");
            return this;
        }

        IEnumerable<CompositeModel<TEntity, T1, T2>> IJoinedQueryBuilder<TEntity, T1, T2>.ExecuteQuery()
        {
            // get data from db 
            var queryResult = Connection.Query(this.ToString());

            // and map to attributes
            var result = ObjectMapper.ToCompositeObjectCollection<TEntity, T1, T2>(queryResult);
            return result;
        }

        IJoinedQueryBuilder<TEntity, T1, T2, T3> IJoinedQueryBuilder<TEntity, T1, T2>.InnerJoin<T3>(string alias)
        {
            string typeName = typeof(T3).Name;
            QueryStringBuilder.Append($" INNER JOIN {StringToolkit.PascalToUnderscore(typeName)} {alias}");
            return new JoinedQueryBuilder<TEntity, T1, T2, T3>(this);
        }
    }

    internal class JoinedQueryBuilder<TEntity, T1, T2, T3>
        : JoinedQueryBuilder<TEntity, T1, T2>, IJoinedQueryBuilder<TEntity, T1, T2, T3>
        where TEntity : class
    {
        public JoinedQueryBuilder(QueryBuilder queryStringBuilder) : base(queryStringBuilder) { }

        public IJoinedQueryBuilder<TEntity, T1, T2, T3> On(Expression<Func<TEntity, T1, T2, T3, bool>> predicate)
        {
            string onStr = ExpressionEvaluator.BuildOnConditionString(predicate);
            QueryStringBuilder.Append($" {onStr}");
            return this;
        }

        public IJoinedQueryBuilder<TEntity, T1, T2, T3> Where(Expression<Func<TEntity, T1, T2, T3, bool>> predicate)
        {
            string whereStr = ExpressionEvaluator.BuildWhereQueryString(predicate);
            RefactorAlias(predicate);
            QueryStringBuilder.Append($" {whereStr}");
            return this;
        }

        IEnumerable<CompositeModel<TEntity, T1, T2, T3>> IJoinedQueryBuilder<TEntity, T1, T2, T3>.ExecuteQuery()
        {
            // get data from db 
            var queryResult = Connection.Query(this.ToString());

            // and map to attributes
            var result = ObjectMapper.ToCompositeObjectCollection<TEntity, T1, T2, T3>(queryResult);
            return result;
        }

        IJoinedQueryBuilder<TEntity, T1, T2, T3, T4> IJoinedQueryBuilder<TEntity, T1, T2, T3>.InnerJoin<T4>(string alias)
        {
            throw new NotImplementedException();
        }
    }
}
