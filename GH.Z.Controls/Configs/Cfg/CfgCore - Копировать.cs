using GH.AppContext;
using GH.NHibernate;
using GH.UserExceptions;
using GH.Utils;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace GH.Configs
{
    public class CfgCore : AbstractEntity
    {
        #region поля и свойства
        internal static bool _loading = false;

        #endregion

        internal string ConfigPath
        {
            get
            {
                if (this is CfgApp cfgApp)
                    return Path.ChangeExtension(RunContext.ExeFullName, ".ini");
                return Path.Combine(RunContext.ConfigsPath, GetName() + ".ini");
            }
        }


        public virtual string GetName()
        {
            return GetType().Name;
        }

        public CfgCore()
        {
            IniHelper.IniFile.TryGetValue(this.GetName(), out CfgCore cfg);
            if (cfg == null)
            {
                IniHelper.IniFile.AddInstance(this);
                Load();
            }
            else
                Assigne(cfg);
        }

        #region методы


        public void Load()
        {
            if (_loading)
                return;

            CfgCore cfg = null;

            FileInfo _fileInfo = new FileInfo(ConfigPath);
            if (!_fileInfo.Exists)
            {
                Save(true);
                return;
            }

            _loading = true;
            Type type = GetType();

            DataContractJsonSerializer _formatter = new DataContractJsonSerializer(type);

            try
            {
                using (FileStream fs = new FileStream(_fileInfo.FullName, FileMode.OpenOrCreate))
                {

                    cfg = _formatter.ReadObject(fs) as CfgCore;
                    Assigne(cfg);
                }
            }
            catch (Exception ex)
            {

                Logger.Fatal(ex);
                Save();
            }
            _loading = false;
        }

        

        protected virtual void CreateSomething()
        {
            throw new NotImplemented(nameof(CreateSomething), this);
        }

        public void Save(bool anything = false)
        {
            if (!(anything || HasChanges))
                return;
            FileInfo _fileInfo = new FileInfo(ConfigPath);
            Type type = GetType();

            DataContractJsonSerializer _formatter = new DataContractJsonSerializer(type);
            Directory.CreateDirectory(Path.GetDirectoryName(_fileInfo.FullName));
            if (_fileInfo.Exists)
                File.Delete(_fileInfo.FullName);

            try
            {
                using (FileStream fs = new FileStream(_fileInfo.FullName, FileMode.OpenOrCreate))
                {
                    _formatter.WriteObject(fs, this);
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
            }

            EndEdit();
        }
        #endregion
    }
}
