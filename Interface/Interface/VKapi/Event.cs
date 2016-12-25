using System;
using System.Diagnostics;
using dict = System.Collections.Generic.Dictionary<string, string>;

namespace vkapi
{

    public class EventArguments : EventArgs
    {
        private string _captchaImageSrc;

        public string CaptchaImageSrc
        {
            get { return _captchaImageSrc; }
            set { _captchaImageSrc = value; }
        }
    }

    class VEventHandler
    {
        public event AuthEvent IssetLogin;
        public event AuthEvent IssetCaptcha;
        public event AuthEvent IseetCode;

        public virtual dict OnIssetLogin()
        {
            var handler = IssetLogin;
//            Debug.Assert(handler != null, "handler != null");
            if (handler != null)
                return handler(this, new EventArguments());

            return null;
        }

        public virtual dict OnIssetCode()
        {
            var handler = IseetCode;
            if (handler != null)
                return handler(this, new EventArguments());

            return null;
        }

        public virtual dict OnIssetCaptcha(string src)
        {
            var handler = IssetCaptcha;
            EventArguments arguments = new EventArguments();
            arguments.CaptchaImageSrc = src;

//            Debug.Assert(handler != null, "handler != null");
            if (handler != null)
                return handler(this, arguments);

            return null;
        }
    }

    public class MaterialEvent
    {
        public virtual dict OnIssetCaptcha(Object sender, EventArguments events)
        {
            // Возвращаем массив dict с ключем "captcha"

            Console.WriteLine(events.CaptchaImageSrc);

            return new dict() {
                {"captcha", "This example captcha code."}
            };
        }

        public virtual dict OnIssetLogin(Object sender, EventArgs evetns)
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