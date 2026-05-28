using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using System.ComponentModel;
namespace GH.Components
{
    public enum LoginInputType
    {
        AsDirectInputData,
        AsSelectFromCombo,
    }
    [ToolboxItem(true)]
    public class LoginControl : LayoutControlItem
    {
        private static LoginControl login;
        public static LoginControl Load<T>(LayoutControlItem item, BindingSource source) where T : BaseUser
        {
            if (login == null)
                new LoginControl(item, source);
            login.LookUpDataSource = (new NHRepository<T>() as INHRepository).KeyEntityLookupList();
            return login;
        }
        private LoginInputType _userType = LoginInputType.AsDirectInputData;
        [DefaultValue(LoginInputType.AsDirectInputData)]
        public LoginInputType UserType
        {
            get => _userType;
            set
            {
                if (_userType == value)
                    return;
                _userType = value;
                SetEditor(value);
            }
        }
        public override void EndInit()
        {
            SetEditor(UserType);
            base.EndInit();
        }
        private void SetEditor(LoginInputType userType)
        {
            //if (!IsInitialized)
            LayoutControl.SuspendLayout();
            base.BeginInit();
            //layoutControl.Root.BeginInit();
            if (Control != null)
            {
                textEdits[userType].Size = new Size(Control.Width, Control.Height);
                textEdits[userType].Location = new Point(Control.Left, Control.Top);
            }
            LayoutControl.Controls.Remove(Control);
            //Control = null;
            Control = textEdits[userType];
            LayoutControl.Controls.Add(Control);
            base.EndInit();
            //layoutControl.Root.EndInit();
            //if (!IsInitialized)
            LayoutControl.ResumeLayout(false);
            if (Control.CanFocus)
                Control.Focus();
        }
        private void Assigne(LayoutControlItem item)
        {
            Text = item.Text;
            Location = new Point(item.Location.X, item.Location.Y);
            Size = new Size(item.Size.Width, item.Size.Height);
            item.Parent.AddItem(this, item, DevExpress.XtraLayout.Utils.InsertType.Bottom);
            Parent.Remove(item);
        }
        public object LookUpDataSource
        {
            get => (textEdits[LoginInputType.AsSelectFromCombo] as LookUpEdit).Properties.DataSource;
            set => (textEdits[LoginInputType.AsSelectFromCombo] as LookUpEdit).Properties.DataSource = value;
        }
        private void CtreateComboLogin(LoginInputType item)
        {
            TextEdit comboLogin;
            if (item == LoginInputType.AsSelectFromCombo)
                comboLogin = new LookUpEdit();
            else
                comboLogin = new TextEdit();
            comboLogin.Name = "comboLogin";
            if (comboLogin is LookUpEdit lookUpEdit)
            {
                lookUpEdit.Properties.DisplayMember = "Value";
                lookUpEdit.Properties.KeyMember = "Key";
                lookUpEdit.Properties.ShowFooter = false;
                lookUpEdit.Properties.ShowHeader = false;
                lookUpEdit.Properties.ValueMember = "Key";
            }
            else
            {
                comboLogin.EditValue = "Какой-то там логин";
            }
            comboLogin.DataBindings.Add(new Binding("EditValue", DataSource, "User", true, DataSourceUpdateMode.OnPropertyChanged));
            //comboLogin.Location = Location;
            //comboLogin.Size = Size;
            comboLogin.StyleController = LayoutControl;
            textEdits.Add(item, comboLogin);
        }
        private object NHRepository<T>()
        {
            throw new NotImplementedException();
        }
        Dictionary<LoginInputType, TextEdit> textEdits = new Dictionary<LoginInputType, TextEdit>();
        BindingSource DataSource;
        LayoutControl LayoutControl;
        public LoginControl(LayoutControlItem item, BindingSource source)
        {
            if (item.Owner is LayoutControl layoutControl)
            {
                login = this;
                LayoutControl = layoutControl;
                DataSource = source;
                Assigne(item);
                foreach (LoginInputType type in Enum.GetValues(typeof(LoginInputType)))
                    CtreateComboLogin(type);
            }
            else
                throw new Exception($"Этот помпонент только для {nameof(DataLayoutControl)}!!!");
            Disposed += LoginControl_Disposed;
            SetEditor(UserType);
        }
        private void LoginControl_Disposed(object sender, EventArgs e)
        {
            foreach (KeyValuePair<LoginInputType, TextEdit> item in textEdits)
                item.Value.Dispose();
            textEdits.Clear();
            textEdits = null;
        }
    }
}
