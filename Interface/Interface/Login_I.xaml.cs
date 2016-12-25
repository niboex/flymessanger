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
    /// Логика взаимодействия для Login_I.xaml
    /// </summary>
    public partial class Login_I : Window
    {
        public string login { get; set; }
        public string pass { get; set; }
        public Login_I()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void nen_MouseDown(object sender, MouseButtonEventArgs e)
        {
            login = Login.Text;
            pass = Pass.Password;
            Close();
        }

        private void nen_MouseEnter(object sender, MouseEventArgs e)
        {
            nen.Opacity = 0;
            nen1.Opacity = 100;
        }

        private void nen_MouseLeave(object sender, MouseEventArgs e)
        {
            nen.Opacity = 100;
            nen1.Opacity = 0;
        }

        private void Login_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Login.Text == "" || Login.Text == "Логин")
            {
                Login.Clear();
            }
        }

        private void Login_MouseLeave(object sender, MouseEventArgs e)
        {
            if(Login.Text == "")
            {
                Login.Clear();
                Login.Text = "Логин";
            }
        }

        private void Pass_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Pass.Password == "" || Pass.Password == "123456")
            {
                Pass.Clear();
            }
        }

        private void Pass_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Pass.Password == "")
            {
                Pass.Clear();
                Pass.Password = "123456";
            }
        }
    }
}
