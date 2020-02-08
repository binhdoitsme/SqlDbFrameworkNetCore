using Newtonsoft.Json;
using SqlDbFrameworkNetCore.Helpers;
using SqlDbFrameworkNetCore.Linq;
using SqlDbFrameworkNetCore.Linq.Relational;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
            var memberAccess = (navigationPath.Body as MemberExpression).Member;
            var memberAttributes = memberAccess.GetCustomAttributes();

            if (memberAttributes.Any(attr => attr is OneToManyAttribute))
            {
                await LoadOneToManyCollection(navigationPath);
            }
            else if (memberAttributes.Any(attr => attr is ManyToManyAttribute))
            {
                Type immediateType = memberAccess.GetCustomAttribute<ManyToManyAttribute>().ImmediateType;
                await LoadManyToManyCollection(navigationPath, immediateType);
            }
            else
            {
                throw new InvalidOperationException("The specified field cannot be explicitly loaded!");
            }
        }

        private async Task LoadOneToManyCollection<TProperty>(
            [NotNull]Expression<Func<IEnumerable<TProperty>>> navigationPath)
            where TProperty : BaseEntity
        {
            Type thisType = GetType();
            Type propertyType = typeof(TProperty);
            var foreignKeyProperties = propertyType.GetForeignKeyProperties(thisType);
            var foreignKeyMap = TypeExtension.GetForeignKeyPropertyValuePairs(foreignKeyProperties, this);
            var lambda = ExpressionExtensions.LambdaConditionExpression<TProperty>(foreignKeyMap);
            var result = await QueryBuilder.Select<TProperty>()
                                            .Where(lambda)
                                            .ExecuteQueryAsync();

            SetValueUsingNavigationPath(navigationPath, result);
        }

        private async Task LoadManyToManyCollection<TProperty>(
            [NotNull]Expression<Func<IEnumerable<TProperty>>> navigationPath,
            Type immediateType) where TProperty : BaseEntity
        {
            // get result when inner joining
            object result = await InnerJoinQuery(QueryBuilder, targetType:typeof(TProperty), typeof(TProperty), immediateType, GetType());
            SetValueUsingNavigationPath(navigationPath, result);
        }

        /// <summary>
        /// Create an InnerJoin query by expression tree.
        /// TargetType must be present in typeArgs.
        /// The order of typeArgs is important.
        /// </summary>
        /// <param name="queryBuilder"></param>
        /// <param name="typeArgs"></param>
        /// <returns></returns>
        private async Task<object> InnerJoinQuery(IQueryBuilder queryBuilder, Type targetType = null, params Type[] typeArgs)
        {
            if (typeArgs.Length == 0)
            {
                throw new ArgumentNullException();
            }

            Type[] typeArguments;
            if (targetType != null)
            {
                List<Type> typeArgsList = new List<Type>() { targetType };
                typeArgsList.AddRange(typeArgs.Where(type => type != targetType));
                typeArguments = typeArgsList.ToArray();
            } else
            {
                typeArguments = typeArgs;
            }

            // get result from task delegate
            Func<Task> innerJoinDelegate = InnerJoinDelegate(queryBuilder, typeArguments);
            Task task = innerJoinDelegate();
            await task.ConfigureAwait(false);
            var resultProperty = task.GetType().GetProperty("Result");
            var result = resultProperty.GetValue(task);
            return result;
        }

        private Func<Task> InnerJoinDelegate(IQueryBuilder queryBuilder, Type[] typeArgs)
        {
            // build lambda expression & return task delegate
            Type queryBuilderType = typeof(IQueryBuilder);
            var instance = Expression.Constant(queryBuilder);
            var method = queryBuilderType.GetMethod("SelectDistinct").MakeGenericMethod(typeArgs[0]);
            var paramType = method.GetParameters().FirstOrDefault().ParameterType.GetElementType();
            Expression expr = Expression.Call(instance, method, Expression.NewArrayInit(paramType));
            Type initialType = method.ReturnType;
            Type currentType = initialType;

            // make inner joins
            expr = MakeInnerJoinMethods(expr, typeArgs, currentType);

            // where
            Expression whereExpr = MakeWhereExpression(typeArgs);
            expr = MakeWhereMethod(expr, typeArgs, whereExpr);

            // ExecuteQueryAsync
            var executeQueryAsyncMethod = typeof(ISortableQueryBuilder<>)
                                            .MakeGenericType(initialType.GenericTypeArguments)
                                            .GetMethod("ExecuteQueryAsync");
            expr = Expression.Call(expr, executeQueryAsyncMethod);
            var lambda = Expression.Lambda<Func<Task>>(expr);
            return lambda.Compile();
        }

        private Expression MakeWhereMethod(Expression instance, Type[] typeArgs, Expression predicate)
        {
            var currentType = (instance as MethodCallExpression).Method.ReturnType;
            var whereMethod = currentType.GetMethod("Where");
            instance = Expression.Call(instance, whereMethod, predicate);
            return instance;
        }

        /// <summary>
        /// Create a WHERE- expression using the [Key] field(s)
        /// </summary>
        /// <returns></returns>
        private Expression MakeWhereExpression(params Type[] typeArgs)
        {
            // get [Key] fields and their values
            Expression thisInstance = Expression.Constant(this);
            IDictionary<PropertyInfo, object> keyFields = new Dictionary<PropertyInfo, object>();
            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                if (prop.GetCustomAttribute<KeyAttribute>() != null)
                {
                    var getMethod = prop.GetGetMethod();
                    var getMethodExpr = Expression.Call(thisInstance, getMethod);
                    Expression objGetMethodExpr = Expression.Convert(getMethodExpr, typeof(object));
                    var getDelegate = Expression.Lambda<Func<object>>(objGetMethodExpr).Compile();
                    keyFields.Add(prop, getDelegate());
                }
            }

            // build where expression : (parameters) => {}
            var parameterExpressions = typeArgs.Select(typeArg
                                                    => Expression
                                                        .Parameter(typeArg, $"alias_{typeArg.Name.ToLower()}"))
                                                .ToArray();
            Expression lambdaBody = MakeWherePredicate(keyFields, parameterExpressions);
            var lambda = Expression.Lambda(lambdaBody, parameterExpressions);
            return lambda;
        }

        private Expression MakeWherePredicate(IDictionary<PropertyInfo, object> keyFields, ParameterExpression[] parameters)
        {
            Expression wherePredicate = keyFields.Select(entry => EntryToPredicate(entry, parameters))
                                                    .Aggregate((e1, e2) => Expression.AndAlso(e1, e2));
            return wherePredicate;
        }

        private Expression EntryToPredicate(KeyValuePair<PropertyInfo, object> entry, ParameterExpression[] parameters)
        {
            Type declaringType = entry.Key.DeclaringType;
            var param = parameters.Where(p => p.Type == declaringType).FirstOrDefault();
            Expression propAccess = Expression.PropertyOrField(param, entry.Key.Name);
            Expression value = Expression.Constant(entry.Value);
            Expression binaryExpr = Expression.Equal(propAccess, value);
            return binaryExpr;
        }

        /// <summary>
        /// Side-effects: modifies <c>instance</c> and <c>currentType</c>
        /// </summary>
        /// <returns></returns>
        private Expression MakeInnerJoinMethods(Expression instance, Type[] typeArgs, Type currentType)
        {
            for (int i = 1; i < typeArgs.Length; i++)
            {
                // inner join method
                var innerJoinMethod = currentType.GetMethod("InnerJoin").MakeGenericMethod(typeArgs[i]);
                instance = Expression.Call(instance, innerJoinMethod, Expression.Constant($"alias_{typeArgs[i].Name.ToLower()}"));
                currentType = innerJoinMethod.ReturnType;

                // on method
                var onMethod = currentType.GetMethod("On");
                var onMethodParamType = onMethod.GetParameters().FirstOrDefault().ParameterType;
                Type[] __typeArgs__ = new Type[i + 1];
                Array.Copy(typeArgs, 0, __typeArgs__, 0, i + 1);
                instance = Expression.Call(instance, 
                                            onMethod, 
                                            Expression.Constant(MakeInnerJoinOnPredicate(__typeArgs__), onMethodParamType));
            }
            return instance;
        }

        private Expression MakeInnerJoinOnPredicate(params Type[] typeArgs)
        {
            Type[] __typeArgs__ = typeArgs.Append(typeof(bool)).ToArray();
            string[] aliases = typeArgs.Select(type => $"alias_{type.Name.ToLower()}").ToArray();
            var paramExpressions = typeArgs.Select(typeArg => Expression.Parameter(typeArg, $"alias_{typeArg.Name.ToLower()}"))
                                            .ToArray();
            Type expressionType = typeof(Expression<>)
                                    .MakeGenericType(
                                        GENERIC_ARGS_COUNT_MAP[__typeArgs__.Length]
                                        .MakeGenericType(__typeArgs__));
            var foreignKeyProperties = new Dictionary<PropertyInfo, IEnumerable<ForeignKeyAttribute>>();
            // consider the last type passed in
            foreach (var currentType in typeArgs)
            {
                foreach (var property in currentType.GetProperties())
                {
                    var foreignKeys = property.GetCustomAttributes<ForeignKeyAttribute>();
                    if (foreignKeys.Any())
                    {
                        string typeName = foreignKeys.FirstOrDefault().Name.Trim().Split('.')[0];
                        if (Array.FindIndex(__typeArgs__, type => type.Name == typeName) != -1)
                        {
                            foreignKeyProperties.Add(property, foreignKeys);
                        }
                    }
                }
            }
            Expression onPredicate = foreignKeyProperties.Select(entry => EntryToPredicate(entry))
                                                        .Aggregate((e1, e2) => Expression.AndAlso(e1, e2));
            var lambda = Expression.Lambda(onPredicate, paramExpressions);
            return lambda;
        }

        private Expression EntryToPredicate(KeyValuePair<PropertyInfo, IEnumerable<ForeignKeyAttribute>> entry)
        {
            Type thisType = entry.Key.DeclaringType;
            Expression thisParam = Expression.Parameter(thisType, $"alias_{thisType.Name.ToLower()}");
            Expression thisMemberAccess = Expression.PropertyOrField(thisParam, entry.Key.Name);
            List<Expression> binaryExpressions = new List<Expression>();
            foreach (var attr in entry.Value)
            {
                // create a member access expression for each foreign key attribute
                string[] splitStr = attr.Name.Trim().Split(".");
                string className = thisType.AssemblyQualifiedName.Replace(thisType.Name, splitStr[0]);
                string propName = splitStr[1];
                Type foreignType = Type.GetType($"{className}");
                Expression param = Expression.Parameter(foreignType, $"alias_{foreignType.Name.ToLower()}");
                Expression memberAccess = Expression.PropertyOrField(param, propName);
                Expression binaryExpr = Expression.Equal(memberAccess, thisMemberAccess);
                binaryExpressions.Add(binaryExpr);
            }
            Expression result = binaryExpressions[0];
            for (int i = 1; i < binaryExpressions.Count; i++)
            {
                result = Expression.AndAlso(result, binaryExpressions[i]);
            }

            return result;
        }

        private readonly IDictionary<int, Type> GENERIC_ARGS_COUNT_MAP = new Dictionary<int, Type>()
        {
            { 1, typeof(Func<>) },
            { 2, typeof(Func<,>) },
            { 3, typeof(Func<,,>) },
            { 4, typeof(Func<,,,>) },
            { 5, typeof(Func<,,,,>) },
            { 6, typeof(Func<,,,,,>) },
            { 7, typeof(Func<,,,,,,>) },
            { 8, typeof(Func<,,,,,,,>) },
            { 9, typeof(Func<,,,,,,,,>) }
        };

        private void SetValueUsingNavigationPath<TDelegate>([NotNull]Expression<Func<TDelegate>> navigationPath, object result)
        {
            var navigationBody = navigationPath.Body;
            // get property type
            var propertyType = (navigationBody as MemberExpression).Type;
            var lambdaBody = Expression.Assign(navigationBody, Expression.Constant(result, propertyType));
            var setExpr = Expression.Lambda<Action>(lambdaBody, navigationPath.Parameters);
            setExpr.Compile().Invoke();
        }
    }
}
