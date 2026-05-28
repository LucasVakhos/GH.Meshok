using DevExpress.Images;
using DevExpress.Utils;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using System.ComponentModel;
//using static System.Windows.Forms.ImageList;
namespace GH.Components
{
    public class ConnectButton : SimpleButton
    {
        const int _minWidth = 150;
        const int _minHeight = 24;
        private bool tested = false;
        private bool testOk = false;
        private IContainer components;
        private ImageCollection images;
        private LayoutControlItem _lcLabelInfo = null;
        protected LayoutControlItem lсLabel
        {
            get => _lcLabelInfo;
            set
            {
                if (_lcLabelInfo == value)
                    return;
                _lcLabelInfo = value;
                if (value != null)
                {
                    value.BeginInit();
                    value.TextSize = new System.Drawing.Size(0, 0);
                    value.TextVisible = false;
                    value.EndInit();
                }
            }
        }
        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = "Проверить соединение";
            }
        }
        private LabelControl _label;
        protected LabelControl Label
        {
            get => _label;
            set
            {
                if (_label == value)
                    return;
                _label = value;
                if (value != null)
                {
                    value.ImageAlignToText = ImageAlignToText.LeftCenter;
                    value.IndentBetweenImageAndText = 5;
                    value.AutoSizeMode = LabelAutoSizeMode.Horizontal;
                    value.ToolTip = "Сведенья о поледней попытке подключения";
                }
            }
        }
        //public override IStyleController StyleController
        //{
        //    get => base.StyleController;
        //    set
        //    {
        //        if (base.StyleController == value)
        //            return;
        //        base.StyleController = value;
        //        Label.StyleController = value;
        //        if (value != null)
        //        {
        //            value.PropertiesChanged += StyleController_PropertiesChanged;
        //        }
        //    }
        //}
        public ConnectButton()
        {
            InitializeComponent();
            Text = "Проверить соединение";
            ToolTip = "Проверить соединение с сервером";
            MaximumSize = new System.Drawing.Size(_minWidth, _minHeight);
            MinimumSize = new System.Drawing.Size(_minWidth, _minHeight);
            Click += ConnectButton_Click;
        }
        protected override void OnParentChanged(EventArgs e)
        {
            if (Parent != null && Parent is DataLayoutControl layoutControl && layoutControl.DataSource is DataSource data)
            {
                StyleController = layoutControl;
                lсLabel = new LayoutControlItem();
                lсLabel.BeginInit();
                BaseLayoutItem lcButton = layoutControl.Items.Where(x => x is LayoutControlItem controlItem && controlItem.Control == this).FirstOrDefault();
                LayoutGroup group = lcButton.Parent;
                lсLabel.Control = Label;
                lсLabel.TextVisible = false;
                layoutControl.SuspendLayout();
                layoutControl.Controls.Add(Label);
                group.AddItem(lсLabel, lcButton, DevExpress.XtraLayout.Utils.InsertType.Right);
                EmptySpaceItem es = new EmptySpaceItem();
                group.AddItem(es, lсLabel, DevExpress.XtraLayout.Utils.InsertType.Right);
                layoutControl.ResumeLayout(false);
                lсLabel.EndInit();
            }
            base.OnParentChanged(e);
        }
        private void RevertControls()
        {
            bool chek = !Enabled;
            switch (chek)
            {
                case true:
                    Label.Text = "Ждите! идёт проверка соединения...";
                    Label.Appearance.Image = images.Images["checking"];
                    break;
                case false:
                    if (!tested)
                    {
                        Label.Text = "Статус не определён...";
                        Label.Appearance.Image = images.Images["no_checking"];
                    }
                    else
                        if (testOk)
                        {
                            Label.Text = "Подключение состоялось...";
                            Label.Appearance.Image = images.Images["check_ok"];
                        }
                        else
                        {
                            Label.Text = "Не удалось подключиться...";
                            Label.Appearance.Image = images.Images["check_fault"];
                        }
                    break;
                default:
                    break;
            }
            Label.Invalidate();
            Invalidate();
        }
        public virtual void ConnectButton_Click(object sender, EventArgs e)
        {
            FocusIt();
            if (Parent is DataLayoutControl dataLayout && dataLayout.DataSource is DataSource data)
            {
                Enabled = false;
                RevertControls();
                try
                {
                    if (data.Current is CfgCoreConnection config)
                    {
                        testOk = config.TestConnection();
                        if (config.IsComplete && testOk)
                            config.Save();
                        tested = true;
                    }
                }
                finally
                {
                    Enabled = true;
                    RevertControls();
                }
            }
        }
        private void FocusIt()
        {
            if (Parent is DataLayoutControl dataLayout && dataLayout.Controls.OfType<BaseEdit>().OrderBy(o => o.TabIndex).FirstOrDefault() is BaseEdit baseEdit)
                FocusIt(baseEdit);
            else
                Focus();
        }
        private static void FocusIt(BaseEdit baseEdit)
        {
            baseEdit.SelectAll();
            baseEdit.Focus();
        }
        private void InitializeComponent()
        {
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(ConnectButton));
            images = new ImageCollection(components);
            Label = new LabelControl();
            ((ISupportInitialize)(images)).BeginInit();
            SuspendLayout();
            images.ImageStream = ((ImageCollectionStreamer)(resources.GetObject("imagees.ImageStream")));
            images.InsertGalleryImage("check_ok", "images/actions/apply_16x16.png", ImageResourceCache.Default.GetImage("images/actions/apply_16x16.png"), 0);
            images.Images.SetKeyName(0, "check_ok");
            images.InsertGalleryImage("check_fault", "images/actions/cancel_16x16.png", ImageResourceCache.Default.GetImage("images/actions/cancel_16x16.png"), 1);
            images.Images.SetKeyName(1, "check_fault");
            images.InsertGalleryImage("checking", "images/actions/convert_16x16.png", ImageResourceCache.Default.GetImage("images/actions/convert_16x16.png"), 2);
            images.Images.SetKeyName(2, "checking");
            images.InsertGalleryImage("no_checking", "images/actions/hide_16x16.png", ImageResourceCache.Default.GetImage("images/actions/hide_16x16.png"), 3);
            images.Images.SetKeyName(3, "no_checking");
            images.InsertGalleryImage("check_connection", "images/business%20objects/bocontact_16x16.png", ImageResourceCache.Default.GetImage("images/business%20objects/bocontact_16x16.png"), 4);
            images.Images.SetKeyName(4, "check_connection");
            Label.Appearance.ImageIndex = 3;
            Label.Appearance.ImageList = images;
            Label.Appearance.Options.UseImageIndex = true;
            Label.Appearance.Options.UseImageList = true;
            Label.MaximumSize = new System.Drawing.Size(0, _minHeight);
            Label.MinimumSize = new System.Drawing.Size(0, _minHeight);
            Label.AutoSizeMode = LabelAutoSizeMode.Horizontal;
            Label.ImageAlignToText = ImageAlignToText.LeftCenter;
            Label.Appearance.TextOptions.HAlignment = HorzAlignment.Near;
            Label.Name = "labelControl";
            Label.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            Label.TabIndex = 1;
            Label.Text = "Статус не определён";
            /*ImageOptions.*/
            Image = images.Images["check_connection"];
            AllowFocus = false;
            Appearance.Options.UseTextOptions = true;
            Appearance.TextOptions.HAlignment = HorzAlignment.Near;
            ImageOptions.ImageToTextAlignment = ImageAlignToText.LeftCenter;
            ImageOptions.ImageToTextIndent = 5;
            Location = new System.Drawing.Point(0, 0);
            Size = new System.Drawing.Size(_minWidth, _minHeight);
            TabIndex = 0;
            ((ISupportInitialize)(images)).EndInit();
            ResumeLayout(false);
        }
    }
}
