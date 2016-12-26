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
        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) //Обрабокта события перетаскивания окна
        {
            DragMove();
        }

        #region //События наведения мыши на поле логина
        private void Login_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Login.Text == "" || Login.Text == "Логин")
            {
                Login.Clear();
            }
        }

        private void Login_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Login.Text == "")
            {
                Login.Clear();
                Login.Text = "Логин";
            }
        }
        #endregion

        #region //События наведения мыши на поле пароля
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
        #endregion

        #region //События нажатия и наведения для кнопки входа
        public bool k = false;

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) //Обработка нажатия левой кнопкой мыши на кнопку входа
        {
            Button.Opacity = 0;
            Button_click.Opacity = 100;
        }

        private void Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) //Обработка события отжатия левой кнопки мыши с кнопки входа
        {
            login = Login.Text;
            pass = Pass.Password;
            k = true;
            Close();
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e) //Обработка события наведения мыши на кнопку входа
        {
            if (k == false)
            {
                Button_click.Opacity = 0;
            }
            Button.Opacity = 0;
            ButtonDown.Opacity = 100;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e) //Обработка события ухода мыши с кнопки входа
        {
            ButtonDown.Opacity = 0;
            Button.Opacity = 100;
        }
        #endregion
        public bool l = false;
        private void close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) //Обработка события нажатия левой кнопкой мыши на кнопку Закрытия
        {
            close.Opacity = 0;
            closeenter.Opacity = 0;
        }
        private void close_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) //Обработка события отжатия левой кнопки мыши с кнопки закрытия
        {
            Environment.Exit(0);
        }

        private void close_MouseEnter(object sender, MouseEventArgs e)
        {
            close.Opacity = 0;
            closeclick.Opacity = 0;
        }

        private void close_MouseLeave(object sender, MouseEventArgs e)
        {
            closeclick.Opacity = 100;
            close.Opacity = 100;
        }
    }
}
