using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace vkapi
{
    public class Configuration
    {
        public Application application { get; set; }

        public Configuration()
        {
           
            LoadingConfiguration();
        }

        private bool CheckConfigurationFile()
        {
            if (!File.Exists(@"./Configuration/application.json"))
            {
                Console.WriteLine("Loading configuration failed !");
                Console.ReadLine();
                Environment.Exit(0);
            }

            return true;
        }

        private void LoadingConfiguration()
        {
            if (CheckConfigurationFile())
            {
                string config;
                using (StreamReader streamReader = new StreamReader(@"./Configuration/application.json"))
                {
                    config = streamReader.ReadToEnd();
                    streamReader.Close();
                }

                application = JsonConvert.DeserializeObject<Application>(config);
            }
        }

        public void SavingConfiguration()
        {
            if (CheckConfigurationFile())
            {
                using (StreamWriter streamWriter = new StreamWriter(@"./Configuration/application.json"))
                {
                    streamWriter.Write(JsonConvert.SerializeObject(application));
                }
            }
        }
    }


    public class Application
    {
        public string protocol { get; set; }
        public Url url { get; set; }
        public Security security { get; set; }
        public string display { get; set; }
        public string version { get; set; }
        public string[] scope { get; set; }
        public string type { get; set; }
        public string temp_folder { get; set; }
        public string conf_folder { get; set; }
    }

    public class Url
    {
        public string oauth { get; set; }
        public string api { get; set; }
        public string uri { get; set; }
    }

    public class Security
    {
        public string app_id { get; set; }
        public string app_sec { get; set; }
    }
}
