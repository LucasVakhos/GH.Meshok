namespace NewsMaker
{
    partial class CfgFrameProgram
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CfgFrameProgram));
            this.ItemForUseCollapce = new DevExpress.XtraLayout.LayoutControlItem();
            this.chkCollapce = new DevExpress.XtraEditors.CheckEdit();
            this.ItemForRunDay = new DevExpress.XtraLayout.LayoutControlItem();
            this.cboRunDay = new DevExpress.XtraEditors.ComboBoxEdit();
            this.ItemForRunTime = new DevExpress.XtraLayout.LayoutControlItem();
            this.editRunTime = new DevExpress.XtraEditors.TimeSpanEdit();
            this.chkAutorun = new DevExpress.XtraEditors.CheckEdit();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rootGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EditGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUseCollapce)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCollapce.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForRunDay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboRunDay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForRunTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editRunTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutorun.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.editRunTime);
            this.layoutControl.Controls.Add(this.chkAutorun);
            this.layoutControl.Controls.Add(this.chkCollapce);
            this.layoutControl.Controls.Add(this.cboRunDay);
            this.layoutControl.OptionsCustomizationForm.DefaultPage = DevExpress.XtraLayout.CustomizationPage.LayoutTreeView;
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(587, 132, 569, 336);
            this.layoutControl.OptionsFocus.AllowFocusGroups = false;
            this.layoutControl.OptionsFocus.AllowFocusReadonlyEditors = false;
            this.layoutControl.OptionsFocus.AllowFocusTabbedGroups = false;
            this.layoutControl.OptionsView.AlwaysScrollActiveControlIntoView = false;
            this.layoutControl.Size = new System.Drawing.Size(565, 94);
            // 
            // rootGroup
            // 
            this.rootGroup.Size = new System.Drawing.Size(565, 94);
            // 
            // EditGroup
            // 
            this.EditGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForUseCollapce,
            this.ItemForRunDay,
            this.layoutControlItem1,
            this.ItemForRunTime});
            this.EditGroup.Size = new System.Drawing.Size(565, 94);
            // 
            // dataSource
            // 
            this.dataSource.PageSupport.EditGroup = this.EditGroup;
            // 
            // ItemForUseCollapce
            // 
            this.ItemForUseCollapce.Control = this.chkCollapce;
            this.ItemForUseCollapce.Location = new System.Drawing.Point(0, 23);
            this.ItemForUseCollapce.Name = "ItemForUseCollapce";
            this.ItemForUseCollapce.OptionsToolTip.ToolTip = "Запускать свёрнуто";
            this.ItemForUseCollapce.Size = new System.Drawing.Size(541, 23);
            this.ItemForUseCollapce.TextSize = new System.Drawing.Size(0, 0);
            this.ItemForUseCollapce.TextVisible = false;
            // 
            // chkCollapce
            // 
            this.chkCollapce.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "RunCollapced", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkCollapce.Location = new System.Drawing.Point(14, 37);
            this.chkCollapce.Name = "chkCollapce";
            this.chkCollapce.Properties.Caption = "Сворачивать в трей, если не исполняется и сразу после загрузки, а так же при мини" +
    "мизации";
            this.chkCollapce.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.chkCollapce.Size = new System.Drawing.Size(537, 19);
            this.chkCollapce.StyleController = this.layoutControl;
            this.chkCollapce.TabIndex = 4;
            // 
            // ItemForRunDay
            // 
            this.ItemForRunDay.Control = this.cboRunDay;
            this.ItemForRunDay.Location = new System.Drawing.Point(0, 46);
            this.ItemForRunDay.MaxSize = new System.Drawing.Size(229, 24);
            this.ItemForRunDay.MinSize = new System.Drawing.Size(229, 24);
            this.ItemForRunDay.Name = "ItemForRunDay";
            this.ItemForRunDay.OptionsToolTip.ToolTip = "День запуска";
            this.ItemForRunDay.Size = new System.Drawing.Size(229, 24);
            this.ItemForRunDay.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.ItemForRunDay.Text = "Run Day";
            this.ItemForRunDay.TextSize = new System.Drawing.Size(44, 13);
            // 
            // cboRunDay
            // 
            this.cboRunDay.DataBindings.Add(new System.Windows.Forms.Binding("SelectedIndex", this.dataSource, "RunDay", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cboRunDay.EditValue = "Воскресенье";
            this.cboRunDay.Location = new System.Drawing.Point(62, 60);
            this.cboRunDay.Name = "cboRunDay";
            this.cboRunDay.Properties.Appearance.Options.UseTextOptions = true;
            this.cboRunDay.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.cboRunDay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboRunDay.Properties.DropDownRows = 8;
            this.cboRunDay.Properties.Items.AddRange(new object[] {
            "Воскресенье",
            "Понедельник",
            "Вторник",
            "Среда",
            "Четверг",
            "Пятница",
            "Суббота",
            "Рассылать в ручном режиме"});
            this.cboRunDay.Size = new System.Drawing.Size(177, 20);
            this.cboRunDay.StyleController = this.layoutControl;
            this.cboRunDay.TabIndex = 5;
            // 
            // ItemForRunTime
            // 
            this.ItemForRunTime.Control = this.editRunTime;
            this.ItemForRunTime.Location = new System.Drawing.Point(229, 46);
            this.ItemForRunTime.MaxSize = new System.Drawing.Size(140, 24);
            this.ItemForRunTime.MinSize = new System.Drawing.Size(132, 24);
            this.ItemForRunTime.Name = "ItemForRunTime";
            this.ItemForRunTime.OptionsToolTip.ToolTip = "Время запуска";
            this.ItemForRunTime.Size = new System.Drawing.Size(312, 24);
            this.ItemForRunTime.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.ItemForRunTime.Text = "Run Time";
            this.ItemForRunTime.TextSize = new System.Drawing.Size(44, 13);
            // 
            // editRunTime
            // 
            this.editRunTime.AllowDrop = true;
            this.editRunTime.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "RunTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged, null, "T"));
            this.editRunTime.EditValue = System.TimeSpan.Parse("737047.18:00:00");
            this.editRunTime.Location = new System.Drawing.Point(291, 60);
            this.editRunTime.Name = "editRunTime";
            this.editRunTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.editRunTime.Properties.MaxDays = 1;
            this.editRunTime.Size = new System.Drawing.Size(88, 20);
            this.editRunTime.StyleController = this.layoutControl;
            this.editRunTime.TabIndex = 6;
            // 
            // chkAutorun
            // 
            this.chkAutorun.Location = new System.Drawing.Point(14, 14);
            this.chkAutorun.Name = "chkAutorun";
            this.chkAutorun.Properties.Caption = "Автозапуск программы";
            this.chkAutorun.Size = new System.Drawing.Size(537, 19);
            this.chkAutorun.StyleController = this.layoutControl;
            this.chkAutorun.TabIndex = 3;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.chkAutorun;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(541, 23);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(531, 69);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(10, 24);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // CfgFrameProgram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Caption = "Настройки запуска";
            this.Image = ((System.Drawing.Image)(resources.GetObject("$this.Image")));
            this.LargeImage = ((System.Drawing.Image)(resources.GetObject("$this.LargeImage")));
            this.Name = "CfgFrameProgram";
            this.Size = new System.Drawing.Size(565, 211);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rootGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EditGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUseCollapce)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCollapce.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForRunDay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboRunDay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForRunTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editRunTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutorun.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            this.ResumeLayout(false);
        }
        private DevExpress.XtraEditors.CheckEdit chkCollapce;
        private DevExpress.XtraEditors.ComboBoxEdit cboRunDay;
        private DevExpress.XtraEditors.TimeSpanEdit editRunTime;
        private DevExpress.XtraLayout.LayoutControlItem ItemForUseCollapce;
        private DevExpress.XtraLayout.LayoutControlItem ItemForRunDay;
        private DevExpress.XtraLayout.LayoutControlItem ItemForRunTime;
        private DevExpress.XtraEditors.CheckEdit chkAutorun;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
    }
}
