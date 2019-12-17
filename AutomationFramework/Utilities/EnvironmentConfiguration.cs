using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomationFramework.Utilities
{
    public static class EnvironmentConfiguration
    {
        public static string GetVariable(string key)
        {
            var variables = new List<string>
            {
                Environment.GetEnvironmentVariable(key),
                Environment.GetEnvironmentVariable(key.ToLower()),
                Environment.GetEnvironmentVariable(key.ToUpper())
            };
            return variables.FirstOrDefault(variable => variable != null);
        }
    }
}
