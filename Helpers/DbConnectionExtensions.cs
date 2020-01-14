using SqlDbFrameworkNetCore.Linq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace SqlDbFrameworkNetCore.Helpers
{
    public static class DbConnectionExtensions
    {
        public static IQueryBuilder CreateQueryBuilder(this DbConnection connection)
        {
            QueryBuilder builder = new QueryBuilder();
            builder.Connection = connection;
            return builder;
        }
    }
}
