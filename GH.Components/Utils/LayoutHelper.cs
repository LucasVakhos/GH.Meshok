using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
namespace GH.Components
{
    public static class LayoutHelper
    {
        public static void CreateLayoutGroup<TEntity, TAttribute>(LayoutControlGroup EditGroup, string[] except = null, bool withKey = false)
            where TEntity : AbstractEntity
            where TAttribute : UpdatablePropertyAttribute
        {
            LayoutControl layoutControl = EditGroup.Owner.Control as LayoutControl;
            if (layoutControl == null)
                throw new Exception("не найденa LayoutControl!!!");
            ContainerControl form = layoutControl.Parent as ContainerControl;
            if (form == null)
                throw new Exception("не найденa Forma!!!");
            BindingSource dataSource = null;
            if (layoutControl is DataLayoutControl dataLayout)
                dataSource = dataLayout.DataSource as BindingSource;
            if (dataSource == null)
                dataSource = ObjectHelper.EnumerateFields<BindingSource>(form).FirstOrDefault();
            if (dataSource == null)
                throw new Exception("не найден ВataSource!!!");
            LayoutControlGroup group = null;
            foreach (Field field in Field.GetFields<TEntity, TAttribute>(except, withKey))
            {
                if (field.Group != null)
                {
                    if (EditGroup.Items.FindByName(field.GroupName) is LayoutControlGroup grp)
                        group = grp;
                    if (group == null || group.Name != field.GroupName)
                    {
                        group = EditGroup.AddGroup();
                        group.Text = field.Group;
                        group.Name = field.GroupName;
                    }
                }
                else
                    group = EditGroup;
                BaseControl control = field.CreateControl();
                control.Name = field.ControlName;
                layoutControl.Controls.Add(control);
                LayoutControlItem lc = new LayoutControlItem();
                lc.Name = field.LayoutName;
                lc.Control = control;
                if (control is SimpleButton)
                    lc.TextVisible = false;
                else
                {
                    lc.Text = field.CaptionText;
                    lc.OptionsToolTip.ToolTip = field.ToolTip;
                    lc.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    lc.TextVisible = true;
                }
                if (field.SubGroup != null)
                {
                    if (!(group.Items.FindByName(field.SubGroupName) is LayoutControlGroup sub))
                    {
                        sub = group.AddGroup();
                        sub.GroupBordersVisible = false;
                        sub.DefaultLayoutType = DevExpress.XtraLayout.Utils.LayoutType.Horizontal;
                        sub.Padding = new DevExpress.XtraLayout.Utils.Padding(0);
                        sub.Name = field.SubGroupName;
                    }
                    //if (lc.Control is BaseEdit baseEdit)
                    //    baseEdit.EditValueChanged += LayoutHelper_EditValueChanged;
                    sub.AddItem(lc);
                }
                else
                    group.AddItem(lc);
                Binding bind = new Binding("EditValue", dataSource, field.Name, true,
                    DataSourceUpdateMode.OnPropertyChanged, null, field.DisplayFormat);
                bind.ControlUpdateMode = ControlUpdateMode.OnPropertyChanged;
                control.DataBindings.Add(bind);
            }
            Align(EditGroup);
        }
        //private static void LayoutHelper_EditValueChanged(object sender, EventArgs e)
        //{
        //     if ((sender as BaseEdit).Parent is LayoutControl layout 
        //        && layout.Parent is ISupportEditorChange support)
        //        support.EditorChanged(sender);
        //}
    public static void Align(LayoutControlGroup group)
        {
            foreach (BaseLayoutItem item in group.Items)
            {
                if (item is LayoutControlGroup sub)
                    Align(sub);
            }
            group.BestFit();
        }
    }
    //public interface ISetComboControl
    //{
    //    void SetComboControl(Control control, object value);
    //}
}
