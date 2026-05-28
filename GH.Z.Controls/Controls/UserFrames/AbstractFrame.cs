using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Reflection;
using System.Drawing.Design;
using System.ComponentModel.Design;
using DevExpress.XtraLayout;
using GH.Utils;
using GH.Components;
using GH.AppContext;
using GH.Interfaces;
using GH.Components.Attributes;

namespace GH.Controls
{
    [ToolboxItem(false)]
    //[Designer(nameof(GHControlDesignerAttribute) + ", System.Design", typeof (IRootDesigner))]
    public class AbstractFrame : XtraUserControl
    {
        #region поля и свойства
        [Browsable(false)]
        public Form MainForm => RunContext.Instance.MainForm;
        [Browsable(false)]
        public IRibbonForm IMmainForm => RunContext.Instance.MainForm as IRibbonForm;

        private Control _owner;

        [GHProperty, Browsable(true)]
        public Control Owner
        {
            get => _owner;
            set
            {
                if (_owner == value)
                    return;

                if (_owner != null)
                    throw new Exception("switching ActionList to another container is not supported");

                _owner = value;

            }
        }

        protected bool IsDesignMode => this.IsDesignMode();

        public override ISite Site
        {
            get
            {
                return base.Site;
            }
            set
            {
                base.Site = value;
                if (value == null)
                    return;
                if (!(value.GetService(typeof(IDesignerHost)) is IDesignerHost service))
                    return;
                IComponent rootComponent = service.RootComponent;
                if (!(rootComponent is Control))
                    return;
                Owner = (Control)rootComponent;
            }
        }

        private Image _smallImage;
        [GHProperty, DefaultValue(null)]
        [Editor("DevExpress.Utils.Design.DXImageEditor, DevExpress.Design.v17.2", typeof(UITypeEditor))]
        public Image Image
        {
            get
            {
                return _smallImage;
            }
            set
            {
                _smallImage = value;
            }
        }

        private Image _largeImage;
        [GHProperty, DefaultValue(null)]
        [Editor("DevExpress.Utils.Design.DXImageEditor, DevExpress.Design.v17.2", typeof(UITypeEditor))]
        public Image LargeImage
        {
            get
            {
                return _largeImage;
            }
            set
            {
                _largeImage = value;
            }
        }

        [GHProperty, Browsable(true)]
        public virtual string Caption { get => Text; set => Text = value; }

        #endregion

        #region методы

        protected void DlgInfo(string message)
        {
            DlgHelper.DlgInfo(message);
        }

        protected void DlgWarning(string message)
        {
            DlgHelper.DlgWarning(message);
        }

        protected void DlgError(string message)
        {
            DlgHelper.DlgError(message);
        }

        protected bool DlgYesNo(string message)
        {
            return DlgHelper.DlgYesNo(message);
        }

        public virtual void OpenData()
        {
            foreach (DataSource source in EnumerateFields<DataSource>())
            {
                source.CloseOpen();
            }
        }

        public virtual void CloseData()
        {
            foreach (DataSource source in EnumerateFields<DataSource>())
            {
                source.Close();
            }
        }

        protected IEnumerable<T> EnumerateFields<T>()
        {
            return from x in GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty)
                   where typeof(T).IsAssignableFrom(x.FieldType)
                   let f = (T)x.GetValue(this)
                   where f != null
                   select f;
        }

        public virtual void SelectFrame()
        {
            if (Owner is AbstractFrame frame)
                frame.SelectFrame();

            if (this is IDetailsFrame details)
            {
                if (details.PageControl != null && details.Page != null)
                    details.PageControl.SelectedTabPage = details.Page;
            }

            Select();
        }

        public void FocusIt()
        {
            LayoutControl layoutControl = Controls.OfType<LayoutControl>().FirstOrDefault(); 
            if (layoutControl?.Controls.OfType<BaseEdit>().OrderBy(o => o.TabIndex).FirstOrDefault() is BaseEdit baseEdit)
                FocusIt(baseEdit);
        }

        private static void FocusIt(BaseEdit baseEdit)
        {
            baseEdit.SelectAll();
            baseEdit.Focus();
        }

        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // AbstractFrame
            // 
            this.Name = "AbstractFrame";
            this.Size = new System.Drawing.Size(356, 225);
            this.ResumeLayout(false);

        }
    }
}
