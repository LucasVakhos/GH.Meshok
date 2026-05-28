using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.ComponentModel;
using GH.Components;

namespace GH.UserForms
{
    [ToolboxItem(false)]
    public class SimpleForm : XtraForm
    {
        public bool IsDesignMode => this.IsDesignMode();
        public bool IsRuntimeMode => !IsDesignMode;


        public SimpleForm()
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            if (IsRuntimeMode)
            {
                Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            }

            base.OnLoad(e);
        }
    }
}