using GH.NHibernate;
using System.Collections.Generic;
namespace MeshokBrowser.NHibernate
{
    public class BaseScanEntity : BaseEntity
    {
        private List<string> _urls = new List<string>();
        public virtual void DeleteCurrent()
        {
            if (_urls.Count > 0)
                _urls.RemoveAt(0);
        }
        public virtual string FullUrl
        {
            get
            {
                if (_urls.Count == 0)
                    return "";
                return CfgMeshok._url + _urls[0];
            }
        }
        public virtual string Url
        {
            get
            {
                if (_urls.Count == 0)
                    return "";
                return _urls[0];
            }
            set
            {
                if (value != null && _urls.IndexOf(value) == -1)
                    _urls.Add(value);
            }
        }
        public virtual string CloseUrl { get { return GetCloseUrl(); } }
        protected virtual string GetCloseUrl()
        {
            return "";
        }
        public virtual bool ParsingSaccess { get; set; }
    }
}
