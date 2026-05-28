using System.Text.Json;
namespace GH.Components
{
    public static class GhBrowserDocumentExtension
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        // =========================
        // Document
        // =========================
        public static async Task<string?> GetTitleAsync(this GhBrowser browser)
        {
            var json = await browser.ExecuteScriptAsync("document.title");
            return JsonSerializer.Deserialize<string?>(json, JsonOptions);
        }
        public static async Task<string?> GetUrlAsync(this GhBrowser browser)
        {
            var json = await browser.ExecuteScriptAsync("location.href");
            return JsonSerializer.Deserialize<string?>(json, JsonOptions);
        }
        public static async Task<string?> GetBodyHtmlAsync(this GhBrowser browser)
        {
            var json = await browser.ExecuteScriptAsync("document.body ? document.body.innerHTML : null");
            return JsonSerializer.Deserialize<string?>(json, JsonOptions);
        }
        public static async Task<string?> GetDocumentHtmlAsync(this GhBrowser browser)
        {
            var json = await browser.ExecuteScriptAsync("document.documentElement ? document.documentElement.outerHTML : null");
            return JsonSerializer.Deserialize<string?>(json, JsonOptions);
        }
        // =========================
        // Search / Query
        // =========================
        public static async Task<List<string>> GetTagsAsync(this GhBrowser browser)
        {
            var script = """
                Array.from(document.querySelectorAll("*"))
                    .map(e => e.tagName.toLowerCase())
                    .filter((v, i, a) => a.indexOf(v) === i)
            """;
            var json = await browser.ExecuteScriptAsync(script);
            return JsonSerializer.Deserialize<List<string>>(json, JsonOptions) ?? new();
        }
        public static async Task<bool> ExistsAsync(this GhBrowser browser, string selector)
        {
            var safeSelector = JsonSerializer.Serialize(selector);
            var script = $$"""
                !!document.querySelector({{safeSelector}})
            """;
            var json = await browser.ExecuteScriptAsync(script);
            return JsonSerializer.Deserialize<bool>(json, JsonOptions);
        }
        public static async Task<GhElementInfo?> QuerySelectorAsync(
            this GhBrowser browser,
            string selector)
        {
            var result = await browser.QuerySelectorAllAsync(selector);
            return result.FirstOrDefault();
        }
        public static async Task<List<GhElementInfo>> QuerySelectorAllAsync(
            this GhBrowser browser,
            string selector)
        {
            var safeSelector = JsonSerializer.Serialize(selector);
            var script = $$"""
                Array.from(document.querySelectorAll({{safeSelector}})).map(e => ({
                    tag: e.tagName ? e.tagName.toLowerCase() : null,
                    id: e.id || null,
                    name: e.getAttribute("name"),
                    type: e.getAttribute("type"),
                    className: typeof e.className === "string" ? e.className : null,
                    text: e.innerText || null,
                    value: "value" in e ? e.value : null,
                    checked: "checked" in e ? e.checked : null,
                    html: e.outerHTML || null,
                    attributes: Array.from(e.attributes || []).reduce((a, x) => {
                        a[x.name] = x.value;
                        return a;
                    }, {})
                }))
            """;
            var json = await browser.ExecuteScriptAsync(script);
            return JsonSerializer.Deserialize<List<GhElementInfo>>(json, JsonOptions) ?? new();
        }
        public static Task<List<GhElementInfo>> GetElementsByTagAsync(
            this GhBrowser browser,
            string tagName)
        {
            return browser.QuerySelectorAllAsync(tagName);
        }
        // =========================
        // Get values
        // =========================
        public static async Task<string?> GetElementTextAsync(
            this GhBrowser browser,
            string selector)
        {
            var safeSelector = JsonSerializer.Serialize(selector);
            var script = $$"""
                (() => {
                    const e = document.querySelector({{safeSelector}});
                    return e ? e.innerText : null;
                })()
            """;
            var json = await browser.ExecuteScriptAsync(script);
            return JsonSerializer.Deserialize<string?>(json, JsonOptions);
        }
        public static async Task<string?> GetElementHtmlAsync(
            this GhBrowser browser,
            string selector)
        {
            var safeSelector = JsonSerializer.Serialize(selector);
            var script = $$"""
                (() => {
                    const e = document.querySelector({{safeSelector}});
                    return e ? e.innerHTML : null;
                })()
            """;
            var json = await browser.ExecuteScriptAsync(script);
            return JsonSerializer.Deserialize<string?>(json, JsonOptions);
        }
        public static async Task<string?> GetElementOuterHtmlAsync(
            this GhBrowser browser,
            string selector)
        {
            var safeSelector = JsonSerializer.Serialize(selector);
            var script = $$"""
                (() => {
                    const e = document.querySelector({{safeSelector}});
                    return e ? e.outerHTML : null;
                })()
            """;
            var json = await browser.ExecuteScriptAsync(script);
            return JsonSerializer.Deserialize<string?>(json, JsonOptions);
        }
        public static async Task<string?> GetElementValueAsync(
            this GhBrowser browser,
            string selector)
        {
            var safeSelector = JsonSerializer.Serialize(selector);
            var script = $$"""
                (() => {
                    const e = document.querySelector({{safeSelector}});
                    return e && "value" in e ? e.value : null;
                })()
            """;
            var json = await browser.ExecuteScriptAsync(script);
            return JsonSerializer.Deserialize<string?>(json, JsonOptions);
        }
        public static async Task<bool?> GetElementCheckedAsync(
            this GhBrowser browser,
            string selector)
        {
            var safeSelector = JsonSerializer.Serialize(selector);
            var script = $$"""
                (() => {
                    const e = document.querySelector({{safeSelector}});
                    return e && "checked" in e ? e.checked : null;
                })()
            """;
            var json = await browser.ExecuteScriptAsync(script);
            return JsonSerializer.Deserialize<bool?>(json, JsonOptions);
        }
        public static async Task<string?> GetElementAttributeAsync(
            this GhBrowser browser,
            string selector,
            string attribute)
        {
            var safeSelector = JsonSerializer.Serialize(selector);
            var safeAttribute = JsonSerializer.Serialize(attribute);
            var script = $$"""
                (() => {
                    const e = document.querySelector({{safeSelector}});
                    return e ? e.getAttribute({{safeAttribute}}) : null;
                })()
            """;
            var json = await browser.ExecuteScriptAsync(script);
            return JsonSerializer.Deserialize<string?>(json, JsonOptions);
        }
        // =========================
        // Set values
        // =========================
        public static async Task<bool> SetElementTextAsync(
            this GhBrowser browser,
            string selector,
            string text)
        {
            var safeSelector = JsonSerializer.Serialize(selector);
            var safeText = JsonSerializer.Serialize(text);
            var script = $$"""
                (() => {
                    const e = document.querySelector({{safeSelector}});
                    if (!e) return false;
                    e.innerText = {{safeText}};
                    return true;
                })()
            """;
            var json = await browser.ExecuteScriptAsync(script);
            return JsonSerializer.Deserialize<bool>(json, JsonOptions);
        }
        public static async Task<bool> SetElementHtmlAsync(
            this GhBrowser browser,
            string selector,
            string html)
        {
            var safeSelector = JsonSerializer.Serialize(selector);
            var safeHtml = JsonSerializer.Serialize(html);
            var script = $$"""
                (() => {
                    const e = document.querySelector({{safeSelector}});
                    if (!e) return false;
                    e.innerHTML = {{safeHtml}};
                    return true;
                })()
            """;
            var json = await browser.ExecuteScriptAsync(script);
            return JsonSerializer.Deserialize<bool>(json, JsonOptions);
        }
        public static async Task<bool> SetElementValueAsync(
            this GhBrowser browser,
            string selector,
            string value)
        {
            var safeSelector = JsonSerializer.Serialize(selector);
            var safeValue = JsonSerializer.Serialize(value);
            var script = $$"""
                (() => {
                    const e = document.querySelector({{safeSelector}});
                    if (!e || !("value" in e)) return false;
                    e.focus();
                    e.value = {{safeValue}};
                    e.dispatchEvent(new Event("input", { bubbles: true }));
                    e.dispatchEvent(new Event("change", { bubbles: true }));
                    return true;
                })()
            """;
            var json = await browser.ExecuteScriptAsync(script);
            return JsonSerializer.Deserialize<bool>(json, JsonOptions);
        }
        public static async Task<bool> SetElementCheckedAsync(
            this GhBrowser browser,
            string selector,
            bool value)
        {
            var safeSelector = JsonSerializer.Serialize(selector);
            var safeValue = value ? "true" : "false";
            var script = $$"""
                (() => {
                    const e = document.querySelector({{safeSelector}});
                    if (!e || !("checked" in e)) return false;
                    e.focus();
                    e.checked = {{safeValue}};
                    e.dispatchEvent(new Event("input", { bubbles: true }));
                    e.dispatchEvent(new Event("change", { bubbles: true }));
                    return true;
                })()
            """;
            var json = await browser.ExecuteScriptAsync(script);
            return JsonSerializer.Deserialize<bool>(json, JsonOptions);
        }
        public static async Task<bool> SetElementAttributeAsync(
            this GhBrowser browser,
            string selector,
            string attribute,
            string value)
        {
            var safeSelector = JsonSerializer.Serialize(selector);
            var safeAttribute = JsonSerializer.Serialize(attribute);
            var safeValue = JsonSerializer.Serialize(value);
            var script = $$"""
                (() => {
                    const e = document.querySelector({{safeSelector}});
                    if (!e) return false;
                    e.setAttribute({{safeAttribute}}, {{safeValue}});
                    return true;
                })()
            """;
            var json = await browser.ExecuteScriptAsync(script);
            return JsonSerializer.Deserialize<bool>(json, JsonOptions);
        }
        public static async Task<bool> RemoveElementAttributeAsync(
            this GhBrowser browser,
            string selector,
            string attribute)
        {
            var safeSelector = JsonSerializer.Serialize(selector);
            var safeAttribute = JsonSerializer.Serialize(attribute);
            var script = $$"""
                (() => {
                    const e = document.querySelector({{safeSelector}});
                    if (!e) return false;
                    e.removeAttribute({{safeAttribute}});
                    return true;
                })()
            """;
            var json = await browser.ExecuteScriptAsync(script);
            return JsonSerializer.Deserialize<bool>(json, JsonOptions);
        }
        // =========================
        // Actions
        // =========================
        public static async Task<bool> ClickElementAsync(
            this GhBrowser browser,
            string selector)
        {
            var safeSelector = JsonSerializer.Serialize(selector);
            var script = $$"""
                (() => {
                    const e = document.querySelector({{safeSelector}});
                    if (!e) return false;
                    e.click();
                    return true;
                })()
            """;
            var json = await browser.ExecuteScriptAsync(script);
            return JsonSerializer.Deserialize<bool>(json, JsonOptions);
        }
        public static async Task<bool> FocusElementAsync(
            this GhBrowser browser,
            string selector)
        {
            var safeSelector = JsonSerializer.Serialize(selector);
            var script = $$"""
                (() => {
                    const e = document.querySelector({{safeSelector}});
                    if (!e || !e.focus) return false;
                    e.focus();
                    return true;
                })()
            """;
            var json = await browser.ExecuteScriptAsync(script);
            return JsonSerializer.Deserialize<bool>(json, JsonOptions);
        }
        public static async Task<bool> SubmitFormAsync(
            this GhBrowser browser,
            string selector)
        {
            var safeSelector = JsonSerializer.Serialize(selector);
            var script = $$"""
                (() => {
                    const e = document.querySelector({{safeSelector}});
                    if (!e) return false;
                    const form = e.tagName && e.tagName.toLowerCase() === "form"
                        ? e
                        : e.closest("form");
                    if (!form) return false;
                    form.requestSubmit
                        ? form.requestSubmit()
                        : form.submit();
                    return true;
                })()
            """;
            var json = await browser.ExecuteScriptAsync(script);
            return JsonSerializer.Deserialize<bool>(json, JsonOptions);
        }
        public static async Task<bool> ExecuteBoolScriptAsync(this GhBrowser browser, string script)
        {
            var json = await browser.ExecuteScriptAsync(script);
            return JsonSerializer.Deserialize<bool>(json, JsonOptions);
        }
    }
    public class GhElementInfo
    {
        public string? Tag { get; set; }
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? ClassName { get; set; }
        public string? Text { get; set; }
        public string? Value { get; set; }
        public bool? Checked { get; set; }
        public string? Html { get; set; }
        public Dictionary<string, string>? Attributes { get; set; }
    }
}
