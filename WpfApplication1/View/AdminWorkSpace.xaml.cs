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
            AdminWorkSpaceContoller adminWorkSpaceContoller = new AdminWorkSpaceContoller();
            InitializeComponent();
            GridUsers.ItemsSource = adminWorkSpaceContoller.GetAllUsers();
        }

        private void ChangePass_Click(object sender, RoutedEventArgs e)
        {
            User u = (User) GridUsers.SelectedItem;
            UserView dialog = new UserView(u);
            dialog.ShowDialog();
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

        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
