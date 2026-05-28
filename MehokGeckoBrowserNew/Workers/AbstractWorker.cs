using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace MeshokBrowser.Workers
{
    public class AbstractWorker : IDisposable, IProcessNotify
    {
        protected bool _workIsDone = false;
        private string _currentStepText = string.Empty;
        private string _currentStepSuffix = string.Empty;
        private static readonly object _locker = new object();
        private int _currentStep = 0;
        private int _byStep = 0;
        private int _totalSteps = 0;
        protected ProcessorWB ProcScreen
        {
            get => ProcessRunHelper.ProcScreen;
            set => ProcessRunHelper.ProcScreen = value;
        }
        protected IMainForm MainForm
        {
            get => ProcessRunHelper.MainForm;
            set => ProcessRunHelper.MainForm = value;
        }
        private readonly Task main_thread;
        private readonly Task exec_thread;
        protected string ProcessName { get; set; } = string.Empty;
        protected int TotalSteps
        {
            get => _totalSteps;
            set
            {
                _totalSteps = value;
                _byStep = Math.Max(1, _totalSteps / 100);
                _currentStep = 0;
                NotifyProgressMax();
            }
        }
        protected int CurrentStep => _currentStep;
        public void SetTotalSteps(int value) => TotalSteps = value;
        public void IncCurrentStep()
        {
            _currentStep++;
            if ((_currentStep < _totalSteps && _currentStep % _byStep == 0) ||
                _currentStep == _totalSteps ||
                (_currentStep < _totalSteps && _totalSteps < 1000))
            {
                _currentStepText = ProcessName + $"=> обработано {CurrentStep} из {_totalSteps}";
                NotifyProgress();
                NotifyStatus(_currentStepText + _currentStepSuffix);
            }
        }
        public void SendMessage(string value) => NotifyStatus(value);
        public AbstractWorker(IMainForm form)
        {
            MainForm = form;
            ProcScreen = new ProcessorWB
            {
                Name = GetType().Name
            };
            exec_thread = new Task(DoExecute);
            main_thread = new Task(WaitExecute);
        }
        private void WaitExecute()
        {
            int step = 0;
            string Steps()
            {
                string steps = " =";
                for (int i = 0; i < step; i++)
                    steps += ">";
                step++;
                if (step == 11)
                    step = 0;
                return steps;
            }
            OnCreate();
            while (ProcessRunHelper.Executing)
            {
                _currentStepSuffix = Steps();
                NotifyStatus(_currentStepText + _currentStepSuffix);
                Thread.Sleep(250);
            }
        }
        protected virtual void OnCreate()
        {
        }
        protected virtual void DoExecute()
        {
            throw new NotImplementedException("Перезапишите метод DoExecute()");
        }
        private void NotifyProgressMax()
        {
            if (!_workIsDone)
                ProcScreen.SetProgressMax(_totalSteps);
        }
        private void NotifyProgress()
        {
            if (!_workIsDone)
                ProcScreen.SetProgress(CurrentStep);
        }
        private void NotifyStatus(string info)
        {
            if (!_workIsDone)
                ProcScreen.SetStatus(info);
        }
        private void NotifyDone()
        {
            lock (_locker)
            {
                if (!_workIsDone && MainForm != null)
                    MainForm.Proces_Finished();
                _workIsDone = true;
            }
        }
        public virtual void Execute()
        {
            ProcessRunHelper.Executing = true;
            ProcScreen.Caption = ProcessName;
            MainForm.AddPage(ProcScreen);
            MainForm.Proces_Begined();
            _currentStepText = ProcessName + "=> процесс начат";
            NotifyStatus(_currentStepText);
            List<Task> tasks = new List<Task>();
            main_thread.Start();
            tasks.Add(main_thread);
            exec_thread.Start();
            tasks.Add(exec_thread);
            Task.Factory.StartNew(() =>
            {
                Task.WaitAll(tasks.ToArray());
                WorkDone();
            });
        }
        protected virtual void WorkDone()
        {
            if (!disposedValue)
            {
                ProcessRunHelper.Executing = false;
                Thread.Sleep(10);
                NotifyDone();
            }
        }
        protected bool clearGC = false;
        protected void ClearCash()
        {
            if (!clearGC)
                return;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            clearGC = false;
        }
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
                return;
            if (disposing)
            {
                ProcessRunHelper.Executing = false;
                MainForm.RemovePage(ProcScreen);
                ProcScreen.Dispose();
                ProcScreen = null;
                MainForm = null;
            }
            disposedValue = true;
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
