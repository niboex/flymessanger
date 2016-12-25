using System.Windows;

namespace flymessanger.Templates
{
    public partial class TfauthWindow: Window
    {
        public string code { get; set; }
        public TfauthWindow()
        {
            InitializeComponent();
        }

        private void send_code_Click(object sender, RoutedEventArgs e)
        {
            code = Code_box.Text;
        }
    }
}
