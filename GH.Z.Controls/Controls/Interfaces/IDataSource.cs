using GH.Components;
using GH.NHibernate;
using System;
using System.Windows.Forms;

namespace GH.Interfaces
{
    public interface IDataSource
    {
        bool NeedLoadingAnimate { get; set; }
        Control Owner { get; }
        BaseEntity Entity { get; }

        event GetRepository GetRepository;
        event EventHandler AfterOpen;
        event EventHandler AfterPost;
        event OnGetSqlString GetSqlString;

        void DeleteFinish(object entity);
        string GetSql(SqlTypes sqlType, object entity);
        void PostFinish(object entity);
        void CloseOpenDocFinish(object entity);
        void CloseOpen();
    }

}
