namespace GH.Configs
{
    public class LoginFormType<T> : CfgFormType<T> where T : CfgBaseFrame
    {
        protected override bool EnterEnabled()
        {
            return CfgFrame.dataSource.Current is CfgCoreConnection cfg && cfg.UserIsValid;
        }
    }
}
