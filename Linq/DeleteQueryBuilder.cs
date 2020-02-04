using System;
using System.Collections.Generic;
using System.Text;

namespace SqlDbFrameworkNetCore.Linq
{
    internal class DeleteQueryBuilder<TEntity> 
        : FilterableQueryBuilder<TEntity>, IDeleteQueryBuilder<TEntity> where TEntity : class
    {
        internal DeleteQueryBuilder(QueryBuilder builder) : base(builder) { }
    }
}
