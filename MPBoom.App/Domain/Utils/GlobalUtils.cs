using Newtonsoft.Json;
using System.Reflection;

namespace MPBoom.App.Domain.Utils
{
    public static class GlobalUtils
    {
        public static T GetDeepCopy<T>(this T source)
        {
            if (source == null)
                return default;

            string serializedObject = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serializedObject);
        }

        public static List<PropertyInfo> GetPropertiesDifference<T>(this T oldObject, T newObject)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();

            var changedProperies = new List<PropertyInfo>();
            foreach (var property in properties)
            {
                object value1 = property.GetValue(oldObject);
                object value2 = property.GetValue(newObject);

                if (!Equals(value1, value2) && !IsNullAndEmptyString(value1, value2))
                    changedProperies.Add(property);
            }

            return changedProperies;
        }

        private static bool IsNullAndEmptyString(object value1, object value2)
        {
            return (value1 == null && string.IsNullOrEmpty(value2 as string)) ||
                   (value2 == null && string.IsNullOrEmpty(value1 as string));
        }
    }
}
