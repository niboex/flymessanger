using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace vkapi
{

    public class Processor : WebClient, IDisposable
    {

        private String TempFolder = @"./temp";
        private String SessionPath;

        Uri _responseUri;

        public CookieContainer CookieContainer { get; set; }
        public Uri ResponseUri { get { return _responseUri; } }

        public Processor()
        {
            SessionPath = CheckSessionFolder();
            CookieContainer = CookieLoad();
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
                    cookieContainer = (CookieContainer) formatter.Deserialize(stream);
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