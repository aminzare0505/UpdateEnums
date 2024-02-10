using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UpdateEnumsInDBFromDll.DataSource;
using UpdateEnumsInDBFromDll.DecryptConnectionString;
using UpdateEnumsInDBFromDll.Utility;
//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UpdateEnumsInDBFromDll
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

            var ookiiDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();
            if (ookiiDialog.ShowDialog() == true)
            {
                DLLPathTextBox.Text = ookiiDialog.FileName; ;
            }

        }
        public class WorkerClass
        {
            public string doSomething(string dllPath, IProgress<int> progress,string error,string connectionString)
            {
                try
                {
                    string NormalConnectionString = Decrypt.DecryptConnectionStrings(connectionString,null,null);
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
                    return "Ok";
                //DirectoryInfo di = new DirectoryInfo(filePath);
                //FileInfo[] fileInfos = di.GetFiles("*.sql", SearchOption.AllDirectories);
                //if (fileInfos == null || fileInfos.Length == 0)
                //{
                //    System.Windows.Forms.MessageBox.Show("فایلی با پسوند .sql  برای پردازش یافت نشد.");
                //    return string.Empty;
                //}
                //else
                //{
                //    string script = "";
                //    bool First = true;
                //    var per = fileInfos.Length / 9;
                //    progress.Report(5);
                //    int Counter = 0;
                //    foreach (var fileInfo in fileInfos)
                //    {

                //        if (Counter == per)
                //            progress.Report(15);
                //        if (Counter == (per * 2))
                //            progress.Report(25);
                //        if (Counter == (per * 3))
                //            progress.Report(35);
                //        if (Counter == (per * 4))
                //            progress.Report(45);
                //        if (Counter == (per * 5))
                //            progress.Report(55);
                //        if (Counter == (per * 6))
                //            progress.Report(65);
                //        if (Counter == (per * 7))
                //            progress.Report(75);
                //        if (Counter == (per * 8))
                //            progress.Report(85);
                //        if (Counter == (per * 9))
                //            progress.Report(95);
                //        // ProgressBarLoading.Value += ProgressBarValue;
                //        if (First)
                //            script += fileInfo.OpenText().ReadToEnd();
                //        else
                //            script += "\n" + "GO" + "\n" + fileInfo.OpenText().ReadToEnd();
                //        First = false;
                //        Counter++;
                //    }

                //    return script;
                //}

                //using (var package = new Package(filePath))
                //{
                //    foreach (var item in package)
                //    {
                //        updateMethod(item); //once this method call is complete I want the ProgressBar to update its Value
                //        progress.Report(...);
                //    }
                //}
            }
        }

        private async void StartProcess_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(ConnectionStringTextBox.Text))
                System.Windows.Forms.MessageBox.Show($"کانکشن استرینگ وارد نشده است");
            var progress = new Progress<int>(x => ProgressBarLoading.Value = x);
            string filePath = DLLPathTextBox.Text;
            var sda = ConnectionStringTextBox.Text;
            WorkerClass worker = new WorkerClass();
            var Result = await Task.Run(() => worker.doSomething(filePath, progress, _Error, sda));
            if (string.IsNullOrEmpty(Result))
                ProgressBarLoading.Value = 0;
            else
                ProgressBarLoading.Value = 100;
            if (string.IsNullOrWhiteSpace(_Error))
                ProcessMessageTextBox.Text = "پردازش با موفقیت انجام شد";
            else
                ProcessMessageTextBox.Text = _Error;
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
    }
}