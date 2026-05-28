using System.ComponentModel;

namespace GH.Components
{
    [ToolboxItem(false)]
    public class ActionDataGh : ActionGh
    {
        EditTypes _buttonType;

        public ActionDataGh(EditTypes buttonType)
        {
            _buttonType = buttonType;
            Category = buttonType.GetCategory();
            Image = ActtionsImages.Instance.SmallImages.Images[(int)_buttonType];
            LargeImage = ActtionsImages.Instance.LargeImages.Images[(int)_buttonType];
        }

        public EditTypes ButtonType { get => _buttonType; }
    }
}

