using SqlDbFrameworkNetCore.Helpers;
using SqlDbFrameworkNetCore.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SqlDbFrameworkNetCore.Repositories.Asynchronous
{
    public class Repository : IRepository
    {
        private readonly DbConnection Connection;
        public readonly IQueryBuilder QueryBuilder;

        public Repository(DbConnection connection)
        {
            Connection = connection;
            if (Connection.State != ConnectionState.Open)
            {
                Connection.OpenAsync();
            }
            QueryBuilder = connection.CreateQueryBuilder();
        }

        public Task Add<T>(T item) where T : class
        {
            return QueryBuilder.InsertInto<T>().Values(new T[] { item })
                        .ExecuteNonQueryAsync();
        }

        public Task AddRange<T>(IEnumerable<T> items) where T : class
        {
            T[] addedItems = items.ToArray();
            return QueryBuilder.InsertInto<T>().Values(addedItems)
                        .ExecuteNonQueryAsync();
        }

        public Task<IEnumerable<T>> All<T>() where T : class
        {
            return QueryBuilder.Select<T>().ExecuteQueryAsync();
        }

        public async Task<bool> Contains<T>(T item) where T : class
        {
            string whereStr = $"WHERE {ObjectEvaluator.ToWhereString<T>(item)}";
            string queryStr = $"SELECT * " +
                            $"FROM {StringToolkit.PascalToUnderscore(typeof(T).Name)} " +
                            $"{whereStr}";
            return (await QueryBuilder.ExecuteQueryAsync<T>(queryStr)).Any();
        }

        public async Task<long> Count<T>(Expression<Func<T, object>> column = null) where T : class
        {
            QueryBuilder.SelectCount<T>(column);
            return (long)(await QueryBuilder.ExecuteScalarAsync());
        }

        public Task<IEnumerable<T>> FindAll<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return QueryBuilder.Select<T>().Where(predicate)
                                .ExecuteQueryAsync();
        }

        public virtual async Task<T> FindByKey<T>(Expression<Func<T, object>> key, object value) where T : class
        {
            string leftHandSide = ExpressionEvaluator.BuildOrderByQueryString(key, false)
                                                    .Replace("ORDER BY ", "");
            string conditionStr = $"WHERE {leftHandSide} = {value}";
            string queryStr = $"SELECT * " +
                            $"FROM {StringToolkit.PascalToUnderscore(typeof(T).Name)} " +
                            $"WHERE {conditionStr} " +
                            $"LIMIT 1";
            return (await QueryBuilder.ExecuteQueryAsync<T>(queryStr)).FirstOrDefault();
        }

        public async Task<T> FindFirst<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return (await QueryBuilder.Select<T>().Where(predicate).Limit(1)
                    .ExecuteQueryAsync())
                    .FirstOrDefault();
        }

        public Task Remove<T>(T item) where T : class
        {
            string queryStr = $"DELETE FROM {StringToolkit.PascalToUnderscore(typeof(T).Name)} " +
                $"WHERE {ObjectEvaluator.ToWhereString<T>(item)}";
            return QueryBuilder.ExecuteQueryAsync<T>(queryStr);
        }

        public Task RemoveAll<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return QueryBuilder.DeleteFrom<T>().Where(predicate).ExecuteNonQueryAsync();
        }

        public Task RemoveRange<T>(IEnumerable<T> items) where T : class
        {
            string queryStr = $"DELETE FROM {StringToolkit.PascalToUnderscore(typeof(T).Name)} " +
                $"WHERE {ObjectEvaluator.ToWhereString<T>(items)}";
            return QueryBuilder.ExecuteQueryAsync<T>(queryStr);
        }

        public Task Set<T>(Expression<Func<T, bool>> predicate, object newValue) where T : class
        {
            return QueryBuilder.Update<T>().Set(newValue).Where(predicate).ExecuteNonQueryAsync();
        }

        public Task Set<T>(T oldValue, T newValue) where T : class
        {
            string setStr = $"SET {ObjectEvaluator.ToWhereString<T>(newValue).Replace(" AND ", ", \n")}";
            string whereStr = $"WHERE {ObjectEvaluator.ToWhereString<T>(oldValue)}";
            string queryStr = $"UPDATE {StringToolkit.PascalToUnderscore(typeof(T).Name)} " +
                                $"{setStr} {whereStr}";
            return QueryBuilder.ExecuteNonQueryAsync(queryStr);
        }

        public async Task<T> Retrieve<T>(T item) where T : class
        {
            string whereStr = $"WHERE {ObjectEvaluator.ToWhereString<T>(item)}";
            var result = await QueryBuilder.Select<T>().Where(whereStr).ExecuteQueryAsync();
            return result.FirstOrDefault();
        }

        public Task<IEnumerable<T>> RetrieveRange<T>(IEnumerable<T> items) where T : class
        {
            string whereConditions = items.Select(item => $"({ObjectEvaluator.ToWhereString<T>(item)}) OR")
                                            .Aggregate((i1, i2) => $"{i1} {i2}").Trim();
            Regex rg = new Regex(" OR$");
            string whereStr = $"WHERE {rg.Replace(whereConditions, "")}";
            return QueryBuilder.Select<T>().Where(whereStr).ExecuteQueryAsync();
        }
    }

    public partial class Repository<T> : Repository, IRepository<T> where T : class
    {
        public Repository(DbConnection connection) : base(connection)
        {
        }

        public Task Add(T item)
        {
            return base.Add(item);
        }

        public Task AddRange(IEnumerable<T> items)
        {
            return base.AddRange(items);    
        }

        public Task<IEnumerable<T>> All()
        {
            return base.All<T>();
        }

        public Task<bool> Contains(T item)
        {
            return base.Contains(item);
        }

        public Task<long> Count(Expression<Func<T, object>> column = null)
        {
            return base.Count<T>(column);
        }

        public Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> predicate)
        {
            return base.FindAll(predicate);
        }

        public virtual Task<T> FindByKey(Expression<Func<T, object>> key, object value)
        {
            return base.FindByKey(key, value);
        }

        public Task<T> FindFirst(Expression<Func<T, bool>> predicate)
        {
            return base.FindFirst(predicate);
        }

        public Task Remove(T item)
        {
            return base.Remove(item);
        }

        public Task RemoveAll(Expression<Func<T, bool>> predicate)
        {
            return base.RemoveAll(predicate);
        }

        public Task RemoveRange(IEnumerable<T> items)
        {
            return base.RemoveRange(items);
        }

        public Task Set(Expression<Func<T, bool>> predicate, object newValue)
        {
            return base.Set(predicate, newValue);
        }

        public Task Set(T oldValue, T newValue)
        {
            return base.Set(oldValue, newValue);
        }

        public Task<T> Retrieve(T item)
        {
            return base.Retrieve(item);
        }

        public Task<IEnumerable<T>> RetrieveRange(IEnumerable<T> items)
        {
            return base.RetrieveRange(items);
        }
    }
}
