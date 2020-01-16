using Microsoft.CodeAnalysis.CSharp.Scripting;
using SqlDbFrameworkNetCore.Helpers;
using SqlDbFrameworkNetCore.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SqlDbFrameworkNetCore.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbConnection Connection;
        private readonly IQueryBuilder QueryBuilder;

        public Repository(DbConnection connection)
        {
            Connection = connection;
            if (Connection.State != ConnectionState.Open)
            {
                Connection.OpenAsync();
            }
            QueryBuilder = connection.CreateQueryBuilder();
        }

        public void Add(T item)
        {
            QueryBuilder.InsertInto<T>().Values(new T[] { item })
                        .ExecuteNonQuery();
        }

        public void AddRange(IEnumerable<T> items)
        {
            T[] addedItems = items.ToArray();
            QueryBuilder.InsertInto<T>().Values(addedItems)
                        .ExecuteNonQuery();
        }

        public IEnumerable<T> All()
        {
            return QueryBuilder.Select<T>().ExecuteQuery();
        }

        public bool Contains(T item)
        {
            string whereStr = $"WHERE {ObjectEvaluator.ToWhereString<T>(item)}";
            string queryStr = $"SELECT * " +
                            $"FROM {StringToolkit.PascalToUnderscore(typeof(T).Name)} " +
                            $"{whereStr}";
            return QueryBuilder.ExecuteQuery<T>(queryStr).Any();
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate)
        {
            return QueryBuilder.Select<T>().Where(predicate)
                                .ExecuteQuery();
        }

        public virtual T FindByKey(Expression<Func<T, object>> key, object value)
        {
            string leftHandSide = ExpressionEvaluator.BuildOrderByQueryString(key, false)
                                                    .Replace("ORDER BY ", "");
            string conditionStr = $"WHERE {leftHandSide} = {value}";
            string queryStr = $"SELECT * " +
                            $"FROM {StringToolkit.PascalToUnderscore(typeof(T).Name)} " +
                            $"WHERE {conditionStr} " +
                            $"LIMIT 1";
            return QueryBuilder.ExecuteQuery<T>(queryStr).FirstOrDefault();
        }

        public T FindFirst(Expression<Func<T, bool>> predicate)
        {
            return QueryBuilder.Select<T>().Where(predicate).Limit(1)
                    .ExecuteQuery()
                    .FirstOrDefault();
        }

        public void Remove(T item)
        {
            string queryStr = $"DELETE FROM {StringToolkit.PascalToUnderscore(typeof(T).Name)} " +
                $"WHERE {ObjectEvaluator.ToWhereString<T>(item)}";
            QueryBuilder.ExecuteQuery<T>(queryStr);
        }

        public void RemoveAll(Expression<Func<T, bool>> predicate)
        {
            QueryBuilder.DeleteFrom<T>().Where(predicate);
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            string queryStr = $"DELETE FROM {StringToolkit.PascalToUnderscore(typeof(T).Name)} " +
                $"WHERE {ObjectEvaluator.ToWhereString<T>(items)}";
            QueryBuilder.ExecuteQuery<T>(queryStr);
        }

        public void Set(Expression<Func<T, bool>> predicate, object newValue)
        {
            QueryBuilder.Update<T>().Set(newValue).Where(predicate).ExecuteNonQuery();
        }

        public void Set(T oldValue, T newValue)
        {
            string setStr = $"SET {ObjectEvaluator.ToWhereString<T>(newValue).Replace(" AND ", ", \n")}";
            string whereStr = $"WHERE {ObjectEvaluator.ToWhereString<T>(oldValue)}";
            string queryStr = $"UPDATE {StringToolkit.PascalToUnderscore(typeof(T).Name)} " +
                                $"{setStr} {whereStr}";
            QueryBuilder.ExecuteNonQuery(queryStr);
        }
    }
}
