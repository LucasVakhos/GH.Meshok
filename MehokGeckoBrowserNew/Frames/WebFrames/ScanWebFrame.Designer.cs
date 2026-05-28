namespace MeshokBrowser.Workers
{
    partial class ScanWebFrame
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
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.webBrowser = new Gecko.GeckoWebBrowser();
            this.lgRoot = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcBrowser = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lgRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcBrowser)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.webBrowser);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Enabled = false;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(22, 121, 650, 400);
            this.layoutControl.Root = this.lgRoot;
            this.layoutControl.Size = new System.Drawing.Size(326, 261);
            this.layoutControl.TabIndex = 0;
            this.layoutControl.Text = "layoutControl1";
            // 
            // webBrowser
            // 
            this.webBrowser.FrameEventsPropagateToMainWindow = false;
            this.webBrowser.Location = new System.Drawing.Point(12, 12);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(302, 237);
            this.webBrowser.TabIndex = 4;
            this.webBrowser.UseHttpActivityObserver = false;
            // 
            // lgRoot
            // 
            this.lgRoot.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lgRoot.GroupBordersVisible = false;
            this.lgRoot.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcBrowser});
            this.lgRoot.Name = "Root";
            this.lgRoot.Size = new System.Drawing.Size(326, 261);
            this.lgRoot.TextVisible = false;
            // 
            // lcBrowser
            // 
            this.lcBrowser.Control = this.webBrowser;
            this.lcBrowser.Location = new System.Drawing.Point(0, 0);
            this.lcBrowser.Name = "lcBrowser";
            this.lcBrowser.Size = new System.Drawing.Size(306, 241);
            this.lcBrowser.TextSize = new System.Drawing.Size(0, 0);
            this.lcBrowser.TextVisible = false;
            // 
            // ParsingBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl);
            this.Name = "ParsingBrowser";
            this.Size = new System.Drawing.Size(326, 261);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lgRoot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcBrowser)).EndInit();
            this.ResumeLayout(false);
        }
        protected DevExpress.XtraLayout.LayoutControl layoutControl;
        protected DevExpress.XtraLayout.LayoutControlGroup lgRoot;
        protected DevExpress.XtraLayout.LayoutControlItem lcBrowser;
        public Gecko.GeckoWebBrowser webBrowser;
    }
}
