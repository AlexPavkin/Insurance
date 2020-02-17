using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;




namespace Insurance
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {

            e.Handled = true;
            string error = e.Exception.Message;
            if (e.Exception.InnerException != null)
            {
                error += Environment.NewLine;
                error += e.Exception.InnerException.Message;
            }
            //MessageBox.Show(error);
            string m0 = error;
            string t0 = "Ошибка!";
            int b0 = 1;
            Message me0 = new Message(m0, t0, b0);
            me0.ShowDialog();
        }

        //private void Application_Startup(object sender, StartupEventArgs e)
        //{
        //    AppDomain currentDomain = AppDomain.CurrentDomain;
        //    currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);
        //}
        //static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        //{
        //    Exception e = (Exception)args.ExceptionObject;

        //    string error = $"Возникло исключение {e.Message} в:{e.Source}";
        //    if (e.InnerException != null)
        //    {
        //        error += Environment.NewLine;
        //        error += e.InnerException.Message;
        //    }
        //    //MessageBox.Show(error);
        //    string m0 = error;
        //    string t0 = "Ошибка!";
        //    int b0 = 1;
        //    Message me0 = new Message(m0, t0, b0);
        //    me0.ShowDialog();
        //}
    }
}
