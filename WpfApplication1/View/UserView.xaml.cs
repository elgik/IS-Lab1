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
using WpfApplication1.Controller;
namespace WpfApplication1.View
{
    /// <summary>
    /// Логика взаимодействия для UserView.xaml
    /// </summary>
    public partial class UserView : Window
    {
        public UserView(User u)
        {
            InitializeComponent();
            Login.Text = AuthController.CurrentUser.Login;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
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
                AuthController.CurrentUser.Password = Repeat.Password;
                AuthController.UpdateDto(AuthController.CurrentUser);
                MessageBox.Show("Данные успешно сохранены", "Смена пароля", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            AuthController.CurrentUser = null;
            MainWindow m = new MainWindow();
            m.Show();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pas = sender as PasswordBox;
            if (pas != null)
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
                else if (AuthController.CurrentUser.isRestricted && pas.Password == AuthController.CurrentUser.Login)
                    PassCheck.Visibility = Visibility.Visible;
                else
                    PassCheck.Visibility = Visibility.Hidden;
            }
            Repeat_PasswordChanged(Repeat, new RoutedEventArgs());
        }

        private void Repeat_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pas = sender as PasswordBox;
            if (pas.Password != Password.Password)
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
