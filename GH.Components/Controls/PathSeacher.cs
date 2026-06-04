using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraSplashScreen;
using System.ComponentModel;
using System.ComponentModel.Design;
namespace GH.Components
{
    //[ToolboxItem(false)]
    public class PathSeacher : ComboBoxEdit, ISupportInitialize
    {
        private IContainer components = null;
    public override ISite Site
        {
            get
            {
                return base.Site;
            }
            set
            {
                base.Site = value;
                if (value == null)
                    return;
                if (!(value.GetService(typeof(IDesignerHost)) is IDesignerHost service))
                    return;
                IComponent rootComponent = service.RootComponent;
                if (!(rootComponent is ContainerControl))
                    return;
                Owner = (ContainerControl)rootComponent;
            }
        }

    private XtraOpenFileDialog openFileDialog;
    private ContainerControl _owner;
        [GHProperty, DefaultValue(null)]
        public ContainerControl Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                if (_owner == value)
                    return;
                if (_owner != null)
                    throw new Exception($"switching {GetType().Name} to another. container is not supported");
                _owner = value;
            }
        }

    private CheckEdit _remoteControl;
        [GHProperty, DefaultValue(null)]
        public CheckEdit RemoteControl
        {
            get => _remoteControl;
            set
            {
                if (_remoteControl == value)
                    return;
                if (_remoteControl != null)
                    _remoteControl.CheckedChanged -= RemoteControl_CheckedChanged;
                _remoteControl = value;
                if (value != null)
                    value.CheckedChanged += RemoteControl_CheckedChanged;
            }
        }

    private TextEdit _serverControl;
        [GHProperty, DefaultValue(null)]
        public TextEdit ServerControl
        {
            get => _serverControl;
            set
            {
                if (_serverControl == value)
                    return;
                _serverControl = value;
            }
        }
        //[GHProperty, DefaultValue(null)]
        //[Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design", typeof(UITypeEditor))]
        //[Localizable(true)]
        //public ComboBoxItemCollection Pathes { get => Properties.Items; set => Properties.Items.Assign(value); }

    public PathSeacher()
        {
            InitializeComponent();
        }
    public void BeginInit()
        {
        }
    public void EndInit()
        {
            if (IsDesignMode)
                return;
            Properties.Buttons.AddRange(
                new EditorButton[]
                {
                    new EditorButton(ButtonPredefines.DropDown),
                    new EditorButton(ButtonPredefines.Ellipsis)
                });
            RemoteControl_CheckedChanged(this, EventArgs.Empty);
            BindingContextChanged += PathSeacher_BindingContextChanged;
        }
    private void PathSeacher_BindingContextChanged(object sender, EventArgs e)
        {
            BindingContextChanged -= PathSeacher_BindingContextChanged;
            BindingManager.CurrentItemChanged += BindingManager_CurrentItemChanged;
        }
    private void BindingManager_CurrentItemChanged(object sender, EventArgs e)
        {
            var obj = BindingManager.Current.GetType().GetProperty("Pathes");
            if (obj.GetValue(BindingManager.Current) is List<string> pathes)
            {
                foreach (var item in pathes)
                {
                    if (!Properties.Items.Contains(item))
                    {
                        Properties.BeginUpdate();
                        try
                        {
                            Properties.Items.Add(item);
                        }
                        finally
                        {
                            Properties.EndUpdate();
                        }
                    }
                }
            }
        }
    private void RemoteControl_CheckedChanged(object sender, EventArgs e)
        {
            if (RemoteControl == null)
                return;
            CheckServer(RemoteControl.Checked);
            Properties.Buttons[1].Enabled = !RemoteControl.Checked;
            switch (RemoteControl.Checked)
            {
                case false:
                    Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
                    break;
                case true:
                    Properties.TextEditStyle = TextEditStyles.Standard;
                    break;
            }
            Invalidate();
        }
    private void CheckServer(bool remote)
        {
            //ReadOnly = !remote;
            Properties.Buttons[1].Enabled = !remote;
            // ищем через проводник, если местное соединение
            if (ServerControl != null)
            {
                ServerControl.ReadOnly = !remote;
                if (ServerControl.ReadOnly)
                    ServerControl.Text = "localhost";
                ServerControl.Invalidate();
            }
        }
    private void FdbPath_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.DropDown)
            {
                return;
            }
            if (SplashScreenManager.Default?.ActiveSplashFormTypeInfo?.Mode == Mode.SplashScreen)
            {
                SplashScreenManager.Default.CloseWaitForm();
                while (SplashScreenManager.Default.ActiveSplashFormTypeInfo != null)
                {
                    Application.DoEvents();
                }
            }
            if (Text != "")
            {
                openFileDialog.FileName = Path.GetFileName(Text);
                openFileDialog.InitialDirectory = Path.GetDirectoryName(Text);
            }
            else
                if (openFileDialog.InitialDirectory == "")
                    openFileDialog.InitialDirectory = Application.StartupPath;
            try
            {
                if (openFileDialog.ShowDialog(Owner) == DialogResult.OK)
                {
                    Text = openFileDialog.FileName;
                    Invalidate();
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                //throw;
            }
        }
    protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    private void InitializeComponent()
        {
            this.components = new Container();
            this.openFileDialog = new XtraOpenFileDialog(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.fProperties)).BeginInit();
            ((ISupportInitialize)(this.Properties)).BeginInit();
            this.SuspendLayout();
            this.Properties.Name = "Properties";
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "FDB";
            this.openFileDialog.Filter = "Файл базы данных (*.FDB)|*.FDB|Файл базы данных (*.GDB)|*.GDB";
            // 
            // FDBPathSeacher
            // 
            this.Size = new System.Drawing.Size(387, 20);
            this.TabIndex = 7;
            this.ButtonClick += new ButtonPressedEventHandler(this.FdbPath_ButtonClick);
            ((ISupportInitialize)(this.fProperties)).EndInit();
            ((ISupportInitialize)(this.Properties)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
