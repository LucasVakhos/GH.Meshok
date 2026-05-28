namespace GH.Components
{
    partial class DataFrame
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataFrame));
            this.actionList = new GH.Components.ActionList();
            this.layoutControl = new GH.Components.LayoutControlGh();
            this.lgRoot = new DevExpress.XtraLayout.LayoutControlGroup();
            this.dataSource = new GH.Components.DataSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lgRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).BeginInit();
            this.SuspendLayout();
            // 
            // actionList
            // 
            this.actionList.ContainerControl = this;
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
            this.layoutControl.Size = new System.Drawing.Size(469, 426);
            this.layoutControl.TabIndex = 0;
            this.layoutControl.Text = "layoutControl";
            // 
            // lgRoot
            // 
            this.lgRoot.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lgRoot.GroupBordersVisible = false;
            this.lgRoot.Name = "Root";
            this.lgRoot.Size = new System.Drawing.Size(469, 426);
            this.lgRoot.TextVisible = false;
            // 
            // dataSource
            // 
            this.dataSource.Owner = this;
            // 
            // DataFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Caption = "Форма с датасетом";
            this.Controls.Add(this.layoutControl);
            this.FrameDataSource = this.dataSource;
            this.LargeImage = ((System.Drawing.Image)(resources.GetObject("$this.LargeImage")));
            this.Name = "DataFrame";
            this.Size = new System.Drawing.Size(469, 426);
            this.SmallImage = ((System.Drawing.Image)(resources.GetObject("$this.SmallImage")));
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lgRoot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public GH.Components.DataSource dataSource;
        public GH.Components.ActionList actionList;
        protected internal DevExpress.XtraLayout.LayoutControlGroup lgRoot;
        protected internal GH.Components.LayoutControlGh layoutControl;
    }
}
