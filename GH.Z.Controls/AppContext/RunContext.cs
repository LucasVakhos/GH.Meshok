using DevExpress.XtraEditors;
using GH.Components;
using GH.Configs;
using GH.Interfaces;
using GH.NHibernate;
using GH.UserExceptions;
using GH.UserForms;
using GH.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace GH.AppContext
{

    public class RunContext : ApplicationContext
    {
        #region статика
        public static readonly string ExeFullName = Process.GetCurrentProcess().MainModule.FileName;
        public static readonly string ExeName = Path.GetFileName(ExeFullName);
        public static readonly string ExePath = Path.GetDirectoryName(ExeFullName);
        public static readonly string ProcessName = Path.GetFileNameWithoutExtension(ExeFullName);
        private static CfgApp _appCfg = null;
        public static CfgApp AppCfg
        {
            get
            {
                if (_appCfg == null)
                    _appCfg = IniHelper.CfgAppForm();
                return _appCfg;
            }
        }

        internal static string ConfigsPath
        {
            get
            {                
                return Path.Combine(RunContext.ExePath, AppCfg.CfgPath);
            }
        }

        private static BaseUser _user = null;
        public static BaseUser SystemUser
        {
            get
            {
                if (_user == null)
                {
                    if (CfgConnection == null)
                        return null;
                    _user = CfgConnection.GetUser();
                }
                return _user;
            }
        }

        protected static SynchronizationContext SyncContext { get; set; }
        public static CfgCoreConnection CfgConnection { get; set; }

        public static string GetConfigsPath(object entity)
        {
            string file_name = Path.Combine(ConfigsPath, "{0}.xml");

            if (entity is Control control)
                return string.Format(file_name, ControlPath(control));

            return string.Format(file_name, entity.GetType());
        }

        private static string ControlPath(Control control)
        {
            if (control == null || control is IAppForm)
                return string.Empty;

            string path = ControlPath(control.Parent);

            if (path == string.Empty)
                return control.Name;

            return path + "." + control.Name;
        }

        public static string AppCaption
        {
            get
            {
                var res = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute)).FirstOrDefault() as AssemblyTitleAttribute;
                return res.Title;
            }
        }

        public static Icon AppIcon
        {
            get
            {
                var res = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location);
                return res;
            }
        }

        public static void Send(SendOrPostCallback d, object state)
        {
            if (SyncContext == null)
                d(state);
            else
                SyncContext.Send(d, state);
        }

        private static RunContext _instance;
        public static RunContext Instance { get => _instance; set => _instance = value; }
        public static Form AppMainForm => Instance?.MainForm;

        public static string RemoteServer
        {
            get
            {
                if (CfgConnection != null)
                    return CfgConnection.GetRemoteServer();

                return "www.google.com";
            }
        }

        private static bool IsDisposed => AppMainForm == null || AppMainForm.IsDisposed;
        private static bool InvokeRequired => AppMainForm != null && AppMainForm.InvokeRequired;
        public static void Invoke(MethodInvoker action)
        {
            if (IsDisposed)
                return;

            try
            {
                if (InvokeRequired)
                    AppMainForm.Invoke(action);
                else
                    action.Invoke();
            }
            catch { }
        }
        #endregion

        #region поля и свойства
        public static bool IsRemoteDataBase
        {
            get
            {
                if (CfgConnection != null)
                    return CfgConnection.IsRemoteDataBase();

                return false;
            }
        }

        new public Form MainForm
        {
            get => base.MainForm;
            set
            {
                if (base.MainForm == value)
                    return;
                base.MainForm = value;
                if (value != null)
                {
                    if (value is IAppForm appForm)
                        appForm.InitForm();
                    AppCfg.Form = value;
                    IniHelper.AddInstance(GetConnectionSetting());
                    //Application.Idle += RunApplication_Idle;
                }
            }
        }

        

        public static bool AppRunning
        {
            get => Instance._appRunning;
            set
            {
                if (Instance._appRunning == value)
                    return;
                Instance._appRunning = value;

                if (value)
                {
                    if (Instance.MainForm is IAppForm frm)
                    {
                        Invoke(() => 
                        {
                            frm.DoApplicatinonRun();
                        });
                        
                    }
                }
            }
        }


        public List<ActionList> ActionQueue = null;
        private bool _attached;
        private bool _appRunning;



        #endregion

        public RunContext()
        {
            Instance = this;
            VersionHelper.CheckNewVersion();
            InitializeSomething();
        }


        #region методы
        public virtual CfgApp GetCfgApp()
        {
            try
            {
                throw new NotImplemented(nameof(GetCfgApp), this);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
            }
            return new CfgApp();
        }

        public virtual CfgCoreConnection GetConnectionSetting()
        {
            throw new NotImplemented(nameof(GetConnectionSetting), this);
        }

        public static CfgForm GetConnectForm()
        {
            return Instance.CreateConnectForm();
        }

        public virtual CfgForm CreateConnectForm()
        {
            throw new NotImplemented(nameof(CreateConnectForm), this);
        }
        protected virtual void InitializeSomething()
        {
            throw new NotImplemented(nameof(InitializeSomething), this);
        }

        protected virtual Type GetSplashScreen()
        {
            return typeof(DefSplashScreen);
        }

        public virtual Form GetMainForm()
        {
            throw new NotImplemented(nameof(GetMainForm), this);
        }

        public static AboutBox InstanceGetAboutBox()
        {
            return Instance.GetAboutBox();
        }

        public virtual AboutBox GetAboutBox()
        {
            throw new NotImplemented(nameof(GetAboutBox), this);
        }

        internal void SetSqlFactoryCriator()
        {
            IFactoryCriator factory = GetSqlFactoryCriator();
            NHHelper.SetMainFactoryCriator(factory);
        }

        public virtual IFactoryCriator GetSqlFactoryCriator()
        {
            throw new NotImplemented(nameof(GetSqlFactoryCriator), this);
        }

        public virtual Form GetLoginForm()
        {
            throw new NotImplemented(nameof(GetLoginForm), this);
        }

        #endregion

        #region регистрация ActionList
        //private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    DetachKeyDownHandler();
        //    Application.Idle -= Application_Idle;
        //    Application.Exit();
        //}

        //private void Instance_RegForQueue(ActionList actionList)
        //{
        //    if (ActionQueue == null)
        //    {
        //        ActionQueue = new List<ActionList>();
        //    }
        //    ActionQueue.Add(actionList);
        //}

        //public static void RegForQueue(ActionList actionList)
        //{
        //    Instance.Instance_RegForQueue(actionList);
        //}

        //private void Instance_UnRegQueue(ActionList actionList)
        //{
        //    if (ActionQueue != null)
        //    {
        //        ActionQueue.Remove(actionList);
        //        if (ActionQueue.Count == 0)
        //        {
        //            //Application.Idle -= Application_Idle;
        //            ActionQueue = null;
        //        }
        //    }
        //}

        //public static void UnRegQueue(ActionList actionList)
        //{
        //    Instance.Instance_UnRegQueue(actionList);
        //}


        //private void RunApplication_Idle(object sender, EventArgs e)
        //{
        //    AppRunning = true;
        //}

        //private void Application_Idle(object sender, EventArgs e)
        //{
        //    if (ActionQueue != null)
        //        foreach (ActionList item in ActionQueue.FindAll(x => x.Active))
        //            item.DoUpdate(e);
        //}

        //private void AttachKeyDownHandler()
        //{
        //    Application.Idle -= RunApplication_Idle;
        //    Application.Idle += Application_Idle;

        //    if (MainForm == null || _attached)
        //        return;
        //    _attached = true;
        //    MainForm.KeyPreview = true;
        //    MainForm.KeyDown += form_KeyDown;
        //}


        private void form_KeyDown(object sender, KeyEventArgs e)
        {
            if (ActionQueue != null)
            {
                foreach (ActionList item in ActionQueue.FindAll(x => x.Enabled && x.ListenKeyDown))
                    item.CheckShortcuts(sender, e);
            }
        }

        private void DetachKeyDownHandler()
        {
            if (MainForm == null || !_attached)
                return;
            _attached = false;
            MainForm.KeyDown -= form_KeyDown;
        }

        #endregion
    }
}
