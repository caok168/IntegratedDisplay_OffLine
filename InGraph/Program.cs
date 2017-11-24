using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.IO;
using System.Text;
using System.Threading;
using System.Globalization;

namespace InGraph
{
    static class Program
    {

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            string fileName = Path.Combine(Path.Combine(Application.StartupPath, "LogData"), "InGraph.log");
            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }
            StreamWriter log = new StreamWriter(new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
            StringBuilder buff = new StringBuilder();
            buff.Append(string.Format("---------------------- Start ({0})------------------\n", DateTime.Now.ToString("o", CultureInfo.CurrentCulture)));
            Exception exp = e.Exception;
            buff.Append("应用程序未处理异常 。\n");
            buff.AppendFormat("异常类型:{0}\n ", exp.GetType().Name);
            buff.AppendFormat("错误信息:{0}\n ", exp.Message);
            buff.AppendFormat("堆栈信息:{0}\n ", exp.StackTrace);
            buff.Append("---------------------- End ------------------\n");
            log.WriteLine(buff.ToString());
            log.Flush();
            log.Close();
            Application.ExitThread();
            Application.Exit();
        }

        private static void Application_ThreadExit(object sender, EventArgs e)
        {
            string fileName = Path.Combine(Path.Combine(Application.StartupPath, "LogData"), "InGraph.log");
            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }
            StreamWriter log = new StreamWriter(new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
            StringBuilder buff = new StringBuilder();
            buff.Append("---------------------- Start------------------\n");
            buff.Append(string.Format("应用程序结束时间：{0} \n", DateTime.Now.ToString("o", CultureInfo.CurrentCulture)));
            buff.Append("---------------------- End ------------------\n");
            log.WriteLine(buff.ToString());
            log.Flush();
            log.Close();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string fileName = Path.Combine(Path.Combine(Application.StartupPath, "LogData"), "InGraph.log");
            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }
            StreamWriter log = new StreamWriter(new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
            StringBuilder buff = new StringBuilder();
            buff.Append(string.Format("---------------------- Start ({0})------------------\n", DateTime.Now.ToString("o", CultureInfo.CurrentCulture)));
            Exception exp = e.ExceptionObject as Exception;
            if (exp != null)
            {
                buff.Append("Application UnhandledError 。\n");
                buff.AppendFormat("异常类型:{0}\n ", exp.GetType().Name);
                buff.AppendFormat("异常信息:{0}\n ", exp.Message);
                buff.AppendFormat("堆栈信息:{0}\n ", exp.StackTrace);
            }
            else
            {
                buff.AppendFormat("Application UnhandledError:{0}\n", e);
            }
            log.WriteLine(buff.ToString());
            log.WriteLine("---------------------- End ------------------");
            log.Flush();
            log.Close();
            Application.ExitThread();
            Application.Exit();
        }


        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new ThreadExceptionEventHandler(Program.Application_ThreadException);
            Application.ThreadExit += new EventHandler(Program.Application_ThreadExit);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.CurrentDomain_UnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new MainForm(args));
            }
            catch (System.Exception ex)
            {
                Application_ThreadException(null, new ThreadExceptionEventArgs(ex));
            }
        }
    }
}
