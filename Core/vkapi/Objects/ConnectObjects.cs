using System;
using System.Collections.Generic;

namespace flymessanger.Core.vkapi.Objects
{
    #region ServiceAccessKey

    public class ServiceAccessKey
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
    }
    #endregion

    #region UserAccessKey

    [Serializable]
    public class AccessToken
    {
        public string access_token = null;
        public long expires_in = 0;
        public string user_id = null;
    }

    [Serializable]
    public class UserAccessKey
    {
        public AccessToken accessToken { get; set; }
        public string code = null;
    }

    public class Captcha
    {
        public bool isIsset = false;
        public string source { get; set; }
    }

    public class HtmlContainer
    {

        public Dictionary<string, string> FDictionary { get; set; }
        public Dictionary<string, string> IDictionary { get; set; }
        public Captcha Captcha { get; set; }

    }
    #endregion

}
