using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using System.ComponentModel;
namespace GH.Components
{
    partial class CfgForm
    {
        private IContainer components = null;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CfgForm));
            this.actionList = new GH.Components.ActionList();
            this.actEnter = new GH.Components.ActionGh();
            this.actCancel = new GH.Components.ActionGh();
            this.btnEnter = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControl = new GH.Components.LayoutControlGh();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.rootGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.FrameGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.YesNoGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.clCancel = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcEnter = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rootGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FrameGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YesNoGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcEnter)).BeginInit();
            this.SuspendLayout();
            // 
            // actionList
            // 
            this.actionList.Actions.Add(this.actEnter);
            this.actionList.Actions.Add(this.actCancel);
            this.actionList.Owner = this;
            // 
            // actEnter
            // 
            this.actEnter.Caption = "Войти";
            this.actEnter.Image = ((System.Drawing.Image)(resources.GetObject("actEnter.Image")));
            this.actEnter.LargeImage = ((System.Drawing.Image)(resources.GetObject("actEnter.LargeImage")));
            this.actEnter.Tag = null;
            this.actEnter.ToolTipText = "Войти в программу";
            // 
            // actCancel
            // 
            this.actCancel.Caption = "Закрыть";
            this.actCancel.Image = ((System.Drawing.Image)(resources.GetObject("actCancel.Image")));
            this.actCancel.LargeImage = ((System.Drawing.Image)(resources.GetObject("actCancel.LargeImage")));
            this.actCancel.Tag = null;
            this.actCancel.ToolTipText = "Закрыть и выйти из программы ";
            // 
            // btnEnter
            // 
            this.actionList.SetAction(this.btnEnter, this.actEnter);
            this.btnEnter.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnEnter.ImageOptions.Image")));
            this.btnEnter.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnEnter.ImageOptions.ImageToTextIndent = 5;
            this.btnEnter.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnEnter.Location = new System.Drawing.Point(10, 10);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new System.Drawing.Size(97, 22);
            this.btnEnter.StyleController = this.layoutControl;
            this.btnEnter.TabIndex = 7;
            this.btnEnter.Text = "Войти";
            // 
            // layoutControl
            // 
            this.layoutControl.AllowCustomization = false;
            this.layoutControl.AutoScroll = false;
            this.layoutControl.Controls.Add(this.btnEnter);
            this.layoutControl.Controls.Add(this.btnCancel);
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
            this.layoutControl.Size = new System.Drawing.Size(379, 43);
            this.layoutControl.TabIndex = 3;
            this.layoutControl.Text = "dataLayoutControl1";
            // 
            // btnCancel
            // 
            this.actionList.SetAction(this.btnCancel, this.actCancel);
            this.btnCancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImageOptions.Image")));
            this.btnCancel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnCancel.ImageOptions.ImageToTextIndent = 5;
            this.btnCancel.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(274, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 22);
            this.btnCancel.StyleController = this.layoutControl;
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Закрыть";
            // 
            // rootGroup
            // 
            this.rootGroup.GroupBordersVisible = false;
            this.rootGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.FrameGroup});
            this.rootGroup.Name = "Root";
            this.rootGroup.Size = new System.Drawing.Size(379, 43);
            // 
            // FrameGroup
            // 
            this.FrameGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.YesNoGroup});
            this.FrameGroup.Location = new System.Drawing.Point(0, 0);
            this.FrameGroup.Name = "FrameGroup";
            this.FrameGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.FrameGroup.Size = new System.Drawing.Size(379, 43);
            this.FrameGroup.TextVisible = false;
            // 
            // YesNoGroup
            // 
            this.YesNoGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem1,
            this.clCancel,
            this.lcEnter});
            this.YesNoGroup.Location = new System.Drawing.Point(0, 0);
            this.YesNoGroup.Name = "YesNoGroup";
            this.YesNoGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.YesNoGroup.Size = new System.Drawing.Size(373, 37);
            this.YesNoGroup.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(101, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.emptySpaceItem1.Size = new System.Drawing.Size(163, 27);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // clCancel
            // 
            this.clCancel.Control = this.btnCancel;
            this.clCancel.Location = new System.Drawing.Point(264, 0);
            this.clCancel.Name = "clCancel";
            this.clCancel.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.clCancel.Size = new System.Drawing.Size(99, 27);
            this.clCancel.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.clCancel.Text = "Cancel";
            this.clCancel.TextSize = new System.Drawing.Size(0, 0);
            this.clCancel.TextVisible = false;
            // 
            // lcEnter
            // 
            this.lcEnter.Control = this.btnEnter;
            this.lcEnter.Location = new System.Drawing.Point(0, 0);
            this.lcEnter.Name = "lcEnter";
            this.lcEnter.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcEnter.Size = new System.Drawing.Size(101, 27);
            this.lcEnter.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.lcEnter.Text = "Enter";
            this.lcEnter.TextSize = new System.Drawing.Size(0, 0);
            this.lcEnter.TextVisible = false;
            // 
            // CfgForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 349);
            this.Controls.Add(this.layoutControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CfgForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Вход в систему";
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rootGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FrameGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YesNoGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcEnter)).EndInit();
            this.ResumeLayout(false);
        }
        public Components.ActionList actionList;
        public Components.ActionGh actEnter;
        public Components.ActionGh actCancel;
        public Controls.LayoutControlGh layoutControl;
        public SimpleButton btnEnter;
        public SimpleButton btnCancel;
        public LayoutControlGroup rootGroup;
        public LayoutControlGroup FrameGroup;
        private EmptySpaceItem emptySpaceItem1;
        private LayoutControlItem clCancel;
        private LayoutControlItem lcEnter;
        public LayoutControlGroup YesNoGroup;
    }
}
