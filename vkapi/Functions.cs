using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using dict = System.Collections.Generic.Dictionary<string, string>;

namespace vkapi
{
    public class Functions
    {
        /// <summary>
        /// Преобразовываем лист прав доступа в строку.
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns>String cope</returns>
        public static string GenerateScope(Application app)
        {
            string scope = "";

            foreach (var line in app.scope)
            {
                scope += line + ",";
            }

            return scope.Remove(scope.Length - 1);
        }

        /// <summary>
        /// Проверяем есть ли элемент в списке
        /// </summary>
        /// <param name="argument"></param>
        /// <returns>boolean</returns>
        private static bool ExistInArray(String argument)
        {
            String[] existParams = new string[] {"_origin", "ip_h", "lg_h", "to", "code", "captcha_sid"};
            foreach (String line in existParams)
            {
                if (line == argument)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Выбираем из html страницы параметры input полей.
        /// </summary>
        /// <param name="html"></param>
        /// <returns>dict</returns>
        public static dict GetInputArguments(String html)
        {
            dict array = new dict();
            String pattern = "input[^>]*[^>]*value=\"([^\"])*\"";

            foreach (Match match in Regex.Matches(html, pattern))
            {
                String[] input = match.ToString().Replace("input ", "").Split(' ');
                if (input[1].Split('=')[0] == "name" && ExistInArray(input[1].Split('=')[1].Replace("\"", "")))
                {
                    if (input[2].Split('=')[0] == "value")
                    {
                        array.Add(input[1].Split('=')[1].Replace("\"", ""), input[2].Split('=')[1].Replace("\"", ""));
                    }
                }
            }

            return array;
        }

        /// <summary>
        /// Вытаскиваем ссылку капчи
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GetCaptchaImg(string html)
        {
            string src = "", pattern = @"img\b[^\<\>]+?\bsrc\s*=\s*[""'](?<L>.+?)[""'][^\<\>]*?\>";
            foreach (Match match in Regex.Matches(html, pattern))
            {
                string id = match.ToString().Split(' ')[1].Split('=')[1].Replace("\"", "");
                if (id == "captcha")
                {

                    src = match.ToString().Split(' ')[3].Split('=')[1].Replace("\"", "") + "=" +
                          match.ToString().Split(' ')[3].Split('=')[2].Replace("\"", "") + "=" +
                          match.ToString().Split(' ')[3].Split('=')[3].Replace("\"", "");
                }

            }

            return src;
        }

        /// <summary>
        /// Выбираем из html страницы параметры form поля.
        /// </summary>
        /// <param name="html"></param>
        /// <returns>string</returns>
        public static dict GetFormArguments(string html)
        {
            dict array = new dict();
            String pattern = "form[^>]*[^>]*action=\"([^\"])*\"";

            foreach (Match match in Regex.Matches(html, pattern))
            {
                String[] input = match.ToString().Replace("form ", "").Split(' ');

                if (input[0].Split('=')[0] == "method")
                    array.Add("method", input[0].Split('=')[1].Replace("\"", "").ToUpper());

                if (input[1].Split('=')[0] == "action")
                    array.Add("url", input[1].Split('"')[1]);

                array.Add("action", array["url"].Split('?')[1].Split('&')[0].Split('=')[1]);

            }

            if (array.Count == 0)
                array.Add("action", "blank");


            return array;
        }

        /// <summary>
        /// Преобразовываем массив данных в POST запрос
        /// </summary>
        /// <param name="sourceData"></param>
        /// <returns>string</returns>
        public static string PostStringConverter(dict sourceData)
        {
            string postData = "";

            foreach (KeyValuePair<string, string> line in sourceData)
                postData += line.Key + "=" + line.Value + "&";

            return postData.Remove(postData.Length - 1);
        }

        /// <summary>
        /// Преобразовываем массив данных в строку GET запроса
        /// </summary>
        /// <returns></returns>
        public static string GenerateAuthorize(Application app)
        {
            dict parameters = new dict
            {
                {"client_id", app.security.app_id},
                {"redirect_uri", app.protocol + app.url.oauth + "/" + app.url.uri},
                {"display", app.display},
                {"scope", GenerateScope(app)},
                {"response_type", app.type},
                {"v", app.version}
            };

            string link = app.protocol + app.url.oauth + "/authorize?";

            foreach (KeyValuePair<string, string> line in parameters)
                link += line.Key + "=" + line.Value + "&";

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
            dict arguments = new dict
            {
                {"client_id", app.security.app_id},
                {"client_secret", app.security.app_sec},
                {"redirect_uri", app.protocol + app.url.oauth + "/" + app.url.uri},
                {"code", code}
            };

            String url = app.protocol + app.url.oauth + "/access_token?";
            foreach (KeyValuePair<string, string> line in arguments)
                url += line.Key + "=" + line.Value + "&";

            return url.Remove(url.Length - 1);
        }

        /// <summary>
        /// Проверяем токен на валидность
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static bool CheckTimeLive(AccessToken accessToken)
        {
            long nowTime = (DateTime.UtcNow.Ticks - 621355968000000000) / 10000;

            if (nowTime < accessToken.expires_in)
                return true;
            return false;
        }

        /// <summary>
        /// Преобразовываем массив параметров в строку.
        /// </summary>
        /// <param name="fileds"></param>
        /// <returns></returns>
        public static string ListFiled(string[] fileds)
        {
            string data = "";
            foreach (string param in fileds)
                data += param + ",";

            return data.Remove(data.Length - 1);
        }

        public static string GenerateApi(Application app, string method)
        {
            return app.protocol + app.url.api + "/method/" + method + "?v=" + app.version;
        }
    }
}