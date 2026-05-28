using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GH.NHibernate
{
    public interface INHRepository
    {
        Type ConcreteType { get; }
        Func<SqlTypes, BaseEntity, string> GetSQL { get; set; }
        Action<object> PostFinish { get; set; }
        Action<BaseEntity> DeleteFinish { get; set; }
        Action<object> CloseOpenDocFinish { get; set; }
        Func<Dictionary<string, bool>> GetSorting { get; set; }
        Func<Dictionary<string, object>> GetParams { get; set; }
        Control Control { get; set; }
        bool RefreshAfterPost { get; set; }
        bool NeedLoadingAnimate { get; set; }

        KeyValuePair<int, string>[] KeyIntLookupList();
        KeyValuePair<BaseEntity, string>[] KeyEntityLookupList();

        BaseEntity Get(object id);
        void Save(object entity);
        void SaveOrUpdate(object entity);
        void Refresh(object entity);
        void Delete(object entity);
        void CloseOpenDoc(object entity);
        IList SelectAll();
        BaseEntity SelectOne();
        BaseEntity SelectFormProcedure(BaseEntity entity, string sql);
        void ExequteQuery(string[] sql);
    }
}