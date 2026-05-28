namespace GH.Configs
{
    partial class LoginFrame
    {
        private System.ComponentModel.IContainer components = null;

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
            this.ItemForAutoEntering = new DevExpress.XtraLayout.LayoutControlItem();
            this.AutoEnteringCheckEdit = new DevExpress.XtraEditors.CheckEdit();
            this.lcLogin = new DevExpress.XtraLayout.LayoutControlItem();
            this.userLogin = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lcPassword = new DevExpress.XtraLayout.LayoutControlItem();
            this.userPassword = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rootGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EditGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForAutoEntering)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AutoEnteringCheckEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcLogin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userLogin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userPassword.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.AutoEnteringCheckEdit);
            this.layoutControl.Controls.Add(this.userPassword);
            this.layoutControl.Controls.Add(this.userLogin);
            this.layoutControl.OptionsCustomizationForm.DefaultPage = DevExpress.XtraLayout.CustomizationPage.LayoutTreeView;
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(587, 132, 569, 336);
            this.layoutControl.OptionsFocus.AllowFocusGroups = false;
            this.layoutControl.OptionsFocus.AllowFocusReadonlyEditors = false;
            this.layoutControl.OptionsFocus.AllowFocusTabbedGroups = false;
            this.layoutControl.OptionsView.AlwaysScrollActiveControlIntoView = false;
            this.layoutControl.Size = new System.Drawing.Size(358, 95);
            // 
            // rootGroup
            // 
            this.rootGroup.Size = new System.Drawing.Size(358, 95);
            // 
            // EditGroup
            // 
            this.EditGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcPassword,
            this.lcLogin,
            this.ItemForAutoEntering});
            this.EditGroup.Size = new System.Drawing.Size(358, 95);
            // 
            // dataSource
            // 
            this.dataSource.DataSource = typeof(GH.Configs.CfgCoreConnection);
            this.dataSource.PageSupport.EditGroup = this.EditGroup;
            // 
            // ItemForAutoEntering
            // 
            this.ItemForAutoEntering.Control = this.AutoEnteringCheckEdit;
            this.ItemForAutoEntering.Location = new System.Drawing.Point(0, 48);
            this.ItemForAutoEntering.Name = "ItemForAutoEntering";
            this.ItemForAutoEntering.OptionsToolTip.ToolTip = "Автовход если доступ разрешён";
            this.ItemForAutoEntering.Size = new System.Drawing.Size(334, 23);
            this.ItemForAutoEntering.TextSize = new System.Drawing.Size(0, 0);
            this.ItemForAutoEntering.TextVisible = false;
            // 
            // AutoEnteringCheckEdit
            // 
            this.AutoEnteringCheckEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "AutoEntering", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.AutoEnteringCheckEdit.Location = new System.Drawing.Point(14, 62);
            this.AutoEnteringCheckEdit.Name = "AutoEnteringCheckEdit";
            this.AutoEnteringCheckEdit.Properties.Caption = "Автовход";
            this.AutoEnteringCheckEdit.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.AutoEnteringCheckEdit.Size = new System.Drawing.Size(330, 19);
            this.AutoEnteringCheckEdit.StyleController = this.layoutControl;
            this.AutoEnteringCheckEdit.TabIndex = 7;
            // 
            // lcLogin
            // 
            this.lcLogin.Control = this.userLogin;
            this.lcLogin.Location = new System.Drawing.Point(0, 0);
            this.lcLogin.Name = "lcLogin";
            this.lcLogin.Size = new System.Drawing.Size(334, 24);
            this.lcLogin.Text = "Логин";
            this.lcLogin.TextSize = new System.Drawing.Size(37, 13);
            // 
            // userLogin
            // 
            this.userLogin.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "UserLogin", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.userLogin.Location = new System.Drawing.Point(55, 14);
            this.userLogin.Name = "userLogin";
            this.userLogin.Size = new System.Drawing.Size(289, 20);
            this.userLogin.StyleController = this.layoutControl;
            this.userLogin.TabIndex = 8;
            // 
            // lcPassword
            // 
            this.lcPassword.Control = this.userPassword;
            this.lcPassword.Location = new System.Drawing.Point(0, 24);
            this.lcPassword.Name = "lcPassword";
            this.lcPassword.Size = new System.Drawing.Size(334, 24);
            this.lcPassword.Text = "Пароль";
            this.lcPassword.TextSize = new System.Drawing.Size(37, 13);
            // 
            // userPassword
            // 
            this.userPassword.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "UserPassword", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.userPassword.Location = new System.Drawing.Point(55, 38);
            this.userPassword.Name = "userPassword";
            this.userPassword.Size = new System.Drawing.Size(289, 20);
            this.userPassword.StyleController = this.layoutControl;
            this.userPassword.TabIndex = 9;
            // 
            // LoginFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Caption = "Логин";
            this.Name = "LoginFrame";
            this.Size = new System.Drawing.Size(358, 236);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rootGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EditGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForAutoEntering)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AutoEnteringCheckEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcLogin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userLogin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userPassword.Properties)).EndInit();
            this.ResumeLayout(false);

        }
        public DevExpress.XtraLayout.LayoutControlItem lcLogin;
        public DevExpress.XtraEditors.ComboBoxEdit userLogin;
        public DevExpress.XtraLayout.LayoutControlItem lcPassword;
        public DevExpress.XtraEditors.TextEdit userPassword;
        public DevExpress.XtraLayout.LayoutControlItem ItemForAutoEntering;
        public DevExpress.XtraEditors.CheckEdit AutoEnteringCheckEdit;
    }

}
