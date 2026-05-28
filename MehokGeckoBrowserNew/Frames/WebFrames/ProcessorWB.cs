using GH.Components;
using MeshokBrowser.Workers;
namespace MeshokBrowser
{
    public class ProcessorWB : InnerWB
    {
        internal DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl;
        internal DevExpress.XtraBars.BarButtonItem btnCancel;
        internal DevExpress.XtraBars.Ribbon.RibbonPage cancelPage;
        internal DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraEditors.Repository.RepositoryItemProgressBar progressBar;
        private DevExpress.XtraBars.BarEditItem barProgress;
        private DevExpress.XtraBars.BarStaticItem lblInfo;
        internal DevExpress.XtraBars.Ribbon.RibbonPageGroup cancelGroup;
        public override string Caption
        {
            get => this.Text;
            set
            {
                this.Text = value;
                if (cancelPage != null)
                    cancelPage.Text = value.ToUpper();
            }
        }
        public ProcessorWB() : base()
        {
            //this.Controls.Remove(this.layoutControl);
            InitializeComponent();
            //this.Controls.Add(this.layoutControl);
            //cancelPage.Text = Text.ToUpper();
            //layoutControl.Enabled = false;
        }
        private void InitializeComponent()
        {
            this.ribbonControl = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.btnCancel = new DevExpress.XtraBars.BarButtonItem();
            this.barProgress = new DevExpress.XtraBars.BarEditItem();
            this.progressBar = new DevExpress.XtraEditors.Repository.RepositoryItemProgressBar();
            this.lblInfo = new DevExpress.XtraBars.BarStaticItem();
            this.cancelPage = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.cancelGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl.ExpandCollapseItem,
            this.btnCancel,
            this.barProgress,
            this.lblInfo});
            this.ribbonControl.Location = new System.Drawing.Point(5, 5);
            this.ribbonControl.MaxItemId = 22;
            this.ribbonControl.Name = "ribbonControl";
            this.ribbonControl.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.cancelPage});
            this.ribbonControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.progressBar});
            this.ribbonControl.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2013;
            this.ribbonControl.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl.Size = new System.Drawing.Size(590, 116);
            this.ribbonControl.StatusBar = this.ribbonStatusBar;
            this.ribbonControl.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // btnCancel
            // 
            this.btnCancel.Caption = "Остановить";
            this.btnCancel.Id = 18;
            this.btnCancel.ImageOptions.ImageUri.Uri = "Cancel";
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCancel_ItemClick);
            // 
            // barProgress
            // 
            this.barProgress.Caption = "Progress";
            this.barProgress.Edit = this.progressBar;
            this.barProgress.EditValue = "0";
            this.barProgress.EditWidth = 150;
            this.barProgress.Id = 12;
            this.barProgress.Name = "barProgress";
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.ShowTitle = true;
            // 
            // lblInfo
            // 
            this.lblInfo.Caption = "Информация всякая";
            this.lblInfo.Id = 21;
            this.lblInfo.ImageOptions.ImageUri.Uri = "Refresh;Size16x16";
            this.lblInfo.Name = "lblInfo";
            // 
            // cancelPage
            // 
            this.cancelPage.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.cancelGroup});
            this.cancelPage.MergeOrder = 0;
            this.cancelPage.Name = "cancelPage";
            this.cancelPage.Text = "<Введите заголовок>";
            // 
            // cancelGroup
            // 
            this.cancelGroup.AllowTextClipping = false;
            this.cancelGroup.ItemLinks.Add(this.btnCancel);
            this.cancelGroup.Name = "cancelGroup";
            this.cancelGroup.ShowCaptionButton = false;
            this.cancelGroup.Text = "Задачи";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.ItemLinks.Add(this.barProgress);
            this.ribbonStatusBar.ItemLinks.Add(this.lblInfo);
            this.ribbonStatusBar.Location = new System.Drawing.Point(5, 349);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbonControl;
            this.ribbonStatusBar.Size = new System.Drawing.Size(590, 27);
            // 
            // ProcessorWB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbonControl);
            this.Name = "ProcessorWB";
            this.Controls.SetChildIndex(this.ribbonControl, 0);
            this.Controls.SetChildIndex(this.ribbonStatusBar, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        internal void ShowMessage(string mess)
        {
            (webBrowser as GhBrowser).ShowMessage(mess);
        }
        public void Stop()
        {
            ProcessRunHelper.Executing = false;
            SetStatus(Caption + "=> процесс остановлен");
        }
        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Stop();
        }
        public void SetProgressMax(int count)
        {
            InvokeIfRequired(() =>
            {
                barProgress.EditValue = 0;
                progressBar.Maximum = count;
            });
        }
        public void SetProgress(int position)
        {
            InvokeIfRequired(() =>
            {
                progressBar.BeginUpdate();
                barProgress.EditValue = position;
                progressBar.EndUpdate();
                //if (pbMain.Maximum > position + 1)
                //    pbMain.Pos = position + 1;
                //pbMain.Step = position;
            });
        }
        public void SetStatus(string message)
        {
            InvokeIfRequired(() => { lblInfo.Caption = message; });
        }
    }
}
