using System.Windows.Forms;
using DevExpress.Skins;
using System;
using System.Reflection;
using System.Threading;
using System.IO;
using System.Diagnostics;
using GH.Utils;
using GH.UserExceptions;
using GH.UserForms;
using GH.Configs;
using DevExpress.XtraSplashScreen;

namespace GH.AppContext
{
    public class AppContext<T> : RunContext where T : RunContext
    {

        #region статические члены)))
        internal static Mutex _mutex;
        static NamedPipeManager namedPipe;

        private static void NamedPipeManager_ReceiveString(string obj)
        {
            switch (obj)
            {
                case NamedPipeManager.ACTIVE_STRING:
                    if (Instance != null && Instance.MainForm != null)
                    {
                        if (Instance.MainForm.WindowState == FormWindowState.Minimized)
                            Instance.MainForm.WindowState = FormWindowState.Normal;
                        Instance.MainForm.Activate();
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

        public static void RunInstance()
        {
            FileVersionInfo.GetVersionInfo(ExeFullName);
            string m_name = "Mutex_" + Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            _mutex = new Mutex(true, m_name, out bool RuningNow);

            if (RuningNow)
            {
                namedPipe = new NamedPipeManager();
                namedPipe.ReceiveString += NamedPipeManager_ReceiveString;
                namedPipe.Start();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                SkinManager.EnableFormSkins();

                SyncContext = new WindowsFormsSynchronizationContext();



                if (CreateAppContext())
                {
                    AppDomain.CurrentDomain.UnhandledException += delegate (object sender, UnhandledExceptionEventArgs e)
                    {
                        Logger.Error(e.ExceptionObject);
                        DlgHelper.DlgError(e.ExceptionObject.ToString());
                    };


                    Application.ThreadException += delegate (Object sender, ThreadExceptionEventArgs e)
                    {
                        Logger.Error(e.Exception);
                        DlgHelper.DlgError(e.Exception.ToString());
                        Environment.Exit(0);
                    };
                }
                else
                    Environment.Exit(0);
            }
            else
            {
                NamedPipeManager.Write(NamedPipeManager.ACTIVE_STRING);
                Application.Exit();
            }

            SplashScreenManager.CloseForm(false, 250, AppMainForm, false);

            Application.Run(Instance);
        }


        private static bool CreateAppContext()
        {
            try
            {
                /*Instance = */
                Assembly.GetEntryAssembly().CreateInstance(typeof(T).FullName);
#pragma warning disable CS0618 // Тип или член устарел
                DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
#pragma warning restore CS0618 // Тип или член устарел
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                Instance = null;
            }

            return Instance != null;
        }

        public AppContext()
        {
            Instance = this;

            SplashScreenManager.ShowForm(null, GetSplashScreen(), true, true, false, 1000);

            CfgConnection = GetConnectionSetting();
            if (CfgConnection != null)
            {
                if (!CfgConnection.ConnectIsOK())
                    throw new UserWantExit();

                if (!LogIn())
                    throw new UserWantExit();

                MainForm = GetMainForm();
            }
        }


        protected bool LogIn()
        {
            SetSqlFactoryCriator();

            using (Form form = GetLoginForm())
            {
                if (form == null || (CfgConnection.AutoEntering && CfgConnection.UserIsValid))
                    return true;

                return form.ShowDialog() == DialogResult.OK;
            }
        }
    }
}
