using SqlDbFrameworkNetCore.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SqlDbFrameworkNetCore.Helpers
{
    internal static class PropertyToolkit
    {
        /// <summary>
        /// Gets the list of class attributes that exist in the database
        /// </summary>
        /// <typeparam name="T">The target type</typeparam>
        /// <returns>A list of attributes that can be inserted to the database</returns>
        internal static IList<PropertyDescriptor> GetInsertProperties<T>()
        {
            PropertyDescriptorCollection propList = TypeDescriptor.GetProperties(typeof(T));
            IList<PropertyDescriptor> insertPropList = new List<PropertyDescriptor>();

            // get the actual prop list
            foreach (PropertyDescriptor prop in propList)
            {
                if (prop.Attributes.OfType<ExplicitLoadingAttribute>().Any())
                {
                    continue;
                }
                else
                {
                    insertPropList.Add(prop);
                }
            }
            return insertPropList;
        }

        /// <summary>
        /// Creates the insert string that is used in SQL insert into syntax.
        /// </summary>
        /// <returns>A string like (col1, col2, ...)</returns>
        internal static string BuildInsertString(IList<PropertyDescriptor> properties)
        {
            string[] insertPropNames = new string[properties.Count];
            for (int i = 0; i < properties.Count; i++)
            {
                insertPropNames[i] = StringToolkit.PascalToUnderscore(properties[i].Name);
            }
            return $"({string.Join(", ", insertPropNames)})";
        }

        /// <summary>
        /// Build an insert value string for an item of type T
        /// </summary>
        /// <typeparam name="T">The target type</typeparam>
        /// <param name="item">The object to be inserted</param>
        /// <returns>A string like (val1, val2, ...) corresponding to the insert column string</returns>
        internal static string BuildInsertValuesString<T>(T item)
        {
            IList<PropertyDescriptor> properties = GetInsertProperties<T>();
            object[] values = new object[properties.Count];
            for (int i = 0; i < properties.Count; i++)
            {
                var prop = properties[i];
                Type propType = prop.PropertyType;
                if (propType.IsEnum)
                {
                    // reconsider this
                    values[i] = (Enum)prop.GetValue(item);
                }
                else if (propType == typeof(bool))
                {
                    values[i] = (bool)prop.GetValue(item) ? "B'1'" : "B'0'";
                }
                else if (propType == typeof(DateTime))
                {
                    values[i] = $"'{((DateTime)prop.GetValue(item)).ToString("yyyy-MM-dd hh:mm:ss")}'";
                }
                else if (propType == typeof(string))
                {
                    string inputStr = prop.GetValue(item).ToString().TrimQuotes();
                    if (inputStr.ContainSqlKeyword())
                    {
                        throw new InvalidOperationException("Are you trying to perform SQL Injection? :)");
                    }
                    values[i] = $"N'{inputStr}'";
                }
                else // presumably numeric??
                {
                    values[i] = prop.GetValue(item).ToString();
                }
            }
            return $"({string.Join(", ", values)})";
        }
    }
}
