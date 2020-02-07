using SqlDbFrameworkNetCore.Helpers;
using SqlDbFrameworkNetCore.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SqlDbFrameworkNetCore.Models
{
    /// <summary>
    /// The base class for a queryable entity used in the SqlDbFramework
    /// </summary>
    public class BaseEntity
    {
        private IQueryBuilder QueryBuilder;

        public BaseEntity SetQueryBuilder(IQueryBuilder queryBuilder)
        {
            QueryBuilder = queryBuilder;
            return this;
        }

        /// <summary>
        /// Load an associated queryable field onto this.
        /// </summary>
        /// <typeparam name="TProperty">Type of the property to be loaded</typeparam>
        /// <param name="navigationPath">The path to navigate from the target object to the target property</param>
        public async Task Load<TProperty>([NotNull]Expression<Func<TProperty>> navigationPath)
            where TProperty : BaseEntity
        {
            Type thisType = GetType();
            Type propertyType = typeof(TProperty);
            var foreignKeyProperties = propertyType.GetForeignKeyProperties(thisType);
            var foreignKeyMap = TypeExtension.GetForeignKeyPropertyValuePairs(foreignKeyProperties, this);
            var lambda = ExpressionExtensions.LambdaConditionExpression<TProperty>(foreignKeyMap);
            Console.WriteLine(lambda);
            var result = (await QueryBuilder.Select<TProperty>()
                                            .Where(lambda)
                                            .ExecuteQueryAsync())
                                            .FirstOrDefault();

            SetValueUsingNavigationPath(navigationPath, result);
        }

        /// <summary>
        /// Load an associated queryable field onto this.
        /// </summary>
        /// <typeparam name="TProperty">Type of the property to be loaded</typeparam>
        /// <param name="navigationPath">The path to navigate from the target object to the target property</param>
        public async Task Load<TProperty>([NotNull]Expression<Func<IEnumerable<TProperty>>> navigationPath)
            where TProperty : BaseEntity
        {
            Type thisType = GetType();
            Type propertyType = typeof(TProperty);
            var foreignKeyProperties = propertyType.GetForeignKeyProperties(thisType);
            var foreignKeyMap = TypeExtension.GetForeignKeyPropertyValuePairs(foreignKeyProperties, this);
            var lambda = ExpressionExtensions.LambdaConditionExpression<TProperty>(foreignKeyMap);
            Console.WriteLine(lambda);
            var result = await QueryBuilder.Select<TProperty>()
                                            .Where(lambda)
                                            .ExecuteQueryAsync();

            SetValueUsingNavigationPath(navigationPath, result);
        }

        private void SetValueUsingNavigationPath<TProperty>([NotNull]Expression<Func<TProperty>> navigationPath, object result)
        {
            var navigationBody = navigationPath.Body;
            var lambdaBody = Expression.Assign(navigationBody, Expression.Constant(result));
            var setExpr = Expression.Lambda<Action>(lambdaBody, navigationPath.Parameters);
            setExpr.Compile().Invoke();
        }
    }
}
