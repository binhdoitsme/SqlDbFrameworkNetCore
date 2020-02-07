using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SqlDbFrameworkNetCore.Helpers
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> LambdaConditionExpression<T>(IDictionary<string, object> conditions) 
        {
            // set the parameter
            var type = typeof(T);
            var param = Expression.Parameter(type, type.Name.Substring(0, 1).ToLower());
            BinaryExpression[] comparisonExpressions = new BinaryExpression[conditions.Count];
            int i = 0;

            // build binary clauses
            foreach (KeyValuePair<string, object> entry in conditions)
            {
                var comparisonExpr = BinaryConditionExpression(param, entry);
                comparisonExpressions[i] = comparisonExpr;
                ++i;
            }

            // append to lambda body
            var lambdaBody = LambdaComparisonBody(comparisonExpressions);

            // make lambda
            var lambda = Expression.Lambda<Func<T, bool>>(lambdaBody, param);
            return lambda;
        }

        private static BinaryExpression BinaryConditionExpression(Expression param, KeyValuePair<string, object> entry)
        {
            string fieldName = entry.Key;
            object fieldValue = entry.Value;
            var memberAccessExpr = Expression.PropertyOrField(param, fieldName);
            var constExpr = Expression.Constant(fieldValue);
            return Expression.Equal(memberAccessExpr, constExpr);
        }

        private static Expression LambdaComparisonBody(params BinaryExpression[] expressions)
        {
            if (expressions.Length < 1)
            {
                throw new ArgumentException("Parameters are missing!");
            }
            if (expressions.Length == 1)
            {
                return expressions[0];
            } else
            {
                Expression expr = expressions[0];
                for (int i = 1; i < expressions.Length; i++)
                {
                    expr = Expression.AndAlso(expr, expressions[i]);
                }
                return expr;
            }
        }
    }
}
