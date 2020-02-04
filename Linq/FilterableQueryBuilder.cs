using SqlDbFrameworkNetCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace SqlDbFrameworkNetCore.Linq
{
    internal class FilterableQueryBuilder<TEntity> : QueryBuilder<TEntity>, IFilterableQueryBuilder<TEntity> where TEntity : class
    {
        public FilterableQueryBuilder(QueryBuilder builder) : base(builder) { }

        public IFilterableQueryBuilder<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            string whereString = ExpressionEvaluator.BuildWhereQueryString(predicate);
            RefactorAlias(predicate);
            QueryStringBuilder.Append($" {whereString}");
            return this;
        }

        public IFilterableQueryBuilder<TEntity> Limit(int count)
        {
            QueryStringBuilder.Append($" LIMIT {count}");
            return this;
        }

        public IFilterableQueryBuilder<TEntity> Offset(int count)
        {
            QueryStringBuilder.Append($" OFFSET {count}");
            return this;
        }

        protected void RefactorAlias(LambdaExpression predicate)
        {
            ParameterExpression[] parameters = ExpressionEvaluator.GetAlias(predicate).ToArray();
            IDictionary<Type, string> aliasMap = ExpressionEvaluator.GetAliasArray(parameters);
            foreach (KeyValuePair<Type, string> alias in aliasMap)
            {
                Regex rg = new Regex($"{StringToolkit.PascalToUnderscore(alias.Key.Name)} [a-z]*"); // already has alias
                string currentQueryValue = this.ToString();
                MatchCollection matches = rg.Matches(currentQueryValue);
                if (matches.Count != 0)
                {
                    foreach (Match m in matches)
                    {
                        QueryStringBuilder.Replace(m.Value,
                            $"{StringToolkit.PascalToUnderscore(alias.Key.Name)} {alias.Value} ");
                    }
                }
                else
                {
                    rg = new Regex($"{StringToolkit.PascalToUnderscore(alias.Key.Name)}$");
                    matches = rg.Matches(currentQueryValue);
                    foreach (Match m in matches)
                    {
                        QueryStringBuilder.Replace(m.Value,
                            $"{StringToolkit.PascalToUnderscore(alias.Key.Name)} {alias.Value} ");
                    }
                }
            }
            QueryStringBuilder.Replace("  ", " ");
        }
    }
}
