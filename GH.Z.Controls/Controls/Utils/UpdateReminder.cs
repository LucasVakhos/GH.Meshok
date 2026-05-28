using System;
using System.ComponentModel;
using GhUtils;
using GH.Components;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars;
using AmigoPlus.Frames;

namespace GH.Components
{
    [ToolboxItem(false)]
    public class UpdateReminder : Component, IDisposable
    {
        private const int _base_interval = 1000 * 60 * 10;
        private const int _1_min = 1000 * 60;
        private const int _05_sec = 250;
        private const int _10_sec = 1000 * 10;
        private const int _blinks = 5;

        private bool disposedValue = false; // Для определения избыточных вызовов
        private IContainer components = null;
        private DataSource dataSource;
        private Timer timer;

        private RibbonStatusBar StatusBar => _mainForm == null ? null : _mainForm.StatusBar;
        private RibbonControl Ribbon => _mainForm== null ? null : _mainForm.Ribbon;
        private BarStaticItem _updateLabel;
        private BarItemLink _updateLink;
        private int _blink_count = 0;
        private IMainForm _mainForm;
        public IUpdate_reminder Update => dataSource.Entity as IUpdate_reminder;

        public event OnGetSqlString GetSqlString
        {
            add
            {
                dataSource.GetSqlString += value;
            }
            remove
            {
                dataSource.GetSqlString -= value;
            }
        }

        public event GetRepository GetRepository
        {
            add
            {
                dataSource.GetRepository += value;
            }
            remove
            {
                dataSource.GetRepository -= value;
            }
        }

        public event HasUpdateHandler OnHasUpdate;

        public event UpdateViewedHandler OnUpdateViewed;

        protected virtual bool HasUpdate()
        {
            return Update.has_update;
        }

        public UpdateReminder(Form form)
        {
            _mainForm = form as IMainForm;
            if (_mainForm == null)
                throw new Exception($"Форма {form.Name} должна содержать интерфейс IMainForm!!!");

            IContainer form_components = EnumerateFields(form).FirstOrDefault();
            form_components?.Add(this);

            InitializeComponent();
            timer.Start();
        }

        private void InitializeComponent()
        {
            components = new Container();

            dataSource = new DataSource(components);
            timer = new Timer(components);

            //dataSource.GetRepository += _dataSource_GetRepository;
            //dataSource.GetSqlString += _dataSource_GetSqlString;
            dataSource.AfterOpen += _dataSource_AfterOpen;
            dataSource.AfterPost += _dataSource_AfterPost;

            timer.Interval = _10_sec;
            timer.Tick += _timer_Tick;
        }

        private void _dataSource_AfterPost(object sender, EventArgs e)
        {
            DisposeUpdateLabel();
        }

        private void _dataSource_AfterOpen(object sender, EventArgs e)
        {
            if (HasUpdate())
                CreateInfo();
            else
            {
                timer.Interval = _base_interval;
                timer.Start();
            }
        }

        private void CreateInfo()
        {
            if (_updateLabel != null)
                return;
            _updateLabel = new BarStaticItem();
            Ribbon.Items.Add(_updateLabel);
            _updateLabel.Alignment = BarItemLinkAlignment.Right;
            _updateLabel.Caption = string.Format("Есть обновление {0:d}", Update.upd_date);
            _updateLabel.Hint = "Кликните, чтобы посмотреть"; 
            _updateLabel.Id = Ribbon.Items.Count;
            _updateLabel.ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/scheduling/reminder_16x16.png");
            _updateLabel.Name = nameof(_updateLabel);
            _updateLink = StatusBar.ItemLinks.Add(_updateLabel);
            _updateLabel.ItemClick += _updateLabel_ItemClick;
            timer.Interval = _05_sec;
            timer.Start();
            OnHasUpdate?.Invoke(this);            

        }

        private void _updateLabel_ItemClick(object sender, ItemClickEventArgs e)
        {
            InformBase();
            FilterProcessor processor = FilterProcessor.GetInstance();
            processor.RefreshUpdLookUpEdit();
            processor.Frame.SelectFrame();
            Application.DoEvents();
            processor.Clear();
            processor.utf_upd_id = Update.old_upd_id;
            processor.AppyFilter();
        }

        public virtual void InformBase()
        {
            if (HasUpdate())
            {
                OnUpdateViewed?.Invoke();
                Update.old_upd_id = Update.new_upd_id;
                dataSource.Repository.Update(Update);
                _blink_count = 0;
                timer.Interval = _base_interval;
                timer.Start();
            }
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            if (_updateLabel == null)
            {
                dataSource.CloseOpen();
            }
            else
            {
                if (_blink_count == 0)
                {
                    timer.Interval = _05_sec;
                    _blink_count = 1;
                }

                switch (_updateLabel.Visibility)
                {
                    case BarItemVisibility.Always:
                        _updateLabel.Visibility = BarItemVisibility.Never;
                        break;
                    case BarItemVisibility.Never:
                        _updateLabel.Visibility = BarItemVisibility.Always;
                        _blink_count++;
                        break;
                    default:
                        break;
                }
                Application.DoEvents();

                if (_blink_count == _blinks)
                {
                    _blink_count = 0;
                    timer.Interval = _1_min;

                }
                timer.Start();
            }
        }

        IEnumerable<IContainer> EnumerateFields(Form form)
        {
            return from x in form.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty)
                   where typeof(IContainer).IsAssignableFrom(x.FieldType)
                   let f = (IContainer)x.GetValue(form)
                   where f != null
                   select f;
        }
      


        #region IDisposable Support
        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DisposeUpdateLabel();

                    if (components != null)
                    {
                        components.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        private void DisposeUpdateLabel()
        {
            if (_updateLabel != null)
            {
                try
                {
                    StatusBar.ItemLinks.Remove(_updateLink);
                }
                catch  { }

                try
                {
                    _updateLink.Dispose();
                }
                catch { }

                _updateLink = null;

                try
                {
                    StatusBar.Ribbon.Items.Remove(_updateLabel);
                }
                catch { }

                try
                {
                    _updateLabel.Dispose();
                }
                catch { }
                _updateLabel = null;
            }
        }

        #endregion
    }

    public delegate void UpdateViewedHandler();

    public delegate void HasUpdateHandler(UpdateReminder reminder);
   
}


