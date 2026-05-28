using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking2010.Views;

namespace GH.Components
{
    public partial class TabbetFrame : NavFrame
    {
        IList<SavedFrame> _frames = new List<SavedFrame>();  

        public override void OpenData()
        {
            _currFrame?.OpenData();
        }

        public override void CloseData()
        {
            _currFrame?.CloseData();
        }

        private SavedFrame _currFrame;
        protected SavedFrame CurrFrame
        {
            get => _currFrame;
            set
            {
                if (_currFrame == value)
                    return;

                if (_currFrame != null)
                {
                    _currFrame.SaveControls();
                    _currFrame.CloseData();
                }

                _currFrame = value;

                if (!AppContext.AppRunning)
                    return;

                if (_currFrame != null)
                {
                    _currFrame.LoadControls();
                    _currFrame.OpenData();
                }
            }
        }

        public TabbetFrame(Control owner) : this()
        {
            Owner = owner as Control;
        }

        public TabbetFrame()
        {
            InitializeComponent();
        }

        public override void LoadControls()
        {
            if (!SaveLayout)
                return;

            CurrFrame?.LoadControls();
        }

        public override void SaveControls()
        {
            if (!SaveLayout)
                return;

            CurrFrame?.SaveControls();
        }


        public override void Init()
        {
            CreateFrames?.Invoke(this, EventArgs.Empty);
            tabbedView.ActivateDocument(CurrFrame);
            tabbedView.QueryControl += TabbedView_QueryControl;
            tabbedView.DocumentActivated += TabbedView_DocumentActivated; 

            base.Init();

            foreach (var item in _frames)
            {
                if (item is IFinalInit finalInit)
                    finalInit.FinalInit();

                HandleFocusTracking(item.Controls);
            }
        }

        private void TabbedView_DocumentActivated(object sender, DocumentEventArgs e)
        {
            CurrFrame = e.Document.Control as NavFrame;// _frames.Where(x => x.Name == e.Document.ControlName).SingleOrDefault();
        }

        private void TabbedView_QueryControl(object sender, QueryControlEventArgs e)
        {
            e.Control = _frames.Where(x => x.Name == e.Document.ControlName).SingleOrDefault();            
        }

        protected void AddFrame(SavedFrame frame)
        {

            _frames.Add(frame);
            frame.Owner = this;

            if (_currFrame == null)
                _currFrame = frame;

            BaseDocument document = tabbedView.AddDocument(frame.Caption, frame.Name);
            document.ImageOptions.Image = frame.SmallImage;
        }

        [MyEvents]
        public event EventHandler CreateFrames;

    }
}