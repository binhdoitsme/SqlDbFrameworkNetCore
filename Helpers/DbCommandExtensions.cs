using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace SqlDbFrameworkNetCore.Helpers
{
    internal static class DbCommandExtensions
    {
        /// <summary>
        /// Evaluate the object into DbParameters, then return the parameter collection
        /// </summary>
        /// <param name="cmd">This command</param>
        /// <param name="obj">The object to be evaluated</param>
        /// <returns>The parameter collection of this</returns>
        internal static DbParameterCollection CreateParameters(this DbCommand cmd, object obj)
        {
            IDictionary<string, object> objMap = ObjectEvaluator.ToDictionary(obj);
            foreach (KeyValuePair<string, object> entry in objMap)
            {
                DbParameter param = cmd.CreateParameter();
                param.ParameterName = entry.Key;
                param.Value = entry.Value;
                cmd.Parameters.Add(param);
            }
            Console.WriteLine(string.Join(",", cmd.Parameters));
            return cmd.Parameters;
        }
    }
}
