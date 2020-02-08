using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace SqlDbFrameworkNetCore.Helpers
{
    static class TypeExtension
    {
        // return props on foreign type
        public static IEnumerable<PropertyInfo> GetForeignKeyProperties(this Type type, Type foreignType)
        {
            string foreignTypeTemplate = $"{foreignType.Name}.";
            PropertyInfo[] propertyList = type.GetProperties();
            PropertyInfo[] foreignPropertyList = foreignType.GetProperties();
            IList<PropertyInfo> foreignKeyProperties = new List<PropertyInfo>();
            foreach (var property in propertyList)
            {
                var foreignKeyAttr = property.GetCustomAttribute<ForeignKeyAttribute>();
                if (foreignKeyAttr != null && foreignKeyAttr.Name.StartsWith(foreignTypeTemplate))
                {
                    string foreignPropName = foreignKeyAttr.Name.Replace(foreignTypeTemplate, "");
                    PropertyInfo foreignProp = foreignPropertyList.Where(prop => prop.Name == foreignPropName)
                                                                    .FirstOrDefault();
                    foreignKeyProperties.Add(foreignProp);
                }
            }
            return foreignKeyProperties;
        }

        public static IDictionary<string, object> GetForeignKeyPropertyValuePairs(IEnumerable<PropertyInfo> properties, object target)
        {
            IDictionary<string, object> propertyValuePairs = new Dictionary<string, object>();
            foreach (PropertyInfo property in properties)
            {
                var getMethod = property.GetGetMethod();
                // foreign key must always be integer
                // var getDelegate = (Func<int>)getMethod.CreateDelegate(typeof(Func<int>), target);
                var getDelegate = GetConvertedGetDelegate<object>(getMethod, target);
                // convention: [ForeignKeyReferenceClass][FieldName]
                string key = $"{target.GetType().Name}{property.Name}";
                object value = getDelegate();
                propertyValuePairs.Add(key, value);
            }
            return propertyValuePairs;
        }

        private static Func<T> GetConvertedGetDelegate<T>(MethodInfo method, object target)
        {
            Expression targetExpr = Expression.Constant(target);
            Expression methodExpr = Expression.Call(targetExpr, method);
            Expression convertedExpr = Expression.Convert(methodExpr, typeof(T));
            Expression<Func<T>> lambda = Expression.Lambda<Func<T>>(convertedExpr);
            return lambda.Compile();
        }
    }
}
