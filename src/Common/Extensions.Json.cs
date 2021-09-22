using Newtonsoft.Json;

namespace Common
{
    public static partial class Extensions
    {
        public static T ConvertTo<T>(this object model)
        {
            if (model is T value)
            {
                return value;
            }

            var json = model.ToJson();
            var entity = json.FromJson<T>(default);
            return entity;
        }

        public static string ToJson(this object model, bool indented = false)
        {
            return JsonHelper.Instance.ToJson(model, indented);
        }

        public static T FromJson<T>(this string json, T defaultValue)
        {
            return JsonHelper.Instance.FromJson(json, defaultValue);
        }
    }

    #region helper

    public interface IJsonHelper
    {
        string ToJson(object model, bool indented = false);
        T FromJson<T>(string json, T defaultValue);
    }

    public class JsonHelper : IJsonHelper
    {
        public string ToJson(object model, bool indented = false)
        {
            if (model is string)
            {
                return model.ToString();
            }
            var settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //settings.Converters.Add(new IpAddressConverter());
            //settings.Converters.Add(new IpEndPointConverter());
            settings.Formatting = indented ? Formatting.Indented : Formatting.None;
            return JsonConvert.SerializeObject(model, settings);
        }

        public T FromJson<T>(string json, T defaultValue)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return defaultValue;
            }

            //If the json value is '[]' => declare the field as List<type>
            //If the json value is '{}' => declare the field IDictionary<type, type>
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static IJsonHelper Instance = new JsonHelper();
    }

    //internal class IpAddressConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type objectType)
    //    {
    //        return (objectType == typeof(IPAddress));
    //    }

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        IPAddress ip = (IPAddress)value;
    //        writer.WriteValue(ip.ToString());
    //    }

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        JToken token = JToken.Load(reader);
    //        return IPAddress.Parse(token.Value<string>());
    //    }
    //}
    //internal class IpEndPointConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type objectType)
    //    {
    //        return (objectType == typeof(IPEndPoint));
    //    }

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        IPEndPoint ep = (IPEndPoint)value;
    //        writer.WriteStartObject();
    //        writer.WritePropertyName("Address");
    //        serializer.Serialize(writer, ep.Address);
    //        writer.WritePropertyName("Port");
    //        writer.WriteValue(ep.Port);
    //        writer.WriteEndObject();
    //    }

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        JObject jo = JObject.Load(reader);
    //        IPAddress address = jo["Address"].ToObject<IPAddress>(serializer);
    //        int port = jo["Port"].Value<int>();
    //        return new IPEndPoint(address, port);
    //    }
    //}

    #endregion
}
