using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using NHibernate;

namespace GH.Components
{

    public interface IPage
    {
        int PageNo { get; set; }
    }

    [ToolboxItem(false)]
    public class CustomPagesNavigator<T> : XtraUserControl where T : ProtoEntity
    {
        private ProtoEntity _pageFilter;
        protected ProtoEntity PageFilter { get => _pageFilter; set => _pageFilter = value; }
        private IPage PageIntf => _pageFilter as IPage;
        const int on_page = 10000;
        class Page
        {
            public Page(int no)
            {
                No = no + 1;
            }

            public int No { get; }

            public override bool Equals(object obj)
            {
                var page = obj as Page;
                return page != null &&
                       No == page.No;
            }

            public override int GetHashCode()
            {
                return 733287466 + No.GetHashCode();
            }
        }

        private Rectangle NavPlace
        {
            get
            {
                return new Rectangle(0, 0, navigator.Width, navigator.Height);
            }
        }

        private Rectangle InfoPlace
        {
            get
            {
                Rectangle nav = NavPlace;
                return new Rectangle(nav.Right + 5, 0, Width - nav.Right - 5, nav.Height);
            }
        }

        private DataNavigator navigator;
        private LabelControl loadInfo;
        private BindingSource pagesSource;
        private MarqueeProgressBarControl progressBar;
        private int _cnt = 0;

        public CustomPagesNavigator()
        {
            InitializeComponent();
            navigator.Bounds = NavPlace;
            progressBar.Bounds = NavPlace;
            loadInfo.Bounds = InfoPlace;
            Height = navigator.Height;
            pagesSource = new BindingSource();
            navigator.DataSource = pagesSource;
            ShowProgress = false;
            pagesSource.PositionChanged += PagesSource_PositionChanged;
        }

        internal void SavePage()
        {
            if (PageIntf == null)
                return;
            if (!Internet.CheckConnectionForDatabase())
                return;

            PageIntf.PageNo = (pagesSource.Current as Page).No;
            using (ISession session = NHHelper.OpenSession())
            {
                session.Update(_pageFilter, _pageFilter.id);
                session.Transaction.Commit();
            }
        }

        public async void FillPages()
        {
            if (!Internet.CheckConnectionForDatabase())
                return;
            ShowProgress = true;
            try
            {


                IList<Page> src = await Task<IList<Page>>.Run(() =>
                {
                    using (ISession session = NHHelper.OpenSession())
                    {
                        string res = session.CreateSQLQuery($"CALL sp_titles_for_users_cnt({UserSetting.InnerUser.Id})").UniqueResult().ToString();
                        if (res == "")
                            _cnt = 0;
                        else
                            _cnt = int.Parse(res);
                    }

                    IList<Page> result = new List<Page>();
                    int rem = _cnt % on_page;
                    int cnt = _cnt / on_page;

                    for (int i = 0; i < cnt; i++)
                        result.Add(new Page(i));

                    if (rem > 0)
                        result.Add(new Page(result.Count));

                    return result;
                });

                pagesSource.DataSource = src;
                if (PageIntf != null && PageIntf.PageNo > 1 && src.Count > 0)
                    pagesSource.Position = PageIntf.PageNo - 1;

            }
            finally
            {
                ShowProgress = false;
            }
        }

        [Browsable(false)]
        public DataSource ReopenSource { get; set; }
        [Browsable(false)]
        public bool ShowProgress
        {
            get => progressBar.Visible;
            set
            {
                progressBar.Visible = value;
                navigator.Visible = !value;
                if (DesignMode)
                    return;
                if (progressBar.Visible)
                {
                    loadInfo.Text = "Ждите: Идет подсчёт страниц...";
                    //pagesSource.RaiseListChangedEvents = false;
                    //pagesSource.PositionChanged -= PagesSource_PositionChanged;
                }
                else
                {
                    //pagesSource.PositionChanged += PagesSource_PositionChanged;
                    loadInfo.Text = "Всего по условиям фильтрвции: " + _cnt.ToString("n0");
                    //pagesSource.RaiseListChangedEvents = true;
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            loadInfo.Bounds = InfoPlace;
            base.OnResize(e);
        }
        private void PagesSource_PositionChanged(object sender, EventArgs e)
        {
            if (ShowProgress)
                return;
            SavePage();
            ReopenSource?.CloseOpen();
        }

        private void InitializeComponent()
        {
            this.progressBar = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.navigator = new DevExpress.XtraEditors.DataNavigator();
            this.loadInfo = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.EditValue = 0;
            this.progressBar.Location = new System.Drawing.Point(0, 0);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 18);
            this.progressBar.TabIndex = 2;
            this.progressBar.Visible = false;
            // 
            // navigator
            // 
            this.navigator.Buttons.Append.Visible = false;
            this.navigator.Buttons.CancelEdit.Visible = false;
            this.navigator.Buttons.EndEdit.Visible = false;
            this.navigator.Buttons.Remove.Visible = false;
            this.navigator.Location = new System.Drawing.Point(0, 0);
            this.navigator.Name = "navigator";
            this.navigator.Size = new System.Drawing.Size(218, 20);
            this.navigator.TabIndex = 0;
            this.navigator.Text = "Navigator";
            this.navigator.TextLocation = DevExpress.XtraEditors.NavigatorButtonsTextLocation.Center;
            this.navigator.TextStringFormat = "Стр. {0} из {1}";
            // 
            // loadInfo
            // 
            this.loadInfo.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.loadInfo.Location = new System.Drawing.Point(224, 0);
            this.loadInfo.Name = "loadInfo";
            this.loadInfo.Size = new System.Drawing.Size(204, 20);
            this.loadInfo.TabIndex = 1;
            this.loadInfo.Text = "Ждите: Идет загрузка страниц";
            // 
            // CustomPagesNavigator
            // 
            this.Controls.Add(this.navigator);
            this.Controls.Add(this.loadInfo);
            this.Controls.Add(this.progressBar);
            this.Name = "CustomPagesNavigator";
            this.Size = new System.Drawing.Size(431, 37);
            ((System.ComponentModel.ISupportInitialize)(this.progressBar.Properties)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
