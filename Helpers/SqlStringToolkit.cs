using System;
using System.Collections.Generic;
using System.Text;

namespace SqlDbFrameworkNetCore.Helpers
{
    internal static class SqlStringToolkit
    {
        private static readonly string[] SQL_KEYWORDS = new string[]
        {
            "SELECT", "FROM", "WHERE", "UPDATE", "SET", "DELETE", "INSERT", "VALUES",
            "ORDER BY", "GROUP BY", "HAVING",
            "DROP", "ALTER", "CREATE",
            "AND", "OR", "NOT",
            "JOIN", "ON"
        };

        public static bool ContainSqlKeyword(this string str)
        {
            if (str.Length < 2) { return false; }
            string _str = str[1..^1];
            if (_str.Contains("'")
                    || _str.Contains("`")
                    || _str.Contains("\""))
            {
                return true;
            }
            foreach (string keyword in SQL_KEYWORDS)
            {
                if (_str.Contains($" {keyword.ToLower()} ")
                    || _str.Contains($" {keyword.ToUpper()}"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
