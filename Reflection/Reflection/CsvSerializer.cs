using System.Reflection;

namespace Reflection
{
    public static class CsvSerializer
    {
        private static readonly Dictionary<Type, FieldInfo[]> _fieldsCache = new Dictionary<Type, FieldInfo[]>();

        public static string Serialize(object obj)
        {
            Type type = obj.GetType();
            FieldInfo[] fields;

            if (!_fieldsCache.TryGetValue(type, out fields))
            {
                fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .OrderBy(f => f.Name)
                    .ToArray();

                _fieldsCache[type] = fields;
            }

            var values = fields.Select(f => f.GetValue(obj)?.ToString() ?? "");

            return string.Join(",", values);
        }

        public static T Deserialize<T>(string csv) where T : new()
        {
            Type type = typeof(T);
            FieldInfo[] fields;

            if (!_fieldsCache.TryGetValue(type, out fields))
            {
                fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .OrderBy(f => f.Name)
                    .ToArray();

                _fieldsCache[type] = fields;
            }

            string[] values = csv.Split(',');

            if (values.Length != fields.Length)
            {
                throw new ArgumentException("CSV values count does not match fields count.");
            }

            T obj = new T();

            for (int i = 0; i < fields.Length; i++)
            {
                object value = Convert.ChangeType(values[i], fields[i].FieldType);

                fields[i].SetValue(obj, value);
            }

            return obj;
        }
    }
}
