#define old_version
using System;
using System.Windows.Forms;
using System.IO;
using MeshokBrowser.Workers;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking2010.Views;
using Gecko;
using Gecko.Events;
using Gecko.DOM;
using System.Linq;
using GH.Utils;
using GH.Configs;
using GH.Interfaces;
using GH.Components;
using GH.AppContext;
namespace MeshokBrowser
{
    public interface IMainForm : IAppForm
    {
        void AddPage(Control control, bool canClose = true);
        void RemovePage(Control control);
        void Proces_Begined();
        void Proces_Finished();
    }
    public partial class MainMeshok : MainForm, IMainForm
    {
        private const int total_try_count = 3;
        public static string sale_token = "sale";
        public static string deals_token = "deals";
        // public Bar MainMenu { get { return barMenu; } }
        private InnerWB mainBrowser;
        //
        Control ControlForRemove = null;
        bool auth_success = false;
        int try_count = 0;
        private AbstractWorker _processor;
        private MessagesSetting messageSettings = null;
        private MainConfigFrame mainSettings = null;
        CfgMeshok cfgMeshok;
        CfgIShop сfgIShop;
        public MainMeshok()
        {
            InitializeComponent();
            if (!IsDesignMode)
            {
                cfgMeshok = IniHelper.Cfg<CfgMeshok>();
                сfgIShop = IniHelper.Cfg<CfgIShop>();
                FbHelper.Init();
                InitMainBrowser();
                InitProcessors();
                tabbedView.DocumentClosing += TabbedView_DocumentClosing;
            }
        }
        private void TabbedView_DocumentClosing(object sender, DocumentCancelEventArgs e)
        {
            if (e.Document.Control is InnerWB)
            {
                e.Cancel = true;
                ProcessRunHelper.ProcScreen.Stop();
            }
            else if (e.Document.Control is BeginingForm)
            {
                e.Cancel = true;
                BeginingForm setting = e.Document.Control as BeginingForm;
                setting.DialogResult = false;
            }
            else if (e.Document.Control is BeginRelistLotsForm)
            {
                e.Cancel = true;
                BeginRelistLotsForm setting = e.Document.Control as BeginRelistLotsForm;
                setting.DialogResult = false;
            }
            else if (e.Document.Control is Workers.ScanSettingFrame)
            {
                e.Cancel = true;
                Workers.ScanSettingFrame setting = e.Document.Control as Workers.ScanSettingFrame;
                setting.Cancel();
            }
            else if (e.Document.Control == messageSettings && messageSettings != null)
            {
                messageSettings = null;
            }
            else if (e.Document.Control == mainSettings && mainSettings != null)
            {
                mainSettings = null;
            }
        }
        private bool BeginProcOrders()
        {
            if (!IniHelper.Cfg<CfgIShop>().TestConnection())
            {
                if (mainSettings == null)
                    actProgramSetting.DoExecute();
                mainSettings.ActiveIShop();
                DlgHelper.DlgWarning("Проверьте подключение к базе!");
                return false;
            }
            if (!Helpers.ScanSetting.Begin(this))
            {
                return false;
            }
            return true;
        }
        private bool BeginScanLots()
        {
            if (!IniHelper.Cfg<CfgBridgeNote>().TestConnection())
            {
                if (mainSettings == null)
                    actProgramSetting.DoExecute();
                mainSettings.ActiveBridgeNote();
                DlgHelper.DlgWarning("Проверьте подключение к сайту!");
                return false;
            }
            if (!WorkerRelistLots.Begin(this))
            {
                return false;
            }
            return true;
        }
        private bool CheckBridgenoteConnection()
        {
            if (!IniHelper.Cfg<CfgBridgeNote>().TestConnection())
            {
                if (mainSettings == null)
                    actProgramSetting.DoExecute();
                mainSettings.ActiveBridgeNote();
                DlgHelper.DlgWarning("Проверьте подключение к сайту!");
                return false;
            }
            return true;
        }
        private void InitMainBrowser()
        {
            SuspendLayout();
            mainBrowser = new MainWB();
            mainBrowser.Name = nameof(mainBrowser);
            mainBrowser.TabIndex = 0;
            mainBrowser.webBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;
            mainBrowser.Text = "Основной браузер";
            AddPage(mainBrowser, false);
            ResumeLayout(false);
        }
        private void WebBrowser_DocumentCompleted(object sender, GeckoDocumentCompletedEventArgs e)
        {
#if old_version
            if (!auth_success && e.Uri.OriginalString == cfgMeshok.Profile_Url && try_count < total_try_count)
            {
                Application.DoEvents();
                auth_success = RegistrationWasCompleted();
                if (!auth_success)
                {
                    LogIn();
                    try_count++;
                    if (try_count == total_try_count)
                        if (DlgHelper.DlgYesNo("Что-то ни так!" + Environment.NewLine + $"Не удалось зайти в ваш профиль с {try_count} попыток..." + Environment.NewLine +
                            "Попробовать ещё раз?"))
                            try_count = 0;
                }
                else
                {
                    CfgMeshok.RegCookie = mainBrowser.Document.Cookie;
                }
            }
#else
            if (!auth_success && e.Uri.OriginalString.Contains(cfgMeshok.Base_Url) && try_count < total_try_count)
            {
                Application.DoEvents();
                //auth_success = mainBrowser.Document.GetElementsByTagName("div").Any(div => div.Id == "desktop-profile-button");
                //string text = mainBrowser.Document.Body.InnerHtml;
                //auth_success = text.Contains("bridgenote1");
                auth_success = RegistrationWasCompleted();
                if (!auth_success)
                {
                    if (e.Uri.LocalPath == "/")
                    {
                        if (e.Uri.Query == "")
                            mainBrowser.Navigate("https://meshok.net/?_auth=1&_mode=login");
                        else
                        if (e.Uri.Query == "?_auth=1&_mode=login")
                        {
                            LogIn();
                            try_count++;
                            if (try_count == total_try_count)
                                if (DlgHelper.DlgYesNo("Что-то ни так!" + Environment.NewLine + $"Не удалось зайти в ваш профиль с {try_count} попыток..." + Environment.NewLine +
                                    "Попробовать ещё раз?"))
                                    try_count = 0;
                        }
                    }
                }
                else
                {
                    CfgMeshok.RegCookie = mainBrowser.Document.Cookie;
                    if (e.Uri.)
                    mainBrowser.Navigate(cfgMeshok.Profile_Url);
                }
            }
#endif
        }
        bool RegistrationWasCompleted()
        {
            GeckoDocument document = mainBrowser.Document;
            // новый вход
            if (document.GetElementsByTagName("div").Any(div => div.Id == "desktop-profile-button" && div.InnerHtml.Contains("bridgenote1")))
                return true;
            if (document.GetElementsByTagName("li").Any(li => li.Id == "uname" && li.InnerHtml.Contains("bridgenote1")))
                return true;
            return false;
        }
        void LogIn()
        {
            GeckoDocument document = mainBrowser.Document;
#if old_version
            GeckoFormElement frm = mainBrowser.Document.GetHtmlElementById("authForm") as GeckoFormElement;
            if (frm == null)
                return;
            bool need_submit = false;
            foreach (GeckoInputElement item in frm.GetElementsByTagName("input"))
            {
                if (item.Type == "email")
                {
                    if (item.Value != cfgMeshok.UserLogin)
                    {
                        item.Value = cfgMeshok.UserLogin;
                        need_submit = true;
                    }
                }
                else
                if (item.Type == "password")
                {
                    if (item.Value != cfgMeshok.UserPassword)
                    {
                        item.Value = cfgMeshok.UserPassword;
                        need_submit = true;
                    }
                }
                else
                if (item.Type == "checkbox" && item.Name == "dremember")
                {
                    if (item.Checked != true)
                    {
                        item.Checked = true;
                        need_submit = true;
                    }
                }
            }
            if (need_submit)
                frm.Submit();
#else
            int count = 0;
            GeckoFormElement frm = null;
            do
            {
                Application.DoEvents();
                frm = document.GetElementsByClassName("v-form").FirstOrDefault() as GeckoFormElement;
                count++;
            } while (frm == null && count < 1000);
            if (frm == null)
                return;
            foreach (GeckoInputElement item in frm.GetElementsByTagName("input"))
            {
                if (item.Type == "email")
                {
                    if (item.Value != cfgMeshok.UserLogin)
                    {
                        SetInput(item, cfgMeshok.UserLogin);
                    }
                }
                else
                if (item.Type == "password")
                {
                    if (item.Value != cfgMeshok.UserPassword)
                    {
                        SetInput(item, cfgMeshok.UserPassword);
                    }
                }
            }
#endif
        }
#if !old_version
        private void SetInput(GeckoInputElement input, string value)
        {
            if (input == null)
                return;
            GeckoWebBrowser browser = mainBrowser.webBrowser;
            mainBrowser.Select();
            input.Focus();
            Application.DoEvents();
            try
            {
                Clipboard.Clear();
                browser.SelectAll();
                Application.DoEvents();
                if (string.IsNullOrEmpty(value))
                {
                    //input.TextContent = "";
                    browser.CutSelection();
                }
                else
                {
                    Clipboard.SetText(value);
                    browser.Paste();
                }
                Clipboard.Clear();
            }
            catch (Exception ex)
            {
            }
        }
#endif
        public void AddPage(Control control, bool canClose = true)
        {
            BaseDocument document = null;
            try
            {
                document = tabbedView.AddOrActivateDocument(doc => doc.Caption == control.Text, () => control);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return;
            }
            //if (control is SettingFrame setting)
            //    setting.Page = document;
            if (!canClose)
                document.Properties.AllowClose = DevExpress.Utils.DefaultBoolean.False;
            Application.DoEvents();
        }
        public void RemovePage(Control control)
        {
            //if (control is SettingFrame setting)
            //    setting.Page = null;
            if (ControlForRemove == control)
                ControlForRemove = null;
            tabbedView.RemoveDocument(control);
            int last = tabbedView.Documents.Count - 1;
            if (last > 0)
            {
                BaseDocument doc = tabbedView.Documents[last];
                AddPage(doc.Control, (doc.Properties.AllowClose == DevExpress.Utils.DefaultBoolean.Default || doc.Properties.AllowClose == DevExpress.Utils.DefaultBoolean.True));
            }
        }
        public void Proces_Begined()
        {
            InvokeIfRequired(() =>
            {
                pageSettigs.Visible = false;
                pageProcess.Visible = false;
                pageGlobalOp.Visible = false;
            });
        }
        public void Proces_Finished()
        {
            InvokeIfRequired(() =>
            {
                pageSettigs.Visible = true;
                pageProcess.Visible = true;
                pageGlobalOp.Visible = true;
                if (_processor != null)
                {
                    _processor.Dispose();
                    _processor = null;
                }
                mainBrowser.RefreshBrowser();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            });
        }
        private bool HasLoads()
        {
            string dirName = Path.Combine(Application.StartupPath, RunContext.AppCfg.ExportPath);
            if (!Directory.Exists(dirName))
                return false;
            return new DirectoryInfo(dirName).GetFiles("мешок*.txt").Length > 0;
        }
        private void barExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        private void MainSettings_Accept(object sender, EventArgs e)
        {
            if (!auth_success)
                mainBrowser.Navigate(cfgMeshok.Base_Url);
        }
        private void MainMeshok_FormClosing(object sender, FormClosingEventArgs e)
        {
            ProcessRunHelper.Executing = false;
            var frames = tabbedView.Documents.Where(x => x.Control is BaseFrame).Select(c => c.Control as BaseFrame).ToList();
            while (frames.Count() > 0)
            {
                BaseFrame item = frames.Last();
                item.CLose();
                frames.Remove(item);
                //RemovePage(item);
                //item.Dispose();
            }
            while (_processor != null)
                Application.DoEvents();
        }
        public void DoApplicatinonRun()
        {
#if old_version
            mainBrowser.Navigate(cfgMeshok.Profile_Url);
#else
            mainBrowser.Navigate(cfgMeshok.Base_Url);
#endif
        }
        private void InitProcessors()
        {
            actProcessOrders.Tag = new WorkerTemplate(typeof(WorkerProcessingOrders)) { DialogAction = BeginProcOrders };
            actProcessOrders.Execute += ActProcess_Execute;
            actProcessOrders.Update += ActProcessOrders_Update;
            actRemoveRasx.Tag = new WorkerTemplate(typeof(WorkerRemoveRasx));
            actRemoveRasx.Execute += ActProcess_Execute;
            actRemoveRasx.Update += ActDeleteLots_Update;
            actAddPrix.Tag = new WorkerTemplate(typeof(WorkerAddPrix), $"Если вы уверены, что все установки по 'Мешку' соответствуют,{Environment.NewLine}то можем приступить к загрузке приходов.{Environment.NewLine}Приступить?");
            actAddPrix.Execute += ActProcess_Execute;
            actAddPrix.Update += ActAddPrix_Update;
            actDeleteLots.Tag = new WorkerTemplate(typeof(WorkerRemoveLots), "Приступить к удалению лотов?");
            actDeleteLots.Execute += ActProcess_Execute;
            actDeleteLots.Update += ActDeleteLots_Update;
            actCreateLots.Tag = new WorkerTemplate(typeof(WorkerCreateCSV)) { DialogAction = CheckBridgenoteConnection };
            actCreateLots.Execute += ActProcess_Execute;
            actCreateLots.Update += ActCreateLots_Update;
            actLoadLots.Tag = new WorkerTemplate(typeof(WorkerLoadingLots), "Приступить к загрузке лотов?");
            actLoadLots.Execute += ActProcess_Execute;
            actLoadLots.Update += ActLoadLots_Update;
            actProgramSetting.Execute += ActProgramSetting_Execute;
            actProgramSetting.Update += ActProgramSetting_Update;
            actMessageSetting.Execute += ActMessageSetting_Execute;
            actMessageSetting.Update += ActMessageSetting_Update;
        }
        private void ActMessageSetting_Update(object sender, EventArgs e)
        {
            if (sender is ActionGh action)
            {
                action.Enabled = messageSettings == null && _processor == null;
            }
        }
        private void ActMessageSetting_Execute(object sender, EventArgs e)
        {
            if (messageSettings == null)
            {
                if (!сfgIShop.TestConnection())
                {
                    if (mainSettings == null)
                        actProgramSetting.DoExecute();
                    mainSettings.ActiveIShop();
                    DlgHelper.DlgWarning("Проверьте подключение к базе!");
                    return;
                }
                messageSettings = new MessagesSetting();
                messageSettings.Caption = actMessageSetting.Caption;
            }
            AddPage(messageSettings);
        }
        private void ActProgramSetting_Update(object sender, EventArgs e)
        {
            if (sender is ActionGh action)
            {
                action.Enabled = mainSettings == null && _processor == null;
            }
        }
        private void ActProgramSetting_Execute(object sender, EventArgs e)
        {
            if (mainSettings == null)
            {
                mainSettings = new MainConfigFrame();
                mainSettings.Caption = actProgramSetting.Caption;
                //mainSettings.Accept += MainSettings_Accept;
            }
            AddPage(mainSettings);
        }
        private void ActAddPrix_Update(object sender, EventArgs e)
        {
            if (sender is ActionGh action)
            {
                action.Enabled = auth_success && mainBrowser.InProfile( ) && _processor == null;
            }
        }
        private void ActProcessOrders_Update(object sender, EventArgs e)
        {
            if (sender is ActionGh action)
            {
                bool inProfile = auth_success && mainBrowser.InProfile() && _processor == null;
                action.Enabled = inProfile && mainBrowser.HasQty(deals_token);
            }
        }
        private void ActDeleteLots_Update(object sender, EventArgs e)
        {
            if (sender is ActionGh action)
            {
                bool inProfile = auth_success && mainBrowser.InProfile() && _processor == null;
                action.Enabled = inProfile && mainBrowser.HasQty(sale_token) && !HasLoads();
            }
        }
        private void ActCreateLots_Update(object sender, EventArgs e)
        {
            if (sender is ActionGh action)
            {
#if TEST_CREATE_LOTS
                action.Enabled = true;
#else
                bool inProfile = auth_success && mainBrowser.InProfile() && _processor == null;
                action.Enabled = inProfile && !mainBrowser.HasQty(sale_token);
#endif
            }
        }
        private void ActLoadLots_Update(object sender, EventArgs e)
        {
            if (sender is ActionGh action)
            {
                bool inProfile = auth_success && mainBrowser.InProfile() && _processor == null;
                action.Enabled = inProfile && HasLoads();
            }
        }
        private void ActProcess_Execute(object sender, EventArgs e)
        {
            if (sender is ActionGh action)
            {
                WorkerTemplate item = action.Tag as WorkerTemplate;
                if (item.CanDo())
                {
                    _processor = item.CreateNew(this);
                    _processor.Execute();
                }
            }
        }
        public void InitForm()
        {
        }
    }
}
