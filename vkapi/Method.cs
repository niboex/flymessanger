using System;
using System.Collections.Generic;
using System.Text;
using dict = System.Collections.Generic.Dictionary<string, string>;

namespace vkapi
{
    public class Method
    {
        private Settings _settings;
        private AccessToken _accessToken;

        public Method(Settings settings, AccessToken accessToken)
        {
            _settings = settings;
            _accessToken = accessToken;
        }

        public void Get(string method, dict parameters)
        {
            string methodData = _settings.Protocol + _settings.Api + "/method/" + method + "?";
            foreach (KeyValuePair<string, string> parameter in parameters)
                methodData += parameter.Key + "=" + parameter.Value + "&";

            methodData += "access_token=" + _accessToken.access_token + "&v=" + _settings.Version;

            using (Processor processor = new Processor())
            {
                processor.Encoding = Encoding.UTF8;

                string response = processor.DownloadString(methodData);
                Console.WriteLine(response);
            }

            Console.ReadLine();

        }
    }
}