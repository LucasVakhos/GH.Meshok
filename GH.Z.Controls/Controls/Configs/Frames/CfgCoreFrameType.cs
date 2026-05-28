using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;

namespace GH.Configs
{

    public class CfgCoreFrameType<T> : CfgCoreFrame where T: CfgCore
    {

        public CfgCoreFrameType()
        {
            dataSource.DataSource = typeof(T);
            if (!IsDesignMode)
            {
                dataSource.OnOpen += DataSource_OnOpen;
                dataSource.Open();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //if (!IsDesignMode)
            //{
            //    dataSource.OnOpen += DataSource_OnOpen;
            //    dataSource.Open();
            //}
            
        }

        private void DataSource_OnOpen(out IList list)
        {
            list = new List<T>();
            list.Add(IniHelper.CoreCfg<T>());
        }

    }
}
