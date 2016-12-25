using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Interface
{
    /// <summary>
    /// Логика взаимодействия для Check_code.xaml
    /// </summary>
    public partial class Check_code : Window
    {
        public string code { get; set; }
        public Check_code()
        {
            InitializeComponent();
        }

        private void send_code_Click(object sender, RoutedEventArgs e)
        {
            code = Code_box.Text;
        }
    }
}
