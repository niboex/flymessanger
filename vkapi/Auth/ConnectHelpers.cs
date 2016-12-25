using System.Collections.Generic;
using log4net;
using vkapi.Objects;
using dict = System.Collections.Generic.Dictionary<string, string>;

namespace vkapi.Auth
{
    class ConnectHelpers
    {
        /// <summary>
        /// Преобразовываем лист прав доступа в строку.
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns>String cope</returns>
        public static string GenerateScope(string[] permission)
        {
            ILog log = LogHelper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType);
            string scope = "";

            foreach (var line in permission)
            {
                scope += line + ",";
            }

            log.Info("Установленные права: " + scope.Remove(scope.Length - 1));
            return scope.Remove(scope.Length - 1);
        }

        /// <summary>
        /// Преобразовываем массив данных в строку GET запроса
        /// </summary>
        /// <returns></returns>
        public static string GenerateAuthorize(Application app)
        {
            ILog log = LogHelper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType);
            dict parameters = new dict
            {
                {"client_id", app.security.app_id},
                {"redirect_uri", app.protocol + app.url.oauth + "/" + app.url.uri},
                {"display", app.display},
                {"scope", GenerateScope(app.scope)},
                {"response_type", app.type},
                {"v", app.version}
            };

            string link = app.protocol + app.url.oauth + "/authorize?";
            log.Debug("Генерируем ссылку авторизации " + link);
            foreach (KeyValuePair<string, string> line in parameters)
            {
                log.Debug("Параметр " + line.Key + ":" + line.Value);
                link += line.Key + "=" + line.Value + "&";
            }

            return link.Remove(link.Length - 1);
        }

        /// <summary>
        /// Преобразовываем переданные данных в строку для GET запроса
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GenerateAccessRequest(Application app, string code)
        {
            ILog log = LogHelper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType);
            log.Info("Генерируем ссылку для получения токена");
            dict arguments = new dict
            {
                {"client_id", app.security.app_id},
                {"client_secret", app.security.app_sec},
                {"redirect_uri", app.protocol + app.url.oauth + "/" + app.url.uri},
                {"code", code}
            };

            string url = app.protocol + app.url.oauth + "/access_token?";
            foreach (KeyValuePair<string, string> line in arguments)
                url += line.Key + "=" + line.Value + "&";

            return url.Remove(url.Length - 1);
        }
    }
}
