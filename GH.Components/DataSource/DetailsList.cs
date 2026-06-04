using DevExpress.XtraLayout;
namespace GH.Components
{
    public class DetailsList : List<DetailSource>
    {
        private List<TabbedGroup> _pages = new List<TabbedGroup>();
    private void Pages_SelectedPageChanged(object sender, LayoutTabPageChangedEventArgs e)
        {
            CloseDetails(e.PrevPage);
            OpenDetails(e.Page);
        }
    internal void ReOpenDetailsByTimer()
        {
            if (Count > 0)
                foreach (DetailSource source in this.Where(x => x.Active))
                {
                    source.ReOpenByTimer();
                }
        }
        //internal void ReOpenDetailsByTimer(CommonDataSource source)
        //{
        //    this.Where(x => x.Source == source && x.Active).FirstOrDefault()?.ReOpenByTimer();
        //}
    private void OpenDetails(LayoutGroup page)
        {
            foreach (var item in this.Where(x => x.Page == page && x.Active).ToArray())
                item.Open();
        }
    private void CloseDetails(LayoutGroup page)
        {
            foreach (var item in this.Where(x => x.Page == page).ToArray())
                item.Close();
        }
    private void RegPageControl(TabbedGroup pages)
        {
            if (pages == null)
                return;
            TabbedGroup has = _pages.Where(x => x == pages).FirstOrDefault();
            if (has == null)
            {
                _pages.Add(pages);
                pages.SelectedPageChanged += Pages_SelectedPageChanged;
            }
        }
    internal void RegDataSource(DataSource source)
        {
            DetailSource detail = this.Where(x => x.Source == source).FirstOrDefault();
            if (detail == null)
            {
                detail = new DetailSource(source);
                if (detail.InitDone)
                {
                    this.Add(detail);
                    RegPageControl(detail.PageControl);
                }
            }
        }
    internal void UnRegDataSource(DataSource source)
        {
            DetailSource detail = this.Where(x => x.Source == source).FirstOrDefault();
            if (detail != null)
                this.Remove(detail);
        }
    }
}
