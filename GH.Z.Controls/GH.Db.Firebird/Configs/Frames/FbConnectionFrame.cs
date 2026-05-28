using DevExpress.XtraEditors;
using FirebirdSql.Data.FirebirdClient;
using GH.Interfaces;
using GH.NHibernate;
using System.Linq;


namespace GH.Configs
{
    public partial class FbConnectionFrame : CfgConnectFrameType<CfgFbConnection> //, ISupportEditorChange
    {
        public FbConnectionFrame()
        {

        }

        //public void EditorChanged(object sender)
        //{
        //    CfgFbConnection cfgFb = Cfg as CfgFbConnection;
        //    BaseEdit ctrl = sender as BaseEdit;
        //    switch (ctrl.DataBindings[0].BindingMemberInfo.BindingField)
        //    {
        //        case nameof(CfgFbConnection.DataSource):
        //            ctrl = GetControl<BaseEdit>(Field.ctrlPrefix + nameof(CfgFbConnection.Remote));
        //            if (cfgFb.DataSource == "localhost")
        //                cfgFb.Remote = false;
        //            break;
        //        case nameof(CfgFbConnection.Remote):
        //            ctrl = GetControl<BaseEdit>(Field.ctrlPrefix + nameof(CfgFbConnection.DataSource));
        //            ctrl.ReadOnly = !cfgFb.Remote;
        //            if (!cfgFb.Remote)
        //                cfgFb.DataSource = "localhost";
        //            break;
        //        default:
        //            break;
        //    }
        //}

    }
}
