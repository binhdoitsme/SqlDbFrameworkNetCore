using SqlDbFrameworkNetCore.Helpers;
using SqlDbFrameworkNetCore.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace SqlDbFrameworkNetCore.Repositories
{
    public partial class Repository : IRepository
    {
        private readonly DbConnection Connection;
        protected readonly IQueryBuilder QueryBuilder;

        public Repository(DbConnection connection)
        {
            Connection = connection;
            if (Connection.State != ConnectionState.Open)
            {
                Connection.OpenAsync();
            }
            QueryBuilder = connection.CreateQueryBuilder();
        }

        public void Add<T>(T item) where T : class
        {
            QueryBuilder.InsertInto<T>().Values(new T[] { item })
                        .ExecuteNonQuery();
        }

        public void AddRange<T>(IEnumerable<T> items) where T : class
        {
            T[] addedItems = items.ToArray();
            QueryBuilder.InsertInto<T>().Values(addedItems)
                        .ExecuteNonQuery();
        }

        public IEnumerable<T> All<T>() where T : class
        {
            return QueryBuilder.Select<T>().ExecuteQuery();
        }

        public bool Contains<T>(T item) where T : class
        {
            string whereStr = $"WHERE {ObjectEvaluator.ToWhereString<T>(item)}";
            string queryStr = $"SELECT * " +
                            $"FROM {StringToolkit.PascalToUnderscore(typeof(T).Name)} " +
                            $"{whereStr}";
            return QueryBuilder.ExecuteQuery<T>(queryStr).Any();
        }

        public long Count<T>(Expression<Func<T, object>> column = null) where T : class
        {
            QueryBuilder.SelectCount(column);
            return (long)QueryBuilder.ExecuteScalar();
        }

        public IEnumerable<T> FindAll<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return QueryBuilder.Select<T>().Where(predicate)
                                .ExecuteQuery();
        }

        public virtual T FindByKey<T>(Expression<Func<T, object>> key, object value) where T : class
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

        public T FindFirst<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return QueryBuilder.Select<T>().Where(predicate).Limit(1)
                    .ExecuteQuery()
                    .FirstOrDefault();
        }

        public void Remove<T>(T item) where T : class
        {
            string queryStr = $"DELETE FROM {StringToolkit.PascalToUnderscore(typeof(T).Name)} " +
                $"WHERE {ObjectEvaluator.ToWhereString<T>(item)}";
            QueryBuilder.ExecuteQuery<T>(queryStr);
        }

        public void RemoveAll<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            QueryBuilder.DeleteFrom<T>().Where(predicate).ExecuteNonQuery();
        }

        public void RemoveRange<T>(IEnumerable<T> items) where T : class
        {
            string queryStr = $"DELETE FROM {StringToolkit.PascalToUnderscore(typeof(T).Name)} " +
                $"WHERE {ObjectEvaluator.ToWhereString<T>(items)}";
            QueryBuilder.ExecuteQuery<T>(queryStr);
        }

        public void Set<T>(Expression<Func<T, bool>> predicate, object newValue) where T : class
        {
            QueryBuilder.Update<T>().Set(newValue).Where(predicate).ExecuteNonQuery();
        }

        public void Set<T>(T oldValue, T newValue) where T : class
        {
            string setStr = $"SET {ObjectEvaluator.ToWhereString<T>(newValue).Replace(" AND ", ", \n")}";
            string whereStr = $"WHERE {ObjectEvaluator.ToWhereString<T>(oldValue)}";
            string queryStr = $"UPDATE {StringToolkit.PascalToUnderscore(typeof(T).Name)} " +
                                $"{setStr} {whereStr}";
            QueryBuilder.ExecuteNonQuery(queryStr);
        }

        public T Retrieve<T>(T item) where T : class
        {
            string whereStr = $"WHERE {ObjectEvaluator.ToWhereString<T>(item)}";
            var result = QueryBuilder.Select<T>().Where(whereStr).ExecuteQuery();
            return result.FirstOrDefault();
        }

        public IEnumerable<T> RetrieveRange<T>(IEnumerable<T> items) where T : class
        {
            string whereConditions = items.Select(item => $"({ObjectEvaluator.ToWhereString<T>(item)}) OR")
                                            .Aggregate((i1, i2) => $"{i1} {i2}").Trim();
            Regex rg = new Regex(" OR$");
            string whereStr = $"WHERE {rg.Replace(whereConditions, "")}";
            return QueryBuilder.Select<T>().Where(whereStr).ExecuteQuery();
        }
    }

    public partial class Repository<T> : Repository, IRepository<T> where T : class
    {
        public Repository(DbConnection connection) : base(connection)
        {
        }

        public void Add(T item)
        {
            base.Add(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            base.AddRange(items);
        }

        public IEnumerable<T> All()
        {
            return base.All<T>();
        }

        public bool Contains(T item)
        {
            return base.Contains(item);
        }

        public long Count(Expression<Func<T, object>> column = null)
        {
            return base.Count<T>(column);
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate)
        {
            return QueryBuilder.Select<T>().Where(predicate)
                                .ExecuteQuery();
        }

        public virtual T FindByKey(Expression<Func<T, object>> key, object value)
        {
            return base.FindByKey(key, value);
        }

        public T FindFirst(Expression<Func<T, bool>> predicate)
        {
            return base.FindFirst(predicate);
        }

        public void Remove(T item)
        {
            base.Remove(item);
        }

        public void RemoveAll(Expression<Func<T, bool>> predicate)
        {
            base.RemoveAll(predicate);
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            base.RemoveRange(items);
        }

        public void Set(Expression<Func<T, bool>> predicate, object newValue)
        {
            base.Set(predicate, newValue);
        }

        public void Set(T oldValue, T newValue)
        {
            base.Set(oldValue, newValue);
        }

        public T Retrieve(T item)
        {
            return base.Retrieve(item);
        }

        public IEnumerable<T> RetrieveRange(T[] items)
        {
            return base.Retrieve(items);
        }
    }
}
