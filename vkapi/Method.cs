using System;
using System.Net;
using System.Text;
using System.Xml;
using dict = System.Collections.Generic.Dictionary<string, string>;

namespace vkapi
{

    public class Users : Configuration
    {
        private WebClient client = new WebClient();
        private AccessToken _token;

        public Users(AccessToken token)
        {
            _token = token;
            client.Encoding = Encoding.UTF8;
        }

        public void get(string user_ids, string[] fields, string name_case = "nom")
        {
            string container = Functions.GenerateApi(application, "users.get.xml") +
                               "&access_token=" + _token.access_token +
                               "&users_ids=" + user_ids +
                               "&name_case=" + name_case +
                               "&fields=" + Functions.ListFiled(fields);

            string output = client.DownloadString(container);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(output);

            Console.WriteLine(output);
        }
    }
}