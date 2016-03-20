using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Odn.WebApiClient
{
    internal static class InternalExtension
    {
        public static string FullMessage(this Exception ex)
        {
            if (ex.InnerException == null)
                return ex.Message;
            else
                return string.Format("{0} -> {1}", ex.Message, ex.InnerException.FullMessage());
        }

        public static string FullStackTrace(this Exception ex)
        {
            if (ex.InnerException == null)
                return ex.StackTrace;
            else
                return string.Format("{0} -> {1}", ex.StackTrace, ex.InnerException.FullStackTrace());
        }

        /// <summary>
        /// Merge Objects to RouteValueDictionary
        /// </summary>
        /// <param name="routeValues"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static RouteValueDictionary Mo(this RouteValueDictionary routeValues, params object[] objs)
        {
            foreach (var o in objs)
            {
                if (o != null)
                    GetKeyValueObject(o).ToList().ForEach(x => routeValues.Add(x.Key, x.Value));
            }
            return routeValues;
        }

        private static IEnumerable<KeyValuePair<string, object>> GetKeyValueObject(object obj)
        {
            return obj.GetType().GetProperties().Where(x => x.PropertyType.IsPrimitive
                || x.PropertyType.IsValueType
                || (Nullable.GetUnderlyingType(x.PropertyType) != null && (Nullable.GetUnderlyingType(x.PropertyType).IsValueType || Nullable.GetUnderlyingType(x.PropertyType).IsPrimitive))
                || x.PropertyType == typeof(string)).Select(x => new KeyValuePair<string, object>(x.Name, x.GetValue(obj)));
        }

        static bool IsNullable<T>(T obj)
        {
            if (obj == null) return true; // obvious
            Type type = typeof(T);
            if (!type.IsValueType) return true; // ref-type
            if (Nullable.GetUnderlyingType(type) != null) return true; // Nullable<T>
            return false; // value-type
        }
    }
}
