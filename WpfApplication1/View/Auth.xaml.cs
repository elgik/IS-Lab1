using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        int count = 0;       

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
            if (authController.firstLogin)
            {
                if (PassCheck.Visibility == Visibility.Visible || Password.Password.Length == 0)
                {
                    MessageBox.Show("Пароль не соответствует требованиям:\r\nДлинна пароля меньше 8 символов\r\nИспользуйте строчные и заглавные буквы,\r\nцифры и символы", "Ошибка авторизации"
                        , MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (RepeatPassCheck.Visibility == Visibility.Visible || (RepeatPassCheck.Visibility == Visibility.Visible && Repeat.Password.Length == 0))
                {
                    MessageBox.Show("Пароли не совпадают", "Ошибка авторизации"
                        , MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    User u = new User();
                    u.Login = Login.Text;
                    u.Password = Password.Password;
                    u.isBlocked = false;
                    u.isRestricted = false;
                    AuthController.SaveDto(u);
                    AuthController.CurrentUser = AuthController.LoadByLogin("Admin");
                    AdminWorkSpace adminWindow = new AdminWorkSpace();
                    Hide();
                    adminWindow.Show();
                }
            }
            else
            {
                string sw = null;
                sw = AuthController.Autorization(Login.Text, Password.Password);
                if (sw != null)
                {
                    if (count == 3)
                    {
                        MessageBox.Show("ПОПЫТКИ ИСЧЕРПАНЫ, ПРИЛОЖЕНИЕ ОТКЛЮЧАЕТСЯ", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                        Application.Current.Shutdown();
                    }
                    MessageBox.Show(sw, "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                    if (sw == "Неправильный пароль") ++count;
                }
                else if (Login.Text == "Admin")
                {
                    Hide();
                    AuthController.CurrentUser = AuthController.LoadByLogin(Login.Text);
                    AdminWorkSpace adminWindow = new AdminWorkSpace();
                    adminWindow.Show();
                }
                else
                {
                    Hide();
                    AuthController.CurrentUser = AuthController.LoadByLogin(Login.Text);
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
            if (authController.firstLogin && pas != null)
            {
                if (!(pas.Password.Any(p => char.IsDigit(p))
                    && pas.Password.Any(p => char.IsLetter(p))
                    && pas.Password.Any(p => char.IsUpper(p))
                    && pas.Password.Any(p => !char.IsLetterOrDigit(p))
                    && pas.Password.All(p => !char.IsWhiteSpace(p))
                    && pas.Password.Length > 8
                    ))
                {
                    PassCheck.Visibility = Visibility.Visible;
                }
                else
                    PassCheck.Visibility = Visibility.Hidden;
            }
            else if (Console.CapsLock)
                Caps.Visibility = Visibility.Visible;
            else
                Caps.Visibility = Visibility.Hidden;
            repeatPassChanged(Repeat, new RoutedEventArgs());
        }

        private void repeatPassChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pas = sender as PasswordBox;
            if (authController.firstLogin && pas.Password != Password.Password)
            {
                RepeatPassCheck.Visibility = Visibility.Visible;
            }
            else
                RepeatPassCheck.Visibility = Visibility.Hidden;

        }

        private void Manual_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Программу реализовал студент группы ИДБ-13-15 Попов Денис", "Справка", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
