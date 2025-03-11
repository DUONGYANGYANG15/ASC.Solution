using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text.Json;

public static class SessionExtensions
{
    public static void SetSession<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }

    public static T GetSession<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default : JsonConvert.DeserializeObject<T>(value);
    }
}
