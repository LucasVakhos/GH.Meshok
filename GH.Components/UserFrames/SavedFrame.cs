using System.ComponentModel;
namespace GH.Components
{
    public class SavedFrame : AbstractFrame, ICaption, ISavedControl, IOpenData
    {
        Control _activeControl = null;
        private bool _saveLayout = true;
        [GH.ComponentsProperty, DefaultValue(true)]
        public bool SaveLayout { get => _saveLayout; set => _saveLayout = value; }
        new public Control Parent
        {
            get => base.Parent;
            set
            {
                if (base.Parent != null)
                {
                    _activeControl = ActiveControl;
                    SaveControls();
                    CloseData();
                }
                base.Parent = value;
                if (value != null)
                {
                    if (RunContext.AppRunning)
                    {
                        LoadControls();
                        Application.DoEvents();
                        if (_activeControl != null)
                            ActiveControl = _activeControl;
                        OpenData();
                    }
                }
            }
        }
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
        }
        public SavedFrame()
        {
        }
        public virtual void LoadControls()
        {
            if (!_saveLayout)
                return;
            foreach (var item in Controls)
            {
                if (item is ISavedControl saved)
                    saved.LoadControls();
            }
        }
        public virtual void SaveControls()
        {
            if (!_saveLayout)
                return;
            foreach (var item in Controls)
            {
                if (item is ISavedControl saved)
                    saved.SaveControls();
            }
        }
    }
}
