using System;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraEditors;
using GH.Components.Attributes;
using GH.Interfaces;
using GH.NHibernate;
using GH.UserExceptions;
using MySql.Data.MySqlClient;

namespace GH.Configs
{
    public partial class MySqlConnectionFrame : CfgConnectFrameType<CfgMySqlConnection>//, ISupportEditorChange
    {
        public MySqlConnectionFrame()
        {
        }

        //protected override string[] GetExceptFields()
        //{
        //    return new string[] { nameof(CfgMySqlConnection.AutoEntering), nameof(CfgMySqlConnection.UserLogin), nameof(CfgMySqlConnection.UserPassword) };
        //}

        //public void EditorChanged(object sender)
        //{
        //    CfgMySqlConnection cfgFb = Cfg as CfgMySqlConnection;
        //    BaseEdit ctrl = sender as BaseEdit;
        //    switch (ctrl.DataBindings[0].BindingMemberInfo.BindingField)
        //    {
        //        case nameof(CfgMySqlConnection.Server):
        //            if (ctrl.Text == "localhost")
        //                cfgFb.Remote = false;
        //            break;
        //        case nameof(CfgMySqlConnection.Remote):
        //            if (ctrl is CheckEdit check)
        //            {
        //                BaseEdit edit = GetControl<BaseEdit>(Field.ctrlPrefix + nameof(CfgMySqlConnection.Server));
        //                edit.ReadOnly = !check.Checked;
        //                if (edit.ReadOnly)
        //                    edit.Text = "localhost";
        //                else
        //                if (edit.Text == "localhost")
        //                    cfgFb.Server = (string)Cfg.GetDefault(nameof(CfgMySqlConnection.Server));
        //            }
        //            break;
        //        default:
        //            break;
        //    }
    }

}
