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

        public IDeleteQueryBuilder<TEntity> DeleteFrom<TEntity>()
        {
            QueryStringBuilder.Append($"DELETE FROM " +
                $"{StringToolkit.PascalToUnderscore(typeof(TEntity).Name)}");
            return new DeleteQueryBuilder<TEntity>(this);
        }

        public IInsertQueryBuilder<TEntity> InsertInto<TEntity>()
        {
            QueryStringBuilder.Append($"INSERT INTO " +
                $"{StringToolkit.PascalToUnderscore(typeof(TEntity).Name)} " +
                $"{PropertyToolkit.BuildInsertString(PropertyToolkit.GetInsertProperties<TEntity>())}");
            return new InsertQueryBuilder<TEntity>(this);
        }

        public ISelectQueryBuilder<TEntity> Select<TEntity>(params Expression<Func<TEntity, object>>[] columns)
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

        public IUpdateQueryBuilder<TEntity> Update<TEntity>()
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

        public virtual int ExecuteNonQuery(string rawSql)
        {
            Console.WriteLine(rawSql);
            DbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = rawSql;
            return cmd.ExecuteNonQuery();
        }

        public virtual IEnumerable<T> ExecuteQuery<T>(string rawSql)
        {
            Console.WriteLine(rawSql);
            return Connection.Query<T>(rawSql);
        }
    }

    class QueryBuilder<TEntity> : QueryBuilder, IQueryBuilder<TEntity>
    {
        public QueryBuilder(QueryBuilder builder) : base(builder) 
        {
            
        }
    }
}
