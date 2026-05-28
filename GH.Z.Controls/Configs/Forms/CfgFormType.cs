using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using GH.Components;
using GH.Helpers;
using GH.UserExceptions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GH.Configs
{
    public class CfgFormType<T> : CfgForm where T : CfgBaseFrame
    {
        private T _cfgFrame;

        public T CfgFrame
        {
            get => _cfgFrame;
            set
            {
                if (value == _cfgFrame)
                    return;

                if (value != _cfgFrame)
                {
                    actEnter.Update -= ActEnter_Update;
                    actEnter.Execute -= ActEnter_Execute;
                    actCancel.Execute -= ActCancel_Execute;
                }

                _cfgFrame = value;
                if (value != null)
                {

                    LayoutControlItem lcControl = new LayoutControlItem();
                    Suspend(value, lcControl);
                    try
                    {
                        Task.Factory.StartNew(() =>
                        {
                                while (_cfgFrame != null && !Disposing)
                                {
                                    if (CfgFrame.dataSource.Current is CfgCoreConnection cfg)
                                        IsConnect = cfg.IsComplete && cfg.TestConnection();
                                    Thread.Sleep(250);
                                }
                        });

                        layoutControl.Dock = DockStyle.None;

                        value.layoutControl.Dock = DockStyle.None;
                        value.ClientSize = value.layoutControl.PreferredSize;
                        value.layoutControl.Dock = DockStyle.Fill;

                        layoutControl.Controls.Add(value);

                        lcControl.Control = value;
                        lcControl.Text = value.Caption;
                        lcControl.Name = "lc" + typeof(T).Name;
                        lcControl.TextSize = value.Size;
                        lcControl.TextVisible = false;

                        FrameGroup.AddItem(lcControl, YesNoGroup, InsertType.Top);

                        Text = value.Caption;

                    }
                    finally
                    {
                        Resume(value, lcControl);
                    }
                    actEnter.Update += ActEnter_Update;
                    actEnter.Execute += ActEnter_Execute;
                    actCancel.Execute += ActCancel_Execute;
                }
            }
        }

        public bool IsConnect { get; private set; }

        private void Suspend(Control value, LayoutControlItem lcControl)
        {
            FrameGroup.BeginInit();
            lcControl.BeginInit();
            value.SuspendLayout();
            layoutControl.SuspendLayout();
            SuspendLayout();
        }

        private void Resume(Control value, LayoutControlItem lcControl)
        {
            FrameGroup.EndInit();
            lcControl.EndInit();
            value.ResumeLayout(false);
            layoutControl.ResumeLayout(false);
            ResumeLayout(false);

            Size size = layoutControl.PreferredSize;
            ClientSize = SizeHeper.NewSize(size);
            layoutControl.Dock = DockStyle.Fill;
        }

        public CfgFormType()
        {
            if (IsDesignMode)
                return;

            //CfgFrame = GetFrame() as T;
            CfgFrame = Assembly.GetEntryAssembly().CreateInstance(typeof(T).FullName) as T;

        }

        //protected virtual object GetFrame()
        //{
        //    throw new NotImplemented(nameof(GetFrame), this);
        //}

        private void ActEnter_Update(object sender, System.EventArgs e)
        {
            actEnter.Enabled = EnterEnabled();
        }


        protected virtual bool EnterEnabled()
        {
            return IsConnect;
        }

        private void ActEnter_Execute(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
            CfgFrame.Save();
        }

        private void ActCancel_Execute(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }


    }

}