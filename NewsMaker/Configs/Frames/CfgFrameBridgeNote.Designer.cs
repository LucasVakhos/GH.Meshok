namespace MeshokBrowser
{
    partial class CfgFrameBridgeNote
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CfgFrameBridgeNote));
            this.ServerTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.DatabaseTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.UserIDTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.PasswordTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.PortTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.CharacterSetTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.cboConnectionProtocol = new DevExpress.XtraEditors.LookUpEdit();
            this.cboSslMode = new DevExpress.XtraEditors.LookUpEdit();
            this.PageAddition = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForPort = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForCharacterSet = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForConnectionProtocol = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForSslMode = new DevExpress.XtraLayout.LayoutControlItem();
            this.tabbedControlGroup1 = new DevExpress.XtraLayout.TabbedControlGroup();
            this.PageConnect = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForServer = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForDatabase = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForUserID = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForPassword = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rootGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EditGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ServerTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DatabaseTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserIDTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PasswordTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PortTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CharacterSetTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboConnectionProtocol.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSslMode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PageAddition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCharacterSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForConnectionProtocol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForSslMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PageConnect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForServer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDatabase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUserID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPassword)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.ServerTextEdit);
            this.layoutControl.Controls.Add(this.DatabaseTextEdit);
            this.layoutControl.Controls.Add(this.UserIDTextEdit);
            this.layoutControl.Controls.Add(this.PasswordTextEdit);
            this.layoutControl.Controls.Add(this.PortTextEdit);
            this.layoutControl.Controls.Add(this.CharacterSetTextEdit);
            this.layoutControl.Controls.Add(this.cboConnectionProtocol);
            this.layoutControl.Controls.Add(this.cboSslMode);
            this.layoutControl.OptionsCustomizationForm.DefaultPage = DevExpress.XtraLayout.CustomizationPage.LayoutTreeView;
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(587, 132, 569, 620);
            this.layoutControl.OptionsFocus.AllowFocusGroups = false;
            this.layoutControl.OptionsFocus.AllowFocusReadonlyEditors = false;
            this.layoutControl.OptionsFocus.AllowFocusTabbedGroups = false;
            this.layoutControl.OptionsView.AlwaysScrollActiveControlIntoView = false;
            this.layoutControl.Size = new System.Drawing.Size(492, 195);
            this.layoutControl.Controls.SetChildIndex(this.cboSslMode, 0);
            this.layoutControl.Controls.SetChildIndex(this.cboConnectionProtocol, 0);
            this.layoutControl.Controls.SetChildIndex(this.CharacterSetTextEdit, 0);
            this.layoutControl.Controls.SetChildIndex(this.PortTextEdit, 0);
            this.layoutControl.Controls.SetChildIndex(this.PasswordTextEdit, 0);
            this.layoutControl.Controls.SetChildIndex(this.UserIDTextEdit, 0);
            this.layoutControl.Controls.SetChildIndex(this.DatabaseTextEdit, 0);
            this.layoutControl.Controls.SetChildIndex(this.ServerTextEdit, 0);
            // 
            // rootGroup
            // 
            this.rootGroup.Size = new System.Drawing.Size(492, 195);
            // 
            // EditGroup
            // 
            this.EditGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.tabbedControlGroup1});
            this.EditGroup.Size = new System.Drawing.Size(492, 195);
            // 
            // dataSource
            // 
            this.dataSource.AllowSaveCancel = true;
            this.dataSource.PageSupport.EditGroup = this.EditGroup;
            // 
            // ServerTextEdit
            // 
            this.ServerTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "Server", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ServerTextEdit.Location = new System.Drawing.Point(126, 48);
            this.ServerTextEdit.Name = "ServerTextEdit";
            this.ServerTextEdit.Size = new System.Drawing.Size(340, 20);
            this.ServerTextEdit.StyleController = this.layoutControl;
            this.ServerTextEdit.TabIndex = 0;
            // 
            // DatabaseTextEdit
            // 
            this.DatabaseTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "Database", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.DatabaseTextEdit.Location = new System.Drawing.Point(126, 72);
            this.DatabaseTextEdit.Name = "DatabaseTextEdit";
            this.DatabaseTextEdit.Size = new System.Drawing.Size(340, 20);
            this.DatabaseTextEdit.StyleController = this.layoutControl;
            this.DatabaseTextEdit.TabIndex = 2;
            // 
            // UserIDTextEdit
            // 
            this.UserIDTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "UserID", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.UserIDTextEdit.Location = new System.Drawing.Point(126, 96);
            this.UserIDTextEdit.Name = "UserIDTextEdit";
            this.UserIDTextEdit.Size = new System.Drawing.Size(340, 20);
            this.UserIDTextEdit.StyleController = this.layoutControl;
            this.UserIDTextEdit.TabIndex = 3;
            // 
            // PasswordTextEdit
            // 
            this.PasswordTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "Password", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.PasswordTextEdit.Location = new System.Drawing.Point(126, 120);
            this.PasswordTextEdit.Name = "PasswordTextEdit";
            this.PasswordTextEdit.Size = new System.Drawing.Size(340, 20);
            this.PasswordTextEdit.StyleController = this.layoutControl;
            this.PasswordTextEdit.TabIndex = 4;
            // 
            // PortTextEdit
            // 
            this.PortTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "Port", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.PortTextEdit.Location = new System.Drawing.Point(126, 48);
            this.PortTextEdit.Name = "PortTextEdit";
            this.PortTextEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.PortTextEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.PortTextEdit.Properties.Mask.EditMask = "N0";
            this.PortTextEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.PortTextEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.PortTextEdit.Size = new System.Drawing.Size(118, 20);
            this.PortTextEdit.StyleController = this.layoutControl;
            this.PortTextEdit.TabIndex = 1;
            // 
            // CharacterSetTextEdit
            // 
            this.CharacterSetTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "CharacterSet", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.CharacterSetTextEdit.Location = new System.Drawing.Point(348, 48);
            this.CharacterSetTextEdit.Name = "CharacterSetTextEdit";
            this.CharacterSetTextEdit.Size = new System.Drawing.Size(118, 20);
            this.CharacterSetTextEdit.StyleController = this.layoutControl;
            this.CharacterSetTextEdit.TabIndex = 1;
            // 
            // cboConnectionProtocol
            // 
            this.cboConnectionProtocol.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "ConnectionProtocol", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cboConnectionProtocol.Location = new System.Drawing.Point(126, 72);
            this.cboConnectionProtocol.Name = "cboConnectionProtocol";
            this.cboConnectionProtocol.Properties.Appearance.Options.UseTextOptions = true;
            this.cboConnectionProtocol.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.cboConnectionProtocol.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboConnectionProtocol.Properties.NullText = "";
            this.cboConnectionProtocol.Properties.PopupSizeable = false;
            this.cboConnectionProtocol.Size = new System.Drawing.Size(340, 20);
            this.cboConnectionProtocol.StyleController = this.layoutControl;
            this.cboConnectionProtocol.TabIndex = 1;
            // 
            // cboSslMode
            // 
            this.cboSslMode.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "SslMode", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cboSslMode.Location = new System.Drawing.Point(126, 96);
            this.cboSslMode.Name = "cboSslMode";
            this.cboSslMode.Properties.Appearance.Options.UseTextOptions = true;
            this.cboSslMode.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.cboSslMode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboSslMode.Properties.NullText = "";
            this.cboSslMode.Properties.PopupSizeable = false;
            this.cboSslMode.Size = new System.Drawing.Size(340, 20);
            this.cboSslMode.StyleController = this.layoutControl;
            this.cboSslMode.TabIndex = 1;
            // 
            // PageAddition
            // 
            this.PageAddition.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForPort,
            this.ItemForConnectionProtocol,
            this.ItemForSslMode,
            this.ItemForCharacterSet});
            this.PageAddition.Location = new System.Drawing.Point(0, 0);
            this.PageAddition.Name = "PageAddition";
            this.PageAddition.Size = new System.Drawing.Size(444, 96);
            this.PageAddition.Text = "Дополнительно";
            // 
            // ItemForPort
            // 
            this.ItemForPort.Control = this.PortTextEdit;
            this.ItemForPort.Location = new System.Drawing.Point(0, 0);
            this.ItemForPort.Name = "ItemForPort";
            this.ItemForPort.Size = new System.Drawing.Size(222, 24);
            this.ItemForPort.Text = "Port";
            this.ItemForPort.TextSize = new System.Drawing.Size(96, 13);
            // 
            // ItemForCharacterSet
            // 
            this.ItemForCharacterSet.Control = this.CharacterSetTextEdit;
            this.ItemForCharacterSet.Location = new System.Drawing.Point(222, 0);
            this.ItemForCharacterSet.Name = "ItemForCharacterSet";
            this.ItemForCharacterSet.Size = new System.Drawing.Size(222, 24);
            this.ItemForCharacterSet.Text = "Character Set";
            this.ItemForCharacterSet.TextSize = new System.Drawing.Size(96, 13);
            // 
            // ItemForConnectionProtocol
            // 
            this.ItemForConnectionProtocol.Control = this.cboConnectionProtocol;
            this.ItemForConnectionProtocol.Location = new System.Drawing.Point(0, 24);
            this.ItemForConnectionProtocol.Name = "ItemForConnectionProtocol";
            this.ItemForConnectionProtocol.Size = new System.Drawing.Size(444, 24);
            this.ItemForConnectionProtocol.Text = "Connection Protocol";
            this.ItemForConnectionProtocol.TextSize = new System.Drawing.Size(96, 13);
            // 
            // ItemForSslMode
            // 
            this.ItemForSslMode.Control = this.cboSslMode;
            this.ItemForSslMode.Location = new System.Drawing.Point(0, 48);
            this.ItemForSslMode.Name = "ItemForSslMode";
            this.ItemForSslMode.Size = new System.Drawing.Size(444, 48);
            this.ItemForSslMode.Text = "Ssl Mode";
            this.ItemForSslMode.TextSize = new System.Drawing.Size(96, 13);
            // 
            // tabbedControlGroup1
            // 
            this.tabbedControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.tabbedControlGroup1.Name = "tabbedControlGroup1";
            this.tabbedControlGroup1.SelectedTabPage = this.PageConnect;
            this.tabbedControlGroup1.SelectedTabPageIndex = 0;
            this.tabbedControlGroup1.Size = new System.Drawing.Size(468, 142);
            this.tabbedControlGroup1.TabPages.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.PageConnect,
            this.PageAddition});
            // 
            // PageConnect
            // 
            this.PageConnect.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForServer,
            this.ItemForDatabase,
            this.ItemForUserID,
            this.ItemForPassword});
            this.PageConnect.Location = new System.Drawing.Point(0, 0);
            this.PageConnect.Name = "PageConnect";
            this.PageConnect.Size = new System.Drawing.Size(444, 96);
            this.PageConnect.Text = "Параметры соединения";
            // 
            // ItemForServer
            // 
            this.ItemForServer.Control = this.ServerTextEdit;
            this.ItemForServer.Location = new System.Drawing.Point(0, 0);
            this.ItemForServer.Name = "ItemForServer";
            this.ItemForServer.Size = new System.Drawing.Size(444, 24);
            this.ItemForServer.Text = "Server";
            this.ItemForServer.TextSize = new System.Drawing.Size(96, 13);
            // 
            // ItemForDatabase
            // 
            this.ItemForDatabase.Control = this.DatabaseTextEdit;
            this.ItemForDatabase.Location = new System.Drawing.Point(0, 24);
            this.ItemForDatabase.Name = "ItemForDatabase";
            this.ItemForDatabase.Size = new System.Drawing.Size(444, 24);
            this.ItemForDatabase.Text = "Database";
            this.ItemForDatabase.TextSize = new System.Drawing.Size(96, 13);
            // 
            // ItemForUserID
            // 
            this.ItemForUserID.Control = this.UserIDTextEdit;
            this.ItemForUserID.Location = new System.Drawing.Point(0, 48);
            this.ItemForUserID.Name = "ItemForUserID";
            this.ItemForUserID.Size = new System.Drawing.Size(444, 24);
            this.ItemForUserID.Text = "User ID";
            this.ItemForUserID.TextSize = new System.Drawing.Size(96, 13);
            // 
            // ItemForPassword
            // 
            this.ItemForPassword.Control = this.PasswordTextEdit;
            this.ItemForPassword.Location = new System.Drawing.Point(0, 72);
            this.ItemForPassword.Name = "ItemForPassword";
            this.ItemForPassword.Size = new System.Drawing.Size(444, 24);
            this.ItemForPassword.Text = "Password";
            this.ItemForPassword.TextSize = new System.Drawing.Size(96, 13);
            // 
            // CfgFrameBridgeNote
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Caption = "Bridgenote.com";
            this.Image = ((System.Drawing.Image)(resources.GetObject("$this.Image")));
            this.LargeImage = ((System.Drawing.Image)(resources.GetObject("$this.LargeImage")));
            this.Name = "CfgFrameBridgeNote";
            this.Size = new System.Drawing.Size(492, 519);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rootGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EditGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ServerTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DatabaseTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserIDTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PasswordTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PortTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CharacterSetTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboConnectionProtocol.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSslMode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PageAddition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForCharacterSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForConnectionProtocol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForSslMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PageConnect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForServer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDatabase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUserID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPassword)).EndInit();
            this.ResumeLayout(false);
        }
        private DevExpress.XtraEditors.TextEdit ServerTextEdit;
        private DevExpress.XtraEditors.TextEdit DatabaseTextEdit;
        private DevExpress.XtraEditors.TextEdit UserIDTextEdit;
        private DevExpress.XtraEditors.TextEdit PasswordTextEdit;
        private DevExpress.XtraEditors.TextEdit PortTextEdit;
        private DevExpress.XtraEditors.TextEdit CharacterSetTextEdit;
        private DevExpress.XtraEditors.LookUpEdit cboConnectionProtocol;
        private DevExpress.XtraLayout.LayoutControlGroup PageAddition;
        private DevExpress.XtraLayout.LayoutControlItem ItemForPort;
        private DevExpress.XtraLayout.LayoutControlItem ItemForCharacterSet;
        private DevExpress.XtraLayout.LayoutControlItem ItemForConnectionProtocol;
        private DevExpress.XtraLayout.LayoutControlItem ItemForSslMode;
        private DevExpress.XtraEditors.LookUpEdit cboSslMode;
        private DevExpress.XtraLayout.TabbedControlGroup tabbedControlGroup1;
        private DevExpress.XtraLayout.LayoutControlGroup PageConnect;
        private DevExpress.XtraLayout.LayoutControlItem ItemForServer;
        private DevExpress.XtraLayout.LayoutControlItem ItemForDatabase;
        private DevExpress.XtraLayout.LayoutControlItem ItemForUserID;
        private DevExpress.XtraLayout.LayoutControlItem ItemForPassword;
    }
}
