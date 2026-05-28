using Gecko;
using GH.NHibernate;
using MeshokBrowser.NHibernate;
using System.Linq;
namespace MeshokBrowser.Workers
{
    public class TaskTitleParse : ParsingTask<Title>
    {
        public TaskTitleParse()
        {
            CaptionOfTask = "Сканирование лота";
        }
        protected override void WorkWithDocument()
        {
            currentObject.ParsingSaccess = false;
            //Field.GetFields<UpdatablePropertyAttribute>(currentObject, new string[] {"id", "Name"});
            Field[] fields = currentObject.GetFields();
            foreach (GeckoHtmlElement table in doc.GetElementsByTagName("table").Where(x => x.GetElementsByTagName("tr").Count() == 10
                && x.GetElementsByTagName("td").Any(td => fields.Any(f => f.CaptionText.Contains(td.TextContent)))))
            {
                foreach (GeckoHtmlElement tr in table.GetElementsByTagName("tr"))
                {
                    GeckoHtmlElement caption = tr.GetElementsByTagName("td").FirstOrDefault() as GeckoHtmlElement;
                    GeckoHtmlElement data = tr.GetElementsByTagName("td").Where(x => x.TextContent != caption.TextContent).FirstOrDefault() as GeckoHtmlElement;
                    if (caption != null)
                    {
                        foreach (Field field in fields.Where(x => x.Caption == caption.TextContent))
                        {
                            currentObject.AsValue(field.Name, data.TextContent);
                            break;
                        }
                    }
                }
            }
        }
    }
}
