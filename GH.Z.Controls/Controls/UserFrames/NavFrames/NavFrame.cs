using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraNavBar;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars;
using System.Drawing.Design;
using DevExpress.XtraLayout;
using GH.Components;
using GH.Interfaces;

namespace GH.Controls
{
    [ToolboxItem(false)]
    public partial class NavFrame : SavedFrame, INavBarGroupFrame
    {
        protected NavBarGroup _group;
        [GHProperty, Browsable(false)]
        public virtual NavBarGroup Group
        {
            get
            {
                if (_group == null)
                    _group = FindGroup(Parent);
                return _group;
            }
        }

        public NavFrame()
        {
            InitializeComponent();
        }

        private NavBarGroup FindGroup(Control control)
        {
            if (control == null)
                return null;

            if (control is INavBarGroupFrame nav && nav.IsBase)
                return nav.Group;

            return FindGroup(control.Parent);
        }

        #region Component Designer generated code

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //}

        private void InitializeComponent()
        {
            //navbaritems = new List<NavBarItem>();
            SuspendLayout();
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ResumeLayout(false);
        }
        #endregion

    }
}
