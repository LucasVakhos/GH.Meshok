namespace GH.Components
{
    partial class AboutBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.lblCompany = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.aboutGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.aboutExtraLabel = new DevExpress.XtraLayout.SimpleLabelItem();
            this.lblProductname = new DevExpress.XtraLayout.SimpleLabelItem();
            this.lblCopyright = new DevExpress.XtraLayout.SimpleLabelItem();
            this.lblVersion = new DevExpress.XtraLayout.SimpleLabelItem();
            this.lcCompany = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcLogo = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.simpleLabelItem1 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.simpleSeparator1 = new DevExpress.XtraLayout.SimpleSeparator();
            this.simpleLabelItem2 = new DevExpress.XtraLayout.SimpleLabelItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aboutGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aboutExtraLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblProductname)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCopyright)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblVersion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcCompany)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleSeparator1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.pictureEdit1);
            this.layoutControl1.Controls.Add(this.lblCompany);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(554, 13, 650, 400);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(464, 275);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.EditValue = ((object)(resources.GetObject("pictureEdit1.EditValue")));
            this.pictureEdit1.Location = new System.Drawing.Point(12, 12);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.PictureAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pictureEdit1.Size = new System.Drawing.Size(141, 251);
            this.pictureEdit1.StyleController = this.layoutControl1;
            this.pictureEdit1.TabIndex = 6;
            // 
            // lblCompany
            // 
            this.lblCompany.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblCompany.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblCompany.Location = new System.Drawing.Point(169, 93);
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Size = new System.Drawing.Size(271, 13);
            this.lblCompany.StyleController = this.layoutControl1;
            this.lblCompany.TabIndex = 5;
            this.lblCompany.Text = "Company";
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.aboutGroup,
            this.lcLogo});
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(464, 275);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // aboutGroup
            // 
            this.aboutGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.aboutExtraLabel,
            this.lblProductname,
            this.lblCopyright,
            this.lblVersion,
            this.lcCompany});
            this.aboutGroup.Location = new System.Drawing.Point(145, 0);
            this.aboutGroup.Name = "aboutGroup";
            this.aboutGroup.Size = new System.Drawing.Size(299, 255);
            this.aboutGroup.Text = "О программе";
            // 
            // aboutExtraLabel
            // 
            this.aboutExtraLabel.AllowHotTrack = false;
            this.aboutExtraLabel.AppearanceItemCaption.Options.UseTextOptions = true;
            this.aboutExtraLabel.AppearanceItemCaption.TextOptions.HotkeyPrefix = DevExpress.Utils.HKeyPrefix.None;
            this.aboutExtraLabel.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.aboutExtraLabel.AppearanceItemCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.aboutExtraLabel.Location = new System.Drawing.Point(0, 68);
            this.aboutExtraLabel.Name = "aboutExtraLabel";
            this.aboutExtraLabel.Size = new System.Drawing.Size(275, 145);
            this.aboutExtraLabel.Text = "About Extra";
            this.aboutExtraLabel.TextSize = new System.Drawing.Size(66, 13);
            // 
            // lblProductname
            // 
            this.lblProductname.AllowHotTrack = false;
            this.lblProductname.Location = new System.Drawing.Point(0, 0);
            this.lblProductname.Name = "lblProductname";
            this.lblProductname.Size = new System.Drawing.Size(275, 17);
            this.lblProductname.Text = "Product name";
            this.lblProductname.TextSize = new System.Drawing.Size(66, 13);
            // 
            // lblCopyright
            // 
            this.lblCopyright.AllowHotTrack = false;
            this.lblCopyright.Location = new System.Drawing.Point(0, 17);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(275, 17);
            this.lblCopyright.Text = "Copyrigth";
            this.lblCopyright.TextSize = new System.Drawing.Size(66, 13);
            // 
            // lblVersion
            // 
            this.lblVersion.AllowHotTrack = false;
            this.lblVersion.Location = new System.Drawing.Point(0, 34);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(275, 17);
            this.lblVersion.Text = "Version";
            this.lblVersion.TextSize = new System.Drawing.Size(66, 13);
            // 
            // lcCompany
            // 
            this.lcCompany.Control = this.lblCompany;
            this.lcCompany.Location = new System.Drawing.Point(0, 51);
            this.lcCompany.Name = "lcCompany";
            this.lcCompany.Size = new System.Drawing.Size(275, 17);
            this.lcCompany.Text = "Company";
            this.lcCompany.TextSize = new System.Drawing.Size(0, 0);
            this.lcCompany.TextVisible = false;
            // 
            // lcLogo
            // 
            this.lcLogo.Control = this.pictureEdit1;
            this.lcLogo.Location = new System.Drawing.Point(0, 0);
            this.lcLogo.MaxSize = new System.Drawing.Size(145, 255);
            this.lcLogo.MinSize = new System.Drawing.Size(145, 255);
            this.lcLogo.Name = "lcLogo";
            this.lcLogo.Size = new System.Drawing.Size(145, 255);
            this.lcLogo.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lcLogo.Text = "Logo";
            this.lcLogo.TextSize = new System.Drawing.Size(0, 0);
            this.lcLogo.TextVisible = false;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.simpleLabelItem1,
            this.simpleSeparator1,
            this.simpleLabelItem2});
            this.layoutControlGroup2.Location = new System.Drawing.Point(155, 0);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Size = new System.Drawing.Size(324, 77);
            // 
            // simpleLabelItem1
            // 
            this.simpleLabelItem1.AllowHotTrack = false;
            this.simpleLabelItem1.Location = new System.Drawing.Point(0, 0);
            this.simpleLabelItem1.Name = "simpleLabelItem1";
            this.simpleLabelItem1.Size = new System.Drawing.Size(152, 23);
            this.simpleLabelItem1.TextSize = new System.Drawing.Size(114, 13);
            // 
            // simpleSeparator1
            // 
            this.simpleSeparator1.AllowHotTrack = false;
            this.simpleSeparator1.Location = new System.Drawing.Point(0, 23);
            this.simpleSeparator1.Name = "simpleSeparator1";
            this.simpleSeparator1.Size = new System.Drawing.Size(152, 6);
            // 
            // simpleLabelItem2
            // 
            this.simpleLabelItem2.AllowHotTrack = false;
            this.simpleLabelItem2.Location = new System.Drawing.Point(0, 29);
            this.simpleLabelItem2.Name = "simpleLabelItem2";
            this.simpleLabelItem2.Size = new System.Drawing.Size(152, 23);
            this.simpleLabelItem2.TextSize = new System.Drawing.Size(114, 13);
            // 
            // AboutBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 275);
            this.Controls.Add(this.layoutControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AboutBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.Load += new System.EventHandler(this.AboutBox_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aboutGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aboutExtraLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblProductname)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCopyright)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblVersion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcCompany)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleSeparator1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem2)).EndInit();
            this.ResumeLayout(false);
        }
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItem1;
        private DevExpress.XtraLayout.SimpleSeparator simpleSeparator1;
        private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItem2;
        private DevExpress.XtraLayout.LayoutControlGroup aboutGroup;
        private DevExpress.XtraEditors.HyperlinkLabelControl lblCompany;
        private DevExpress.XtraLayout.LayoutControlItem lcCompany;
        private DevExpress.XtraLayout.SimpleLabelItem lblCopyright;
        private DevExpress.XtraLayout.SimpleLabelItem lblProductname;
        private DevExpress.XtraLayout.SimpleLabelItem lblVersion;
        private DevExpress.XtraLayout.SimpleLabelItem aboutExtraLabel;
        private DevExpress.XtraLayout.LayoutControlItem lcLogo;
        public DevExpress.XtraEditors.PictureEdit pictureEdit1;
    }
}
