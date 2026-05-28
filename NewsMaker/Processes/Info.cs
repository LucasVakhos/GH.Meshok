using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using CustomAttributeExtensions = System.Reflection.CustomAttributeExtensions;
namespace NewsMaker
{
    public enum InfoType
    {
        [Display(Name = "Рассылка сообщений")] MainInfo,
        [Display(Name = "Итог по операции")] SummaryOfProcess,
        [Display(Name = "Отправлено")] Sended,
        [Display(Name = "Ошибок")] Errors,
        [Display(Name = "Статистика")] Elapsed,
        [Display(Name = "Обработано")] WorkStatus,
        [Display(Name = "Рассылка завершена")] EndProcess
    }
    internal static class TypeInfoExtensions
    {
        public static string GetDisplayValue(this InfoType value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null) return null;
            var attribute =
                (DisplayAttribute)CustomAttributeExtensions.GetCustomAttribute(fieldInfo, typeof(DisplayAttribute));
            return attribute.Name;
        }
        public static int Int(this InfoType value)
        {
            return (int)value;
        }
    }
    public class Informator : IDisposable
    {
        public static Informator info;
        private readonly IMainForm _main;
        private readonly List<SimpleInfoText> labels = new List<SimpleInfoText>();
        internal MemoEdit memoEdit;
        public Informator(IMainForm main)
        {
            _main = main;
            info = this;
        }
        internal LayoutControlGroup Report => _main.Report;
        public int TotalCount
        {
            get => TotalCounter.TotalCount;
            set
            {
                if (readyState) TotalCounter.TotalCount = value;
                if (TotalCount > 0) _main.SetProgressMax(TotalCount);
            }
        }
        protected SimpleInfoText this[InfoType info]
        {
            get
            {
                if (labels.Count > info.Int()) return labels[info.Int()];
                return null;
            }
        }
        public override string ToString()
        {
            var str = "";
            for (var i = InfoType.MainInfo; i < InfoType.EndProcess; i++)
            {
                this[i].RestoreText();
                str += i.GetDisplayValue() + ": " + this[i].Text + "\r\n";
            }
            return str;
        }
        internal void Init()
        {
            if (readyState) return;
            Report.BeginUpdate();
            try
            {
                LayoutControlItem first = null;
                foreach (LayoutControlItem item in Report.Items)
                    if (item.Control is MemoEdit)
                    {
                        memoEdit = item.Control as MemoEdit;
                        first = item;
                        break;
                    }
                    else
                    {
                        item.Visibility = LayoutVisibility.Never;
                    }
                for (var i = InfoType.MainInfo; i < InfoType.EndProcess; i++)
                {
                    var simpleInfo = SimpleInfoText.Create(i);
                    labels.Add(simpleInfo);
                    var item = new LayoutControlItem(Report);
                    item.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Far;
                    item.Text = i.GetDisplayValue() + ":";
                    this[i].Tag = item;
                    item.Control = this[i];
                    item.TextVisible = true;
                    _main.InvokeIfRequired(() => { Report.AddItem(item, first, InsertType.Top); });
                }
                readyState = true;
            }
            finally
            {
                Report.EndUpdate();
            }
            Application.DoEvents();
        }
        private void IncSended()
        {
            if (readyState)
                this[InfoType.Sended]?.IncCurrent();
        }
        private void IncErrors()
        {
            if (readyState)
                this[InfoType.Errors]?.IncCurrent();
        }
        internal void RegError(string value)
        {
            if (readyState)
            {
                _main.InvokeIfRequired(() => { memoEdit.Text += value + "\r\n"; });
            }
        }
        internal void Done()
        {
            if (!readyState)
                return;
            readyState = false;
            _main.InvokeIfRequired(() =>
            {
                memoEdit.Text = ToString() + "\r\n\r\n" + memoEdit.Text;
                Report.BeginUpdate();
                try
                {
                    for (var i = Report.Items.Count - 1; i > 0; i--)
                    {
                        var item = Report.Items[i] as LayoutControlItem;
                        if (item.Control is LabelControl)
                            item.Dispose();
                    }
                    foreach (LayoutControlItem item in Report.Items)
                        item.Visibility = LayoutVisibility.Always;
                    //counters.Clear();
                    for (var i = 0; i < labels.Count; i++)
                        labels[i].Dispose();
                    labels.Clear();
                }
                finally
                {
                    Report.EndUpdate();
                }
            });
        }
        private void RegIPrepare(IPrepare prepare)
        {
            if (prepare == null)
                return;
            TotalCount = 0;
            prepare.OnStep += DoOnStep;
            prepare.Info = this;
        }
        private void DoOnStep()
        {
            TotalCounter.IncCurrent();
            _main.SetProgress(TotalCounter.Current);
        }
        private void RegLetter(Letter letter)
        {
            if (letter == null)
                return;
            _main.InvokeIfRequired(() =>
            {
                if (letter.CallBack.HasError)
                {
                    /*
                    if (letter.CallBack.ErrorLimit)
                        return;
                    */
                    memoEdit.Text += letter.ToString();
                    IncErrors();
                }
                else
                {
                    IncSended();
                }
                this[InfoType.WorkStatus]?.IncCurrent();
            });
        }
        public static void RegObject(object ob)
        {
            var prepare = ob as IPrepare;
            if (prepare != null)
            {
                info.RegIPrepare(prepare);
                return;
            }
            var letter = ob as Letter;
            if (letter != null)
                info.RegLetter(letter);
        }
        public void FireInfo(string mess, bool asSumary = false)
        {
            if (readyState)
            {
                if (asSumary)
                    this[InfoType.SummaryOfProcess]?.SetText(mess);
                else
                    this[InfoType.MainInfo]?.SetText(mess);
            }
        }
        public void FireInfo(string mess, InfoType info)
        {
            if (readyState) this[info]?.SetText(mess);
        }
        private bool disposedValue; // Для определения избыточных вызовов
        //private Task main_thread;
        private bool readyState;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing) Done();
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
