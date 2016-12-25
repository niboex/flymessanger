using System;
using vkapi;
using dict = System.Collections.Generic.Dictionary<string, string>;

namespace Interface
{
    class WPF_Event : MaterialEvent
    {
        public override dict OnIssetLogin(object sender, EventArgs evetns)
        {
            Login_I i1 = new Login_I();
            i1.ShowDialog();
            return new dict() { { "email", i1.login }, { "pass", i1.pass } };
        }

        public override dict OnIssetCode(object sender, EventArguments events)
        {
            Check_code c1 = new Check_code();
            c1.ShowDialog();
            return new dict() { { "code", c1.code } };
        }
        public override dict OnIssetCaptcha(object sender, EventArguments events)
        {

            return base.OnIssetCaptcha(sender, events);
        }
    }
}
