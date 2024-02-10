using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
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
using UpdateEnums.DataSource;
using UpdateEnums.DecryptConnectionString;
using UpdateEnums.Utility;

namespace UpdateEnums
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string _Error = "";
        public MainWindow()
        {
            InitializeComponent();
        }
        private async void Browse_Click(object sender, RoutedEventArgs e)
        {
            // this is based on https://www.antoniovalentini.com/how-to-handle-file-and- folder-dialog-windows-in-a-wpf-application/

            var ookiiDialog = new VistaOpenFileDialog();
            if (ookiiDialog.ShowDialog() == true)
            {
                DLLPathTextBox.Text = ookiiDialog.FileName; ;
            }

        }
       

        private async void StartProcess_Click(object sender, RoutedEventArgs e)
        {
            
            ProcessMessageTextBox.Text = string.Empty;
            if (string.IsNullOrWhiteSpace(ConnectionStringTextBox.Text) && string.IsNullOrWhiteSpace(UpdateEnums.Utility.ConnectionString.ConnectionStringText))
            {
                MessageBox.Show($"کانکشن استرینگ وارد نشده است");
                return;
            }

            if (string.IsNullOrWhiteSpace(DLLPathTextBox.Text))
            {
                MessageBox.Show($"DLL انتخاب نشده است");
                return;
            }
            if (!File.Exists(DLLPathTextBox.Text))
            {
                MessageBox.Show($"مسیر انتخاب شده برای {"DLL"} صحیح نمی باشد.");
                return;
            }

            var progress = new Progress<int>(x => ProgressBarLoading.Value = x);
            string filePath = DLLPathTextBox.Text;
            var ConnectionString = UpdateEnums.Utility.ConnectionString.ConnectionStringText??ConnectionStringTextBox.Text;
            if (!CheckConnection(ConnectionString))
            {
                ProcessMessageTextBox.Text = _Error;
                return;
            }
            WorkerClass worker = new WorkerClass();
            DLLPathTextBox.IsEnabled = false;
            BrowseBtn.IsEnabled = false;
            ConnectionStringTextBox.IsEnabled = false;
            StartProcessBtn.IsEnabled = false;
            var Result = await Task.Run(() => worker.doSomething(filePath, progress, _Error, ConnectionString));
            if (string.IsNullOrEmpty(Result))
                ProgressBarLoading.Value = 0;
            else
                ProgressBarLoading.Value = 100;
            if (string.IsNullOrWhiteSpace(_Error))
                ProcessMessageTextBox.Text = "پردازش با موفقیت انجام شد";
            else
                ProcessMessageTextBox.Text = _Error;
            DLLPathTextBox.IsEnabled = true;
            BrowseBtn.IsEnabled = true;
            ConnectionStringTextBox.IsEnabled = true;
            StartProcessBtn.IsEnabled = true;
        }

        private bool CheckConnection(string connectionString)
        {
            try
            {
                string NormalConnectionString = "";
                if (IsEncriptTextBox.Text=="False")
                    NormalConnectionString = UpdateEnums.Utility.ConnectionString.ConnectionStringText;
               else
                    NormalConnectionString = Decrypt.DecryptConnectionStrings(connectionString, null, null);
                SqlConnection connection = new SqlConnection(NormalConnectionString);
                connection.Open();
                connection.Close();
             return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"کانکشن صحیح نمی باشد  \n Error Is: {ex.Message}");
                return false;
               
            }
        }
        private string Create(string connectionString, string dllPath)
        {
            try
            {
                var Dt = new EnumDataSource();
                Assembly DllAssembly = Assembly.LoadFrom(dllPath);
                Dt.CheckScehma(connectionString);
                var enums = DllAssembly.GetTypes().Where(t => t.IsEnum);
                foreach (Type t in enums)
                {
                    var EnumValueList = GenerateEnum.GetEnums(t);
                    Dt.InsertInDB(connectionString, t.Name, EnumValueList, _Error);
                }
            }
            catch (Exception ex)
            {
                _Error += ex.Message + ex.InnerException;
            }
            if (!string.IsNullOrWhiteSpace(_Error))
                return _Error;
            else
                return "Ok";
        }
        private void AddConnection_Click(object sender, RoutedEventArgs e)
        {

            AddConnectionWindows AddConnectionWindows = new AddConnectionWindows(this);
            AddConnectionWindows.ServerTextBox.Text = string.Empty;
            AddConnectionWindows.UserNameTextBox.Text = string.Empty;
            AddConnectionWindows.PasswordTextBox.Password = string.Empty;
            UpdateEnums.Utility.ConnectionString.ConnectionStringText = string.Empty;
            AddConnectionWindows.Owner = this;
            this.IsEnabled = false;
            AddConnectionWindows.Show();
        }


        public class WorkerClass
        {
            public string doSomething(string dllPath, IProgress<int> progress, string error, string connectionString)
            {
                try
                {
                    string NormalConnectionString = UpdateEnums.Utility.ConnectionString.ConnectionStringText;
                    if (string.IsNullOrWhiteSpace(NormalConnectionString))
                        NormalConnectionString = Decrypt.DecryptConnectionStrings(connectionString, null, null);
                   
                    var Dt = new EnumDataSource();
                    Assembly DllAssembly = Assembly.LoadFrom(dllPath);
                    Dt.CheckScehma(NormalConnectionString);

                    var enums = DllAssembly.GetTypes().Where(t => t.IsEnum);
                    var per = enums.Count() / 9;
                    progress.Report(5);
                    int Counter = 0;
                    foreach (Type t in enums)
                    {
                        var EnumValueList = GenerateEnum.GetEnums(t);
                        Dt.InsertInDB(NormalConnectionString, t.Name, EnumValueList, error);
                        if (Counter == per)
                            progress.Report(15);
                        if (Counter == (per * 2))
                            progress.Report(25);
                        if (Counter == (per * 3))
                            progress.Report(35);
                        if (Counter == (per * 4))
                            progress.Report(45);
                        if (Counter == (per * 5))
                            progress.Report(55);
                        if (Counter == (per * 6))
                            progress.Report(65);
                        if (Counter == (per * 7))
                            progress.Report(75);
                        if (Counter == (per * 8))
                            progress.Report(85);
                        if (Counter == (per * 9))
                            progress.Report(95);
                        Counter++;
                    }
                }
                catch (Exception ex)
                {
                    error += ex.Message + ex.InnerException;
                }
                if (!string.IsNullOrWhiteSpace(error))
                    return error;
                else
                    return string.Empty;

                
            }
        }
    }
}
