using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using GH.Components.Attributes;
using GH.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace GH.NHibernate
{
    public struct Field
    {
        public const string ctrlPrefix = "edit";
        public const string layouPrefix = "lc";
        public const string groupSuffix = "Group";

        public static Field[] GetFields<TEntity, TAttribute>(string[] except = null, bool withKey = false)
            where TEntity : AbstractEntity
            where TAttribute : UpdatablePropertyAttribute

        {
            List<Field> fields = new List<Field>();

            foreach (PropertyInfo info in typeof(TEntity).GetProperties().Where(x =>
                {
                    var att = x.GetCustomAttribute<TAttribute>();
                    if (null == att)
                        return false;

                    if (att.Key == true)
                        return withKey;

                    if (except != null && except.Contains(x.Name))
                        return false;

                    return true;
                }).OrderBy(x => x.GetCustomAttribute<TAttribute>().Order))
            {
                fields.Add(new Field(info));
            }
            return fields.ToArray();
        }

        public static Field[] GetFields<TAttribute>(AbstractEntity entity, string[] except = null, bool withKey = false)
            where TAttribute : UpdatablePropertyAttribute
        {
            List<Field> fields = new List<Field>();

            foreach (PropertyInfo info in entity.GetType().GetProperties().Where(x =>
            {
                var att = x.GetCustomAttribute<TAttribute>();
                if (null == att)
                    return false;

                if (att.Key == true)
                    return withKey;

                if (except != null && except.Contains(x.Name))
                    return false;

                return true;
            }).OrderBy(x => x.GetCustomAttribute<TAttribute>().Order))
            {
                fields.Add(new Field(info, entity));
            }
            return fields.ToArray();
        }

        public readonly bool Key;
        public readonly string Name;
        public readonly string Caption;
        public readonly string ToolTip;
        public readonly object Value;
        public readonly Type FieldType;
        public readonly string DisplayFormat;
        public readonly string Format;
        public readonly int Order;
        public readonly EditorType EditorType;
        public readonly string Group;
        public readonly string SubGroup;
        public readonly object Default;
        public readonly bool ReadOnly;
        public readonly bool Required;
        public readonly CharacterCasing CharacterCasing;
        public readonly int MaxLength;

        public bool? AsBoolean
        {
            get
            {
                if (Value == null)
                    return null;
                else
                    return Convert.ToBoolean(Value);
            }
        }
        public int? AsInteger
        {
            get
            {
                if (Value == null)
                    return null;
                else
                    return Convert.ToInt32(Value);
            }
        }
        public double? AsDouble
        {
            get
            {
                if (Value == null)
                    return null;
                else
                    return Convert.ToDouble(Value);
            }
        }
        public DateTime? AsDateTime
        {
            get
            {
                if (Value == null)
                    return null;
                else
                    return Convert.ToDateTime(Value);
            }
        }
        public string AsString
        {
            get
            {
                if (Value == null)
                    return null;
                else
                    return Value.ToString();
            }
        }


        public string ControlName => ctrlPrefix + Name;
        public string LayoutName => layouPrefix + Name;
        public string GroupName => Group + groupSuffix;
        public string SubGroupName => SubGroup + groupSuffix;
        public string CaptionText => Caption + ":";

        public AbstractEntity Entity { get; }

        public Field(PropertyInfo propInfo, AbstractEntity entity = null) : this()
        {
            Entity = entity;
            UpdatablePropertyAttribute attr = propInfo.GetCustomAttribute<UpdatablePropertyAttribute>();

            Key = attr.Key;
            Caption = attr.Caption;
            ToolTip = attr.ToolTip;
            Group = attr.Group;
            SubGroup = attr.SubGroup;
            Order = attr.Order;
            EditorType = attr.EditorType;
            Default = attr.Default;
            ReadOnly = Key || attr.ReadOnly;
            Required = attr.Required;
            CharacterCasing = attr.CharacterCasing;
            MaxLength = attr.MaxLength;

            Name = propInfo.Name;
            FieldType = propInfo.PropertyType;

            if (FieldType == typeof(int))
            {
                DisplayFormat = "{0:N0}";
                Format = "{N0}";
            }
            else
            if (FieldType == typeof(double))
            {
                DisplayFormat = "{0:f2}";
                Format = "{f2}";
            }
            else
            if (FieldType == typeof(DateTime))
            {
                DisplayFormat = "{d}";
                Format = "{d}";
            }
            else
            {
                DisplayFormat = null;
                Format = null;
            }
            if (entity != null)
                Value = propInfo.GetValue(entity);
            else
                Value = null;

        }

        BaseControl control;
        public BaseControl CreateControl()
        {
            switch (EditorType)
            {
                case EditorType.Text:
                    if (FieldType == typeof(int) || FieldType == typeof(double))
                    {
                        SpinEdit spin = new SpinEdit();
                        spin.Properties.DisplayFormat.FormatString = DisplayFormat;
                        spin.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        spin.Properties.EditFormat.FormatString = Format;
                        spin.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        control = spin;
                    }
                    else
                    if (FieldType == typeof(DateTime))
                    {
                        DateEdit spin = new DateEdit();
                        spin.Properties.DisplayFormat.FormatString = DisplayFormat;
                        spin.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        spin.Properties.EditFormat.FormatString = Format;
                        spin.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        control = spin;
                    }
                    else
                    if (FieldType == typeof(TimeSpan))
                    {
                        TimeSpanEdit spin = new TimeSpanEdit();
                        spin.Properties.DisplayFormat.FormatString = DisplayFormat;
                        spin.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        spin.Properties.EditFormat.FormatString = Format;
                        spin.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        control = spin;
                    }
                    else
                    {
                        control = new TextEdit();
                    }
                    break;
                case EditorType.Combo:


                    if (FieldType.IsEnum)
                    {
                        LookUpEdit combo = new LookUpEdit();
                        combo.Properties.DataSource = GetEnum(FieldType);
                        combo.Properties.ShowFooter = false;
                        combo.Properties.ShowHeader = false;
                        combo.Properties.SearchMode = SearchMode.AutoFilter;
                        combo.Properties.ValueMember = "Key";
                        combo.Properties.KeyMember = "Key";
                        combo.Properties.DisplayMember = "Value";
                        combo.Properties.UseCtrlScroll = true;
                        control = combo;
                    }
                    else
                    {
                        ComboBoxEdit combo = new ComboBoxEdit();
                        combo.Properties.UseCtrlScroll = true;
                        control = combo;
                    }
                    break;
                case EditorType.Check:
                    control = new CheckEdit();
                    control.Text = null;
                    break;
                case EditorType.Button:
                    control = new SimpleButton();
                    control.Text = Caption;
                    break;
                case EditorType.PathSeacher:
                    PathSeacher path = new PathSeacher();
                    path.EndInit();
                    control = path;
                    break;
                default:
                    break;
            }
            if (control == null)
                return null;

            control.ToolTip = ToolTip;
            if (control is BaseEdit baseEdit)
            {

                baseEdit.ReadOnly = ReadOnly;
                if (control is TextEdit textEdit)
                {
                    textEdit.Properties.MaxLength = MaxLength;
                    textEdit.Properties.CharacterCasing = CharacterCasing;
                    textEdit.Properties.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Character;
                }
            }
            return control;
        }

        private KeyValuePair<object, string>[] GetEnum(Type fieldType)
        {
            Dictionary<object, string> keys = new Dictionary<object, string>();
            try
            {
                foreach (Enum item in Enum.GetValues(fieldType))
                    if (!keys.ContainsKey(item))
                        keys.Add(item, item.ToString());
            }
            catch (Exception)
            {

                throw;
            }
            return keys.ToArray();
        }
    }

}
