using DameChales.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DameChales.Common.Extensions
{
    public static class HashSetExtensions
    {
        /// <summary>
        /// Converts HashSet\<Enum> to a "_" separated string 
        /// </summary>
        public static string EnumSetToString<T>(this HashSet<T> set)
        {
            string result = "";
            foreach(var item in set)
            {
                result += item.ToString() + "_";
            }
            return result.TrimEnd('_');
        }

        /// <summary>
        /// Convers a "_" separated string to HashSet\<T>
        /// </summary>
        /// <param name="set"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static HashSet<T> StringToEnumSet<T>(this HashSet<T> set, Type Te, string s)
        {
            return new HashSet<T>(s.Split("_").Select(a => (T)Enum.Parse(Te, a)));
        }
    }
}
