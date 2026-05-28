using GH.Browser;
using GH.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
namespace MeshokBrowser.Workers
{
    public class WorkerAddPrix : Worker
    {
        private enum ProcStep
        {
            Setting,
            Compare,
            GoToSetting
        }
        private const string meshok_url = "https://meshok.net";
        private const string loading_start_url = meshok_url + "/profile.php?what=bulk";
        private const string compare_url = meshok_url + "/profile.php?submit=1&what=bulk";
        private readonly List<string> files = new List<string>();
        private ProcStep curSettingStep = ProcStep.Setting;
        private string bridgenoteUrl = string.Empty;
        private bool bridgenoteLoaded = false;
        public WorkerAddPrix(IMainForm form) : base(form)
        {
            ProcessName = "Загрузка лотов";
            Begin_url = loading_start_url;
        }
        protected override void OnCreate()
        {
            files.Clear();
            files.AddRange(LoadingHelper.GetFileList());
            TotalSteps = files.Count;
            if (files.Count == 0)
            {
                WorkDone();
                DlgHelper.DlgWarning("Нет файлов для заливки!!!");
                return;
            }
            ProcScreen.Navigate(Begin_url);
        }
        protected override async void OnDocumentCompleted(string url)
        {
            if (Begin_url == url)
            {
                if (Begin_url == bridgenoteUrl)
                    bridgenoteLoaded = true;
                else if (Begin_url == loading_start_url)
                    await FillPageAndPostAsync();
            }
            else if (url == compare_url)
            {
                if (curSettingStep == ProcStep.Compare)
                {
                    curSettingStep = ProcStep.GoToSetting;
                    await ProcScreen.SubmitFormAsync("#form2");
                }
                else if (curSettingStep == ProcStep.GoToSetting)
                {
                    DoNextStep();
                }
            }
        }
        private async Task FillPageAndPostAsync()
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
            {
                curSettingStep = ProcStep.Setting;
                ProcScreen.Navigate(Begin_url);
            }
        }
        private void DoNextStep()
        {
            if (files.Count > 0 && File.Exists(files[0]))
                File.Delete(files[0]);
            if (files.Count > 0)
                files.RemoveAt(0);
            IncCurrentStep();
            curSettingStep = ProcStep.Setting;
            if (files.Count > 0)
                ProcScreen.Navigate(Begin_url);
            else
                WorkDone();
        }
    }
}
