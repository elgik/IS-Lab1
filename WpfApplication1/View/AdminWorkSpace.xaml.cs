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
    /// Логика взаимодействия для AdminWorkSpace.xaml
    /// </summary>
    public partial class AdminWorkSpace : Window
    {
        public AdminWorkSpace()
        {
            InitializeComponent();
            GridUsers.ItemsSource = AuthController.GetAllUsers();
        }

        private void ChangePass_Click(object sender, RoutedEventArgs e)
        {
            User u;
            if (sender as MenuItem != null)
                u = AuthController.LoadByLogin("Admin");
            else u = (User) GridUsers.SelectedItem;
            if (u != null)
            {
                UserDialogView dialog = new UserDialogView(u);
                dialog.ShowDialog();
                GridUsers.ItemsSource = null;
                GridUsers.ItemsSource = AuthController.GetAllUsers();
            }
            else
                MessageBox.Show("Пользователь не выбран", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainWindow m = new MainWindow();
            m.Show();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void NewUser_Click(object sender, RoutedEventArgs e)
        {
            UserDialogView dialog = new UserDialogView();
            dialog.ShowDialog();
            GridUsers.ItemsSource = null;
            GridUsers.ItemsSource = AuthController.GetAllUsers();
        }

        private void Find_Click(object sender, RoutedEventArgs e)
        {
            User u = AuthController.LoadByLogin(FindBox.Text);
            if (u == null)
            {
                MessageBox.Show("Пользователь не найден", "Ошибка поиска", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                UserDialogView dialog = new UserDialogView(u);
                dialog.ShowDialog();
                FindBox.Text = null;
                GridUsers.ItemsSource = null;
                GridUsers.ItemsSource = AuthController.GetAllUsers();
            }
        }

        private void Manual_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Программу реализовал студент группы ИДБ-13-15 Попов Денис", "Справка", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
