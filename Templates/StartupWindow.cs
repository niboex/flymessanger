using System.Windows;
using flymessanger.Core.Events;

using vkapi.Auth;
using vkapi.Objects;

namespace flymessanger.Templates
{
    public partial class StartupWindow : Window
    {
        public StartupWindow()
        {
            InitializeComponent();
            Connect connecter = new Connect(new AuthEventOverride());

            UserAccessKey account = connecter.UserAccessKey();
            FileSession fileSession = new FileSession();
            fileSession.SaveSession(account);
        }
    }
}
