namespace MeshokBrowser
{
    partial class ConfigBridgeFrame
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
            this.BaseTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ItemForBase = new DevExpress.XtraLayout.LayoutControlItem();
            this.UserTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ItemForUser = new DevExpress.XtraLayout.LayoutControlItem();
            this.PassWrdTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ItemForPassWrd = new DevExpress.XtraLayout.LayoutControlItem();
#pragma warning disable CS0436 // Тип конфликтует с импортированным типом
            this.connectButton1 = new GH.Components.ConnectButton();
#pragma warning restore CS0436 // Тип конфликтует с импортированным типом
            this.lcConnection = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lgRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EditGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ServerTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForServer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BaseTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForBase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PassWrdTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPassWrd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.connectButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcConnection)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.connectButton1);
            this.layoutControl.Controls.Add(this.ServerTextEdit);
            this.layoutControl.Controls.Add(this.BaseTextEdit);
            this.layoutControl.Controls.Add(this.UserTextEdit);
            this.layoutControl.Controls.Add(this.PassWrdTextEdit);
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(565, 126, 1001, 773);
            this.layoutControl.OptionsFocus.AllowFocusGroups = false;
            this.layoutControl.OptionsFocus.AllowFocusReadonlyEditors = false;
            this.layoutControl.OptionsFocus.AllowFocusTabbedGroups = false;
            // 
            // lgRoot
            // 
            this.lgRoot.OptionsItemText.TextToControlDistance = 5;
            // 
            // EditGroup
            // 
            this.EditGroup.CustomizationFormText = "Bridgenote";
            this.EditGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForServer,
            this.ItemForBase,
            this.ItemForUser,
            this.ItemForPassWrd,
            this.lcConnection});
            this.EditGroup.OptionsItemText.TextToControlDistance = 5;
            this.EditGroup.Size = new System.Drawing.Size(816, 176);
            this.EditGroup.Text = "Bridgenote.com";
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 176);
            this.emptySpaceItem1.Size = new System.Drawing.Size(816, 198);
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
            this.ItemForServer.Size = new System.Drawing.Size(810, 30);
            this.ItemForServer.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.ItemForServer.TextSize = new System.Drawing.Size(80, 13);
            // 
            // BaseTextEdit
            // 
            this.BaseTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "Base", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.BaseTextEdit.Location = new System.Drawing.Point(93, 56);
            this.BaseTextEdit.Name = "BaseTextEdit";
            this.BaseTextEdit.Size = new System.Drawing.Size(310, 20);
            this.BaseTextEdit.StyleController = this.layoutControl;
            this.BaseTextEdit.TabIndex = 5;
            // 
            // ItemForBase
            // 
            this.ItemForBase.Control = this.BaseTextEdit;
            this.ItemForBase.Location = new System.Drawing.Point(0, 30);
            this.ItemForBase.MaxSize = new System.Drawing.Size(405, 30);
            this.ItemForBase.MinSize = new System.Drawing.Size(405, 30);
            this.ItemForBase.Name = "ItemForBase";
            this.ItemForBase.OptionsToolTip.ToolTip = "Путь к базе данных";
            this.ItemForBase.Size = new System.Drawing.Size(810, 30);
            this.ItemForBase.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.ItemForBase.TextSize = new System.Drawing.Size(80, 13);
            // 
            // UserTextEdit
            // 
            this.UserTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "User", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.UserTextEdit.Location = new System.Drawing.Point(93, 86);
            this.UserTextEdit.Name = "UserTextEdit";
            this.UserTextEdit.Size = new System.Drawing.Size(310, 20);
            this.UserTextEdit.StyleController = this.layoutControl;
            this.UserTextEdit.TabIndex = 6;
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
            this.PassWrdTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "PassWrd", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.PassWrdTextEdit.Location = new System.Drawing.Point(93, 116);
            this.PassWrdTextEdit.Name = "PassWrdTextEdit";
            this.PassWrdTextEdit.Size = new System.Drawing.Size(310, 20);
            this.PassWrdTextEdit.StyleController = this.layoutControl;
            this.PassWrdTextEdit.TabIndex = 7;
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
            this.connectButton1.Size = new System.Drawing.Size(800, 22);
            this.connectButton1.StyleController = this.layoutControl;
            this.connectButton1.TabIndex = 8;
            // 
            // lcConnection
            // 
            this.lcConnection.Control = this.connectButton1;
            this.lcConnection.Location = new System.Drawing.Point(0, 120);
            this.lcConnection.Name = "lcConnection";
            this.lcConnection.Size = new System.Drawing.Size(810, 32);
            this.lcConnection.TextSize = new System.Drawing.Size(0, 0);
            this.lcConnection.TextVisible = false;
            // 
            // ConfigBridgeFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Caption = "Bridgenote.com";
            this.Name = "ConfigBridgeFrame";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lgRoot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EditGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ServerTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForServer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BaseTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForBase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PassWrdTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForPassWrd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.connectButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcConnection)).EndInit();
            this.ResumeLayout(false);
        }
        private DevExpress.XtraEditors.TextEdit ServerTextEdit;
        private DevExpress.XtraLayout.LayoutControlItem ItemForServer;
        private DevExpress.XtraLayout.LayoutControlItem ItemForBase;
        private DevExpress.XtraEditors.TextEdit BaseTextEdit;
        private DevExpress.XtraLayout.LayoutControlItem ItemForUser;
        private DevExpress.XtraEditors.TextEdit UserTextEdit;
        private DevExpress.XtraLayout.LayoutControlItem ItemForPassWrd;
        private DevExpress.XtraEditors.TextEdit PassWrdTextEdit;
#pragma warning disable CS0436 // Тип конфликтует с импортированным типом
        private GH.Components.ConnectButton connectButton1;
#pragma warning restore CS0436 // Тип конфликтует с импортированным типом
        private DevExpress.XtraLayout.LayoutControlItem lcConnection;
    }
}
