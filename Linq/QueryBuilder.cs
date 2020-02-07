using Dapper;
using SqlDbFrameworkNetCore.Helpers;
using SqlDbFrameworkNetCore.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SqlDbFrameworkNetCore.Linq
{
    public class QueryBuilder : IQueryBuilder
    {
        protected StringBuilder QueryStringBuilder;
        internal DbConnection Connection { get; set; }
        protected DbParameterCollection ParameterCollection;

        public QueryBuilder()
        {
            QueryStringBuilder = new StringBuilder();
        }

        internal QueryBuilder(DbConnection connection) : this()
        {
            Connection = connection;
        }

        protected QueryBuilder(QueryBuilder builder)
        {
            this.QueryStringBuilder = builder.QueryStringBuilder;
            this.Connection = builder.Connection;
            this.ParameterCollection = builder.ParameterCollection;
        }

        public IDeleteQueryBuilder<TEntity> DeleteFrom<TEntity>() where TEntity : class
        {
            QueryStringBuilder.Append($"DELETE FROM " +
                $"{StringToolkit.PascalToUnderscore(typeof(TEntity).Name)}");
            return new DeleteQueryBuilder<TEntity>(this);
        }

        public IInsertQueryBuilder<TEntity> InsertInto<TEntity>() where TEntity : class
        {
            QueryStringBuilder.Append($"INSERT INTO " +
                $"{StringToolkit.PascalToUnderscore(typeof(TEntity).Name)} " +
                $"{PropertyToolkit.BuildInsertString(PropertyToolkit.GetInsertProperties<TEntity>())}");
            return new InsertQueryBuilder<TEntity>(this);
        }

        public ISelectQueryBuilder<TEntity> Select<TEntity>(params Expression<Func<TEntity, object>>[] columns)
            where TEntity : class
        {
            string columnStr;
            if (columns.Length == 0)
            {
                columnStr = "*";
            }
            else
            {
                string[] clmCpy = new string[columns.Length];
                for (int i = 0; i < columns.Length; i++)
                {
                    clmCpy[i] = $"{ExpressionEvaluator.BuildOrderByQueryString(columns[i], false).Replace("ORDER BY", "")}"
                                .Trim();
                }
                columnStr = string.Join(", ", clmCpy)
                                .Replace("[", "(")
                                .Replace("]", ")");
            }
            QueryStringBuilder.Append($"SELECT {columnStr} " +
                $"FROM {StringToolkit.PascalToUnderscore(typeof(TEntity).Name)}");
            return new SelectQueryBuilder<TEntity>(this);
        }

        public ISelectQueryBuilder<TEntity> SelectCount<TEntity>(Expression<Func<TEntity, object>> columns = null)
            where TEntity : class
        {
            QueryStringBuilder.Append("SELECT COUNT(")
                .Append((columns == null ? "*"
                : $"{StringToolkit.PascalToUnderscore(((columns.Body as BinaryExpression).Right as MemberExpression).Member.Name)}"))
                .Append(")")
                .Append($" FROM {StringToolkit.PascalToUnderscore(typeof(TEntity).Name)}");
            return new SelectQueryBuilder<TEntity>(this);
        }

        public IUpdateQueryBuilder<TEntity> Update<TEntity>()
            where TEntity : class
        {
            QueryStringBuilder.Append($"UPDATE {StringToolkit.PascalToUnderscore(typeof(TEntity).Name)}");
            return new UpdateQueryBuilder<TEntity>(this);
        }

        public override string ToString()
        {
            return QueryStringBuilder.ToString();
        }

        public void Clear()
        {
            QueryStringBuilder.Clear();
        }

        public virtual int ExecuteNonQuery()
        {
            Console.WriteLine(this.ToString());
            DbCommand cmd = Connection.CreateCommand();
            if (ParameterCollection != null)
            {
                foreach (var param in ParameterCollection)
                {
                    cmd.Parameters.Add(param);
                }
            }
            cmd.CommandText = ToString();
            int affectedRows = cmd.ExecuteNonQuery();

            // clear this after query finishes
            Clear();

            return affectedRows;
        }

        public virtual async Task<int> ExecuteNonQueryAsync()
        {
            Console.WriteLine(this.ToString());
            DbCommand cmd = Connection.CreateCommand();
            if (ParameterCollection != null)
            {
                foreach (var param in ParameterCollection)
                {
                    cmd.Parameters.Add(param);
                }
            }
            cmd.CommandText = ToString();
            int affectedRows = await cmd.ExecuteNonQueryAsync();

            // clear this after query finishes
            Clear();

            return affectedRows;
        }

        public virtual int ExecuteNonQuery(string rawSql)
        {
            Console.WriteLine(rawSql);
            DbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = rawSql;
            return cmd.ExecuteNonQuery();
        }

        public virtual Task<int> ExecuteNonQueryAsync(string rawSql)
        {
            Console.WriteLine(rawSql);
            DbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = rawSql;
            return cmd.ExecuteNonQueryAsync();
        }

        public virtual IEnumerable<T> ExecuteQuery<T>(string rawSql) where T : class
        {
            Console.WriteLine(rawSql);
            return ObjectMapper.ToObjectCollection<T>(Connection.Query(rawSql));
        }

        public virtual async Task<IEnumerable<T>> ExecuteQueryAsync<T>(string rawSql) where T : class
        {
            Console.WriteLine(rawSql);
            return ObjectMapper.ToObjectCollection<T>(await Connection.QueryAsync(rawSql));
        }

        public object ExecuteScalar()
        {
            return ExecuteScalar(this.ToString());
        }

        public Task<object> ExecuteScalarAsync()
        {
            return ExecuteScalarAsync(this.ToString());
        }

        public object ExecuteScalar(string rawSql)
        {
            Console.WriteLine(rawSql);
            DbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = rawSql;
            Clear();
            return cmd.ExecuteScalar();
        }

        public Task<object> ExecuteScalarAsync(string rawSql)
        {
            Console.WriteLine(rawSql);
            DbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = rawSql;
            Clear();
            return cmd.ExecuteScalarAsync();
        }
    }

    class QueryBuilder<TEntity> : QueryBuilder, IQueryBuilder<TEntity> where TEntity : class
    {
        public QueryBuilder(QueryBuilder builder) : base(builder) 
        {
            
        }

        public virtual IEnumerable<TEntity> ExecuteQuery(string rawSql)
        {
            Console.WriteLine(rawSql);
            return Connection.Query<TEntity>(rawSql);
        }

        public virtual async Task<IEnumerable<TEntity>> ExecuteQueryAsync(string rawSql)
        {
            Console.WriteLine(rawSql);
            return await Connection.QueryAsync<TEntity>(rawSql);
        }
    }
}
