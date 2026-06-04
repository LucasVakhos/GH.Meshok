using NHibernate;
using NHibernate.Criterion;
using System.Collections;
using System.Reflection;
namespace GH.Components
{
    public enum SqlTypes
    {
        SelectSql,
        InsertSql,
        UpdateSql,
        SaveOrUpdateSql,
        CloseDocSql,
        DeleteSql,
        RefreshSql,
        ExecuteSql
    }

    public class NHRepositorySetting
    {
        static int _pageSize = 10000;
    public static int PageSize { get => _pageSize; set => _pageSize = value; }

    private int _pageNumber = 1;
    public int PageNumber { get => _pageNumber; set => _pageNumber = value; }
    }

    public class NHRepository<T> : NHRepositorySetting, INHRepository where T : BaseEntity
    {
        protected DbServerType DbServerType;
    public NHRepository()
        {
            DbServerType = NHHelper.BaseCriator.DbServerType;
        }
        Type INHRepository.ConcreteType => ConcreteType;
    private Type ConcreteType => typeof(T);
        Func<SqlTypes, BaseEntity, string> INHRepository.GetSQL { get => GetSQL; set => GetSQL = value; }

    private Func<SqlTypes, BaseEntity, string> GetSQL { get; set; } = null;
        Action<object> INHRepository.PostFinish { get => PostFinish; set => PostFinish = value; }

    public Action<object> PostFinish { get; set; } = null;
    private void FinishPost(T entity)
        {
            PostFinish?.Invoke(entity);
        }
        Action<BaseEntity> INHRepository.DeleteFinish { get => DeleteFinish; set => DeleteFinish = value; }

    private Action<BaseEntity> DeleteFinish { get; set; } = null;
    private void FinishDelete(T entity)
        {
            DeleteFinish?.Invoke(entity);
        }
        bool INHRepository.RefreshAfterPost { get => NeedRefresh; set => NeedRefresh = value; }

    private bool NeedRefresh { get; set; }
    private bool Animate(SqlTypes sqlType)
        {
            switch (sqlType)
            {
                case SqlTypes.SelectSql:
                    return NeedLoadingAnimate;
                case SqlTypes.InsertSql:
                    break;
                case SqlTypes.UpdateSql:
                    break;
                case SqlTypes.DeleteSql:
                    break;
                case SqlTypes.RefreshSql:
                    break;
                case SqlTypes.CloseDocSql:
                    return true;
                default:
                    break;
            }
            return false;
        }
        bool INHRepository.NeedLoadingAnimate { get => NeedLoadingAnimate; set => NeedLoadingAnimate = value; }

    private bool NeedLoadingAnimate { get; set; }
        Control INHRepository.Control { get => Control; set => Control = value; }

    private Control Control { get; set; }
        KeyValuePair<int, string>[] INHRepository.KeyIntLookupList()
        {
            return ((IList<T>)InnerSelectAll()).ToDictionary(x => x.id, x => x.Name).ToArray();
        }
        KeyValuePair<BaseEntity, string>[] INHRepository.KeyEntityLookupList()
        {
            return ((IList<T>)InnerSelectAll()).ToDictionary(x => (BaseEntity)x, x => x.Name).ToArray();
        }
    private ISession OpenSession()
        {
            return NHHelper.OpenSession(DbServerType);
        }
    private string GetDeleteSql(T entity)
        {
            if (GetSQL != null)
                return GetSQL.Invoke(SqlTypes.DeleteSql, entity);
            return null;
        }
        void INHRepository.Delete(object entity)
        {
            Delete((T)entity);
        }
    private void Delete(T entity)
        {
            UpdateProcessor processor = new UpdateProcessor(
                SqlTypes.DeleteSql,
                Control,
                Animate(SqlTypes.DeleteSql),
                () =>
                {
                    string sql = GetDeleteSql(entity);
                    using (ISession session = OpenSession())
                    {
                        if (session != null)
                        {
                            try
                            {
                                if (sql == null)
                                    session.Delete(entity);
                                else
                                    session.CreateSQLQuery(sql).ExecuteUpdate();
                                session.GetCurrentTransaction().Commit();
                            }
                            catch (Exception ex)
                            {
                                Logger.Error("NHRepository, Delete", ex);
                            }
                        }
                    }
                },
                () =>
                {
                    FinishDelete(entity);
                });
            processor.Execute();
        }
    public void DeleteAll(ICollection<T> entitys)
        {
            UpdateProcessor processor = new UpdateProcessor(
                SqlTypes.DeleteSql,
                Control,
                Animate(SqlTypes.DeleteSql),
                () =>
                {
                    using (ISession session = OpenSession())
                    {
                        try
                        {
                            foreach (T entity in entitys)
                            {
                                string sql = GetDeleteSql(entity);
                                if (sql == null)
                                    session.Delete(entity);
                                else
                                    session.CreateSQLQuery(sql).ExecuteUpdate();
                            }
                            session.GetCurrentTransaction().Commit();
                        }
                        catch (Exception ex)
                        {
                            Logger.Error("NHRepository, DeleteAll", ex);
                        }
                    }
                },
                () =>
                {
                    foreach (object entity in entitys)
                        FinishDelete((T)entity);
                });
            processor.Execute();
        }
        void INHRepository.ExequteQuery(string[] sql)
        {
            if (sql.Length == 1)
            {
                ExequteQuery(sql[0]);
                return;
            }
            UpdateProcessor processor = new UpdateProcessor(
                SqlTypes.ExecuteSql,
                Control,
                Animate(SqlTypes.ExecuteSql),
                () =>
                {
                    using (ISession session = OpenSession())
                    {
                        try
                        {
                            foreach (string item in sql)
                                session.CreateSQLQuery(item).ExecuteUpdate();
                        }
                        catch (Exception ex)
                        {
                            Logger.Error("NHRepository, ExequteQuery", ex);
                        }
                        session.GetCurrentTransaction().Commit();
                    }
                });
            processor.Execute();
        }
    private void ExequteQuery(string execSQL)
        {
            UpdateProcessor processor = new UpdateProcessor(
                SqlTypes.ExecuteSql,
                Control,
                Animate(SqlTypes.ExecuteSql),
                () =>
                {
                    using (ISession session = OpenSession())
                    {
                        try
                        {
                            session.CreateSQLQuery(execSQL).ExecuteUpdate();
                            session.GetCurrentTransaction().Commit();
                        }
                        catch (Exception ex)
                        {
                            Logger.Error("NHRepository, ExequteQuery", ex);
                        }
                    }
                });
            processor.Execute();
        }
    public BaseEntity Get(object id)
        {
            BaseEntity bindable = null;
            if (Internet.CheckConnectionForDatabase())
            {
                using (ISession session = OpenSession())
                {
                    try
                    {
                        bindable = session.Get<T>(id);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("NHRepository, Get", ex);
                    }
                }
            }
            return bindable;
        }
    private string GetInsertSql(T entity)
        {
            if (GetSQL != null)
                return GetSQL.Invoke(SqlTypes.InsertSql, entity);
            return null;
        }
        void INHRepository.Save(object entity)
        {
            Save((T)entity);
        }
    private void Save(T entity)
        {
            UpdateProcessor processor = new UpdateProcessor(
                SqlTypes.InsertSql,
                Control,
                Animate(SqlTypes.InsertSql),
                () =>
                {
                    string sql = GetInsertSql(entity);
                    using (ISession session = OpenSession())
                    {
                        if (session != null)
                        {
                            try
                            {
                                object obj = null;
                                if (sql == null)
                                {
                                    session.Save(entity);
                                }
                                else
                                    obj = session.CreateSQLQuery(sql).AddEntity(typeof(T)).UniqueResult<T>();
                                session.GetCurrentTransaction().Commit();
                                if (obj == null)
                                    Refresh(entity);
                                else
                                    entity.Assigne(obj);
                            }
                            catch (Exception ex)
                            {
                                Logger.Error("NHRepository, Save", ex);
                            }
                        }
                    }
                },
                () =>
                {
                    FinishPost(entity);
                });
            processor.Execute();
        }
    private string GetUpdateSql(T entity)
        {
            if (GetSQL != null)
                return GetSQL.Invoke(SqlTypes.UpdateSql, entity);
            return null;
        }
        void INHRepository.SaveOrUpdate(object entity)
        {
            Update((T)entity);
        }
    private void Update(T entity)
        {
            UpdateProcessor processor = new UpdateProcessor(
                SqlTypes.UpdateSql,
                Control,
                Animate(SqlTypes.UpdateSql),
                () =>
                {
                    string sql = GetUpdateSql(entity);
                    using (ISession session = OpenSession())
                    {
                        if (session != null)
                        {
                            try
                            {
                                if (sql == null)
                                    session.Update(entity);
                                else
                                    session.CreateSQLQuery(sql).ExecuteUpdate();
                                session.GetCurrentTransaction().Commit();
                                if (NeedRefresh)
                                    Refresh(entity);
                            }
                            catch (Exception ex)
                            {
                                Logger.Error("NHRepository, Update", ex);
                            }
                        }
                    }
                },
                () =>
                {
                    FinishPost(entity);
                });
            processor.Execute();
        }
    private string GetSaveOrUpdate(T entity)
        {
            if (GetSQL != null)
                return GetSQL.Invoke(SqlTypes.SaveOrUpdateSql, entity);
            return null;
        }
    public void SaveOrUpdate(T entity)
        {
            UpdateProcessor processor = new UpdateProcessor(
                SqlTypes.SaveOrUpdateSql,
                Control,
                Animate(SqlTypes.SaveOrUpdateSql),
                () =>
                {
                    string sql = GetSaveOrUpdate(entity);
                    using (ISession session = OpenSession())
                    {
                        if (session != null)
                        {
                            try
                            {
                                if (sql == null)
                                    session.SaveOrUpdate(entity);
                                else
                                {
                                    T query = session.CreateSQLQuery(sql).AddEntity(typeof(T)).UniqueResult<T>();
                                    entity.id = query.id;
                                    //entity.Assigne(query);
                                }
                                //.AddEntity(typeof(T));.ExecuteUpdate();
                                //session.Transaction.Commit();
                                //if (NeedRefresh)
                                //    Refresh(entity);
                            }
                            catch (Exception ex)
                            {
                                Logger.Error("NHRepository, SaveOrUpdate", ex);
                            }
                        }
                    }
                },
                () =>
                {
                    FinishPost(entity);
                });
            processor.Execute();
        }
        Action<object> INHRepository.CloseOpenDocFinish { get => CloseOpenDocFinish; set => CloseOpenDocFinish = value; }

    private Action<object> CloseOpenDocFinish { get; set; } = null;
    private void DoCloseOpenDocFinish(T entity)
        {
            CloseOpenDocFinish?.Invoke(entity);
        }
    private string GetCloseOpenDoc(T entity)
        {
            if (GetSQL != null)
                return GetSQL.Invoke(SqlTypes.CloseDocSql, entity);
            return null;
        }
        void INHRepository.CloseOpenDoc(object entity)
        {
            CloseOpenDoc((T)entity);
        }
    private void CloseOpenDoc(T entity)
        {
            UpdateProcessor processor = new UpdateProcessor(
                SqlTypes.CloseDocSql,
                Control,
                Animate(SqlTypes.CloseDocSql),
                () =>
                {
                    string sql = GetCloseOpenDoc(entity);
                    using (ISession session = OpenSession())
                    {
                        if (session != null)
                        {
                            try
                            {
                                if (sql == null)
                                    session.Update(entity);
                                else
                                    session.CreateSQLQuery(sql).ExecuteUpdate();
                                session.GetCurrentTransaction().Commit();
                                if (NeedRefresh)
                                {
                                    session.GetCurrentTransaction().Begin();
                                    session.Refresh(entity);
                                }
                                //entity.Assigne(session.Get(typeof(T), entity.id));
                            }
                            catch (Exception ex)
                            {
                                Logger.Error("NHRepository, CloseOpenDoc", ex);
                            }
                        }
                    }
                },
                () =>
                {
                    DoCloseOpenDocFinish(entity);
                });
            processor.Execute();
        }
    public void UpdateAll(ICollection<T> entitys)
        {
            UpdateProcessor processor = new UpdateProcessor(
                SqlTypes.UpdateSql,
                Control,
                Animate(SqlTypes.UpdateSql),
                () =>
                {
                    using (ISession session = OpenSession())
                    {
                        if (session != null)
                        {
                            try
                            {
                                foreach (T entity in entitys)
                                {
                                    string sql = GetUpdateSql(entity);
                                    if (sql == null)
                                        session.Update(entity);
                                    else
                                        session.CreateSQLQuery(sql).ExecuteUpdate();
                                }
                                session.GetCurrentTransaction().Commit();
                            }
                            catch (Exception ex)
                            {
                                Logger.Error("NHRepository, UpdateAll", ex);
                            }
                        }
                    }
                },
                () =>
                {
                    foreach (object entity in entitys)
                        FinishPost((T)entity);
                });
            processor.Execute();
        }
    private string GetResreshSql(T entity)
        {
            if (GetSQL != null)
                return GetSQL.Invoke(SqlTypes.RefreshSql, entity);
            return null;
        }
        void INHRepository.Refresh(object entity)
        {
            Refresh((T)entity);
        }
    private void Refresh(T entity)
        {
            if (!Internet.CheckConnectionForDatabase())
                return;
            // Важно!!! не запускать в процессор
            string sql = GetResreshSql(entity);
            using (ISession session = OpenSession())
            {
                if (session != null)
                {
                    try
                    {
                        if (sql == null)
                            session.Refresh(entity);
                        else
                            entity.Assigne(session.CreateSQLQuery(sql).AddEntity(typeof(T)).UniqueResult<T>());
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("NHRepository, Refresh", ex);
                    }
                }
            }
        }
    public void Replicate(T entity)
        {
            if (!Internet.CheckConnectionForDatabase())
                return;
            using (ISession session = OpenSession())
            {
                if (session != null)
                {
                    try
                    {
                        session.Replicate(entity, ReplicationMode.Overwrite);
                        session.GetCurrentTransaction().Commit();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("NHRepository, Replicate", ex);
                    }
                }
            }
        }
    private string SelectSql()
        {
            if (GetSQL != null)
                return GetSQL.Invoke(SqlTypes.SelectSql, null);
            return null;
        }
        Func<Dictionary<string, bool>> INHRepository.GetSorting { get => GetSorting; set => GetSorting = value; }

    private Func<Dictionary<string, bool>> GetSorting { get; set; }

    private Dictionary<string, bool> Sorting()
        {
            if (GetSorting != null)
                return GetSorting.Invoke();
            return null;
        }
        Func<Dictionary<string, object>> INHRepository.GetParams { get => GetParams; set => GetParams = value; }

    private Func<Dictionary<string, object>> GetParams { get; set; } = null;
    private Dictionary<string, object> Params()
        {
            if (GetParams != null)
                return GetParams.Invoke();
            return null;
        }
    public IList<T> AsTypeList()
        {
            var lst = InnerSelectAll();
            return lst;
        }
    private IList<T> InnerSelectAll()
        {
            string sql = SelectSql();
            List<T> lst = null;
            Dictionary<string, bool> sort = Sorting();
            Dictionary<string, object> pars = Params();
            try
            {
                using (ISession session = OpenSession())
                {
                    if (session != null)
                    {
                        try
                        {
                            if (sql == null)
                            {
                                ICriteria data = session.CreateCriteria<T>();
                                if (pars != null)
                                {
                                    foreach (KeyValuePair<string, object> item in pars)
                                        if (item.Value == null)
                                            data.Add(Expression.IsNull(item.Key));
                                        else
                                            data.Add(Expression.Eq(item.Key, item.Value));
                                }
                                //data.SetFirstResult((NHRepository.PageNumber - 1) * NHRepository.PageSize);
                                //data.SetMaxResults(pageSize);
                                if (sort != null)
                                    foreach (KeyValuePair<string, bool> item in sort)
                                        data.AddOrder(new Order(item.Key, item.Value));
                                lst = data.List<T>().ToList();
                            }
                            else
                            {
                                ISQLQuery query = session.CreateSQLQuery(sql).AddEntity(typeof(T));
                                if (pars != null)
                                {
                                    foreach (KeyValuePair<string, object> item in pars)
                                        query.SetParameter(item.Key, item.Value);
                                }
                                lst = query.List<T>().ToList();
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error("NHRepository, InnerSelectAll", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal("NHRepository InnerSelectAll", ex);
            }
            return lst ?? new List<T>();
        }
    private IList<T> InnerSelectAllAsync()
        {
            IList<T> lst = null;
            UpdateProcessor processor = new UpdateProcessor(
                SqlTypes.SelectSql,
                Control,
                Animate(SqlTypes.SelectSql),
                () => { lst = InnerSelectAll(); },
                null);
            processor.Execute();
            return lst ?? new List<T>();
        }
        IList INHRepository.SelectAll()
        {
            if (!Internet.CheckConnectionForDatabase())
                return new List<T>();
            IList<T> lst = InnerSelectAllAsync();
            return lst.ToList();
        }
        BaseEntity Select()
        {
            string sql = SelectSql();
            BaseEntity entity = null;
            Dictionary<string, object> pars = Params();
            try
            {
                using (ISession session = OpenSession())
                {
                    try
                    {
                        if (sql == null)
                        {
                            ICriteria data = session.CreateCriteria<T>();
                            if (pars != null)
                            {
                                foreach (KeyValuePair<string, object> item in pars)
                                    if (item.Value == null)
                                        data.Add(Expression.IsNull(item.Key));
                                    else
                                        data.Add(Expression.Eq(item.Key, item.Value));
                            }
                            entity = data.List<T>().FirstOrDefault();
                        }
                        else
                        {
                            entity = session.CreateSQLQuery(sql).AddEntity(typeof(T)).List<T>().FirstOrDefault();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("NHRepository, Select()", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal("NHRepository Select()", ex);
            }
            return entity;
        }
        BaseEntity INHRepository.SelectOne()
        {
            return Select();
        }
        BaseEntity INHRepository.SelectFormProcedure(BaseEntity entity, string sql)
        {
            return SelectFormProcedure(entity, sql);
        }
    private BaseEntity SelectFormProcedure(BaseEntity entity, string sql)
        {
            BaseEntity res = null;
            try
            {
                using (ISession session = OpenSession())
                {
                    try
                    {
                        IQuery q = session.CreateSQLQuery(sql).AddEntity(typeof(T));
                        SetParams(entity, q);
                        res = q.UniqueResult<T>();
                        session.GetCurrentTransaction().Commit();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("NHRepository", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal("NHRepository", ex);
            }
            return res;
        }
    private static void SetParams(BaseEntity entity, IQuery q)
        {
            if (q.NamedParameters.Length == 0)
                return;
            foreach (string par in q.NamedParameters)
            {
                PropertyInfo info = AbstractEntity.GetProperty(entity, par);
                object value = info.GetValue(entity);
                if (value == null)
                {
                    Type type = GetPureType(info);
                    if (type == typeof(int))
                        q.SetParameter(par, value, NHibernateUtil.Int32);
                    else
                        if (type == typeof(decimal))
                            q.SetParameter(par, value, NHibernateUtil.Decimal);
                        else
                            if (type == typeof(double))
                                q.SetParameter(par, value, NHibernateUtil.Double);
                            else
                                if (type == typeof(DateTime))
                                    q.SetParameter(par, value, NHibernateUtil.DateTime);
                                else
                                    if (type == typeof(string))
                                        q.SetParameter(par, value, NHibernateUtil.String);
                                    else
                                        if (type == typeof(bool))
                                            q.SetParameter(par, value, NHibernateUtil.Boolean);
                                        else
                                            q.SetParameter(par, value);
                }
                else
                    q.SetParameter(par, value);
            }
        }
    private static Type GetPureType(PropertyInfo info)
        {
            if (info.PropertyType.IsGenericType)
            {
                Type[] types = info.PropertyType.GetGenericArguments();
                if (types.Length == 1)
                    return types[0];
            }
            return info.PropertyType;
        }
    }
}
