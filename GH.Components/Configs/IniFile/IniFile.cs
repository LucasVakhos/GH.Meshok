namespace GH.Components
{
    public class IniFile : Dictionary<string, CfgCore>
    {
        public IniFile()
        {
        }
    public void SaveAll()
        {
            foreach (KeyValuePair<string, CfgCore> instance in this)
                instance.Value.Save();
        }
    public void AddInstance(CfgCore instanse)
        {
            if (instanse is CfgApp cfgApp)
            {
                this[nameof(CfgApp)] = cfgApp;
                cfgApp.Form = RunContext.AppMainForm;
            }
            else
                if (!ContainsValue(instanse))
                {
                    this[instanse.GetName()] = instanse;
                }
        }
    }
}
