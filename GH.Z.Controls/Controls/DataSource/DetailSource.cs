using DevExpress.XtraLayout;
using GH.Interfaces;
using System.Collections.Generic;

namespace GH.Components
{
    public class DetailSource
    {
        DataSource _source;
        private IDetailsFrame _frame;

        internal bool InitDone => _frame != null;
        public LayoutControlGroup Page => InitDone ? _frame.Page : null;
        public TabbedGroup PageControl => InitDone ? _frame.PageControl : null;
        public bool Active => InitDone && (PageControl == null || PageControl.SelectedTabPage == Page);

        public DataSource Source => _source;

        public DetailSource(DataSource source)
        {
            _source = source;
            _frame = (source.Owner) as IDetailsFrame;
        }

        internal void ReOpenByTimer()
        {
            if (!Active)
                return;

            Source.ReOpenByTimer();
        }

        internal void Open()
        {
            if (!Active)
                return;

            Source.Open();
        }

        internal void Close()
        {
            Source.Close();
        }

        public override bool Equals(object obj)
        {
            var source = obj as DetailSource;
            return source != null &&
                   EqualityComparer<DataSource>.Default.Equals(Source, source.Source);
        }

        public override int GetHashCode()
        {
            return 2123741979 + EqualityComparer<DataSource>.Default.GetHashCode(Source);
        }

    }


}

