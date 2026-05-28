namespace MeshokBrowser
{
    partial class MeshokSetting
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
            this.MeshokUser = new DevExpress.XtraEditors.TextEdit();
            this.layoutControl = new GH.Components.LayoutControlGh();
            this.labelInfo = new DevExpress.XtraEditors.LabelControl();
            this.MeshokCountry = new DevExpress.XtraEditors.SpinEdit();
            this.MeshokCity = new DevExpress.XtraEditors.SpinEdit();
            this.MeshokCommission = new DevExpress.XtraEditors.SpinEdit();
            this.MeshokCurs = new DevExpress.XtraEditors.SpinEdit();
            this.MeshokPass = new DevExpress.XtraEditors.TextEdit();
            this.MeshokWorld = new DevExpress.XtraEditors.SpinEdit();
            this.MeshokAddInfo = new DevExpress.XtraEditors.MemoEdit();
            this.lgRoot = new DevExpress.XtraLayout.LayoutControlGroup();
            this.EditGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lcLoginMesok = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcPaswordMeshok = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem9 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.clCurs = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcCommission = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcDeliveryCity = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem10 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lcDeliveryCountry = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcDeliveryWorld = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcDescription = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcTitle = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem8 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeshokUser.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MeshokCountry.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeshokCity.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeshokCommission.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeshokCurs.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeshokPass.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeshokWorld.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeshokAddInfo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lgRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EditGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcLoginMesok)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcPaswordMeshok)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clCurs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcCommission)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcDeliveryCity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcDeliveryCountry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcDeliveryWorld)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataSource
            // 
            this.dataSource.DataSource = typeof(MeshokBrowser.Meshok);
            this.dataSource.PageSupport.EditGroup = this.EditGroup;
            this.dataSource.State = GH.Components.DataState.Browsing;
            this.dataSource.VitualDataSet = true;
            this.dataSource.OnOpen += new GH.Components.OpenHandler(this.dataSource_OnOpen);
            // 
            // MeshokUser
            // 
            this.MeshokUser.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "User", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.MeshokUser.EditValue = "bridgenote@gmail.com";
            this.MeshokUser.Location = new System.Drawing.Point(94, 26);
            this.MeshokUser.Name = "MeshokUser";
            this.MeshokUser.Size = new System.Drawing.Size(196, 20);
            this.MeshokUser.StyleController = this.layoutControl;
            this.MeshokUser.TabIndex = 0;
            // 
            // layoutControl
            // 
            this.layoutControl.AllowCustomization = false;
            this.layoutControl.Controls.Add(this.labelInfo);
            this.layoutControl.Controls.Add(this.MeshokCountry);
            this.layoutControl.Controls.Add(this.MeshokCity);
            this.layoutControl.Controls.Add(this.MeshokCommission);
            this.layoutControl.Controls.Add(this.MeshokCurs);
            this.layoutControl.Controls.Add(this.MeshokPass);
            this.layoutControl.Controls.Add(this.MeshokUser);
            this.layoutControl.Controls.Add(this.MeshokWorld);
            this.layoutControl.Controls.Add(this.MeshokAddInfo);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 47);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(916, 103, 650, 614);
            this.layoutControl.OptionsFocus.AllowFocusGroups = false;
            this.layoutControl.OptionsFocus.AllowFocusReadonlyEditors = false;
            this.layoutControl.OptionsFocus.AllowFocusTabbedGroups = false;
            this.layoutControl.Owner = this;
            this.layoutControl.Root = this.lgRoot;
            this.layoutControl.SaveLayout = false;
            this.layoutControl.Size = new System.Drawing.Size(743, 449);
            this.layoutControl.TabIndex = 16;
            this.layoutControl.Text = "layoutControlGh1";
            // 
            // labelInfo
            // 
            this.labelInfo.Location = new System.Drawing.Point(8, 146);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(179, 13);
            this.labelInfo.StyleController = this.layoutControl;
            this.labelInfo.TabIndex = 8;
            this.labelInfo.Text = "Доп. информация (0 из 200 знаков)";
            // 
            // MeshokCountry
            // 
            this.MeshokCountry.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "PriceCountry", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.MeshokCountry.EditValue = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.MeshokCountry.Location = new System.Drawing.Point(240, 116);
            this.MeshokCountry.Name = "MeshokCountry";
            this.MeshokCountry.Properties.EditFormat.FormatString = "n2";
            this.MeshokCountry.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.MeshokCountry.Properties.MaxValue = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.MeshokCountry.Size = new System.Drawing.Size(50, 20);
            this.MeshokCountry.StyleController = this.layoutControl;
            this.MeshokCountry.TabIndex = 5;
            // 
            // MeshokCity
            // 
            this.MeshokCity.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "PriceCity", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.MeshokCity.EditValue = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.MeshokCity.Location = new System.Drawing.Point(94, 116);
            this.MeshokCity.Name = "MeshokCity";
            this.MeshokCity.Properties.EditFormat.FormatString = "n2";
            this.MeshokCity.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.MeshokCity.Properties.MaxValue = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.MeshokCity.Size = new System.Drawing.Size(50, 20);
            this.MeshokCity.StyleController = this.layoutControl;
            this.MeshokCity.TabIndex = 4;
            // 
            // MeshokCommission
            // 
            this.MeshokCommission.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "Comission", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.MeshokCommission.EditValue = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.MeshokCommission.Location = new System.Drawing.Point(240, 86);
            this.MeshokCommission.Name = "MeshokCommission";
            this.MeshokCommission.Properties.EditFormat.FormatString = "n2";
            this.MeshokCommission.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.MeshokCommission.Size = new System.Drawing.Size(50, 20);
            this.MeshokCommission.StyleController = this.layoutControl;
            this.MeshokCommission.TabIndex = 3;
            // 
            // MeshokCurs
            // 
            this.MeshokCurs.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "Curs", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.MeshokCurs.EditValue = new decimal(new int[] {
            70,
            0,
            0,
            0});
            this.MeshokCurs.Location = new System.Drawing.Point(94, 86);
            this.MeshokCurs.Name = "MeshokCurs";
            this.MeshokCurs.Properties.EditFormat.FormatString = "n2";
            this.MeshokCurs.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.MeshokCurs.Size = new System.Drawing.Size(50, 20);
            this.MeshokCurs.StyleController = this.layoutControl;
            this.MeshokCurs.TabIndex = 2;
            // 
            // MeshokPass
            // 
            this.MeshokPass.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "PassWrd", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.MeshokPass.EditValue = "CtlmvjqEhj;fq2018";
            this.MeshokPass.Location = new System.Drawing.Point(94, 56);
            this.MeshokPass.Name = "MeshokPass";
            this.MeshokPass.Size = new System.Drawing.Size(196, 20);
            this.MeshokPass.StyleController = this.layoutControl;
            this.MeshokPass.TabIndex = 1;
            // 
            // MeshokWorld
            // 
            this.MeshokWorld.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "PriceWorld", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.MeshokWorld.EditValue = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.MeshokWorld.Location = new System.Drawing.Point(386, 116);
            this.MeshokWorld.Name = "MeshokWorld";
            this.MeshokWorld.Size = new System.Drawing.Size(50, 20);
            this.MeshokWorld.StyleController = this.layoutControl;
            this.MeshokWorld.TabIndex = 6;
            // 
            // MeshokAddInfo
            // 
            this.MeshokAddInfo.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.dataSource, "AddInfo", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.MeshokAddInfo.EditValue = "Стоимость отпр. Почтой России зависит от веса и оценочной стоимости ПО и составля" +
    "ет от 250 до 450 р., на стандартный CD или LP. Остальное обсуждается отдельно!";
            this.MeshokAddInfo.Location = new System.Drawing.Point(8, 169);
            this.MeshokAddInfo.Name = "MeshokAddInfo";
            this.MeshokAddInfo.Properties.AcceptsReturn = false;
            this.MeshokAddInfo.Properties.MaxLength = 200;
            this.MeshokAddInfo.Size = new System.Drawing.Size(428, 110);
            this.MeshokAddInfo.StyleController = this.layoutControl;
            this.MeshokAddInfo.TabIndex = 7;
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
            this.lgRoot.Size = new System.Drawing.Size(743, 449);
            this.lgRoot.TextVisible = false;
            // 
            // EditGroup
            // 
            this.EditGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem3,
            this.lcLoginMesok,
            this.lcPaswordMeshok,
            this.emptySpaceItem9,
            this.clCurs,
            this.lcCommission,
            this.lcDeliveryCity,
            this.emptySpaceItem10,
            this.lcDeliveryCountry,
            this.lcDeliveryWorld,
            this.lcDescription,
            this.lcTitle,
            this.emptySpaceItem8});
            this.EditGroup.Location = new System.Drawing.Point(0, 0);
            this.EditGroup.Name = "EditGroup";
            this.EditGroup.OptionsItemText.TextToControlDistance = 5;
            this.EditGroup.Size = new System.Drawing.Size(743, 287);
            this.EditGroup.Text = "Meshok.ru";
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.Location = new System.Drawing.Point(292, 0);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(445, 60);
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lcLoginMesok
            // 
            this.lcLoginMesok.Control = this.MeshokUser;
            this.lcLoginMesok.Location = new System.Drawing.Point(0, 0);
            this.lcLoginMesok.Name = "lcLoginMesok";
            this.lcLoginMesok.Size = new System.Drawing.Size(292, 30);
            this.lcLoginMesok.Text = "Login";
            this.lcLoginMesok.TextSize = new System.Drawing.Size(81, 13);
            // 
            // lcPaswordMeshok
            // 
            this.lcPaswordMeshok.Control = this.MeshokPass;
            this.lcPaswordMeshok.Location = new System.Drawing.Point(0, 30);
            this.lcPaswordMeshok.Name = "lcPaswordMeshok";
            this.lcPaswordMeshok.Size = new System.Drawing.Size(292, 30);
            this.lcPaswordMeshok.Text = "Pasword";
            this.lcPaswordMeshok.TextSize = new System.Drawing.Size(81, 13);
            // 
            // emptySpaceItem9
            // 
            this.emptySpaceItem9.AllowHotTrack = false;
            this.emptySpaceItem9.Location = new System.Drawing.Point(292, 60);
            this.emptySpaceItem9.Name = "emptySpaceItem9";
            this.emptySpaceItem9.Size = new System.Drawing.Size(445, 30);
            this.emptySpaceItem9.TextSize = new System.Drawing.Size(0, 0);
            // 
            // clCurs
            // 
            this.clCurs.Control = this.MeshokCurs;
            this.clCurs.Location = new System.Drawing.Point(0, 60);
            this.clCurs.Name = "clCurs";
            this.clCurs.Size = new System.Drawing.Size(146, 30);
            this.clCurs.Text = "Curs";
            this.clCurs.TextSize = new System.Drawing.Size(81, 13);
            // 
            // lcCommission
            // 
            this.lcCommission.Control = this.MeshokCommission;
            this.lcCommission.Location = new System.Drawing.Point(146, 60);
            this.lcCommission.Name = "lcCommission";
            this.lcCommission.Size = new System.Drawing.Size(146, 30);
            this.lcCommission.Text = "Commission";
            this.lcCommission.TextSize = new System.Drawing.Size(81, 13);
            // 
            // lcDeliveryCity
            // 
            this.lcDeliveryCity.Control = this.MeshokCity;
            this.lcDeliveryCity.Location = new System.Drawing.Point(0, 90);
            this.lcDeliveryCity.Name = "lcDeliveryCity";
            this.lcDeliveryCity.Size = new System.Drawing.Size(146, 30);
            this.lcDeliveryCity.Text = "Delivery City";
            this.lcDeliveryCity.TextSize = new System.Drawing.Size(81, 13);
            // 
            // emptySpaceItem10
            // 
            this.emptySpaceItem10.AllowHotTrack = false;
            this.emptySpaceItem10.Location = new System.Drawing.Point(438, 90);
            this.emptySpaceItem10.Name = "emptySpaceItem10";
            this.emptySpaceItem10.Size = new System.Drawing.Size(299, 30);
            this.emptySpaceItem10.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lcDeliveryCountry
            // 
            this.lcDeliveryCountry.Control = this.MeshokCountry;
            this.lcDeliveryCountry.Location = new System.Drawing.Point(146, 90);
            this.lcDeliveryCountry.Name = "lcDeliveryCountry";
            this.lcDeliveryCountry.Size = new System.Drawing.Size(146, 30);
            this.lcDeliveryCountry.Text = "Delivery Country";
            this.lcDeliveryCountry.TextSize = new System.Drawing.Size(81, 13);
            // 
            // lcDeliveryWorld
            // 
            this.lcDeliveryWorld.Control = this.MeshokWorld;
            this.lcDeliveryWorld.Location = new System.Drawing.Point(292, 90);
            this.lcDeliveryWorld.Name = "lcDeliveryWorld";
            this.lcDeliveryWorld.Size = new System.Drawing.Size(146, 30);
            this.lcDeliveryWorld.Text = "Delivery World";
            this.lcDeliveryWorld.TextSize = new System.Drawing.Size(81, 13);
            // 
            // lcDescription
            // 
            this.lcDescription.Control = this.MeshokAddInfo;
            this.lcDescription.Location = new System.Drawing.Point(0, 143);
            this.lcDescription.MaxSize = new System.Drawing.Size(0, 120);
            this.lcDescription.MinSize = new System.Drawing.Size(20, 119);
            this.lcDescription.Name = "lcDescription";
            this.lcDescription.Size = new System.Drawing.Size(438, 120);
            this.lcDescription.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lcDescription.Text = "Description";
            this.lcDescription.TextSize = new System.Drawing.Size(0, 0);
            this.lcDescription.TextVisible = false;
            // 
            // lcTitle
            // 
            this.lcTitle.Control = this.labelInfo;
            this.lcTitle.Location = new System.Drawing.Point(0, 120);
            this.lcTitle.Name = "lcTitle";
            this.lcTitle.Size = new System.Drawing.Size(737, 23);
            this.lcTitle.TextSize = new System.Drawing.Size(0, 0);
            this.lcTitle.TextVisible = false;
            // 
            // emptySpaceItem8
            // 
            this.emptySpaceItem8.AllowHotTrack = false;
            this.emptySpaceItem8.Location = new System.Drawing.Point(438, 143);
            this.emptySpaceItem8.Name = "emptySpaceItem8";
            this.emptySpaceItem8.Size = new System.Drawing.Size(299, 120);
            this.emptySpaceItem8.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 287);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(743, 162);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(50, 25);
            // 
            // MeshokSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Caption = "Meshok.ru";
            this.Controls.Add(this.layoutControl);
            this.Name = "MeshokSetting";
            this.SaveLayout = false;
            this.Size = new System.Drawing.Size(743, 523);
            this.Controls.SetChildIndex(this.layoutControl, 0);
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeshokUser.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MeshokCountry.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeshokCity.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeshokCommission.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeshokCurs.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeshokPass.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeshokWorld.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeshokAddInfo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lgRoot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EditGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcLoginMesok)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcPaswordMeshok)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clCurs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcCommission)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcDeliveryCity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcDeliveryCountry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcDeliveryWorld)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private GH.Components.LayoutControlGh layoutControl;
        private DevExpress.XtraLayout.LayoutControlGroup lgRoot;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.LabelControl labelInfo;
        private DevExpress.XtraEditors.SpinEdit MeshokCountry;
        private DevExpress.XtraEditors.SpinEdit MeshokCity;
        private DevExpress.XtraEditors.SpinEdit MeshokCommission;
        private DevExpress.XtraEditors.SpinEdit MeshokCurs;
        private DevExpress.XtraEditors.TextEdit MeshokPass;
        private DevExpress.XtraEditors.TextEdit MeshokUser;
        private DevExpress.XtraEditors.SpinEdit MeshokWorld;
        private DevExpress.XtraEditors.MemoEdit MeshokAddInfo;
        private DevExpress.XtraLayout.LayoutControlGroup EditGroup;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
        private DevExpress.XtraLayout.LayoutControlItem lcLoginMesok;
        private DevExpress.XtraLayout.LayoutControlItem lcPaswordMeshok;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem9;
        private DevExpress.XtraLayout.LayoutControlItem clCurs;
        private DevExpress.XtraLayout.LayoutControlItem lcCommission;
        private DevExpress.XtraLayout.LayoutControlItem lcDeliveryCity;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem10;
        private DevExpress.XtraLayout.LayoutControlItem lcDeliveryCountry;
        private DevExpress.XtraLayout.LayoutControlItem lcDeliveryWorld;
        private DevExpress.XtraLayout.LayoutControlItem lcDescription;
        private DevExpress.XtraLayout.LayoutControlItem lcTitle;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem8;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
    }
}
