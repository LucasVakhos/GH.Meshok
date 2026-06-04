using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraNavBar;
namespace GH.Components
{
    public static class ActionsCreateHelper
    {
    public static RibbonPageGroup CreateRibbonPageGroup(string categoy, List<ActionGh> actions, RibbonPage editPage, RibbonBarManager manager)
        {
            RibbonPageGroup editGroup = new RibbonPageGroup();
            editPage.Groups.Add(editGroup);
            editGroup.AllowTextClipping = false;
            editGroup.Name = categoy.Replace(" ", "").ToLower() + "Group";
            editGroup.ShowCaptionButton = false;
            editGroup.Text = categoy;
            foreach (ActionGh item in actions)
            {
                BarButtonItemGH barItem = new BarButtonItemGH(manager, item);
                editGroup.ItemLinks.Add(barItem);
                item.ActionList.SetAction(barItem, item);
            }
            return editGroup;
        }
    public static void CreateMenuGroup(string categoy, List<ActionGh> actions, ContextMenuStrip menuStrip)
        {
            if (menuStrip.Items.Count > 0)
            {
                ToolStripSeparator separatorItem = new ToolStripSeparator();
                menuStrip.Items.Add(separatorItem);
            }
            foreach (ActionGh item in actions)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem();
                menuStrip.Items.Add(menuItem);
                item.ActionList.SetAction(menuItem, item);
            }
        }
    public static void CreateNavBarItemGroup(string categoy, List<ActionGh> actions, IList<NavBarItem> navbaritems)
        {
            NavBarSeparatorItem separatorItem = new NavBarSeparatorItem();
            navbaritems.Add(separatorItem);
            foreach (ActionGh item in actions)
            {
                NavBarItem barItem = new NavBarItem();
                navbaritems.Add(barItem);
                item.ActionList.SetAction(barItem, item);
            }
        }
    }

    public class BarButtonItemGH : BarButtonItem
    {
        public BarButtonItemGH(RibbonBarManager manager, ActionGh item) : base(manager, item.Caption)
        {
            ItemShortcut = new BarShortcut(item.ShortcutKeys);
            Id = manager.GetNewItemId();
            Glyph = item.Image;
            LargeGlyph = item.LargeImage;
        }
    }
}
