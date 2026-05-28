using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Ribbon;
using GH.AppContext;
using GH.Controls;
namespace MeshokBrowser
{
    public class BaseFrame : SavedFrame
    {
        internal MainForm OwnerForm { get { return RunContext.Instance.MainForm as MainForm; } }
        protected RibbonControl MainRibbon { get { return OwnerForm.Ribbon; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        protected virtual RibbonControl ChildRibbon
        {
            get
            {
                return FindRibbon(Controls);
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        protected virtual RibbonStatusBar ChildRibbonStatusBar
        {
            get
            {
                if (ChildRibbon != null)
                    return ChildRibbon.StatusBar;
                return null;
            }
        }
        RibbonControl FindRibbon(ControlCollection controls)
        {
            RibbonControl res = controls.OfType<RibbonControl>().FirstOrDefault() as RibbonControl;
            if (res != null)
                return res;
            foreach (Control control in controls)
            {
                if (control.HasChildren)
                {
                    res = FindRibbon(control.Controls);
                    if (res != null)
                        return res;
                }
            }
            return null;
        }
        public BaseFrame()
        {
        }
        protected void InvokeIfRequired(MethodInvoker action)
        {
            if (Disposing)
                return;
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }
        internal virtual void ShowRibbon()
        {
            if (OwnerForm == null)
                return;
            if (ChildRibbon != null)
            {
                OwnerForm.Ribbon.MergeRibbon(ChildRibbon);
                if (ChildRibbonStatusBar != null)
                    OwnerForm.RibbonStatusBar.MergeStatusBar(ChildRibbonStatusBar);
                OwnerForm.Ribbon.SelectedPage = ChildRibbon.Pages[0];
                RibbonPage page = OwnerForm.Ribbon.Pages.GetPageByName("rpSettigs");
                if (page != null)
                {
                    OwnerForm.Ribbon.MergedPages.Remove(page);
                    OwnerForm.Ribbon.MergedPages.Insert(OwnerForm.Ribbon.MergedPages.Count, page);
                }
            }
        }
        internal virtual void HideRibbon()
        {
            if (OwnerForm == null)
                return;
            if (ChildRibbon != null)
            {
                if (OwnerForm.Ribbon.MergedRibbon == ChildRibbon)
                {
                    RibbonPage page = OwnerForm.Ribbon.MergedPages.GetPageByName("rpSettigs");
                    if (page != null)
                        OwnerForm.Ribbon.Pages.Add(page);
                    OwnerForm.Ribbon.UnMergeRibbon();
                    OwnerForm.RibbonStatusBar.UnMergeStatusBar();
                }
            }
        }
        void CapitalizeChildRibbonPages()
        {
            if (ChildRibbon == null)
                return;
            foreach (RibbonPage page in ChildRibbon.Pages)
                page.Text = page.Text.ToUpper();
            foreach (RibbonPageCategory category in ChildRibbon.PageCategories)
            {
                foreach (RibbonPage page in category.Pages)
                    page.Text = page.Text.ToUpper();
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);            
            CapitalizeChildRibbonPages();
        }
        protected override void OnEnter(EventArgs e)
        {
            ShowRibbon();
            base.OnEnter(e);
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            HideRibbon();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                HideRibbon();
            }
            base.Dispose(disposing);
        }
        public virtual void CLose()
        {
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BaseFrame
            // 
            this.Name = "BaseFrame";
            this.Size = new System.Drawing.Size(174, 150);
            this.ResumeLayout(false);
        }
    }
}
