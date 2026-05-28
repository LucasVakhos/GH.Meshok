using Gecko;
using Gecko.DOM;
using MeshokBrowser.NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace MeshokBrowser.Workers
{
    public static class PostMessageHelper
    {
        public static void PostingMessage(GeckoWebBrowser browser, OrderLine currentObject)
        {
            GeckoDocument doc = browser.Document;
            GeckoTextAreaElement memo = doc.GetElementsByTagName("textarea").Where(x => x.Id == "MESS").FirstOrDefault() as GeckoTextAreaElement;
            if (memo == null)
                return;
            string bodiText = doc.Body.TextContent;
            foreach (CheckMesage item in currentObject.Infos.Where(x => x.NeedMessaging).ToArray())
            {
                if (bodiText.Contains(item.ticket))
                    item.NeedMessaging = false;
            }
            if (!currentObject.HasMessages)
                return;
            currentObject.ParsingSaccess = false;
            foreach (CheckMesage item in currentObject.Infos.Where(x => x.NeedMessaging))
            {
                memo.Focus();
                string mess = item.ticket + "\r\n" + item.mess_text;
                memo.Value = mess;
                Application.DoEvents();
                foreach (GeckoInputElement btn in doc.GetElementsByTagName("input").Where(x => x.GetAttribute("type") == "submit" && x.GetAttribute("value") == "Отправить"))
                {
                    btn.Focus();
                    Application.DoEvents();
#if !NOT_POST_MESSAGE
                    btn.Form.Submit();
                    Application.DoEvents();
                    while (browser.IsBusy)
                    {
                        Application.DoEvents();
                    }
#endif
                    goto success_lbl;
                }
                success_lbl:
                item.NeedMessaging = false;
            }
        }
    }
}
