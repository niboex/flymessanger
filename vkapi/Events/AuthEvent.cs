using System;
using log4net;
using vkapi.Auth;
using dict = System.Collections.Generic.Dictionary<string, string>;

namespace vkapi.Events
{
    public class EventArguments
    {
        private string _captchaImageSrc;

        public string CaptchaImageSrc
        {
            get { return _captchaImageSrc; }
            set { _captchaImageSrc = value; }
        }
    }

    class AuthEventHandler
    {
        public event AuthEvent IssetLogin;
        public event AuthEvent IssetCaptcha;
        public event AuthEvent IseetCode;

        private ILog log = LogHelper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType);

        public virtual dict OnIssetLogin()
        {
            var handler = IssetLogin;
            if (handler != null)
            {
                log.Info("Вызванно событие IssetLogin");
                return handler(this, new EventArguments());
            }

            return null;
        }

        public virtual dict OnIssetCode()
        {
            var handler = IseetCode;
            if (handler != null)
            {
                log.Info("Вызванно событие IssetCode");
                return handler(this, new EventArguments());
            }

            return null;
        }

        public virtual dict OnIssetCaptcha(string src)
        {
            var handler = IssetCaptcha;
            EventArguments arguments = new EventArguments();
            arguments.CaptchaImageSrc = src;

            if (handler != null)
            {
                log.Info("Вызванно событие IssetCapthcha");
                return handler(this, arguments);
            }

            return null;
        }
    }
    

    public class AuthEventMethods
    {
        public virtual dict OnIssetCaptcha(Object sender, EventArguments events)
        {
            // Возвращаем массив dict с ключем "captcha"
            Console.WriteLine(events.CaptchaImageSrc);

            return new dict() {
                {"captcha", "This example captcha code."}
            };
        }

        public virtual dict OnIssetLogin(Object sender, EventArguments evetns)
        {
            // Возвращаем массив dict с ключами "email", "pass"

            return new dict() {
                {"email","mymail@mail.com"}, {"pass", "password12356"}
            };
        }

        public virtual dict OnIssetCode(Object sender, EventArguments events)
        {
            // Возвращаем массив dict с ключем "code"
            return new dict() {
                {"code","123548"}
            };
        }
    }
}
