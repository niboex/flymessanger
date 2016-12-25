using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using dict = System.Collections.Generic.Dictionary<string, string>;

namespace vkapi
{
    public class Settings
    {
        internal string Protocol = "https://";
        internal string Oauth = "oauth.vk.com";
        internal string Api = "api.vk.com";

        public string ClientId;
        public string ClientSecret;

        internal string RedirectUri = "blank.html";
        internal string Display = "mobile";
        internal string Version = "5.60";
        internal string Type = "code";

        public string scope = "";
        public string[] Permissions;

        public string Scope
        {
            get { return Functions.GenerateScope(Permissions); }
            set { scope = value; }
        }
    }

    public delegate dict AuthEvent(Object sender, EventArguments events);
    public class Auth
    {
        private Settings Settings;
        private VEventHandler _event;

        public Auth(Settings settings, MaterialEvent materialEvent = null)
        {
            Settings = settings;
            _event = new VEventHandler();

            if (materialEvent == null)
                materialEvent = new MaterialEvent();

            _event.IssetLogin += new AuthEvent(materialEvent.OnIssetLogin);
            _event.IseetCode += new AuthEvent(materialEvent.OnIssetCode);
            _event.IssetCaptcha += new AuthEvent(materialEvent.OnIssetCaptcha);

        }

        /// <summary>
        /// Авторизация на сайте vk.com как пользователь.
        /// </summary>
        /// <returns>string code</returns>
        public string Authorization()
        {
            dict InputArgument, FormArgument, eventObject;

            String html, code = null, url = Functions.GenerateAuthorize(Settings);

            using (Processor processor = new Processor())
            {
                processor.Headers.Add("user-agent",
                    "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                processor.Encoding = System.Text.Encoding.UTF8;

                html = processor.DownloadString(@url);

                while (code == null)
                {
                    InputArgument = Functions.GetInputArguments(html);
                    FormArgument = Functions.GetFormArguments(html);

                    if (FormArgument["action"] == "login")
                    {
                        if (InputArgument.ContainsKey("captcha_sid"))
                        {
                            string imgSrc = Functions.GetCaptchaImg(html);
                            InputArgument.Add("captcha_key", _event.OnIssetCaptcha(imgSrc)["captcha"]);
                        }

                        eventObject = _event.OnIssetLogin();
                        
                        InputArgument.Add("email", eventObject["email"]);
                        InputArgument.Add("pass", eventObject["pass"]);

                        html = processor.UploadString(FormArgument["url"], Functions.PostStringConverter(InputArgument));
                    }

                    if (FormArgument["action"] == "authcheck_code")
                    {

                        url = "https://m.vk.com/" + FormArgument["url"];
                        eventObject = _event.OnIssetCode();

                        if (eventObject != null)
                            InputArgument.Add("code", eventObject["code"]);

                        InputArgument.Add("remember", "1");

                        html = processor.UploadString(url, Functions.PostStringConverter(InputArgument));
                    }

                    if (FormArgument["action"] == "blank")
                    {
                        code = processor.ResponseUri.ToString().Split('#')[1].Split('=')[1];
                    }

                    if (FormArgument["action"] == "grant_access")
                    {
                        url = FormArgument["url"];
                        InputArgument.Clear();

                        html = processor.DownloadString(@url);
                    }
                }
            }

            return code;
        }

        /// <summary>
        /// Отправляем запрос на получения ключа доступа для приложения
        /// </summary>
        /// <param name="code"></param>
        /// <returns>AccessToken</returns>
        public AccessToken GettingAccessToken(string code = null)
        {
            if (code == null)
                code = Authorization();

            AccessToken accessToken;

            using (Processor processor = new Processor())
            {
                string url = Functions.GenerateAccessRequest(Settings, code);
                string json = processor.DownloadString(@url);
                accessToken = JsonConvert.DeserializeObject<AccessToken>(json);

                accessToken.expires_in = ((DateTime.UtcNow.Ticks - 621355968000000000) / 10000) + accessToken.expires_in;
            }

            return accessToken;
        }

        /// <summary>
        /// Сохраняем токен
        /// </summary>
        /// <param name="token"></param>
        public void SaveAccessToken(AccessToken token)
        {
            string Path = @"./temp/access_token.bin";

            if (!File.Exists(Path))
                File.Create(Path).Close();

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(Path, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, token);
            stream.Close();
        }

        /// <summary>
        /// Загружаем токен
        /// </summary>
        /// <returns></returns>
        public AccessToken LoadAccessToken()
        {
            AccessToken accessToken = new AccessToken();

            string Path = @"./temp/access_token.bin";
            if (File.Exists(Path))
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read);

                try
                {
                    accessToken = (AccessToken) formatter.Deserialize(stream);
                    stream.Close();
                }
                catch (Exception){ stream.Close(); }


            }

            if (accessToken.access_token == null || !Functions.CheckTimeLive(accessToken))
            {
                accessToken = GettingAccessToken();
                SaveAccessToken(accessToken);

            }

            return accessToken;
        }

    }
}