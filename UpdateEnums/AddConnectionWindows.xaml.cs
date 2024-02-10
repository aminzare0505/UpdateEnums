using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
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
using UpdateEnums.Utility;

namespace UpdateEnums
{
    /// <summary>
    /// Interaction logic for AddConnectionWindows.xaml
    /// </summary>
    public partial class AddConnectionWindows : Window
    {
        MainWindow _mainWindow = null;
        public AddConnectionWindows(MainWindow window)
        {
            _mainWindow = window;
            InitializeComponent();
        }

        private void TestConnection_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ServerTextBox.Text))
            {
                MessageBox.Show($" سرور وارد نشده است.");
                return;
            }

            if (string.IsNullOrWhiteSpace(UserNameTextBox.Text))
            {
                MessageBox.Show($"نام کاربری وارد نشده است");
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordTextBox.Password.ToString()))
            {
                MessageBox.Show($"رمز عبود وارد نشده است.");
                return;
            }
            string connectionString = $"Data Source={ServerTextBox.Text};Initial Catalog=testEnums;User ID={UserNameTextBox.Text}; " +
                $"Password={PasswordTextBox.Password.ToString()}";
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                connection.Close();
                MessageBox.Show($"ارتباط با موفقیت انجام شد.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ServerTextBox.Text))
            {
                MessageBox.Show($" سرور وارد نشده است.");
                return;
            }

            if (string.IsNullOrWhiteSpace(UserNameTextBox.Text))
            {
                MessageBox.Show($"نام کاربری وارد نشده است");
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordTextBox.Password.ToString()))
            {
                MessageBox.Show($"رمز عبود وارد نشده است.");
                return;
            }
            ConnectionString.ConnectionStringText = $"Data Source={ServerTextBox.Text};Initial Catalog=testEnums;User ID={UserNameTextBox.Text}; " +
                $"Password={PasswordTextBox.Password.ToString()}";
            this.Close();
            _mainWindow.ConnectionStringTextBox.Text = "Connection String Set Successfully";
            _mainWindow.IsEncriptTextBox.Text="False"; 
            _mainWindow.Show();
            _mainWindow.IsEnabled = true;
            _mainWindow.Focus();
        }

        private void UserNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void AddConnectionWindows_Closed(object sender, EventArgs e)
        {
            _mainWindow.Show();
            _mainWindow.IsEnabled = true;
            _mainWindow.Focus();
        }
    }
}
