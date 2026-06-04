using DevExpress.XtraEditors;
using System.ComponentModel;
namespace GH.Components
{
    public partial class ConnectionForm : SimpleForm
    {
        protected DataSource dataSource;
    protected ActionList actionList;
    protected GH.Components.LayoutControlGh layoutControl;
    protected DevExpress.XtraLayout.LayoutControlGroup rootGroup;
    protected DevExpress.XtraLayout.LayoutControlGroup EditGroup;
    protected ActionGh actOK;
    protected ActionGh actCancel;
    protected SimpleButton btnOK;
    protected SimpleButton btnCancel;
    protected DevExpress.XtraLayout.LayoutControlItem lcOK;
    protected DevExpress.XtraLayout.LayoutControlItem lcCancel;
    private IContainer components;
    protected DevExpress.XtraLayout.EmptySpaceItem emptySpaceNearButtons;
    public ConnectionForm()
        {
            InitializeComponent();
        }
    private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataSource = new GH.Components.DataSource(this.components);
            this.actionList = new GH.Components.ActionList();
            this.actOK = new GH.Components.ActionGh();
            this.actCancel = new GH.Components.ActionGh();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControl = new GH.Components.LayoutControlGh();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.rootGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.EditGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcOK = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcCancel = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceNearButtons = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rootGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EditGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcOK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceNearButtons)).BeginInit();
            this.SuspendLayout();
            // 
            // dataSource
            // 
            this.dataSource.ActionList = this.actionList;
            this.dataSource.AllowDelete = false;
            this.dataSource.AllowInsert = false;
            this.dataSource.AllowSaveCancel = false;
            this.dataSource.AllowUdate = false;
            this.dataSource.Owner = this;
            this.dataSource.PageSupport.EditGroup = this.EditGroup;
            this.dataSource.State = GH.Components.DataState.Browsing;
            this.dataSource.IsLocalDataSet = true;
            // 
            // actionList
            // 
            this.actionList.Actions.Add(this.actOK);
            this.actionList.Actions.Add(this.actCancel);
            this.actionList.Owner = this;
            // 
            // actOK
            // 
            this.actOK.Caption = "Продолжить";
            this.actOK.ToolTipText = "Продолжить выполнение";
            this.actOK.Execute += new System.EventHandler(this.actOK_Execute);
            // 
            // actCancel
            // 
            this.actCancel.Caption = "Завершить";
            this.actCancel.ToolTipText = "Завершить выполнение";
            this.actCancel.Execute += new System.EventHandler(this.actCancel_Execute);
            // 
            // btnOK
            // 
            this.actionList.SetAction(this.btnOK, this.actOK);
            this.btnOK.Location = new System.Drawing.Point(15, 29);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(180, 22);
            this.btnOK.StyleController = this.layoutControl;
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "Продолжить";
            // 
            // layoutControl
            // 
            this.layoutControl.AllowCustomization = false;
            this.layoutControl.AutoScroll = false;
            this.layoutControl.Controls.Add(this.btnOK);
            this.layoutControl.Controls.Add(this.btnCancel);
            this.layoutControl.DataSource = this.dataSource;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.OptionsCustomizationForm.DefaultPage = DevExpress.XtraLayout.CustomizationPage.LayoutTreeView;
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(725, 211, 650, 400);
            this.layoutControl.OptionsFocus.AllowFocusGroups = false;
            this.layoutControl.OptionsFocus.AllowFocusReadonlyEditors = false;
            this.layoutControl.OptionsFocus.AllowFocusTabbedGroups = false;
            this.layoutControl.Owner = this;
            this.layoutControl.Root = this.rootGroup;
            this.layoutControl.Size = new System.Drawing.Size(310, 56);
            this.layoutControl.TabIndex = 0;
            this.layoutControl.Text = "layoutControlGh1";
            // 
            // btnCancel
            // 
            this.actionList.SetAction(this.btnCancel, this.actCancel);
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(205, 29);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 22);
            this.btnCancel.StyleController = this.layoutControl;
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Завершить";
            // 
            // rootGroup
            // 
            this.rootGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.rootGroup.GroupBordersVisible = false;
            this.rootGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.EditGroup,
            this.lcOK,
            this.lcCancel,
            this.emptySpaceNearButtons});
            this.rootGroup.Name = "Root";
            this.rootGroup.OptionsItemText.TextToControlDistance = 5;
            this.rootGroup.Size = new System.Drawing.Size(310, 56);
            this.rootGroup.TextVisible = false;
            // 
            // EditGroup
            // 
            this.EditGroup.Location = new System.Drawing.Point(0, 0);
            this.EditGroup.Name = "EditGroup";
            this.EditGroup.OptionsItemText.TextToControlDistance = 5;
            this.EditGroup.Size = new System.Drawing.Size(310, 24);
            // 
            // lcOK
            // 
            this.lcOK.Control = this.btnOK;
            this.lcOK.Location = new System.Drawing.Point(10, 24);
            this.lcOK.MaxSize = new System.Drawing.Size(190, 32);
            this.lcOK.MinSize = new System.Drawing.Size(190, 32);
            this.lcOK.Name = "lcOK";
            this.lcOK.Size = new System.Drawing.Size(190, 32);
            this.lcOK.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lcOK.Text = "OK";
            this.lcOK.TextSize = new System.Drawing.Size(0, 0);
            this.lcOK.TextVisible = false;
            // 
            // lcCancel
            // 
            this.lcCancel.Control = this.btnCancel;
            this.lcCancel.Location = new System.Drawing.Point(200, 24);
            this.lcCancel.MaxSize = new System.Drawing.Size(110, 32);
            this.lcCancel.MinSize = new System.Drawing.Size(110, 32);
            this.lcCancel.Name = "lcCancel";
            this.lcCancel.Size = new System.Drawing.Size(110, 32);
            this.lcCancel.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lcCancel.Text = "Cancel";
            this.lcCancel.TextSize = new System.Drawing.Size(0, 0);
            this.lcCancel.TextVisible = false;
            // 
            // emptySpaceNearButtons
            // 
            this.emptySpaceNearButtons.AllowHotTrack = false;
            this.emptySpaceNearButtons.Location = new System.Drawing.Point(0, 24);
            this.emptySpaceNearButtons.Name = "emptySpaceItem1";
            this.emptySpaceNearButtons.Size = new System.Drawing.Size(10, 32);
            this.emptySpaceNearButtons.TextSize = new System.Drawing.Size(0, 0);
            // 
            // ConnectionForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(495, 175);
            this.Controls.Add(this.layoutControl);
            this.Name = "ConnectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Подключение к БД";
            this.Load += new System.EventHandler(this.Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rootGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EditGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcOK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceNearButtons)).EndInit();
            this.ResumeLayout(false);
        }
    private void Form_Load(object sender, EventArgs e)
        {
            if (Created && !DesignMode)
            {
                layoutControl.AutoSize = true;
                AutoSize = true;
                layoutControl.Dock = DockStyle.Fill;
                dataSource.CloseOpen();
            }
        }
    private void actOK_Execute(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    private void actCancel_Execute(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
