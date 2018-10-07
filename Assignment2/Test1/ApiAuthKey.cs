namespace Test1
{
    public class ApiAuthKey
    {
        public string key;
        public string adminKey;
        public ApiAuthKey(string apiKey, string apiAdminKey)
        {
            key = apiKey;
            adminKey = apiAdminKey;
        }
    }
}