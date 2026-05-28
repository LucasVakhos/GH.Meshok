namespace MeshokBrowser
{
    partial class DefSplashScreen
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
            instance = null;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DefSplashScreen));
            this.marqueeProgress = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.labelCopyrith = new DevExpress.XtraEditors.LabelControl();
            this.labelRunBegin = new DevExpress.XtraEditors.LabelControl();
            this.pictureEdit2 = new DevExpress.XtraEditors.PictureEdit();
            this.progressBar = new DevExpress.XtraEditors.ProgressBarControl();
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgress.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // marqueeProgress
            // 
            this.marqueeProgress.EditValue = 0;
            this.marqueeProgress.Location = new System.Drawing.Point(23, 263);
            this.marqueeProgress.Name = "marqueeProgress";
            this.marqueeProgress.Size = new System.Drawing.Size(404, 17);
            this.marqueeProgress.TabIndex = 5;
            // 
            // labelCopyrith
            // 
            this.labelCopyrith.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.labelCopyrith.Location = new System.Drawing.Point(23, 286);
            this.labelCopyrith.Name = "labelCopyrith";
            this.labelCopyrith.Size = new System.Drawing.Size(87, 13);
            this.labelCopyrith.TabIndex = 6;
            this.labelCopyrith.Text = "Copyright © 2018";
            // 
            // labelRunBegin
            // 
            this.labelRunBegin.Location = new System.Drawing.Point(23, 244);
            this.labelRunBegin.Name = "labelRunBegin";
            this.labelRunBegin.Size = new System.Drawing.Size(105, 13);
            this.labelRunBegin.TabIndex = 7;
            this.labelRunBegin.Text = "Запуск программы...";
            // 
            // pictureEdit2
            // 
            this.pictureEdit2.Cursor = System.Windows.Forms.Cursors.Default;
            this.pictureEdit2.EditValue = ((object)(resources.GetObject("pictureEdit2.EditValue")));
            this.pictureEdit2.Location = new System.Drawing.Point(12, 12);
            this.pictureEdit2.Name = "pictureEdit2";
            this.pictureEdit2.Properties.AllowFocused = false;
            this.pictureEdit2.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pictureEdit2.Properties.Appearance.Options.UseBackColor = true;
            this.pictureEdit2.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit2.Properties.ShowMenu = false;
            this.pictureEdit2.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pictureEdit2.Size = new System.Drawing.Size(426, 220);
            this.pictureEdit2.TabIndex = 9;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(177, 239);
            this.progressBar.Name = "progressBar";
            this.progressBar.Properties.ShowTitle = true;
            this.progressBar.Size = new System.Drawing.Size(250, 18);
            this.progressBar.TabIndex = 10;
            this.progressBar.Visible = false;
            // 
            // DefSplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 308);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.pictureEdit2);
            this.Controls.Add(this.labelRunBegin);
            this.Controls.Add(this.labelCopyrith);
            this.Controls.Add(this.marqueeProgress);
            this.Name = "DefSplashScreen";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.DefSplashScreen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgress.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private DevExpress.XtraEditors.MarqueeProgressBarControl marqueeProgress;
        private DevExpress.XtraEditors.LabelControl labelCopyrith;
        private DevExpress.XtraEditors.LabelControl labelRunBegin;
        private DevExpress.XtraEditors.ProgressBarControl progressBar;
        public DevExpress.XtraEditors.PictureEdit pictureEdit2;
    }
}
