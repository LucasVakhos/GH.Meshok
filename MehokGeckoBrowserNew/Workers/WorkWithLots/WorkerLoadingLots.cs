using GH.Browser;
using GH.Components;
using GH.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
namespace MeshokBrowser.Workers
{
    public class WorkerLoadingLots : Worker
    {
        private enum ProcStep
        {
            Setting,
            Compare,
            GoBegin
        }
        private const string meshok_url = "https://meshok.net";
        private const string compare_url = meshok_url + "/profile.php?submit=1&what=bulk";
        private List<string> files = new List<string>();
        private ProcStep curSettingStep = ProcStep.Setting;
        public WorkerLoadingLots(IMainForm form) : base(form)
        {
            ProcessName = "Загрузка лотов";
            Begin_url = meshok_url + "/profile.php?what=bulk";
        }
        protected override void RedoNavigate(string url)
        {
            if (curSettingStep == ProcStep.GoBegin)
                DoEnd();
            else
                base.RedoNavigate(url);
        }
        protected override async void OnDocumentCompleted(string url)
        {
            if (url == Begin_url)
                await SettingPagesAsync();
            else if (url == compare_url)
            {
                if (curSettingStep == ProcStep.Compare)
                {
                    curSettingStep = ProcStep.GoBegin;
                    await ProcScreen.SubmitFormAsync("#form2");
                }
                else if (curSettingStep == ProcStep.GoBegin)
                {
                    DoEnd();
                }
            }
        }
        private void DoEnd()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            curSettingStep = ProcStep.Setting;
            if (files.Count > 0 && File.Exists(files[0]))
                File.Delete(files[0]);
            if (files.Count > 0)
                files.RemoveAt(0);
            IncCurrentStep();
            if (files.Count > 0)
                ProcScreen.Navigate(Begin_url);
            else
                WorkDone();
        }
        protected override void OnCreate()
        {
            SetProgressTotal();
            if (TotalSteps == 0)
                WorkDone();
            else
                ProcScreen.Navigate(Begin_url);
        }
        protected void SetProgressTotal()
        {
            files = LoadingHelper.GetFileList();
            if (files.Count == 0)
            {
                WorkDone();
                ProcScreen.Navigate(meshok_url + "/profile.php");
                DlgHelper.DlgWarning("Нет файлов для заливки!!!");
                return;
            }
            TotalSteps = files.Count;
        }
        private async Task SettingPagesAsync()
        {
            bool isFillBegin = await LoadingHelper.FillBeginPageAsync(ProcScreen, files[0]);
            if (!isFillBegin)
            {
                if (await ProcScreen.ClickElementAsync("input[name='SLCONSENT']"))
                    return;
                curSettingStep = ProcStep.Setting;
                ProcScreen.Navigate(Begin_url);
                return;
            }
            curSettingStep = ProcStep.Compare;
            if (!await ProcScreen.SubmitFormAsync("#form2"))
                ProcScreen.Navigate(Begin_url);
        }
    }
}
