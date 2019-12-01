using Season;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RESI.Sorter
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {

            Startup += App_Startup;
        }
        /// <summary>
        /// 程序启动时，注册以下事件 捕获系统异常，导致灾难性错误的处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_Startup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;

        }

        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("我们很抱歉，当前应用程序遇到一些问题.." + e.Exception,
                "智能:意外的操作", MessageBoxButton.OK, MessageBoxImage.Information);//这里通常需要给用户一些较为友好的提示，并且后续可能的操作
            e.Handled = true;//使用这一行代码告诉运行时，该异常被处理了，不再作为UnhandledException抛出了。

            //记录日志
            Log.Error("系统全局异常：", e.Exception);
            //OwnerLog.Instance.MainLog.Error("系统全局异常：" + e.Exception.Message + e.Exception.Source);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show("我们很抱歉，当前应用程序遇到一些问题.请联系管理员." + e.ExceptionObject, "意外的操作", MessageBoxButton.OK, MessageBoxImage.Information);

            //记录日志,MessageShow 发布版本时可以先取消掉
            Log.Error("系统全局异常：", (Exception)e.ExceptionObject);
        }
    }

}
