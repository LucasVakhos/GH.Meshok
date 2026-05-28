using GH.Configs;
using Microsoft.Win32;
using System;
using System.Reflection;
using System.Windows.Forms;
namespace NewsMaker
{
    public partial class CfgFrameProgram : CfgCoreFrameType<CfgProgram>
    {
        private const string _subKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Run\\";
        private readonly string appName = Application.ProductName;
        private readonly string exePath = Assembly.GetExecutingAssembly().Location + " -autorun";
        public CfgFrameProgram()
        {
            InitializeComponent();
            chkAutorun.Checked = GetAutorunValue();
            chkAutorun.CheckedChanged += chkAutorun_CheckedChanged;
        }
        private void chkAutorun_CheckedChanged(object sender, EventArgs e)
        {
            SetAutorunValue(chkAutorun.Checked);
        }
        private bool SetAutorunValue(bool autorun)
        {
            try
            {
                var reg = Registry.CurrentUser.CreateSubKey(_subKey);
                if (autorun)
                    reg.SetValue(appName, exePath);
                else
                    reg.DeleteValue(appName);
                reg.Flush();
                reg.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }
        private bool GetAutorunValue()
        {
            var autorun = false;
            try
            {
                var reg = Registry.CurrentUser.CreateSubKey(_subKey);
                var val = reg.GetValue(appName);
                autorun = val != null && val.ToString() == exePath;
                reg.Close();
            }
            catch
            {
            }
            return autorun;
        }
    }
}
