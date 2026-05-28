namespace MeshokBrowser
{
    partial class MainConfigFrame
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
            this.layoutControl = new GH.Controls.LayoutControlGh();
            this.lgRoot = new DevExpress.XtraLayout.LayoutControlGroup();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lgRoot)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.AllowCustomization = false;
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(9, 9);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(883, 252, 650, 400);
            this.layoutControl.OptionsFocus.AllowFocusGroups = false;
            this.layoutControl.OptionsFocus.AllowFocusReadonlyEditors = false;
            this.layoutControl.OptionsFocus.AllowFocusTabbedGroups = false;
            this.layoutControl.Owner = this;
            this.layoutControl.Root = this.lgRoot;
            this.layoutControl.SaveLayout = false;
            this.layoutControl.Size = new System.Drawing.Size(1019, 695);
            this.layoutControl.TabIndex = 16;
            this.layoutControl.Text = "layoutControl";
            // 
            // lgRoot
            // 
            this.lgRoot.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lgRoot.GroupBordersVisible = false;
            this.lgRoot.Name = "Root";
            this.lgRoot.OptionsItemText.TextToControlDistance = 5;
            this.lgRoot.Size = new System.Drawing.Size(1019, 695);
            this.lgRoot.TextVisible = false;
            // 
            // MainSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Caption = "Настройки программы";
            this.Controls.Add(this.layoutControl);
            this.Name = "MainSetting";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.SaveLayout = false;
            this.Size = new System.Drawing.Size(1037, 713);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lgRoot)).EndInit();
            this.ResumeLayout(false);
        }
        private GH.Controls.LayoutControlGh layoutControl;
        private DevExpress.XtraLayout.LayoutControlGroup lgRoot;
    }
}
