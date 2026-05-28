using Gecko;
using Gecko.WebIDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GH.Components
{
    public static class GeckoHelper
    {
        public static void RaiseHTMLEvent(GeckoDocument document, GeckoHtmlElement element, string eventName)
        {
            DomEventArgs e = document.CreateEvent("HTMLEvents");
            Event ev = new Event(document.Window, e.DomEvent as nsISupports);
            ev.InitEvent(eventName, true, true);
            element.GetEventTarget().DispatchEvent(e);
        }
    }
}
