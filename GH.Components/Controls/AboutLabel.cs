using DevExpress.XtraBars;
using System.ComponentModel;
namespace GH.Components
{
    [ToolboxItem(false)]
    public class AboutLabel : BarStaticItem
    {
        Form _form;
        public AboutLabel(Form form)
        {
            _form = form;
            if (form.Icon != null)
                ImageOptions.Image = form.Icon.ToBitmap();
            Caption = $"{Application.CompanyName} - {Application.ProductVersion}";
            Hint = "Кликни чтоб узнать подробности";
            Name = "AboutLabel";
            ItemClick += AboutLabel_ItemClick;
            if (form is IRibbonForm ribbon)
            {
                ribbon.Ribbon.Items.Add(this);
                ribbon.StatusBar.ItemLinks.Add(this);
                return;
            }
            if (form is IBarsForm barsForm)
            {
                barsForm.BarManager.Items.Add(this);
                barsForm.StatusBar.ItemLinks.Add(this);
            }
        }
        private void AboutLabel_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (AboutBox about = RunContext.InstanceGetAboutBox())
            {
                about.ShowDialog(_form);
            }
        }
    }
}
