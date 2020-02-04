using SqlDbFrameworkNetCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SqlDbFrameworkNetCore.Repositories
{
    public partial class Repository : IRepository
    {
        public async Task AddAsync<T>(T item) where T : class
        {
            await QueryBuilder.InsertInto<T>().Values(new T[] { item })
                        .ExecuteNonQueryAsync();
        }

        public async Task AddRangeAsync<T>(IEnumerable<T> items) where T : class
        {
            T[] addedItems = items.ToArray();
            await QueryBuilder.InsertInto<T>().Values(addedItems)
                        .ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<T>> AllAsync<T>() where T : class
        {
            return await QueryBuilder.Select<T>().ExecuteQueryAsync();
        }

        public async Task<bool> ContainsAsync<T>(T item) where T : class
        {
            string whereStr = $"WHERE {ObjectEvaluator.ToWhereString<T>(item)}";
            string queryStr = $"SELECT * " +
                            $"FROM {StringToolkit.PascalToUnderscore(typeof(T).Name)} " +
                            $"{whereStr}";
            return (await QueryBuilder.ExecuteQueryAsync<T>(queryStr)).Any();
        }

        public async Task<IEnumerable<T>> FindAllAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await QueryBuilder.Select<T>().Where(predicate)
                                .ExecuteQueryAsync();
        }

        public virtual async Task<T> FindByKeyAsync<T>(Expression<Func<T, object>> key, object value) where T : class
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

        public async Task<T> FindFirstAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return (await QueryBuilder.Select<T>().Where(predicate).Limit(1)
                    .ExecuteQueryAsync())
                    .FirstOrDefault();
        }

        public async Task RemoveAsync<T>(T item) where T : class
        {
            string queryStr = $"DELETE FROM {StringToolkit.PascalToUnderscore(typeof(T).Name)} " +
                $"WHERE {ObjectEvaluator.ToWhereString<T>(item)}";
            await QueryBuilder.ExecuteQueryAsync<T>(queryStr);
        }

        public async Task RemoveAllAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            await QueryBuilder.DeleteFrom<T>().Where(predicate).ExecuteNonQueryAsync();
        }

        public async Task RemoveRangeAsync<T>(IEnumerable<T> items) where T : class
        {
            string queryStr = $"DELETE FROM {StringToolkit.PascalToUnderscore(typeof(T).Name)} " +
                $"WHERE {ObjectEvaluator.ToWhereString<T>(items)}";
            await QueryBuilder.ExecuteQueryAsync<T>(queryStr);
        }

        public async Task SetAsync<T>(Expression<Func<T, bool>> predicate, object newValue) where T : class
        {
            await QueryBuilder.Update<T>().Set(newValue).Where(predicate).ExecuteNonQueryAsync();
        }

        public async Task SetAsync<T>(T oldValue, T newValue) where T : class
        {
            string setStr = $"SET {ObjectEvaluator.ToWhereString<T>(newValue).Replace(" AND ", ", \n")}";
            string whereStr = $"WHERE {ObjectEvaluator.ToWhereString<T>(oldValue)}";
            string queryStr = $"UPDATE {StringToolkit.PascalToUnderscore(typeof(T).Name)} " +
                                $"{setStr} {whereStr}";
            await QueryBuilder.ExecuteNonQueryAsync(queryStr);
        }
    }

    public partial class Repository<T> : Repository, IRepository<T> where T : class
    {
        public async Task AddAsync(T item)
        {
            await base.AddAsync(item);
        }

        public async Task AddRangeAsync(IEnumerable<T> items)
        {
            await base.AddRangeAsync(items);    
        }

        public async Task<IEnumerable<T>> AllAsync()
        {
            return await base.AllAsync<T>();
        }

        public async Task<bool> ContainsAsync(T item)
        {
            return await base.ContainsAsync(item);
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await base.FindAllAsync(predicate);
        }

        public virtual async Task<T> FindByKeyAsync(Expression<Func<T, object>> key, object value)
        {
            return await base.FindByKeyAsync(key, value);
        }

        public async Task<T> FindFirstAsync(Expression<Func<T, bool>> predicate)
        {
            return await base.FindFirstAsync(predicate);
        }

        public async Task RemoveAsync(T item)
        {
            await base.RemoveAsync(item);
        }

        public async Task RemoveAllAsync(Expression<Func<T, bool>> predicate)
        {
            await base.RemoveAllAsync(predicate);
        }

        public async Task RemoveRangeAsync(IEnumerable<T> items)
        {
            await base.RemoveRangeAsync(items);
        }

        public async Task SetAsync(Expression<Func<T, bool>> predicate, object newValue)
        {
            await base.SetAsync(predicate, newValue);
        }

        public async Task SetAsync(T oldValue, T newValue)
        {
            await base.SetAsync(oldValue, newValue);
        }
    }
}
