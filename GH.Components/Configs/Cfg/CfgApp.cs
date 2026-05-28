using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using System.Runtime.Serialization;
namespace GH.Components
{
    public class CfgApp : CfgCore
    {
        internal const string keyName = "Application.key";
        internal const string exportPath = "Exports";
        internal const string cfgPath = "Configs";
        internal const string logsFolder = "Logs";
        internal const string downloadWebFolder = "DownloadWebFolder";
        internal const string importSourceFolder = "ImportSources";
        internal const string ordersFolder = "Orders";
        internal static DefaultLookAndFeel _defaultLook;
        internal static DefaultLookAndFeel DefaultLook
        {
            get
            {
                if (_defaultLook == null)
                    _defaultLook = new DefaultLookAndFeel();
                return _defaultLook;
            }
        }
        private Form _form;
        public event Action<Form> OnRestore;
        [DataMember]
        [UpdatableProperty(Caption = "Version", ToolTip = "Текущая версия программы", ReadOnly = true)]
        public string ProductVersion { get; set; } = "0.0.0.0";
        [DataMember]
        public string SkinName { get; set; } = "DevExpress Style";
        [DataMember]
        [UpdatableProperty(Caption = "Key Name", ToolTip = "Ключ приложения", Group = "Основная", Required = true)]
        public string KeyName { get; set; } = keyName;
        [DataMember]
        [UpdatableProperty(Caption = "Configs Path", ToolTip = "Место хранения файлов конфигурации *.ini", Group = "Основная", Required = true)]
        public string CfgPath { get; set; } = cfgPath;
        [DataMember]
        [UpdatableProperty(Caption = "Logs Path", ToolTip = "Место хранения лог-файлов", Group = "Основная", Required = true, Default = logsFolder)]
        public string LogsPath { get; set; } = logsFolder;
        [DataMember]
        [UpdatableProperty(Caption = "Export Path", ToolTip = "Место создания Excel *.xls|*.xlsx файлов", Group = "Основная", Required = true, Default = exportPath)]
        public string ExportPath { get; set; } = exportPath;
        [DataMember]
        [UpdatableProperty(Caption = "Download Folder", ToolTip = "Папка на сервере для загрузки обновлений", Group = "Дополнительная", Required = true, Default = downloadWebFolder)]
        public string DownloadWebFolder { get; set; } = downloadWebFolder;
        [DataMember]
        [UpdatableProperty(Caption = "Import Folder", ToolTip = "Место хранения файлов-источников данных для загрузки на сервер", Group = "Дополнительная", Required = true, Default = importSourceFolder)]
        public string ImportSourceFolder { get; set; } = importSourceFolder;
        [DataMember]
        [UpdatableProperty(Caption = "Orders Folder", ToolTip = "Место хранения файлов-заказов  Excel *.xls|*.xlsx разным дистрибьюторам", Group = "Дополнительная", Required = true, Default = ordersFolder)]
        public string OrdersFolder { get; set; } = ordersFolder;
        [DataMember]
        public Size FormSize { get; set; } = new Size(800, 600);
        [DataMember]
        public Point Location { get; set; } = Point.Empty;
        [DataMember]
        public FormWindowState WindowState { get; set; } = FormWindowState.Normal;
        [DataMember]
        public int NavBarExpandedWidth { get; set; } = 185;
        [DataMember]
        public NavPaneState NavBarPaneState { get; set; } = NavPaneState.Expanded;
        internal virtual Form Form
        {
            get => _form;
            set
            {
                if (_form == value)
                    return;
                if (_form != null)
                {
                    _form.Load -= MainForm_Load;
                    _form.FormClosing -= MainForm_FormClosing;
                }
                _form = value;
                if (_form != null)
                {
                    _form.Load += MainForm_Load;
                    _form.FormClosing += MainForm_FormClosing;
                }
            }
        }
        internal void Restore()
        {
            Restore(RunContext.AppMainForm);
        }
        protected virtual void Restore(Form form)
        {
            bool isMain = RunContext.AppMainForm == form;
            Point location = Location;
            Size size = FormSize;
            FormWindowState windowState = WindowState;
            if (RunContext.AppMainForm is XtraForm xtra)
            {
                if (location.X <= 0 && location.Y <= 0)
                {
                    Rectangle rect = Rectangle.Empty;
                    if (isMain)
                        rect = Screen.GetWorkingArea(location);
                    else
                        rect = new Rectangle(location, size);
                    location = new Point((rect.Width - size.Width) / 2, (rect.Height - size.Height) / 2);
                }
            }
            form.Location = location;
            form.Size = size;
            if (windowState != FormWindowState.Minimized)
            {
                if (form.ShowInTaskbar)
                {
                    form.WindowState = windowState;
                }
            }
            form.ResizeEnd += MainForm_Resize;
            if (form is INavBarForm navBarForm)
            {
                navBarForm.NavBar.OptionsNavPane.ExpandedWidth = NavBarExpandedWidth;
                navBarForm.NavBar.OptionsNavPane.NavPaneState = NavBarPaneState;
                navBarForm.NavBar.SizeChanged += NavBarControl_SizeChanged;
            }
            OnRestore?.Invoke(form);
        }
        private void NavBarControl_SizeChanged(object sender, EventArgs e)
        {
            if (sender is NavBarControl navBar)
            {
                if (NavBarPaneState == NavPaneState.Expanded)
                    NavBarExpandedWidth = navBar.Width;
                NavBarPaneState = navBar.OptionsNavPane.NavPaneState;
            }
        }
        private void MainForm_Resize(object sender, System.EventArgs e)
        {
            if (sender is Form form)
            {
                FormSize = form.Size;
                Location = form.Location;
            }
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            WindowState = _form.WindowState;
            Save(true);
            IniHelper.SaveAll();
        }
        public CfgApp() : base()
        {
            if (_loading)
                return;
            DefaultLook.LookAndFeel.SkinName = SkinName;
            DefaultLook.LookAndFeel.StyleChanged += LookAndFeel_StyleChanged;
        }
        void LookAndFeel_StyleChanged(object sender, System.EventArgs e)
        {
            SkinName = DefaultLook.LookAndFeel.ActiveSkinName;
        }
        public bool TestConnection()
        {
            if (!Internet.PingServer(new string[] { DownloadWebFolder }))
            {
                DlgHelper.DlgError($"Нет связи с сайтом \"{DownloadWebFolder}\"!!!\r\n" +
                    "Проверьте правилность ссылки на сайт загрузки" +
                    "а так же, убедитесь в наличае Интернет соединения...");
                return false;
            }
            return true;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (sender is Form form)
            {
                Restore();
            }
        }
    }
}
