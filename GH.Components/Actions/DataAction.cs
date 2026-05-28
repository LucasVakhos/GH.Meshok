using System.ComponentModel;
namespace GH.Components
{
    [ToolboxItem(false)]
    public class DataAction : ActionBase
    {
        private EditTypes _buttonType;
        public DataAction(EditTypes buttonType)
        {
            _buttonType = buttonType;
            Category = buttonType.GetCategory();
            Image = ActtionsImages.Instance.SmallImages.Images[(int)_buttonType];
            LargeImage = ActtionsImages.Instance.LargeImages.Images[(int)_buttonType];
        }
        public EditTypes ButtonType { get => _buttonType; }
    }
}
