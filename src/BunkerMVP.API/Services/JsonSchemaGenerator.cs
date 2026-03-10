using System.Text.Json;

namespace BunkerMVP.API.Services;

public static class JsonSchemaGenerator
{
    public static string Generate(Type type)
    {
        var schema = BuildSchema(type, new HashSet<Type>());
        return JsonSerializer.Serialize(schema);
    }

    private static Dictionary<string, object> BuildSchema(Type type, HashSet<Type> visited)
    {
        // Unwrap Nullable<T>
        var underlying = Nullable.GetUnderlyingType(type);
        if (underlying != null)
            return BuildSchema(underlying, visited);

        if (type == typeof(string))
            return new Dictionary<string, object> { ["type"] = "string" };

        if (type == typeof(int) || type == typeof(long))
            return new Dictionary<string, object> { ["type"] = "integer" };

        if (type == typeof(decimal) || type == typeof(double) || type == typeof(float))
            return new Dictionary<string, object> { ["type"] = "number" };

        if (type == typeof(DateTime) || type == typeof(DateTimeOffset))
            return new Dictionary<string, object> { ["type"] = "string", ["format"] = "date-time" };

        if (type == typeof(bool))
            return new Dictionary<string, object> { ["type"] = "boolean" };

        // IEnumerable<T> (but not string)
        var enumInterface = type.GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        if (enumInterface != null)
        {
            var itemType = enumInterface.GetGenericArguments()[0];
            return new Dictionary<string, object>
            {
                ["type"] = "array",
                ["items"] = BuildSchema(itemType, visited)
            };
        }

        // Complex class
        if (type.IsClass && type != typeof(object))
        {
            if (visited.Contains(type))
                return new Dictionary<string, object> { ["type"] = "object" };

            visited.Add(type);

            var properties = new Dictionary<string, object>();
            foreach (var prop in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                var camelName = JsonNamingPolicy.CamelCase.ConvertName(prop.Name);
                properties[camelName] = BuildSchema(prop.PropertyType, visited);
            }

            visited.Remove(type);

            return new Dictionary<string, object>
            {
                ["type"] = "object",
                ["properties"] = properties
            };
        }

        // Fallback
        return new Dictionary<string, object> { ["type"] = "string" };
    }
}
