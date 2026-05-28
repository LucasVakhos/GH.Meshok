using GH.AppContext;
using System.Collections.Generic;

namespace GH.Configs
{
    public class IniFile : Dictionary<string, CfgCore>
    {
        public IniFile()
        {
        }

        #region методы

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
        #endregion
    }
}
