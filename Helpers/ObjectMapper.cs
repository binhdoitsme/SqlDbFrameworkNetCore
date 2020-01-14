using Newtonsoft.Json.Linq;
using static Newtonsoft.Json.JsonConvert;
using SqlDbFrameworkNetCore.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SqlDbFrameworkNetCore.Helpers
{
    internal static class ObjectMapper
    {
        private static JObject UnderscoreToPascalProps(JObject o)
        {
            Regex rg = new Regex("\"([a-zA-Z]+_)*([a-zA-Z]+)\"");
            string serialized = SerializeObject(o);
            MatchCollection matches = rg.Matches(serialized);
            if (matches.Count == 0)
            {
                return o;
            }
            else
            {
                foreach (Match m in matches)
                {
                    string snake = m.Value.ToLower();
                    string pascal = StringToolkit.UnderscoreToPascal(snake);
                    serialized = serialized.Replace(snake, pascal);
                }
                return DeserializeObject<JObject>(serialized);
            }
        }

        internal static IEnumerable<T> ToObjectCollection<T>(IEnumerable<dynamic> queryResult)
        {
            // modify the json first
            JArray json = JArray.FromObject(queryResult);

            IList<T> result = new List<T>();
            foreach (JObject obj in json)
            {
                var converted = UnderscoreToPascalProps(obj);
                var concreteConverted = converted.ToObject<T>();
                result.Add(concreteConverted);
            }
            return result;
        }

        internal static IEnumerable<CompositeModel<T, T1>> 
            ToCompositeObjectCollection<T, T1>
            (IEnumerable<dynamic> queryResult)
        {
            // modify the json first
            JArray json = JArray.FromObject(queryResult);

            IList<CompositeModel<T, T1>> result = new List<CompositeModel<T, T1>>();
            foreach (JObject obj in json)
            {
                var converted = UnderscoreToPascalProps(obj);
                CompositeModel<T, T1> compositeObj = new CompositeModel<T, T1>(converted);
                result.Add(compositeObj);
            }
            return result;
        }

        internal static IEnumerable<CompositeModel<T, T1, T2>> 
            ToCompositeObjectCollection<T, T1, T2>
            (IEnumerable<dynamic> queryResult)
        {
            JArray json = JArray.FromObject(queryResult);
            IList<CompositeModel<T, T1, T2>> result = new List<CompositeModel<T, T1, T2>>();
            foreach (JObject obj in json)
            {
                var converted = UnderscoreToPascalProps(obj);
                CompositeModel<T, T1, T2> compositeObj = new CompositeModel<T, T1, T2>(converted);
                result.Add(compositeObj);
            }
            return result;
        }

        internal static IEnumerable<CompositeModel<T, T1, T2, T3>> 
            ToCompositeObjectCollection<T, T1, T2, T3>
            (IEnumerable<dynamic> queryResult)
        {
            JArray json = JArray.FromObject(queryResult);
            IList<CompositeModel<T, T1, T2, T3>> result = new List<CompositeModel<T, T1, T2, T3>>();
            foreach (JObject obj in json)
            {
                var converted = UnderscoreToPascalProps(obj);
                CompositeModel<T, T1, T2, T3> compositeObj = new CompositeModel<T, T1, T2, T3>(converted);
                result.Add(compositeObj);
            }
            return result;
        }

        internal static IEnumerable<CompositeModel<T, T1, T2, T3, T4>> 
            ToCompositeObjectCollection<T, T1, T2, T3, T4>
            (IEnumerable<dynamic> queryResult)
        {
            JArray json = JArray.FromObject(queryResult);
            var result = new List<CompositeModel<T, T1, T2, T3, T4>>();
            foreach (JObject obj in json)
            {
                var converted = UnderscoreToPascalProps(obj);
                var compositeObj = new CompositeModel<T, T1, T2, T3, T4>(converted);
                result.Add(compositeObj);
            }
            return result;
        }

        internal static IEnumerable<CompositeModel<T, T1, T2, T3, T4, T5>>
            ToCompositeObjectCollection<T, T1, T2, T3, T4, T5>
            (IEnumerable<dynamic> queryResult)
        {
            JArray json = JArray.FromObject(queryResult);
            var result = new List<CompositeModel<T, T1, T2, T3, T4, T5>>();
            foreach (JObject obj in json)
            {
                var converted = UnderscoreToPascalProps(obj);
                var compositeObj = new CompositeModel<T, T1, T2, T3, T4, T5>(converted);
                result.Add(compositeObj);
            }
            return result;
        }

        internal static IEnumerable<CompositeModel<T, T1, T2, T3, T4, T5, T6>>
            ToCompositeObjectCollection<T, T1, T2, T3, T4, T5, T6>
            (IEnumerable<dynamic> queryResult)
        {
            JArray json = JArray.FromObject(queryResult);
            var result = new List<CompositeModel<T, T1, T2, T3, T4, T5, T6>>();
            foreach (JObject obj in json)
            {
                var converted = UnderscoreToPascalProps(obj);
                var compositeObj = new CompositeModel<T, T1, T2, T3, T4, T5, T6>(converted);
                result.Add(compositeObj);
            }
            return result;
        }

        internal static IEnumerable<CompositeModel<T, T1, T2, T3, T4, T5, T6, T7>>
            ToCompositeObjectCollection<T, T1, T2, T3, T4, T5, T6, T7>
            (IEnumerable<dynamic> queryResult)
        {
            JArray json = JArray.FromObject(queryResult);
            var result = new List<CompositeModel<T, T1, T2, T3, T4, T5, T6, T7>>();
            foreach (JObject obj in json)
            {
                var converted = UnderscoreToPascalProps(obj);
                var compositeObj = new CompositeModel<T, T1, T2, T3, T4, T5, T6, T7>(converted);
                result.Add(compositeObj);
            }
            return result;
        }
    }
}
