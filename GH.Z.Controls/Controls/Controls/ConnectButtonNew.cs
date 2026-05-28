using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using GH.Components.Configs;
using System;
using System.Linq;

namespace GH.Components
{
    public class TestConnectButton : SimpleButton
    {
        const int _minWidth = 150;
        const int _minHeight = 24;

        private SimpleButton connectButton;
        private LayoutControlItem infoLabel;
        private DevExpress.Utils.ImageCollection imagees;

        //private System.ComponentModel.IContainer components;
        private DataLayoutControl styleController;
        private bool tested = false;
        private System.ComponentModel.IContainer components;
        private bool testOk = false;

        public DataLayoutControl StyleController
        {
            get => styleController;
            set
            {
                if (styleController == value)
                    return;

                styleController = value;

                connectButton.StyleController = value;
                infoLabel.StyleController = value;
            }
        }


        public TestConnectButton()
        {
            InitializeComponent();
            RuntimeInitialize();
        }
        //this.imagees.InsertGalleryImage("check_ok", "images/actions/apply_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/apply_16x16.png"), 0);
        //this.imagees.Images.SetKeyName(0, "check_ok");
        //this.imagees.InsertGalleryImage("check_fault", "images/actions/cancel_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/cancel_16x16.png"), 1);
        //this.imagees.Images.SetKeyName(1, "check_fault");
        //this.imagees.InsertGalleryImage("checking", "images/actions/convert_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/convert_16x16.png"), 2);
        //this.imagees.Images.SetKeyName(2, "checking");
        //this.imagees.InsertGalleryImage("no_checking", "images/actions/hide_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/hide_16x16.png"), 3);
        //this.imagees.Images.SetKeyName(3, "no_checking");
        //this.imagees.InsertGalleryImage("check_connection", "images/business%20objects/bocontact_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/business%20objects/bocontact_16x16.png"), 4);
        //this.imagees.Images.SetKeyName(4, "check_connection");

        private void RevertControls()
        {
            bool chek = !connectButton.Enabled;
            switch (chek)
            {
                case true:
                    FocusIt();
                    infoLabel.Text = "Ждите! идёт проверка соединения...";
                    infoLabel.ImageIndex = imagees.Images["checking"].PropertyItems.FirstOrDefault().Id;
                    break;
                case false:
                    if (!tested)
                    {
                        infoLabel.ce.ImageIndex = imagees.Images["no_checking"].PropertyItems.FirstOrDefault().Id;
                        infoLabel.Text = "Статус не определён...";
                    }
                    else
                    if (testOk)
                    {
                        infoLabel.Appearance.ImageIndex = imagees.Images["check_ok"].PropertyItems.FirstOrDefault().Id;
                        infoLabel.Text = "Подкление состоялось...";
                    }
                    else
                    {
                        infoLabel.Appearance.ImageIndex = imagees.Images["check_fault"].PropertyItems.FirstOrDefault().Id;
                        infoLabel.Text = "Не удалось подключиться...";
                    }
                    break;
                default:
                    break;
            }
            connectButton.Enabled = chek;
        }

        public virtual void ConnectButton_Click(object sender, EventArgs e)
        {
            RevertControls();
            try
            {
                if (StyleController.DataSource is DataSource data)
                {
                    if (data.Current is CfgCoreConnection config)
                    {
                        testOk = config.TestConnection();
                        if (testOk)
                            config.Save();
                        tested = true;
                    }
                }

            }
            finally
            {
                FocusIt();
                RevertControls();
            }
        }


        private void FocusIt()
        {
            if (StyleController?.Controls.OfType<BaseEdit>().OrderBy(o => o.TabIndex).FirstOrDefault() is BaseEdit baseEdit)
                FocusIt(baseEdit);
            else
                Focus();
        }


        private static void FocusIt(BaseEdit baseEdit)
        {
            baseEdit.SelectAll();
            baseEdit.Focus();
        }

        private void RuntimeInitialize()
        {
            //if (DesignMode)
            //    return;

            this.SuspendLayout();

            this.connectButton.MaximumSize = new System.Drawing.Size(_minWidth, _minHeight);
            this.connectButton.MinimumSize = new System.Drawing.Size(_minWidth, _minHeight);
            this.infoLabel.MaximumSize = new System.Drawing.Size(0, _minHeight);
            this.infoLabel.MinimumSize = new System.Drawing.Size(0, _minHeight);

            this.ResumeLayout(false);

        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestConnectButton));
            this.connectButton = new DevExpress.XtraEditors.SimpleButton();
            this.imagees = new DevExpress.Utils.ImageCollection(this.components);
            this.infoLabel = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.imagees)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButton
            // 
            this.connectButton.AllowFocus = false;
            this.connectButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.connectButton.ImageOptions.ImageIndex = 4;
            this.connectButton.ImageOptions.ImageList = this.imagees;
            this.connectButton.Location = new System.Drawing.Point(0, 0);
            this.connectButton.MaximumSize = new System.Drawing.Size(_minWidth, _minHeight);
            this.connectButton.MinimumSize = new System.Drawing.Size(_minWidth, _minHeight);
            this.connectButton.Name = "simpleButton";
            this.connectButton.Size = new System.Drawing.Size(_minWidth, _minHeight);
            this.connectButton.TabIndex = 0;
            this.connectButton.Text = "Проверить соединение";
            this.connectButton.ToolTip = "Проверить соединение с сервером";
            this.connectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // imagees
            // 
            this.imagees.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imagees.ImageStream")));
            this.imagees.InsertGalleryImage("check_ok", "images/actions/apply_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/apply_16x16.png"), 0);
            this.imagees.Images.SetKeyName(0, "check_ok");
            this.imagees.InsertGalleryImage("check_fault", "images/actions/cancel_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/cancel_16x16.png"), 1);
            this.imagees.Images.SetKeyName(1, "check_fault");
            this.imagees.InsertGalleryImage("checking", "images/actions/convert_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/convert_16x16.png"), 2);
            this.imagees.Images.SetKeyName(2, "checking");
            this.imagees.InsertGalleryImage("no_checking", "images/actions/hide_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/hide_16x16.png"), 3);
            this.imagees.Images.SetKeyName(3, "no_checking");
            this.imagees.InsertGalleryImage("check_connection", "images/business%20objects/bocontact_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/business%20objects/bocontact_16x16.png"), 4);
            this.imagees.Images.SetKeyName(4, "check_connection");
            // 
            // labelControl
            // 
            this.infoLabel.Appearance.ImageIndex = 3;
            this.infoLabel.Appearance.ImageList = this.imagees;
            this.infoLabel.Appearance.Options.UseImageIndex = true;
            this.infoLabel.Appearance.Options.UseImageList = true;
            this.infoLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.infoLabel.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.infoLabel.Location = new System.Drawing.Point(_minWidth, 0);
            this.infoLabel.MaximumSize = new System.Drawing.Size(0, 22);
            this.infoLabel.MinimumSize = new System.Drawing.Size(_minWidth, 22);
            this.infoLabel.Name = "labelControl";
            this.infoLabel.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.infoLabel.Size = new System.Drawing.Size(182, 22);
            this.infoLabel.TabIndex = 1;
            this.infoLabel.Text = "Статус не определён";
            // 
            // ConnectButton
            // 
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.connectButton);
            this.Name = "ConnectButton";
            this.Size = new System.Drawing.Size(332, 158);
            ((System.ComponentModel.ISupportInitialize)(this.imagees)).EndInit();
            this.ResumeLayout(false);

        }

        private void openFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
