using GH.Components;
using MeshokBrowser.Frames;
using System;
using System.Collections.Generic;
namespace MeshokBrowser
{
    public partial class BaseSetting<T> : SettingFrame where T : PrivateSetting
    {
        public BaseSetting()
        {
            InitializeComponent();
            dataSource.DataSource = typeof(T);
        }
        private void dataSource_OnPost(object sender, EventArgs e)
        {
            (dataSource.Entity as PrivateConfig).SaveToIni();
        }
        private void dataSource_OnCancel(object sender, EventArgs e)
        {
            (dataSource.Entity as PrivateConfig).LoadFromIni();
        }
        private void dataSource_OnOpen(out System.Collections.IList list)
        {
            list = new List<T>();
            list.Add(ConfigMain.GetConfig<T>());
        }
}
}
