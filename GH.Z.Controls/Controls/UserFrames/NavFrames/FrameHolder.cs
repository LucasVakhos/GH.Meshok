using System;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraNavBar;
using System.Reflection;
using DevExpress.XtraSplashScreen;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using GH.Components;
using static GH.UserForms.DefSplashScreen;
using GH.Utils;
using GH.Interfaces;
using GH.AppContext;

namespace GH.Controls
{
    public class FrameHolder : XtraUserControl
    {
        private IList<SavedFrame> frames = new List<SavedFrame>();

        private static FrameHolder _panel;
        public static FrameHolder Holder
        {
            get
            {
                if (_panel == null)
                {
                    if (RunContext.AppMainForm is INavBarForm navBarForm)
                    {
                        _panel = navBarForm.FrameHolder;
                    }

                    if (_panel == null)
                        throw new Exception($"На форме {RunContext.AppMainForm.Name} должен быть {nameof(FrameHolder)}!!!");
                }

                return _panel;
            }
        }

        private RibbonPageGroup _frameGroup;
        private NavBarControl _navBar;
        private NavFrame _currFrame;
        private bool _appRunning;
        //private FinalizeList finalizeList;
        private RibbonControl _ribbon;
        private RibbonStatusBar _statusBar;
        [Browsable(false)]
        public Control Owner { get; set; }

        [GHProperty, DefaultValue(null)]
        public RibbonPageGroup FrameGroup { get => _frameGroup; set => _frameGroup = value; }
        [GHProperty, DefaultValue(null)]
        public NavBarControl NavBar
        {
            get => _navBar;
            set
            {
                if (_navBar == value)
                    return;
                _navBar = value;
                if (_navBar != null)
                    _navBar.ActiveGroupChanged += NavBar_ActiveGroupChanged;
            }
        }
        [GHProperty, DefaultValue(null)]
        public RibbonControl Ribbon { get => _ribbon; set => _ribbon = value; }
        [GHProperty, DefaultValue(null)]
        public RibbonStatusBar StatusBar { get => _statusBar; set => _statusBar = value; }

        public FrameHolder()
        {
            Dock = DockStyle.Fill;
        }

        public void InitFrames(Type[] types)
        {
            SplashScreenManager.Default?.SendCommand(SplashScreenCommand.SetProgressMax, new SplashCommandArgs(types.Length, 0));
            int i = 1;
            foreach (Type item in types)
            {
                AddNavFrame(item);
                SplashScreenManager.Default?.SendCommand(SplashScreenCommand.SetProgress, new SplashCommandArgs(types.Length, i));
                i++;
                Thread.Sleep(25);
            }
            Thread.Sleep(250);
            SplashScreenManager.Default?.SendCommand(SplashScreenCommand.EndProgress, new SplashCommandArgs(0, 0));
        }



        [Browsable(false)]
        public bool AppRunning
        {
            get => _appRunning;
            set
            {
                _appRunning = value;
                if (_appRunning)
                {
                    CurrFrame?.LoadControls();
                    CurrFrame?.OpenData();
                }
            }
        }

        private void NavBar_ActiveGroupChanged(object sender, NavBarGroupEventArgs e)
        {
            CurrFrame = e.Group.Tag as NavFrame;
        }


        public void AddNavFrame(Type frame)
        {
            Type[] types = new Type[1];
            types[0] = typeof(Control);

            ConstructorInfo ctor = frame.GetConstructor(types);

            if (ctor != null)
            {
                try
                {
                    NavFrame Module = ctor.Invoke(new object[] { this }) as NavFrame;

                    Module.InitAsBase();

                    frames.Add(Module);

                    if (CurrFrame == null)
                        CurrFrame = Module;
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message, e);
                    throw new ApplicationException(e.Message, e);
                }
            }
        }

        public void CloseData(Type frame)
        {
            foreach (SavedFrame item in frames.Where(x=> x.GetType() == frame))
            {
                item.CloseData();
            }  
        }

        [Browsable(false)]
        public NavFrame CurrFrame
        {
            get
            {
                return _currFrame as NavFrame;
            }
            set
            {
                if (CurrFrame == value)
                    return;

                if (CurrFrame != null)
                {
                    CurrFrame.Parent = null;
                    Controls.Clear();
                    if (StatusBar != null)
                        StatusBar.UnMergeStatusBar();
                }

                _currFrame = value;

                if (CurrFrame != null)
                {
                    if (StatusBar != null)
                        StatusBar.MergeStatusBar(CurrFrame.StatusBar);
                    CurrFrame.Dock = DockStyle.Fill;
                    CurrFrame.Parent = this;                    
                }
            }
        }

        public void CLose()
        {
            CurrFrame = null;
        }




    }
}
