﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SqlDbFrameworkNetCore.Linq
{
    public interface IJoinedQueryBuilder<TEntity, T1> 
        : ISelectQueryBuilder<TEntity>
    {
        IJoinedQueryBuilder<TEntity, T1> On(Expression<Func<TEntity, T1, bool>> predicate);
        IJoinedQueryBuilder<TEntity, T1> Where(Expression<Func<TEntity, T1, bool>> predicate);
        new IJoinedQueryBuilder<TEntity, T1, T2> InnerJoin<T2>(string alias);

        new IEnumerable<CompositeModel<TEntity, T1>> ExecuteQuery();
    }

    public interface IJoinedQueryBuilder<TEntity, T1, T2> 
        : IJoinedQueryBuilder<TEntity, T1>
    {
        IJoinedQueryBuilder<TEntity, T1, T2> On(Expression<Func<TEntity, T1, T2, bool>> predicate);
        IJoinedQueryBuilder<TEntity, T1, T2> Where(Expression<Func<TEntity, T1, T2, bool>> predicate);
        new IJoinedQueryBuilder<TEntity, T1, T2, T3> InnerJoin<T3>(string alias);

        new IEnumerable<CompositeModel<TEntity, T1, T2>> ExecuteQuery();
    }

    public interface IJoinedQueryBuilder<TEntity, T1, T2, T3> 
        : IJoinedQueryBuilder<TEntity, T1, T2>
    {
        IJoinedQueryBuilder<TEntity, T1, T2, T3> On(Expression<Func<TEntity, T1, T2, T3, bool>> predicate);
        IJoinedQueryBuilder<TEntity, T1, T2, T3> Where(Expression<Func<TEntity, T1, T2, T3, bool>> predicate);
        new IJoinedQueryBuilder<TEntity, T1, T2, T3, T4> InnerJoin<T4>(string alias);

        new IEnumerable<CompositeModel<TEntity, T1, T2, T3>> ExecuteQuery();
    }

    public interface IJoinedQueryBuilder<TEntity, T1, T2, T3, T4> 
        : IJoinedQueryBuilder<TEntity, T1, T2, T3>
    {
        IJoinedQueryBuilder<TEntity, T1, T2, T3, T4> On(Expression<Func<TEntity, T1, T2, T3, T4, bool>> predicate);
        IJoinedQueryBuilder<TEntity, T1, T2, T3, T4> Where(Expression<Func<TEntity, T1, T2, T3, T4, bool>> predicate);
        new IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5> InnerJoin<T5>(string alias);

        new IEnumerable<CompositeModel<TEntity, T1, T2, T3, T4>> ExecuteQuery();
    }

    public interface IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5> 
        : IJoinedQueryBuilder<TEntity, T1, T2, T3, T4>
    {
        IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5> On(Expression<Func<TEntity, T1, T2, T3, T4, T5, bool>> predicate);
        IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5> Where(Expression<Func<TEntity, T1, T2, T3, T4, T5, bool>> predicate);
        new IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5, T6> InnerJoin<T6>(string alias);

        new IEnumerable<CompositeModel<TEntity, T1, T2, T3, T4, T5>> ExecuteQuery();
    }

    public interface IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5, T6> 
        : IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5>
    {
        IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5, T6> On(Expression<Func<TEntity, T1, T2, T3, T4, T5, T6, bool>> predicate);
        IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5, T6> Where(Expression<Func<TEntity, T1, T2, T3, T4, T5, T6, bool>> predicate);
        new IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5, T6, T7> InnerJoin<T7>(string alias);

        new IEnumerable<CompositeModel<TEntity, T1, T2, T3, T4, T5, T6>> ExecuteQuery();
    }

    public interface IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5, T6, T7> 
        : IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5, T6>
    {
        IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5, T6, T7> On(Expression<Func<TEntity, T1, T2, T3, T4, T5, T6, T7, bool>> predicate);
        IJoinedQueryBuilder<TEntity, T1, T2, T3, T4, T5, T6, T7> Where(Expression<Func<TEntity, T1, T2, T3, T4, T5, T6, T7, bool>> predicate);

        new IEnumerable<CompositeModel<TEntity, T1, T2, T3, T4, T5, T6, T7>> ExecuteQuery();
    }
}