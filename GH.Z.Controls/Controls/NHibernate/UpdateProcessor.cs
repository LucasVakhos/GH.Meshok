using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraWaitForm;
using GH.Utils;

namespace GH.NHibernate
{

    public partial class ProcWaitForm : WaitForm
    {
        internal static Queue<string> infos = new Queue<string>();

        public ProcWaitForm()
        {
            InitializeComponent();
            this.progressPanel1.AutoHeight = true;
            UpdateProcessor.waitForm = this;
            NewDescription();
        }

        #region Overrides

        public override void SetCaption(string caption)
        {
            base.SetCaption(caption);
            this.progressPanel1.Caption = caption;
        }
        public override void SetDescription(string description)
        {
            base.SetDescription(description);
            this.progressPanel1.Description = description;
        }
        public override void ProcessCommand(Enum cmd, object arg)
        {
            switch ((WaitFormCommand)cmd)
            {
                case WaitFormCommand.NextInfo:
                    NewDescription();
                    break;
                default:
                    break;
            }
            base.ProcessCommand(cmd, arg);
        }

        private void NewDescription()
        {
            string info = infos.Dequeue();
            SetDescription(info);
        }

        #endregion

        public enum WaitFormCommand
        {
            NextInfo,
        }
    }

    public class UpdateProcessor
    {
        #region Fields
        internal static ProcWaitForm waitForm;
        private static object _lock = new object();
        private static int _splashCount = 0;
        private Control _paretnt;
        private bool _animate = true;
        private SqlTypes _procType = SqlTypes.SelectSql;
        private Action _execute;
        private Action _afterExecute;
        #endregion

        #region Constructors
        public UpdateProcessor(SqlTypes procType, Action execute, Action afterExecute = null)
        {
            _procType = procType;
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _afterExecute = afterExecute ?? new Action(() => { });
        }

        public UpdateProcessor(SqlTypes procType, bool animate, Action execute, Action afterExecute = null) : this(procType, execute, afterExecute)
        {
            _animate = animate;
        }

        public UpdateProcessor(SqlTypes procType, Control paretnt, bool animate, Action execute, Action afterExecute = null) : this(procType, animate, execute, afterExecute)
        {
            _paretnt = paretnt;
        }

        public UpdateProcessor(SqlTypes procType, Control paretnt, Action execute, Action afterExecute = null) : this(procType, execute, afterExecute)
        {
            _paretnt = paretnt;
        }
        #endregion

        #region Methods
        public void Execute()
        {
            if (!Internet.CheckConnectionForDatabase())
                return;

            if (_animate)
            {
                ShowWaitInfo(_procType == SqlTypes.SelectSql ? "Идет загрузка данных..." : "Идет обработка данных...", _paretnt);
                Application.DoEvents();
            }

            Task.Factory.StartNew(() =>
            {
                try
                {
                    _execute?.Invoke();
                }
                catch (Exception ex)
                {
                    Logger.Error("UpdateProcessor", ex);
                }
            }).Wait();

            _afterExecute?.Invoke();

            if (_animate)
                HideWaitInfo();
        }

        static void Display(Task t)
        {
            Console.WriteLine($"Id задачи: {Task.CurrentId}");
            Console.WriteLine($"Id предыдущей задачи: {t.Id}");
            Thread.Sleep(3000);
        }

        private static void ShowWaitInfo(string info, Control paretnt)
        {
            lock (_lock)
            {
                ProcWaitForm.infos.Enqueue(info);
                _splashCount++;
                if (_splashCount == 1)
                {
                    if (paretnt == null)
                        SplashScreenManager.ShowForm(typeof(ProcWaitForm));
                    else if (paretnt is UserControl userControl)
                        SplashScreenManager.ShowForm(userControl, typeof(ProcWaitForm), true, true);
                    else if (paretnt is Form form)
                        SplashScreenManager.ShowForm(form, typeof(ProcWaitForm), true, true);
                }
            }
        }

        private static void HideWaitInfo()
        {
            lock (_lock)
            {
                _splashCount--;
                if (_splashCount == 0)
                    SplashScreenManager.CloseForm();
                else
                {
                    SplashScreenManager.Default.SendCommand(ProcWaitForm.WaitFormCommand.NextInfo, null);
                }
            }
        }
        #endregion

    }
}