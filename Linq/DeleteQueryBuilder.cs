using System;
using System.Collections.Generic;
using System.Text;

namespace SqlDbFrameworkNetCore.Linq
{
    internal class DeleteQueryBuilder<TEntity> : FilterableQueryBuilder<TEntity>, IDeleteQueryBuilder<TEntity>
    {
        internal DeleteQueryBuilder(QueryBuilder builder) : base(builder) { }
    }
}
