using GH.Configs;
using GH.NHibernate;
using System.Collections;
using System.Collections.Generic;

namespace GH.Configs
{
    public class FbLoginFrame<TUser> : LoginFrameType<CfgFbConnection, TUser>
        where TUser : BaseUser
    {
        public FbLoginFrame()
        {
            LoginInputType = GH.Controls.LoginInputType.AsSelectFromCombo;
        }

        protected override IList GetAllUsers()
        {
            INHRepository repository = new NHRepository<TUser>();
            repository.GetParams = () =>
            {
                Dictionary<string, object> valuePairs = new Dictionary<string, object>();
                valuePairs.Add("Active", true);
                return valuePairs;
            };

            repository.GetSorting = () =>
            {
                Dictionary<string, bool> valuePairs = new Dictionary<string, bool>();
                valuePairs.Add("Name", true);
                return valuePairs;
            };

            return repository.SelectAll();
        }
    }
}

