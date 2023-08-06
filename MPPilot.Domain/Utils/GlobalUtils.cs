using Newtonsoft.Json;
using System.Reflection;

namespace MPPilot.Domain.Utils
{
    public static class GlobalUtils
    {
        public static T GetDeepCopy<T>(this T source)
        {
            var serializedObject = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serializedObject);
        }

        public static List<PropertyInfo> GetPropertiesDifference<T>(this T oldObject, T newObject)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();

            var changedProperies = new List<PropertyInfo>();
            foreach (var property in properties)
            {
                object oldValue = property.GetValue(oldObject);
                object newValue = property.GetValue(newObject);

                if (!Equals(oldValue, newValue) && !IsNullAndEmptyString(oldValue, newValue))
                    changedProperies.Add(property);
            }

            return changedProperies;
        }

        private static bool IsNullAndEmptyString(object value1, object value2)
        {
            return value1 == null && string.IsNullOrEmpty(value2 as string) || value2 == null && string.IsNullOrEmpty(value1 as string);
        }
    }
}
