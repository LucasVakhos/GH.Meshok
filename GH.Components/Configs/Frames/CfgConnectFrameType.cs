using DevExpress.XtraLayout;
using System.Collections;
using static GH.Components.LayoutHelper;
namespace GH.Components
{
    public class CfgConnectFrameType<T> : CfgConnectFrame where T : CfgCoreConnection
    {
        //public const string ctrlPrefix = "edit";
        private T _cfg;
        protected T Cfg
        {
            get => _cfg;
            set
            {
                _cfg = value;
                if (_cfg != null)
                    _cfg.SetFrame(this as CfgConnectFrame);
            }
        }
        public CfgConnectFrameType()
        {
            //Cfg = GetCfg() as T;
            Cfg = IniHelper.Cfg<T>();
            //dataSource.DataSource = Cfg.GetType();
            dataSource.DataSource = typeof(T);
            InitEditGroup();
        }
        //protected virtual CfgCoreConnection GetCfg()
        //{
        //    throw new NotImplemented(nameof(GetCfg), this);
        //    //return IniHelper.Cfg<T>();
        //}
        protected Field[] GetFields()
        {
            return Cfg.GetFields();
        }
        public T1 GetControl<T1>(string name) where T1 : Control
        {
            T1 val = layoutControl.Controls.Find(name, false).OfType<T1>().FirstOrDefault() as T1;
            return val;
        }
        protected void InitEditGroup()
        {
            if (IsDesignMode)
                return;
            CreateLayoutGroup<T, DbConnectionProperty>(EditGroup, GetExceptFields());
            GH.Components.ConnectButton connectButton = new GH.Components.ConnectButton();
            LayoutControlItem lcConnect = new LayoutControlItem();
            EditGroup.AddItem(lcConnect);
            actionList.SetAction(connectButton, null);
            lcConnect.Control = connectButton;
            lcConnect.Name = "lcConnect";
            lcConnect.TextSize = new System.Drawing.Size(0, 0);
            lcConnect.TextVisible = false;
            layoutControl.Controls.Add(connectButton);
            dataSource.AddSaveSaveCancelPanel();
            Align(EditGroup);
        }
        protected /*virtual*/ string[] GetExceptFields()
        {
            return new string[] { nameof(CfgCoreConnection.AutoEntering), nameof(CfgCoreConnection.UserLogin), nameof(CfgCoreConnection.UserPassword) };
            //throw new NotImplemented(nameof(GetExceptFields), this);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsDesignMode)
            {
                dataSource.OnOpen += DataSource_OnOpen;
                dataSource.Open();
            }
        }
        private void DataSource_OnOpen(out IList list)
        {
            list = new List<T>();
            list.Add(Cfg);
        }
    }
}
