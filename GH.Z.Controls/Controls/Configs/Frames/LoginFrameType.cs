using GH.Components;
using GH.Controls;
using GH.NHibernate;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using GH.UserExceptions;

namespace GH.Configs
{

    public class LoginFrameType<TConfig, TUser> : LoginFrame 
        where TConfig : CfgCoreConnection 
        where TUser : BaseUser
    {

        private TConfig _config; 
        private IList<TUser> _users = null;
        protected virtual IList<TUser> Users { get => _users; set => _users = value; }

        [GHProperty]
        public LoginInputType LoginInputType { get; set; }

        public LoginFrameType()
        {
            FinalInitialize();
        }

        private void FinalInitialize()
        {
            if (!IsDesignMode)
            {
                //_config = GetCfg() as TConfig;
                _config = IniHelper.Cfg<TConfig>();

                //dataSource.DataSource = _config.GetType();
                dataSource.DataSource = typeof(TConfig);

                dataSource.OnOpen += DataSource_OnOpen;
                Users = GetAllUsers() as IList<TUser>;
                _config.GetBaseUser += LoginFrameType_GetBaseUser;

            }
        }

        //protected virtual CfgCoreConnection GetCfg()
        //{
        //    throw new NotImplemented(nameof(GetCfg), this);
        //    //return IniHelper.Cfg<TConfig>();
        //}

        protected virtual IList GetAllUsers()
        {
            return (new NHRepository<TUser>() as INHRepository).SelectAll();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsDesignMode)
            {
                if (LoginInputType == LoginInputType.AsSelectFromCombo)
                {
                    SuspendLayout();
                    try
                    {
                        userLogin.Properties.Buttons.AddRange(
                            new DevExpress.XtraEditors.Controls.EditorButton[] {
                            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
                            }
                        );

                        userLogin.Properties.Items.AddRange(
                            Users.Select(u => u.Name).ToArray()
                            );

                    }
                    finally
                    {
                        ResumeLayout(true);
                    }
                }
                dataSource.Open();
            }
        }

        private void LoginFrameType_GetBaseUser(ref BaseUser user)
        {
           user = Users.Where(x => x.Login == _config.UserLogin && x.Password == _config.UserPassword).FirstOrDefault();
        }

        private void DataSource_OnOpen(out IList list)
        {
            list = new List<TConfig>();
            list.Add(_config);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _config.GetBaseUser -= LoginFrameType_GetBaseUser;
                _config = null;
            }
            base.Dispose(disposing);
        }
    }
}
