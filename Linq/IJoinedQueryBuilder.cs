using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SqlDbFrameworkNetCore.Linq
{
    public interface IJoinedQueryBuilder<TEntity, T1> 
        : ISelectQueryBuilder<TEntity> where TEntity : class
    {
        IJoinedQueryBuilder<TEntity, T1> On(Expression<Func<TEntity, T1, bool>> predicate);
        IJoinedQueryBuilder<TEntity, T1> Where(Expression<Func<TEntity, T1, bool>> predicate);
        new IJoinedQueryBuilder<TEntity, T1, T2> InnerJoin<T2>(string alias);

        new IEnumerable<CompositeModel<TEntity, T1>> ExecuteQuery();
        new Task<IEnumerable<CompositeModel<TEntity, T1>>> ExecuteQueryAsync();
    }

    public interface IJoinedQueryBuilder<TEntity, T1, T2> 
        : IJoinedQueryBuilder<TEntity, T1> where TEntity : class
    {
        IJoinedQueryBuilder<TEntity, T1, T2> On(Expression<Func<TEntity, T1, T2, bool>> predicate);
        IJoinedQueryBuilder<TEntity, T1, T2> Where(Expression<Func<TEntity, T1, T2, bool>> predicate);
        new IJoinedQueryBuilder<TEntity, T1, T2, T3> InnerJoin<T3>(string alias);

        new IEnumerable<CompositeModel<TEntity, T1, T2>> ExecuteQuery();
        new Task<IEnumerable<CompositeModel<TEntity, T1, T2>>> ExecuteQueryAsync();
    }

    public interface IJoinedQueryBuilder<TEntity, T1, T2, T3> 
        : IJoinedQueryBuilder<TEntity, T1, T2> where TEntity : class
    {
        IJoinedQueryBuilder<TEntity, T1, T2, T3> On(Expression<Func<TEntity, T1, T2, T3, bool>> predicate);
        IJoinedQueryBuilder<TEntity, T1, T2, T3> Where(Expression<Func<TEntity, T1, T2, T3, bool>> predicate);
        new IJoinedQueryBuilder<TEntity, T1, T2, T3, T4> InnerJoin<T4>(string alias);

        new IEnumerable<CompositeModel<TEntity, T1, T2, T3>> ExecuteQuery();
        new Task<IEnumerable<CompositeModel<TEntity, T1, T2, T3>>> ExecuteQueryAsync();
    }

    public interface IJoinedQueryBuilder<TEntity, T1, T2, T3, T4> 
        : IJoinedQueryBuilder<TEntity, T1, T2, T3> where TEntity : class
    {
        IJoinedQueryBuilder<TEntity, T1, T2, T3, T4> On(Expression<Func<TEntity, T1, T2, T3, T4, bool>> predicate);
        IJoinedQueryBuilder<TEntity, T1, T2, T3, T4> Where(Expression<Func<TEntity, T1, T2, T3, T4, bool>> predicate);
        new IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5> InnerJoin<T5>(string alias);

        new IEnumerable<CompositeModel<TEntity, T1, T2, T3, T4>> ExecuteQuery();
        new Task<IEnumerable<CompositeModel<TEntity, T1, T2, T3, T4>>> ExecuteQueryAsync();
    }

    public interface IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5> 
        : IJoinedQueryBuilder<TEntity, T1, T2, T3, T4> where TEntity : class
    {
        IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5> On(Expression<Func<TEntity, T1, T2, T3, T4, T5, bool>> predicate);
        IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5> Where(Expression<Func<TEntity, T1, T2, T3, T4, T5, bool>> predicate);
        new IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5, T6> InnerJoin<T6>(string alias);

        new IEnumerable<CompositeModel<TEntity, T1, T2, T3, T4, T5>> ExecuteQuery();
    }

    public interface IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5, T6> 
        : IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5> where TEntity : class
    {
        IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5, T6> On(Expression<Func<TEntity, T1, T2, T3, T4, T5, T6, bool>> predicate);
        IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5, T6> Where(Expression<Func<TEntity, T1, T2, T3, T4, T5, T6, bool>> predicate);
        new IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5, T6, T7> InnerJoin<T7>(string alias);

        new IEnumerable<CompositeModel<TEntity, T1, T2, T3, T4, T5, T6>> ExecuteQuery();
        new Task<IEnumerable<CompositeModel<TEntity, T1, T2, T3, T4, T5, T6>>> ExecuteQueryAsync();
    }

    public interface IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5, T6, T7> 
        : IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5, T6> where TEntity : class
    {
        IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5, T6, T7> On(Expression<Func<TEntity, T1, T2, T3, T4, T5, T6, T7, bool>> predicate);
        IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5, T6, T7> Where(Expression<Func<TEntity, T1, T2, T3, T4, T5, T6, T7, bool>> predicate);

        new IEnumerable<CompositeModel<TEntity, T1, T2, T3, T4, T5, T6, T7>> ExecuteQuery();
        new Task<IEnumerable<CompositeModel<TEntity, T1, T2, T3, T4, T5, T6, T7>>> ExecuteQueryAsync();
    }
}
