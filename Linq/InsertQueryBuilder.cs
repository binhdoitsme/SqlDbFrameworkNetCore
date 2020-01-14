using SqlDbFrameworkNetCore.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SqlDbFrameworkNetCore.Linq
{
    internal class InsertQueryBuilder<TEntity> : QueryBuilder<TEntity>, IInsertQueryBuilder<TEntity>
    {
        internal InsertQueryBuilder(QueryBuilder builder) : base(builder) { }

        public IQueryBuilder<TEntity> Values(TEntity[] items)
        {
            // TODO: Code here
            if (items == null)
            {
                throw new NullReferenceException("Cannot insert null into database!");
            }
            if (items.Length == 0)
            {
                throw new InvalidOperationException("Cannot insert nothing to database!");
            }
            else
            {
                IList<PropertyDescriptor> insertProperties = PropertyToolkit.GetInsertProperties<TEntity>();
                string insertColumnStr = PropertyToolkit.BuildInsertString(insertProperties);
                string[] insertValueStrArray = new string[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    TEntity item = items[i];
                    insertValueStrArray[i] = PropertyToolkit.BuildInsertValuesString(item);
                }
                this.QueryStringBuilder.Append($" {insertColumnStr} VALUES {string.Join(", ", insertValueStrArray)}");
                return this;
            }
        }
    }
}
