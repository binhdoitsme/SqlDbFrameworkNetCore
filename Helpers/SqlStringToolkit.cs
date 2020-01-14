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
            foreach (string keyword in SQL_KEYWORDS)
            {
                if (str.Contains($" {keyword.ToLower()} "))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
