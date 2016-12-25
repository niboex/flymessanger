using System.Windows;
using vkapi;
using System.IO;


namespace Interface
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Settings set = new Settings();
            set.ClientId = "5783292";
            set.ClientSecret = "C3id6lTN0wQVmnU5Urtw";
            set.Permissions = new string[] { "music", "message" };
            WPF_Event w1 = new WPF_Event();
            Auth a1 = new Auth(set, w1);
            a1.LoadAccessToken();
        }

        private void send_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            File.Delete(@"D:\Development\flymessanger\Interface\Interface\bin\Debug\temp\access_token.bin");
            File.Delete(@"D:\Development\flymessanger\Interface\Interface\bin\Debug\temp\session.bin");
        }
    }
}
