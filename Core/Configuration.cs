using System;
using System.IO;
using flymessanger.Core.vkapi.Objects;
using log4net;
using Newtonsoft.Json;

namespace flymessanger.Core
{
    public class Configuration
    {
        private ILog log = LogHelper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType);
        public Application application { get; set; }

        public Configuration()
        {
            LoadingConfiguration();
        }

        private bool CheckConfigurationFile()
        {
            if (!File.Exists(@"./Configuration/application.json"))
            {
                log.Fatal("Ошибка загрузки конфигурации \"application.json\"");
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
}
