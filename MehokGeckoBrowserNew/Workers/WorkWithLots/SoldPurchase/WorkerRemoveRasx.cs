using GH.Browser;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
namespace MeshokBrowser.Workers
{
    public class WorkerRemoveRasx : Worker
    {
        private const string profile_url = "https://meshok.net/profile.php";
        private const string done_url = "https://meshok.net/profile.php?submit=1&what=sale";
        private bool _lotFounded;
        public WorkerRemoveRasx(IMainForm form) : base(form)
        {
            ProcessName = "Удаление расходников";
            Begin_url = profile_url + $"?num_of_lines={cfgMeshok.OnPageForDelete}&opt=1&sort=pic&way=asc&what=sale";
        }
        protected override async void OnDocumentCompleted(string url)
        {
            if (url == done_url)
            {
                NavigateNext();
            }
            else if (UrlIsEqual(url, Begin_url))
            {
                if (!ProcScreen.HasQty(MainMeshok.sale_token))
                {
                    WorkDone();
                    return;
                }
                if (await DeletePageAsync())
                {
                    clearGC = true;
                    IncCurrentStep();
                    if (!_lotFounded)
                        NavigateNext();
                }
                else
                {
                    WorkDone();
                }
            }
        }
        private bool UrlIsEqual(string a, string b)
        {
            return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        }
        private void NavigateNext()
        {
            ProcScreen.Navigate(Begin_url);
        }
        private async Task<bool> DeletePageAsync()
        {
            ClearCash();
            _lotFounded = false;
            DeletePageResult result = await ProcScreen.ExecuteJsonScriptAsync<DeletePageResult>("""
                (() => {
                    const checkboxes = Array.from(document.querySelectorAll('input[name="work[]"]'));
                    if (checkboxes.length === 0) {
                        return { ok: true, founded: false };
                    }
                    for (const checkbox of checkboxes) {
                        checkbox.checked = true;
                        checkbox.dispatchEvent(new Event("input", { bubbles: true }));
                        checkbox.dispatchEvent(new Event("change", { bubbles: true }));
                    }
                    const div = document.getElementById("saleBAction");
                    const select = div ? div.querySelector('select[name="do_work"]') : null;
                    const submit = document.getElementById("submitDoWork");
                    if (!select || !submit) {
                        return { ok: false, founded: true };
                    }
                    select.focus();
                    select.value = "D";
                    select.dispatchEvent(new Event("input", { bubbles: true }));
                    select.dispatchEvent(new Event("change", { bubbles: true }));
                    submit.click();
                    return { ok: true, founded: true };
                })()
            """);
            _lotFounded = result?.Founded ?? false;
            return result?.Ok ?? false;
        }
        private sealed class DeletePageResult
        {
            [JsonPropertyName("ok")]
            public bool Ok { get; set; }
            [JsonPropertyName("founded")]
            public bool Founded { get; set; }
        }
    }
}
