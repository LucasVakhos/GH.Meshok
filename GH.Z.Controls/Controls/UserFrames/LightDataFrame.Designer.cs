namespace GH.Components
{
    partial class LightDataFrame
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.actionList = new GH.Components.ActionList();
            this.dataSource = new GH.Components.DataSource(this.components);
            this.layoutControl = new GH.Components.LayoutControlGh();
            this.lgRoot = new DevExpress.XtraLayout.LayoutControlGroup();
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lgRoot)).BeginInit();
            this.SuspendLayout();
            // 
            // actionList
            // 
            this.actionList.ContainerControl = this;
            // 
            // dataSource
            // 
            this.dataSource.Owner = this;
            // 
            // layoutControl
            // 
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(639, 135, 650, 400);
            this.layoutControl.OptionsFocus.AllowFocusGroups = false;
            this.layoutControl.OptionsFocus.AllowFocusReadonlyEditors = false;
            this.layoutControl.OptionsFocus.AllowFocusTabbedGroups = false;
            this.layoutControl.Root = this.lgRoot;
            this.layoutControl.Size = new System.Drawing.Size(551, 367);
            this.layoutControl.TabIndex = 1;
            this.layoutControl.Text = "layoutControl";
            // 
            // lgRoot
            // 
            this.lgRoot.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lgRoot.GroupBordersVisible = false;
            this.lgRoot.Name = "Root";
            this.lgRoot.Size = new System.Drawing.Size(551, 367);
            this.lgRoot.TextVisible = false;
            // 
            // LightDataFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl);
            this.FrameDataSource = this.dataSource;
            this.Name = "LightDataFrame";
            this.Size = new System.Drawing.Size(551, 367);
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lgRoot)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public GH.Components.DataSource dataSource;
        public GH.Components.ActionList actionList;
        protected internal GH.Components.LayoutControlGh layoutControl;
        protected internal DevExpress.XtraLayout.LayoutControlGroup lgRoot;
    }
}
