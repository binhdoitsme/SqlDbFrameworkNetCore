using SqlDbFrameworkNetCore.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Text.Json;

namespace SqlDbFrameworkNetCore.Linq
{
    internal class UpdateQueryBuilder<TEntity> : FilterableQueryBuilder<TEntity>, IUpdateQueryBuilder<TEntity>
    {
        // additional attribute
        protected IDictionary<string, object> ParameterMap;

        internal UpdateQueryBuilder(QueryBuilder queryStringBuilder) : base(queryStringBuilder) 
        {
            ParameterMap = new Dictionary<string, object>();
        }

        public IFilterableQueryBuilder<TEntity> Set(dynamic newValues)
        {
            ParameterMap = ObjectEvaluator.ToDictionary(newValues);
            Console.WriteLine(string.Join("\n", ParameterMap));
            StringBuilder setStringBuilder = new StringBuilder();
            setStringBuilder.Append(ObjectEvaluator.EvaluateToParameterizedSqlString(ParameterMap));
            string setStr = setStringBuilder.ToString();
            var cmd = Connection.CreateCommand();
            ParameterCollection = cmd.CreateParameters((object)newValues);
            QueryStringBuilder.Append($" SET {setStr}");
            return this;
        }
    }
}
