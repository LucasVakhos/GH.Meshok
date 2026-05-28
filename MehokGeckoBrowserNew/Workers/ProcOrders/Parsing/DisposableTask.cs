using MeshokBrowser.NHibernate;
using System;
namespace MeshokBrowser.Workers
{
    public class DisposableTask : IDisposable
    {
        protected ScanWebFrame _scanFrame;
        private bool _taskFinished = false;
        public virtual void AddTask(BaseScanEntity scanEntity)
        {
        }
        public virtual void Execute() { }
        public void Dispose()
        {
            if (_scanFrame != null)
                _scanFrame.Dispose();
            _scanFrame = null;
        }
        public bool TaskFinished
        {
            get => _taskFinished;
            set
            {
                _taskFinished = value;
                if (_taskFinished)
                {
                }
            }
        }
    }
}
