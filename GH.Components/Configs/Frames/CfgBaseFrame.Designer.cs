namespace GH.Components
{
    partial class CfgBaseFrame
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
            this.components = new System.ComponentModel.Container();
            this.layoutControl = new GH.Components.LayoutControlGh();
            this.dataSource = new GH.Components.DataSource(this.components);
            this.actionList = new GH.Components.ActionList();
            this.EditGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.rootGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EditGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rootGroup)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.AllowCustomization = false;
            this.layoutControl.AutoScroll = false;
            this.layoutControl.DataSource = this.dataSource;
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.OptionsCustomizationForm.DefaultPage = DevExpress.XtraLayout.CustomizationPage.LayoutTreeView;
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(587, 132, 569, 336);
            this.layoutControl.OptionsFocus.AllowFocusGroups = false;
            this.layoutControl.OptionsFocus.AllowFocusReadonlyEditors = false;
            this.layoutControl.OptionsFocus.AllowFocusTabbedGroups = false;
            this.layoutControl.OptionsView.AlwaysScrollActiveControlIntoView = false;
            this.layoutControl.Owner = this;
            this.layoutControl.Root = this.rootGroup;
            this.layoutControl.Size = new System.Drawing.Size(386, 52);
            this.layoutControl.TabIndex = 2;
            this.layoutControl.Text = "dataLayoutControl1";
            // 
            // dataSource
            // 
            this.dataSource.ActionList = this.actionList;
            this.dataSource.AllowDelete = false;
            this.dataSource.AllowInsert = false;
            this.dataSource.AllowSaveCancel = false;
            this.dataSource.AllowUdate = false;
            this.dataSource.IsLocalDataSet = true;
            this.dataSource.Owner = this;
            this.dataSource.PageSupport.EditGroup = this.EditGroup;
            this.dataSource.OnPost += new System.EventHandler(this.dataSource_OnPost);
            // 
            // actionList
            // 
            this.actionList.Owner = this;
            // 
            // EditGroup
            // 
            this.EditGroup.Location = new System.Drawing.Point(0, 0);
            this.EditGroup.Name = "EditGroup";
            this.EditGroup.Size = new System.Drawing.Size(386, 52);
            this.EditGroup.TextVisible = false;
            // 
            // rootGroup
            // 
            this.rootGroup.GroupBordersVisible = false;
            this.rootGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.EditGroup});
            this.rootGroup.Name = "Root";
            this.rootGroup.Size = new System.Drawing.Size(386, 52);
            // 
            // CfgBaseFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Caption = "Вход в систему";
            this.Controls.Add(this.layoutControl);
            this.Name = "CfgBaseFrame";
            this.Size = new System.Drawing.Size(386, 137);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EditGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rootGroup)).EndInit();
            this.ResumeLayout(false);
        }
        public GH.Components.LayoutControlGh layoutControl;
        public DevExpress.XtraLayout.LayoutControlGroup rootGroup;
        public DevExpress.XtraLayout.LayoutControlGroup EditGroup;
        public GH.Components.DataSource dataSource;
        public Components.ActionList actionList;
    }
}
