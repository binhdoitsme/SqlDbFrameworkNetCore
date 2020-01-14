using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SqlDbFrameworkNetCore.Helpers
{
    internal static class ExpressionEvaluator
    {
        // constant definitions
        public static readonly IDictionary<ExpressionType, string> EXPRESSION_MAP = new Dictionary<ExpressionType, string>() {
                { ExpressionType.AndAlso, "AND" },
                { ExpressionType.OrElse, "OR" },
                { ExpressionType.LessThan, "<" },
                { ExpressionType.GreaterThan, ">" },
                { ExpressionType.LessThanOrEqual, "<=" },
                { ExpressionType.GreaterThanOrEqual, ">=" },
                { ExpressionType.Equal, "=" },
                { ExpressionType.NotEqual, "!" }
            };

        // public methods
        public static string BuildOrderByQueryString(LambdaExpression expression, bool orderDesc)
        {
            MemberExpression expr = GetOperand(expression) as MemberExpression;
            string orderColumn = $"{expr.Expression}.{StringToolkit.PascalToUnderscore(expr.Member.Name.ToString())}";
            return $"ORDER BY {orderColumn} {(orderDesc ? "DESC" : "")}";
        }

        // TODO: change to parameterized statement usage
        public static string BuildWhereQueryString(LambdaExpression expression)
        {
            string conditionStr = BuildConditionQueryString(expression);
            if (conditionStr.ContainSqlKeyword())
            {
                throw new InvalidOperationException("Are you trying to perform SQL Injection? :)");
            }
            return $"WHERE {conditionStr}";
        }

        public static string BuildOnConditionString(LambdaExpression expression)
        {
            string conditionStr = BuildConditionQueryString(expression);
            if (conditionStr.ContainSqlKeyword())
            {
                throw new InvalidOperationException("Are you trying to perform SQL Injection? :)");
            }
            return $"ON {conditionStr}";
        }

        // since 1.1.0: changed last GetOperand line
        private static string ToQueryString(BinaryExpression simpleExpression)
        {
            var leftOp = GetOperand(simpleExpression.Left) as MemberExpression;
            string leftHandSide = $"{leftOp.Expression}" +
                $".{StringToolkit.PascalToUnderscore(leftOp.Member.Name)}";
            var rightOp = GetOperand(simpleExpression.Right);
            string rightHandSide;
            if (rightOp is MemberExpression)
            {
                rightHandSide = $"{(rightOp as MemberExpression).Expression}" +
                    $".{StringToolkit.PascalToUnderscore((rightOp as MemberExpression).Member.Name)}";
            }
            else
            {
                var newRightOp = (rightOp as ConstantExpression).Value;
                if (newRightOp is DateTime)
                {
                    rightHandSide = $"'{(rightOp as DateTime?).Value.ToString("yyyy-MM-dd")}'";
                }
                else if (newRightOp is bool)
                {
                    rightHandSide = (bool)newRightOp ? "B'1'" : "B'0'";
                }
                else
                {
                    rightHandSide = rightOp.ToString();
                }

            }
            return string.Format("{0} {1} {2}",
                leftHandSide.Replace("'", ""),
                EXPRESSION_MAP[simpleExpression.NodeType],
                rightHandSide);
        }

        private static string BuildConditionQueryString(LambdaExpression expression)
        {
            var simpleClauses = SplitToBinaryClause(expression);
            var operators = GetLogicalOperators(expression);
            //var aliasDictionary = GetAliasDictionary(expression);
            string output = ToQueryString(simpleClauses[0]);
            for (int i = 1; i < simpleClauses.Count; i++)
            {
                output += string.Format(" {0}", EXPRESSION_MAP[operators[i - 1]]);
                output += string.Format(" {0}", ToQueryString(simpleClauses[i]));
            }
            return output;
        }

        private static List<ExpressionType> GetLogicalOperators(Expression expression)
        {
            var output = new List<ExpressionType>();
            if (expression.NodeType == ExpressionType.Lambda)
            {
                expression = (expression as LambdaExpression).Body;
            }
            while (IsOperatorNode(expression.NodeType))
            {
                output.Add(expression.NodeType);
                expression = (expression as BinaryExpression).Left;
            }
            output.Reverse();
            return output;
        }

        private static List<BinaryExpression> SplitToBinaryClause(Expression expression)
        {
            var output = new List<BinaryExpression>();

            if (expression.NodeType == ExpressionType.Lambda)
            {
                expression = (expression as LambdaExpression).Body;
            }

            // base case
            if (!IsOperatorNode(expression.NodeType))
            {
                output.Add(expression as BinaryExpression);
            }

            // induction
            else
            {
                output.AddRange(SplitToBinaryClause((expression as BinaryExpression).Left));
                output.AddRange(SplitToBinaryClause((expression as BinaryExpression).Right));
            }

            return output;
        }

        private static bool IsOperatorNode(ExpressionType e)
        {
            return e == ExpressionType.AndAlso || e == ExpressionType.OrElse;
        }

        // Since 1.1.0: changed the return type to object & altered the behaviour
        public static object GetOperand(Expression expr)
        {
            if (expr is UnaryExpression) return (expr as UnaryExpression).Operand;
            if (expr is LambdaExpression) return GetOperand((expr as LambdaExpression).Body);
            if (expr is MemberExpression)
            {
                // return a string in this case
                if (!((expr as MemberExpression).Expression is ParameterExpression))
                {
                    var _out = Expression.Lambda(expr).Compile().DynamicInvoke();
                    if (_out is string)
                    {
                        return string.Format("'{0}'", _out as string);
                    }
                    else
                    {
                        return _out;
                    }
                }
            }
            return expr;
        }

        public static IReadOnlyCollection<ParameterExpression> GetAlias(LambdaExpression expression)
        {
            return expression.Parameters;
        }

        public static IDictionary<Type, string> GetAliasArray(ParameterExpression[] parameters)
        {
            IDictionary<Type, string> aliasMap = new Dictionary<Type, string>();
            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterExpression currentParam = parameters[i];
                aliasMap.Add(currentParam.Type, currentParam.Name);
            }
            return aliasMap;
        }
    }
}
