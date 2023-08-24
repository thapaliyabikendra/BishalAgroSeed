using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace BishalAgroSeed.Helpers
{
    public static class DataHelper
    {
        public static List<(string value, string description)> GetValues<T>()
        {
            var data = new List<(string value, string description)>();

            var type = typeof(T);
            var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (var field in fields)
            {
                var value = field.GetValue(null)?.ToString();
                if (value != null)
                {
                    DescriptionAttribute attr = field.GetCustomAttribute<DescriptionAttribute>(false);
                    data.Add((value, attr?.Description));
                }
            }

            return data;
        }
    }
}
