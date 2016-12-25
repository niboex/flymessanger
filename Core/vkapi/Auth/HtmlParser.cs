using System;
using System.Text.RegularExpressions;
using flymessanger.Core.vkapi.Objects;
using log4net;
using dict = System.Collections.Generic.Dictionary<string, string>;

namespace flymessanger.Core.vkapi.Auth
{
    class HtmlParser
    {
        /// <summary>
        /// Проверяем есть ли элемент в списке
        /// </summary>
        /// <param name="argument"></param>
        /// <returns>boolean</returns>
        private static bool ExistInArray(String argument)
        {
            String[] existParams = new string[] { "_origin", "ip_h", "lg_h", "to", "code", "captcha_sid" };
            foreach (String line in existParams)
            {
                if (line == argument)
                {
                    return true;
                }
                    
            }
            return false;
        }

        /// <summary>
        /// Выбираем из html страницы параметры input полей.
        /// </summary>
        /// <param name="html"></param>
        /// <returns>dict</returns>
        private static dict InputArguments(string html)
        {
            ILog log = LogHelper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType);

            dict array = new dict();
            String pattern = "input[^>]*[^>]*value=\"([^\"])*\"";

            foreach (Match match in Regex.Matches(html, pattern))
            {
                String[] input = match.ToString().Replace("input ", "").Split(' ');
                if (input[1].Split('=')[0] == "name" && ExistInArray(input[1].Split('=')[1].Replace("\"", "")))
                {
                    if (input[2].Split('=')[0] == "value")
                    {
                        log.Debug("[InputArgunents] Добавленно значение " + input[1].Split('=')[1].Replace("\"", "") + ":" + input[2].Split('=')[1].Replace("\"", ""));
                        array.Add(input[1].Split('=')[1].Replace("\"", ""), input[2].Split('=')[1].Replace("\"", ""));
                    }
                }
            }
            return array;
        }

        /// <summary>
        /// Выбираем из html страницы параметры form поля.
        /// </summary>
        /// <param name="html"></param>
        /// <returns>string</returns>
        private static dict FromArgument(string html)
        {
            ILog log = LogHelper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType);

            dict array = new dict();
            String pattern = "form[^>]*[^>]*action=\"([^\"])*\"";

            log.Debug("Получаем значения формы");
            foreach (Match match in Regex.Matches(html, pattern))
            {
                String[] input = match.ToString().Replace("form ", "").Split(' ');

                if (input[0].Split('=')[0] == "method")
                    log.Debug("Добавляем метод: " + input[0].Split('=')[1].Replace("\"", "").ToUpper());
                    array.Add("method", input[0].Split('=')[1].Replace("\"", "").ToUpper());

                if (input[1].Split('=')[0] == "action")
                    log.Debug("Добавляем действие: " + input[1].Split('"')[1]);
                    array.Add("url", input[1].Split('"')[1]);

                log.Debug("Добавляем адрес: " + array["url"].Split('?')[1].Split('&')[0].Split('=')[1]);
                array.Add("action", array["url"].Split('?')[1].Split('&')[0].Split('=')[1]);

            }

            if (array.Count == 0)
            {
                log.Debug("Форма не найдена. Передаем Пустую страницу");
                array.Add("action", "blank");
            }

            return array;
        }

        /// <summary>
        /// Вытаскиваем ссылку капчи
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static Captcha  GetCaptchaImg(string html)
        {
            ILog log = LogHelper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType);
            string src = "", pattern = @"img\b[^\<\>]+?\bsrc\s*=\s*[""'](?<L>.+?)[""'][^\<\>]*?\>";
            Captcha cp = new Captcha();

            foreach (Match match in Regex.Matches(html, pattern))
            {
                string id = match.ToString().Split(' ')[1].Split('=')[1].Replace("\"", "");
                if (id == "captcha")
                {
                    cp.source = match.ToString().Split(' ')[3].Split('=')[1].Replace("\"", "") + "=" +
                          match.ToString().Split(' ')[3].Split('=')[2].Replace("\"", "") + "=" +
                          match.ToString().Split(' ')[3].Split('=')[3].Replace("\"", "");
                    log.Info("Обнаруженна каптча: " + src);
                    cp.isIsset = true;
                }
            }
            return cp;
        }

        /// <summary>
        /// Собираем данные с полученной страницы
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static HtmlContainer CollectingData(string source)
        {
            ILog log = LogHelper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType);
            HtmlContainer container = new HtmlContainer();

            log.Info("Собираем данные с полученной страницы");
            container.FDictionary = FromArgument(source);
            container.IDictionary = InputArguments(source);
            container.Captcha = GetCaptchaImg(source);

            return container;
        }
    }
}