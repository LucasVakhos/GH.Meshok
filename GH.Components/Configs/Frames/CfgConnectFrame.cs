namespace GH.Components
{
    public partial class CfgConnectFrame : CfgBaseFrame
    {
        public CfgConnectFrame()
        {
            InitializeComponent();
        }
        private void dataSource_OnPost(object sender, System.EventArgs e)
        {
            Save();
        }
        private void dataSource_OnCancel(object sender, System.EventArgs e)
        {
            if (dataSource.Current is CfgCore cfgCore)
            {
                cfgCore.CancelEdit();
            }
        }
    }
}
