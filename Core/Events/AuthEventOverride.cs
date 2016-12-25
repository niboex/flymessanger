using flymessanger.Core.vkapi.Events;
using flymessanger.Templates;
using dict = System.Collections.Generic.Dictionary<string, string>;

namespace flymessanger.Core.Events
{
    class AuthEventOverride : AuthEventMethods
    {
        public override dict OnIssetLogin(object sender, EventArguments events)
        {
            LoginWindow i1 = new LoginWindow();
            i1.ShowDialog();
            return new dict() { { "email", i1.login }, { "pass", i1.pass } };
        }

        public override dict OnIssetCode(object sender, EventArguments events)
        {
            TfauthWindow c1 = new TfauthWindow();
            c1.ShowDialog();
            return new dict() { { "code", c1.code } };
        }

//        public override dict OnIssetCaptcha(object sender, EventArguments events)
//        {
//
//            return base.OnIssetCaptcha(sender, events);
//        }
    }
}
