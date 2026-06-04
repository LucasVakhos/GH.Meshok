namespace GH.Components
{
    public partial class CfgCoreFrame : CfgBaseFrame
    {
        public CfgCoreFrame()
        {
            InitializeComponent();
        }
    private void dataSource_OnPost(object sender, System.EventArgs e)
        {
            if (dataSource.Current is CfgCore cfgCore)
            {
                cfgCore.Save();
            }
        }
    private void dataSource_OnCancel(object sender, System.EventArgs e)
        {
            if (dataSource.Current is CfgCore cfgCore)
            {
                cfgCore.Load();
            }
        }
    }
}
