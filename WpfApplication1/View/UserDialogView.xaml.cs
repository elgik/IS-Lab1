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
    /// Логика взаимодействия для UserDialogView.xaml
    /// </summary>
    public partial class UserDialogView : Window
    {
        private User selectedUser =  null;

        public UserDialogView(User u)
        {
            InitializeComponent();
            if (u == null)
            {
                Close();
            }
            else
            {
                if (u.Login == "Admin")
                {
                    Login.IsReadOnly = true;
                    RestrictionCheck.IsEnabled = false;
                    BlockCheck.IsEnabled = false;
                    Delete.Visibility = Visibility.Hidden;
                }
                selectedUser = AuthController.LoadByLogin(u.Login);
                Login.Text = selectedUser.Login;
                RestrictionCheck.IsChecked = selectedUser.isRestricted;
                BlockCheck.IsChecked = selectedUser.isBlocked;
            }
        }

        public UserDialogView()
        {
            InitializeComponent();            
            Label1.Content = "Добавить нового пользователя";
            Delete.Visibility = Visibility.Hidden;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

            if (PassCheck.Visibility == Visibility.Visible || (Password.Password.Length == 0 && selectedUser == null))
            {
                MessageBox.Show("Пароль не соответствует требованиям:\r\nДлинна пароля меньше 8 символов\r\nИспользуйте строчные и заглавные буквы,\r\nцифры и символы", "Ошибка авторизации"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (RepeatPassCheck.Visibility == Visibility.Visible || (RepeatPassCheck.Visibility == Visibility.Visible && Repeat.Password.Length == 0))
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка авторизации"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (selectedUser == null)
            {
                User u = new User();
                if (Login.Text == null || Login.Text == string.Empty)
                {
                    MessageBox.Show("Необходимо ввести логин", "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if(AuthController.LoadByLogin(Login.Text) != null)
                {
                    MessageBox.Show("В базе уже есть пользователь с таким логином", "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                    u.Login = Login.Text;
                u.Password = Password.Password;
                u.isRestricted = (bool)RestrictionCheck.IsChecked;
                u.isBlocked = (bool)BlockCheck.IsChecked;
                AuthController.SaveDto(u);
                Close();
            }
            else
            {
                User u = AuthController.LoadByLogin(selectedUser.Login);
                if (Login.Text == null || Login.Text == string.Empty)
                    MessageBox.Show("Необходимо ввести логин", "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
                if (Login.Text != u.Login && AuthController.LoadByLogin(Login.Text) != null)
                {
                    MessageBox.Show("В базе уже есть пользователь с таким логином", "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                    u.Login = Login.Text;
                if (Password.Password != null && Password.Password != string.Empty)
                    u.Password = Password.Password;
                u.isRestricted = (bool)RestrictionCheck.IsChecked;
                u.isBlocked = (bool)BlockCheck.IsChecked;
                AuthController.UpdateDto(u);
                Close();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pas = sender as PasswordBox;
            if (pas != null)
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

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            AuthController.DeleteDto(AuthController.LoadByLogin(selectedUser.Login));
            Close();
        }
    }
}
