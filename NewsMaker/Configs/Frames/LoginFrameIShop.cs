using GH.Configs;
using GH.NHibernate;
using MeshokBrowser.NHibernate;
using System.Collections;
using System.Collections.Generic;
namespace MeshokBrowser
{
    public class LoginFrameIShop : LoginFrameType<CfgIShop, User>
    {
        protected override IList GetAllUsers()
        {
            INHRepository repository = new NHRepository<User>();
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
        private void InitializeComponent()
        {
        }
    }
}
