using System;

namespace vkapi
{

    [Serializable]
    public class AccessToken
    {
        public string access_token = null;
        public long expires_in = 0;
        public string user_id = null;
    }
}