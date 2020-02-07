using SqlDbFrameworkNetCore.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SqlDbFrameworkNetCore.Helpers
{
    public static class ObjectEvaluator
    {
        public static IDictionary<string, object> ToDictionary(object obj)
        {
            var properties = TypeDescriptor.GetProperties(obj);
            IDictionary<string, object> propDictionary = new Dictionary<string, object>();
            foreach (PropertyDescriptor property in properties)
            {
                if (property.PropertyType.FullName == "System.DateTime")
                {
                    propDictionary.Add(property.Name,
                                    ((DateTime)property.GetValue(obj))
                                    .ToString("yyyy-MM-dd H:mm:ss"));
                }
                else
                {
                    if (!property.Attributes.OfType<ExplicitLoadingAttribute>().Any())
                    {
                        propDictionary.Add(StringToolkit.PascalToUnderscore(property.Name),
                                            property.GetValue(obj));
                    }
                }
            }
            return propDictionary;
        }

        // sample: SET user = @user, value = @value
        public static string EvaluateToParameterizedSqlString(IDictionary<string, object> propDictionary)
        {
            var keySet = propDictionary.Keys.ToArray();
            IList<string> parameters = new List<string>();
            foreach (var key in keySet)
            {
                parameters.Add($"{StringToolkit.PascalToUnderscore(key)} = @{$"{key}"}");
            }
            string output = $"{string.Join(", ", parameters)}";
            return output;
        }

        internal static string ToWhereString<T>(T item)
        {
            // list of left-hand-side
            IList<PropertyDescriptor> tableColumns = PropertyToolkit.GetInsertProperties<T>();
            string[] columnNames = new string[tableColumns.Count];
            for (int i = 0; i < tableColumns.Count; i++)
            {
                columnNames[i] = StringToolkit.PascalToUnderscore(tableColumns[i].Name);
            }

            // list of right-hand-side
            string[] values = PropertyToolkit.BuildInsertValuesString(item)
                                            .Trim().Replace("(", "").Replace(")", "")
                                            .Split(", ");

            // build the expression
            StringBuilder queryBuilder = new StringBuilder();
            for (int i = 0; i < values.Length; i++)
            {
                queryBuilder.Append(columnNames[i])
                            .Append(" = ")
                            .Append(values[i]);
                if (i != values.Length - 1)
                {
                    queryBuilder.Append(" AND ");
                }
            }

            return queryBuilder.ToString();
        }

        internal static string ToWhereString<T>(IEnumerable<T> items)
        {
            // list of left-hand-side
            IList<PropertyDescriptor> tableColumns = PropertyToolkit.GetInsertProperties<T>();
            string[] columnNames = new string[tableColumns.Count];
            for (int i = 0; i < tableColumns.Count; i++)
            {
                columnNames[i] = StringToolkit.PascalToUnderscore(tableColumns[i].Name);
            }

            // list of right-hand-side
            IList<string[]> values = new List<string[]>();
            foreach (T item in items)
            {
                values.Add(PropertyToolkit.BuildInsertValuesString(item)
                                                .Trim().Replace("(", "").Replace(")", "")
                                                .Split(", "));
            }

            // build the expression
            StringBuilder queryBuilder = new StringBuilder();
            for (int j = 0; j < values.Count; j++)
            {
                string[] valueArray = values[j];
                for (int i = 0; i < valueArray.Length; i++)
                {
                    queryBuilder.Append(columnNames[i])
                                .Append(" = ")
                                .Append(values[i]);
                    if (i != valueArray.Length - 1)
                    {
                        queryBuilder.Append(" AND ");
                    }
                }
                if (j != values.Count - 1)
                {
                    queryBuilder.Append("\nOR ");
                }
            }
            return queryBuilder.ToString();
        }
    }
}

