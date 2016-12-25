namespace vkapi.Objects
{

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
