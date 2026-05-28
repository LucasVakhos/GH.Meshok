namespace MeshokBrowser
{
    partial class BaseSetting<T>
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
            this.layoutControl = new GH.Components.LayoutControlGh();
            this.lgRoot = new DevExpress.XtraLayout.LayoutControlGroup();
            this.EditGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lgRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EditGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // _ribbonControl
            // 
            // 
            // dataSource
            // 
            this.dataSource.AllowDelete = false;
            this.dataSource.AllowInsert = false;
            this.dataSource.AllowUdate = false;
            this.dataSource.PageSupport.EditGroup = this.EditGroup;
            this.dataSource.VitualDataSet = true;
            this.dataSource.OnOpen += new GH.Components.OpenHandler(this.dataSource_OnOpen);
            this.dataSource.OnPost += new System.EventHandler(this.dataSource_OnPost);
            this.dataSource.OnCancel += new System.EventHandler(this.dataSource_OnCancel);
            // 
            // layoutControl
            // 
            this.layoutControl.AllowCustomization = false;
            this.layoutControl.DataSource = this.dataSource;
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 47);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(659, 220, 1001, 773);
            this.layoutControl.OptionsFocus.AllowFocusGroups = false;
            this.layoutControl.OptionsFocus.AllowFocusReadonlyEditors = false;
            this.layoutControl.OptionsFocus.AllowFocusTabbedGroups = false;
            this.layoutControl.Owner = this;
            this.layoutControl.Root = this.lgRoot;
            this.layoutControl.Size = new System.Drawing.Size(816, 300);
            this.layoutControl.TabIndex = 16;
            this.layoutControl.Text = "layoutControlGh1";
            // 
            // lgRoot
            // 
            this.lgRoot.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lgRoot.GroupBordersVisible = false;
            this.lgRoot.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.EditGroup,
            this.emptySpaceItem1});
            this.lgRoot.Name = "Root";
            this.lgRoot.OptionsItemText.TextToControlDistance = 5;
            this.lgRoot.Size = new System.Drawing.Size(816, 300);
            this.lgRoot.TextVisible = false;
            // 
            // EditGroup
            // 
            this.EditGroup.Location = new System.Drawing.Point(0, 0);
            this.EditGroup.Name = "EditGroup";
            this.EditGroup.Size = new System.Drawing.Size(816, 176);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 176);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(816, 124);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // BaseConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Caption = "Название";
            this.Controls.Add(this.layoutControl);
            this.Name = "BaseConfig";
            this.SaveLayout = false;
            this.Size = new System.Drawing.Size(816, 374);
            this.Controls.SetChildIndex(this.layoutControl, 0);
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lgRoot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EditGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        public GH.Components.LayoutControlGh layoutControl;
        public DevExpress.XtraLayout.LayoutControlGroup lgRoot;
        public DevExpress.XtraLayout.LayoutControlGroup EditGroup;
        public DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
    }
}
