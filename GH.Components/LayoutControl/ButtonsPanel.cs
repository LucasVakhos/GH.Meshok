using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
namespace GH.Components
{
    public enum ItemType { OK, Cancel, Spase }

    public class Buttonitem : LayoutControlItem
    {
        int height = 28;
        int width = 125;
    public Buttonitem(ItemType item, SimpleButton btn, ButtonsPanel buttonsPanel)
        {
            Control = btn;
            btn.Appearance.Options.UseTextOptions = true;
            btn.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            btn.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            btn.ImageOptions.ImageToTextIndent = 5;
            btn.TabStop = false;
            btn.Name = "btn" + item.ToString();
            Name = "lc" + item.ToString();
            MaxSize = new System.Drawing.Size(width, height);
            MinSize = new System.Drawing.Size(width, height);
            Size = new System.Drawing.Size(width, height);
            SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            TextSize = new System.Drawing.Size(0, 0);
            TextVisible = false;
            switch (item)
            {
                case ItemType.OK:
                    Location = new System.Drawing.Point(0, 0);
                    buttonsPanel.btnOK = btn;
                    break;
                case ItemType.Cancel:
                    Location = new System.Drawing.Point(width + 1, 0);
                    buttonsPanel.btnCancel = btn;
                    break;
                case ItemType.Spase:
                    break;
                default:
                    break;
            }
        }
    }

    public class ButtonsPanel : LayoutControlGroup
    {
        public ButtonsPanel(LayoutGroup parentGroup) : base(parentGroup)
        {
            TextVisible = false;
            Padding = new DevExpress.XtraLayout.Utils.Padding(0);
            Name = "lgButtons";
            Text = "Кнопочки";
            AllowCustomizeChildren = false;
            ShowInCustomizationForm = false;
            BaseLayoutItem layoutItem = null;
            foreach (ItemType item in Enum.GetValues(typeof(ItemType)))
                layoutItem = AddMyItem(item, (item != ItemType.Spase ? new SimpleButton() : null), layoutItem, InsertType.Right);
        }
    private BaseLayoutItem AddMyItem(ItemType item, SimpleButton btn, BaseLayoutItem baseLayout, InsertType right)
        {
            LayoutControlItem newLayout = null;
            if (item == ItemType.Spase)
                AddItem(new EmptySpaceItem(), baseLayout, InsertType.Right);
            else
                newLayout = AddItem(new Buttonitem(item, btn, this), baseLayout, InsertType.Right);
            return newLayout;
        }

    public SimpleButton btnOK { get; set; }

    public SimpleButton btnCancel { get; set; }
    }
}
