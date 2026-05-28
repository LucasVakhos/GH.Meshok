using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using GH.AppContext;
using GH.Components.Attributes;
using GH.Interfaces;
using GH.NHibernate;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace GH.Configs
{
    public class SectionMainForm
    {
        [DataMember]
        public Size FormSize { get; set; } = new Size(800, 600);
        [DataMember]
        public Point Location { get; set; } = Point.Empty;
        [DataMember]
        public FormWindowState WindowState { get; set; } = FormWindowState.Normal;
        [DataMember]
        public int NavBarExpandedWidth { get; set; } = 185;
        [DataMember]
        public NavPaneState NavBarPaneState { get; set; } = NavPaneState.Expanded;


        public AbstractEntity Owner { get => _owner; set => _owner = value; }
        private AbstractEntity _owner;

        public event Action<Form> OnRestore;

        public SectionMainForm()
        {
        }

        public SectionMainForm(AbstractEntity owner)
        {
        }


        internal void Restore()
        {
            Restore(RunContext.AppMainForm);
        }



        protected virtual void Restore(Form form)
        {
            bool isMain = RunContext.AppMainForm == form;

            Point location = Location;

            Size size = FormSize;

            FormWindowState windowState = WindowState;

            if (RunContext.AppMainForm is XtraForm xtra)
            {

                if (location.X <= 0 && location.Y <= 0)
                {
                    Rectangle rect = Rectangle.Empty;
                    if (isMain)
                        rect = Screen.GetWorkingArea(location);
                    else
                        rect = new Rectangle(location, size);


                    location = new Point((rect.Width - size.Width) / 2, (rect.Height - size.Height) / 2);
                }
            }

            form.Location = location;
            form.Size = size;

            if (windowState != FormWindowState.Minimized)
                form.WindowState = windowState;

            form.ResizeEnd += MainForm_Resize;

            if (form is INavBarForm navBarForm)
            {
                navBarForm.NavBar.OptionsNavPane.ExpandedWidth = NavBarExpandedWidth;
                navBarForm.NavBar.OptionsNavPane.NavPaneState = NavBarPaneState;
                navBarForm.NavBar.SizeChanged += NavBarControl_SizeChanged;
            }

            //if (form is IRibbonForm ribbonForm)
            //{
            //    ribbonForm.Ribbon.ExpandCollapseItem. OptionsNavPane.ExpandedWidth = NavBarExpandedWidth;
            //    navBarForm.NavBar.OptionsNavPane.NavPaneState = NavBarPaneState;
            //    navBarForm.NavBar.SizeChanged += NavBarControl_SizeChanged;
            //}

            OnRestore?.Invoke(form);
        }

        private void NavBarControl_SizeChanged(object sender, EventArgs e)
        {
            if (sender is NavBarControl navBar)
            {
                if (NavBarPaneState == NavPaneState.Expanded)
                    NavBarExpandedWidth = navBar.Width;

                NavBarPaneState = navBar.OptionsNavPane.NavPaneState;
            }
        }

        public void SetWindowState(FormWindowState windowState)
        {
            WindowState = windowState;
        }

        private void MainForm_Resize(object sender, System.EventArgs e)
        {
            if (sender is Form form)
            {
                FormSize = form.Size;
                Location = form.Location;
            }
        }


    }

}

