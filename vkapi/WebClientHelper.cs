using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;


namespace vkapi
{
    public class WebClientHelper : WebClient
    {

        private String TempFolder = @"./Temps";
        private String SessionPath;

        Uri _responseUri;

        public CookieContainer CookieContainer { get; set; }
        public Uri ResponseUri { get { return _responseUri; } }

        public WebClientHelper()
        {
            Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            Encoding = Encoding.UTF8;
            SessionPath = CheckSessionFolder();
            CookieContainer = CookieLoad();
        }

        /// <summary>
        /// Преобразовываем массив данных в POST запрос
        /// </summary>
        /// <param name="sourceData"></param>
        /// <returns>string</returns>
        public static string PostStringConverter(Dictionary<string, string> sourceData)
        {
            string postData = "";

            foreach (KeyValuePair<string, string> line in sourceData)
                postData += line.Key + "=" + line.Value + "&";

            return postData.Remove(postData.Length - 1);
        }


        /// <summary>
        /// Проверяем дериктории и файл сессии
        /// </summary>
        /// <returns>path</returns>
        private string CheckSessionFolder()
        {

            if (!File.Exists(TempFolder))
                Directory.CreateDirectory(TempFolder);

            if (!File.Exists(TempFolder + "/session.bin"))
                File.Create(TempFolder + "/session.bin").Close();

            return TempFolder + "/session.bin";
        }

        /// <summary>
        /// Загружаем файл session.bin (Cookie) для эмитации работы браузера
        /// </summary>
        /// <returns>CookieContainer</returns>
        private CookieContainer CookieLoad()
        {
            CookieContainer cookieContainer = new CookieContainer();

            if (File.Exists(SessionPath))
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(SessionPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                try
                {
                    cookieContainer = (CookieContainer)formatter.Deserialize(stream);
                    stream.Close();
                }
                catch (Exception)
                {
                    stream.Close();
                }

            }

            return cookieContainer;
        }

        /// <summary>
        /// Сохраняем сессию, для последующией работы.
        /// </summary>
        public void CookieSave() /* Сохраняем cookie в файле, для последующей работы */
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(SessionPath, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, CookieContainer);
            stream.Close();
        }

        /// <summary>
        /// Удаляем куки
        /// </summary>
        public void CookieDel()
        {
            if (File.Exists(SessionPath))
                File.Delete(SessionPath);
        }

        /// <summary>
        /// Преобразовываем WebClient для работы с файлом session.bat
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);

            HttpWebRequest webRequest = request as HttpWebRequest;
            if (webRequest != null)
            {
                webRequest.CookieContainer = CookieContainer;
            }

            return request;
        }

        /// <summary>
        /// Метод сохранения сессии после закрытия класса
        /// </summary>
        public new void Dispose()
        {
            CookieSave();
        }

        /// <summary>
        /// Делаем обработку перенаправлений на классе WebClient
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);
            // ReSharper disable once PossibleNullReferenceException
            _responseUri = response.ResponseUri;
            return response;
        }
    }
}
