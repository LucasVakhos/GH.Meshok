using GH.AppContext;
using GH.Components.Attributes;
using GH.NHibernate;
using GH.UserExceptions;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace GH.Configs
{
    public delegate void GetBaseUserEvent(ref BaseUser user);

    public class CfgCoreConnection : CfgCore
    {
        protected override void LoadDefauls()
        {
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(this, false))
            {
                if (property.Attributes[typeof(DbConnectionProperty)] is DbConnectionProperty att)
                    Default(property, att.Default);
            }
        }

        public bool IsComplete
        {
            get
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(this, new Attribute[1] { new UpdatablePropertyAttribute() }))
                    if (property.Name == nameof(UserLogin) || property.Name == nameof(UserPassword) || property.Name == nameof(AutoEntering))
                        continue;
                    else
                    if (property.GetValue(this) == null)
                        return false;


                return true;
            }
        }

        public void CheckIdentity()
        {
            if (UserLogin != User.Login || UserPassword != User.Password)
            {
                UserLogin = User.Login;
                UserPassword = User.Password;
                Save();
            }
        }

        [DataMember]
        [DbConnectionProperty(Category = Category.User, Caption = "Login", ToolTip = "Логин", EditorType = EditorType.Combo)]
        public virtual string UserLogin { get; set; }

        [DataMember]
        [DbConnectionProperty(Category = Category.User, Caption = "Password", ToolTip = "Пароль")]
        public virtual string UserPassword { get; set; }

        [DataMember]
        [DbConnectionProperty(Category = Category.User, Caption = "Auto entering", ToolTip = "Автовход если доступ разрешён", Default = false)]
        public bool AutoEntering { get; set; } = false;

        private BaseUser _user = null;

        protected BaseUser User 
        {
            get
            {
                if (_user == null || _user.Login != UserLogin || _user.Password != UserPassword)
                {
                    GetBaseUser?.Invoke(ref _user);
                }
                return _user;
            }
            set => _user = value;
        }

        public BaseUser GetUser()
        {
            return User;
        }

        public event GetBaseUserEvent GetBaseUser;

        public bool UserIsValid => User != null && User.Login == UserLogin && User.Password == UserPassword;

        protected CfgConnectFrame Frame { get; set; }

        public virtual string ConnectionString()
        {
            throw new NotImplemented(nameof(ConnectionString), this);
        }

        public virtual bool TestConnection()
        {
            throw new NotImplemented(nameof(TestConnection), this);
        }

        public bool ConnectIsOK()
        {
            CfgForm connectForm = RunContext.GetConnectForm();
            if (connectForm == null)
                return true;

            using (connectForm)
            {
                if (IsComplete && TestConnection())
                    return true;

                return connectForm.ShowDialog() == DialogResult.OK;
            }
        }

        public virtual bool IsRemoteDataBase()
        {
            throw new NotImplemented(nameof(IsRemoteDataBase), this);
        }

        public virtual string GetRemoteServer()
        {
            throw new NotImplemented(nameof(GetRemoteServer), this);
        }

        internal void SetFrame(CfgConnectFrame cfgConnectFrame)
        {
            Frame = cfgConnectFrame;
        }
    }


}
