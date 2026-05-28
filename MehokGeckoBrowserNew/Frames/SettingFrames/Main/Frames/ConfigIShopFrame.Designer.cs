namespace MeshokBrowser
{
    partial class ConfigIShopFrame
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ServerTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ItemForServer = new DevExpress.XtraLayout.LayoutControlItem();
            this.RemoteCheckEdit = new DevExpress.XtraEditors.CheckEdit();
            this.ItemForRemote = new DevExpress.XtraLayout.LayoutControlItem();
            this.UserTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ItemForUser = new DevExpress.XtraLayout.LayoutControlItem();
            this.PassWrdTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ItemForPassWrd = new DevExpress.XtraLayout.LayoutControlItem();
#pragma warning disable CS0436 // Тип конфликтует с импортированным типом
            this.connectButton1 = new GH.Components.ConnectButton();
#pragma warning restore CS0436 // Тип конфликтует с импортированным типом
            this.lcConnect = new DevExpress.XtraLayout.LayoutControlItem();
            this.fdbPath = new GH.Components.Controls.FDBPathSeacher();
            this.lcFdbPuth = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lgRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EditGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ServerTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForServer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RemoteCheckEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForRemote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PassWrdTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPassWrd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.connectButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcConnect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fdbPath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcFdbPuth)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.fdbPath);
            this.layoutControl.Controls.Add(this.connectButton1);
            this.layoutControl.Controls.Add(this.ServerTextEdit);
            this.layoutControl.Controls.Add(this.RemoteCheckEdit);
            this.layoutControl.Controls.Add(this.UserTextEdit);
            this.layoutControl.Controls.Add(this.PassWrdTextEdit);
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(883, 139, 1001, 773);
            this.layoutControl.OptionsFocus.AllowFocusGroups = false;
            this.layoutControl.OptionsFocus.AllowFocusReadonlyEditors = false;
            this.layoutControl.OptionsFocus.AllowFocusTabbedGroups = false;
            this.layoutControl.Size = new System.Drawing.Size(816, 348);
            // 
            // lgRoot
            // 
            this.lgRoot.OptionsItemText.TextToControlDistance = 5;
            this.lgRoot.Size = new System.Drawing.Size(816, 348);
            // 
            // EditGroup
            // 
            this.EditGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForServer,
            this.ItemForRemote,
            this.lcConnect,
            this.lcFdbPuth,
            this.ItemForUser,
            this.ItemForPassWrd});
            this.EditGroup.OptionsItemText.TextToControlDistance = 5;
            this.EditGroup.Size = new System.Drawing.Size(816, 177);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 177);
            this.emptySpaceItem1.Size = new System.Drawing.Size(816, 171);
            // 
            // dataSource
            // 
            this.dataSource.PageSupport.EditGroup = this.EditGroup;
            // 
            // ServerTextEdit
            // 
            this.ServerTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "Server", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ServerTextEdit.Location = new System.Drawing.Point(93, 26);
            this.ServerTextEdit.Name = "ServerTextEdit";
            this.ServerTextEdit.Size = new System.Drawing.Size(310, 20);
            this.ServerTextEdit.StyleController = this.layoutControl;
            this.ServerTextEdit.TabIndex = 4;
            // 
            // ItemForServer
            // 
            this.ItemForServer.Control = this.ServerTextEdit;
            this.ItemForServer.Location = new System.Drawing.Point(0, 0);
            this.ItemForServer.MaxSize = new System.Drawing.Size(405, 30);
            this.ItemForServer.MinSize = new System.Drawing.Size(405, 30);
            this.ItemForServer.Name = "ItemForServer";
            this.ItemForServer.OptionsToolTip.ToolTip = "Серевер или имя компьютера";
            this.ItemForServer.Size = new System.Drawing.Size(405, 30);
            this.ItemForServer.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.ItemForServer.TextSize = new System.Drawing.Size(80, 13);
            // 
            // RemoteCheckEdit
            // 
            this.RemoteCheckEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "Remote", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.RemoteCheckEdit.Location = new System.Drawing.Point(413, 26);
            this.RemoteCheckEdit.Name = "RemoteCheckEdit";
            this.RemoteCheckEdit.Properties.Caption = "Remote";
            this.RemoteCheckEdit.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.RemoteCheckEdit.Size = new System.Drawing.Size(395, 19);
            this.RemoteCheckEdit.StyleController = this.layoutControl;
            this.RemoteCheckEdit.TabIndex = 5;
            // 
            // ItemForRemote
            // 
            this.ItemForRemote.Control = this.RemoteCheckEdit;
            this.ItemForRemote.Location = new System.Drawing.Point(405, 0);
            this.ItemForRemote.Name = "ItemForRemote";
            this.ItemForRemote.Size = new System.Drawing.Size(405, 30);
            this.ItemForRemote.Text = "Remote";
            this.ItemForRemote.TextSize = new System.Drawing.Size(0, 0);
            this.ItemForRemote.TextVisible = false;
            // 
            // UserTextEdit
            // 
            this.UserTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "DBA_Login", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.UserTextEdit.Location = new System.Drawing.Point(93, 86);
            this.UserTextEdit.Name = "UserTextEdit";
            this.UserTextEdit.Size = new System.Drawing.Size(310, 20);
            this.UserTextEdit.StyleController = this.layoutControl;
            this.UserTextEdit.TabIndex = 7;
            // 
            // ItemForUser
            // 
            this.ItemForUser.Control = this.UserTextEdit;
            this.ItemForUser.Location = new System.Drawing.Point(0, 60);
            this.ItemForUser.MaxSize = new System.Drawing.Size(405, 30);
            this.ItemForUser.MinSize = new System.Drawing.Size(405, 30);
            this.ItemForUser.Name = "ItemForUser";
            this.ItemForUser.OptionsToolTip.ToolTip = "Пользователь";
            this.ItemForUser.Size = new System.Drawing.Size(810, 30);
            this.ItemForUser.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.ItemForUser.TextSize = new System.Drawing.Size(80, 13);
            // 
            // PassWrdTextEdit
            // 
            this.PassWrdTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "DBA_Password", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.PassWrdTextEdit.Location = new System.Drawing.Point(93, 116);
            this.PassWrdTextEdit.Name = "PassWrdTextEdit";
            this.PassWrdTextEdit.Size = new System.Drawing.Size(310, 20);
            this.PassWrdTextEdit.StyleController = this.layoutControl;
            this.PassWrdTextEdit.TabIndex = 8;
            // 
            // ItemForPassWrd
            // 
            this.ItemForPassWrd.Control = this.PassWrdTextEdit;
            this.ItemForPassWrd.Location = new System.Drawing.Point(0, 90);
            this.ItemForPassWrd.MaxSize = new System.Drawing.Size(405, 30);
            this.ItemForPassWrd.MinSize = new System.Drawing.Size(405, 30);
            this.ItemForPassWrd.Name = "ItemForPassWrd";
            this.ItemForPassWrd.OptionsToolTip.ToolTip = "Пароль";
            this.ItemForPassWrd.Size = new System.Drawing.Size(810, 30);
            this.ItemForPassWrd.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.ItemForPassWrd.TextSize = new System.Drawing.Size(80, 13);
            // 
            // connectButton1
            // 
            this.connectButton1.Location = new System.Drawing.Point(8, 146);
            this.connectButton1.Name = "connectButton1";
            this.connectButton1.Size = new System.Drawing.Size(800, 23);
            this.connectButton1.StyleController = this.layoutControl;
            this.connectButton1.TabIndex = 0;
            // 
            // lcConnect
            // 
            this.lcConnect.Control = this.connectButton1;
            this.lcConnect.Location = new System.Drawing.Point(0, 120);
            this.lcConnect.MaxSize = new System.Drawing.Size(0, 33);
            this.lcConnect.MinSize = new System.Drawing.Size(11, 33);
            this.lcConnect.Name = "lcConnect";
            this.lcConnect.Size = new System.Drawing.Size(810, 33);
            this.lcConnect.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lcConnect.TextSize = new System.Drawing.Size(0, 0);
            this.lcConnect.TextVisible = false;
            // 
            // fdbPath
            // 
            this.fdbPath.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "Base", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.fdbPath.Location = new System.Drawing.Point(93, 56);
            this.fdbPath.Name = "fdbPath";
            this.fdbPath.RemoteControl = this.RemoteCheckEdit;
            this.fdbPath.Size = new System.Drawing.Size(456, 20);
            this.fdbPath.StyleController = this.layoutControl;
            this.fdbPath.TabIndex = 7;
            // 
            // lcFdbPuth
            // 
            this.lcFdbPuth.Control = this.fdbPath;
            this.lcFdbPuth.Location = new System.Drawing.Point(0, 30);
            this.lcFdbPuth.MaxSize = new System.Drawing.Size(551, 30);
            this.lcFdbPuth.MinSize = new System.Drawing.Size(551, 30);
            this.lcFdbPuth.Name = "lsFdbPuth";
            this.lcFdbPuth.Size = new System.Drawing.Size(810, 30);
            this.lcFdbPuth.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lcFdbPuth.Text = "Путь к базе";
            this.lcFdbPuth.TextSize = new System.Drawing.Size(80, 13);
            // 
            // ConfigIShopFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Caption = "IShop database";
            this.Name = "ConfigIShopFrame";
            this.Size = new System.Drawing.Size(816, 348);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lgRoot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EditGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ServerTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForServer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RemoteCheckEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForRemote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PassWrdTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPassWrd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.connectButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcConnect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fdbPath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcFdbPuth)).EndInit();
            this.ResumeLayout(false);
        }
        private DevExpress.XtraEditors.TextEdit ServerTextEdit;
        private DevExpress.XtraLayout.LayoutControlItem ItemForServer;
        private DevExpress.XtraLayout.LayoutControlItem ItemForRemote;
        private DevExpress.XtraEditors.CheckEdit RemoteCheckEdit;
        private DevExpress.XtraLayout.LayoutControlItem ItemForUser;
        private DevExpress.XtraEditors.TextEdit UserTextEdit;
        private DevExpress.XtraLayout.LayoutControlItem ItemForPassWrd;
        private DevExpress.XtraEditors.TextEdit PassWrdTextEdit;
#pragma warning disable CS0436 // Тип конфликтует с импортированным типом
        private GH.Components.ConnectButton connectButton1;
#pragma warning restore CS0436 // Тип конфликтует с импортированным типом
        private DevExpress.XtraLayout.LayoutControlItem lcConnect;
        private GH.Components.Controls.FDBPathSeacher fdbPath;
        private DevExpress.XtraLayout.LayoutControlItem lcFdbPuth;
    }
}
