using SqlDbFrameworkNetCore.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace SqlDbFrameworkNetCore.Helpers
{
    internal static class ObjectEvaluator
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
                    if (!property.Attributes.OfType<LazyLoadingAttribute>().Any())
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
    }
}

