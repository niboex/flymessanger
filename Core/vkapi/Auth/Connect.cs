using System;
using System.Net;
using log4net;
using Newtonsoft.Json;
using flymessanger.Core.vkapi.Events;
using flymessanger.Core.vkapi.Objects;

using dict = System.Collections.Generic.Dictionary<string, string>;

namespace flymessanger.Core.vkapi.Auth
{

    public delegate dict AuthEvent(Object sender, EventArguments events);

    public class Connect : Configuration
    {
        private ILog logg;
        private AuthEventHandler _event;

        public Connect(AuthEventMethods eventMethods = null)
        {
            logg = LogHelper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType);
            _event = new AuthEventHandler();
            if (eventMethods == null)
            {
                logg.Debug("События не переопределены");
                logg.Debug("Загрузка стандарных событий");
                eventMethods = new AuthEventMethods();
            }

            logg.Info("Регистрация слушателей");
            _event.IssetLogin += eventMethods.OnIssetLogin;
            _event.IssetCaptcha += eventMethods.OnIssetCaptcha;
            _event.IseetCode += eventMethods.OnIssetCode;
        }

        /// <summary>
        /// Метод для получения сервисного кода.
        /// </summary>
        /// <returns></returns>
        public ServiceAccessKey ServiceAccessKey()
        {
            ServiceAccessKey key;
            string recuestStringData = application.protocol + application.url.oauth + "/access_token?" +
                                  "client_id=" + application.security.app_id +
                                  "&client_secret=" + application.security.app_sec +
                                  "&v=" + application.version +
                                  "&grant_type=client_credentials";

            try
            {
                using (WebClient client = new WebClientHelper())
                {
                    logg.Info("Оправляем запрос на сервеисный ключ");
                    key = JsonConvert.DeserializeObject<ServiceAccessKey>(client.DownloadString(recuestStringData));
                }

                logg.Info("Ключ: " + key.access_token);
                return key;
            }
            catch (Exception e)
            {
                logg.Error("Ошибка получения сервисного ключа.\n", e);
            }

            return new ServiceAccessKey();
        }

        /// <summary>
        /// Получение ключа доступа для пользователя
        /// </summary>
        /// <returns></returns>
        public UserAccessKey UserAccessKey()
        {
            UserAccessKey account = new UserAccessKey();
            HtmlContainer container;
            dict eventObject;
            string html;

            using (WebClientHelper helper = new WebClientHelper())
            {
                logg.Info("Запрос на получение ключа доступа пользователя");
                html = helper.DownloadString(ConnectHelpers.GenerateAuthorize(application));

                while (account.code == null)
                {
                    container = HtmlParser.CollectingData(html);

                    if (container.FDictionary["action"] == "login")
                    {
                        logg.Info("Авторизация пользователя");
                        if (container.Captcha.isIsset)
                        {
                            logg.Info("Ожидание ввода пользователем капчи");
                            container.IDictionary.Add("captcha_key", _event.OnIssetCaptcha(container.Captcha.source)["captcha"]);
                            logg.Info("Код с картинки: " + container.IDictionary["captcha_key"]);
                        }

                        eventObject = _event.OnIssetLogin();
                        logg.Info("Ожидание ввода логина и пароля");

                        container.IDictionary.Add("email", eventObject["email"]);
                        container.IDictionary.Add("pass", eventObject["pass"]);

                        logg.Info("Логин: " + eventObject["email"]);
                        logg.Info("Пароль: " + new string('*', eventObject["pass"].Length));
                        
                        html = helper.UploadString(container.FDictionary["url"],
                            WebClientHelper.PostStringConverter(container.IDictionary));

                    }

                    if (container.FDictionary["action"] == "authcheck_code")
                    {
                        logg.Info("Ожидаение ввода кода доступа");
                        eventObject = _event.OnIssetCode();
                        logg.Debug("Код: " + eventObject["code"]);

                        container.IDictionary.Add("code", eventObject["code"]);
                        container.IDictionary.Add("remember", "1");

                        html = helper.UploadString(
                            "https://m.vk.com/" + container.FDictionary["url"],
                            WebClientHelper.PostStringConverter(container.IDictionary)
                        );
                    }

                    if (container.FDictionary["action"] == "grant_access")
                    {
                        logg.Info("Подтверждение правил доступа");
                        container.IDictionary.Clear();
                        html = helper.DownloadString(container.FDictionary["url"]);
                    }

                    if (container.FDictionary["action"] == "blank")
                    {
                        account.code = helper.ResponseUri.ToString().Split('#')[1].Split('=')[1];
                        logg.Info("Получен код пользователя");
                        logg.Info("Код: " + account.code);
                    }
                }

                string jsonResponse = helper.DownloadString(ConnectHelpers.GenerateAccessRequest(application, account.code));
                AccessToken accessToken = JsonConvert.DeserializeObject<AccessToken>(jsonResponse);
                account.accessToken = accessToken;
                logg.Info("Токен получен: " + accessToken.access_token);

                helper.CookieSave();
            }

            return account;
        }
    }
}
