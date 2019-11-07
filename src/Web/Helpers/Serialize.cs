using Newtonsoft.Json;

namespace Web.Helpers
{
    public static class Serialize
    {
        public static string SerializeObj(object data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}
