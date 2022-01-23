using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using System.Linq.Expressions;

namespace DStudioTest.Utils
{
    /// <summary>
    /// Helper class for some extensions
    /// </summary>
    public static class Extensions
    {
        public class Holder<TEnum>
        {
            public static readonly TEnum[] values = (TEnum[])Enum.GetValues(typeof(TEnum));
            public static readonly string[] names = Enum.GetNames(typeof(TEnum));
        }

        public static TEnum GetEnum<TEnum>(
            string key,
            TEnum defaultValue = default(TEnum),
            bool ignoreCase = true)
        where TEnum : struct, IConvertible
        {
            var comparisonType = ignoreCase ? StringComparison.OrdinalIgnoreCase :
                                              StringComparison.Ordinal;
            for (int i = 0; i < Holder<TEnum>.names.Length; i++)
            {
                if (Holder<TEnum>.names[i].Equals(key, comparisonType))
                    return Holder<TEnum>.values[i];
            }
            return defaultValue;
        }

        public static string GetDescription<TEnum>(this TEnum value)
        {
            DescriptionAttribute[] da = (DescriptionAttribute[])(typeof(TEnum).GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false));
            return da.Length > 0 ? da[0].Description : value.ToString();
        }

        public static List<KeyValuePair<string, string>> GetEnum<TEnum>() where TEnum : struct, IConvertible
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            foreach (int val in Enum.GetValues(typeof(TEnum)))
            {
                //KeyValuePair<string, int> item = new KeyValuePair<string, int>(Enum.Parse(typeof(TEnum), val.ToString()).ToString(), val);
                TEnum result;
                string name;
                if (Enum.TryParse(val.ToString(), out result))
                    name = Extensions.GetDescription<TEnum>(result);
                else
                    name = Enum.Parse(typeof(TEnum), val.ToString()).ToString();
                list.Add(new KeyValuePair<string, string>(val.ToString(), name));
            }
            return list;
        }
        public static List<KeyValuePair<int, string>> GetEnum2<TEnum>() where TEnum : struct, IConvertible
        {
            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
            foreach (int val in Enum.GetValues(typeof(TEnum)))
            {
                //KeyValuePair<string, int> item = new KeyValuePair<string, int>(Enum.Parse(typeof(TEnum), val.ToString()).ToString(), val);
                TEnum result;
                string name;
                if (Enum.TryParse(val.ToString(), out result))
                    name = Extensions.GetDescription<TEnum>(result);
                else
                    name = Enum.Parse(typeof(TEnum), val.ToString()).ToString();
                list.Add(new KeyValuePair<int, string>(val, result.ToString())); //name
            }
            return list;
        }
    }

    public class HelperSort
    {
        public static IEnumerable<T> OrderByDynamic<T>(IEnumerable<T> items, string sortby, string sort_direction)
        {
            var property = typeof(T).GetProperty(sortby);

            var result = typeof(HelperSort)
                .GetMethod("OrderByDynamic_Private", BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(typeof(T), property.PropertyType)
                .Invoke(null, new object[] { items, sortby, sort_direction });

            return (IEnumerable<T>)result;
        }

        private static IEnumerable<T> OrderByDynamic_Private<T, TKey>(IEnumerable<T> items, string sortby, string sort_direction)
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            Expression<Func<T, TKey>> property_access_expression =
                Expression.Lambda<Func<T, TKey>>(
                    Expression.Property(parameter, sortby),
                    parameter);

            if (sort_direction == "asc")
            {
                return items.OrderBy(property_access_expression.Compile());
            }

            if (sort_direction == "desc")
            {
                return items.OrderByDescending(property_access_expression.Compile());
            }

            throw new Exception("Invalid Sort Direction");
        }
    }
}
