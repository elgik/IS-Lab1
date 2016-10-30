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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApplication1.Controller;
using WpfApplication1.View;

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AuthController authController;

        public MainWindow()
        {
            authController = new AuthController();
            InitializeComponent();
            PassCheck.Visibility = Visibility.Hidden;
            if (authController.firstLogin)
            {
                Login.IsReadOnly = true;
                Login.Text = "Admin";
                Ok.Content = "Сохранить";
            }
            else
            {
                Repeat.Visibility = Visibility.Hidden;
                RepeatLabel.Visibility = Visibility.Hidden;
                RepeatPassCheck.Visibility = Visibility.Hidden;
                Login.IsReadOnly = false;
                Ok.Content = "Войти";
            }

        }

        private void OkClicked(object sender, RoutedEventArgs e)
        {
            if(PassCheck.Visibility == Visibility.Visible || Password.Password.Length == 0)
            {
                MessageBox.Show("Пароль не соответствует требованиям:\r\nДлинна пароля меньше 8 символов\r\nИспользуйте строчные и заглавные буквы,\r\nцифры и символы", "Ошибка авторизации"
                    , MessageBoxButton.OK,MessageBoxImage.Error);
            }
            else if(RepeatPassCheck.Visibility == Visibility.Visible || (RepeatPassCheck.Visibility == Visibility.Visible && Repeat.Password.Length == 0))
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка авторизации"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (authController.firstLogin)
            {
                User u = new User();
                u.Login = Login.Text;
                u.Password = Password.Password;
                u.isBlocked = false;
                u.isRestricted = false;
                authController.SaveDto(u);
            }
            else
            {
                string sw = null;
                sw = authController.Autorization(Login.Text, Password.Password);
                if(sw != null)
                    MessageBox.Show(sw, "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                else if(Login.Text == "Admin")
                {
                    Hide();
                    AdminWorkSpace adminWindow = new AdminWorkSpace();
                    adminWindow.Show();
                }
                else
                {
                    Hide();
                    UserView userView = new UserView(AuthController.LoadByLogin(Login.Text));
                    userView.Show();
                }
            }
        }

        private void exitClicked(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void passChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pas = sender as PasswordBox;
            if(pas != null)
            {
                if (!(pas.Password.Any(p => char.IsDigit(p))
                    && pas.Password.Any(p => char.IsLetter(p))
                    && pas.Password.Any(p => char.IsUpper(p))
                    && pas.Password.Any(p => char.IsSymbol(p))
                    && pas.Password.Length > 8
                    ))
                {
                    PassCheck.Visibility = Visibility.Visible;
                }
                else
                    PassCheck.Visibility = Visibility.Hidden;
            }
        }

        private void repeatPassChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pas = sender as PasswordBox;
            if (pas.Password != Password.Password)
            {
                RepeatPassCheck.Visibility = Visibility.Visible;
            }
            else
                RepeatPassCheck.Visibility = Visibility.Hidden;
        }

    }
}
