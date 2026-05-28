using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace NewsMaker
{
    public class WorkStatusCounter : TotalCounter
    {
        private bool _disposed;
        private Task _progressor;
        private int _step;
        private string _stepsText = "";
        public WorkStatusCounter(SimpleInfoText text) : base(text)
        {
            _text.VisibleChanged += _text_VisibleChanged;
        }
        private void ProgressText()
        {
            if (_step == 0)
                _stepsText = " ";
            else
                _stepsText += ">";
            _step++;
            if (_step == 11)
                _step = 0;
        }
        private void _text_VisibleChanged(object sender, EventArgs e)
        {
            if (_text.Visible)
            {
                _disposed = false;
                _progressor = Task.Run(() =>
                {
                    while (AppContextNM.Executing && !_disposed)
                    {
                        ProgressText();
                        Thread.Sleep(250);
                    }
                });
            }
            else
            {
                _disposed = true;
            }
        }
        internal override string GetText()
        {
            return $"обработано {Current} из {TotalCount}" + _stepsText;
        }
        internal void Dispose()
        {
            if (_progressor == null)
                return;
            if (_progressor.IsFaulted)
            {
                DisposeProgress();
                return;
            }
            var treads = new List<Task>();
            treads.Add(_progressor);
            Task.Factory.StartNew(() =>
            {
                _disposed = true;
                Task.WaitAll(treads.ToArray());
                DisposeProgress();
            });
        }
        private void DisposeProgress()
        {
            _progressor.Dispose();
            _progressor = null;
        }
    }
}
