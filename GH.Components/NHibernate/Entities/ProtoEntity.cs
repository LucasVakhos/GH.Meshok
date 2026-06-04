// GH.Components\NHibernate\Entities\ProtoEntity.cs
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
namespace GH.Components
{
    public class ProtoEntity : IEditableObject
    {
        object savedCopy = null;
        [Key, Display(Name = "ID", Order = 0), Editable(false), ReadOnly(true)]
        public virtual int id { get; set; }
        [Display(Name = "Наименование", Order = 1)]
        public virtual string name { get; set; }
    public override bool Equals(object obj)
        {
            return obj is ProtoEntity entity && obj.GetType() == GetType() && id == entity.id;
        }
    public override int GetHashCode()
        {
            return 1877310944 + id.GetHashCode();
        }
    public virtual void BeginEdit()
        {
            if (savedCopy != null)
                return;
            savedCopy = MemberwiseClone();
        }
    public virtual void CancelEdit()
        {
            if (savedCopy == null || id == 0)
                return;
            var copyProps = GetOrCreatePropertyInfos(savedCopy as IEditableObject);
            foreach (var prop in GetOrCreatePropertyInfos(this))
            {
                var copyProp = copyProps.First(x => x.Name == prop.Name);
                prop.SetValue(this, copyProp.GetValue(savedCopy));
            }
            EndEdit();
        }
    public virtual void CancelEdit(string group = null)
        {
            if (savedCopy == null)
                return;
            var copyProps = GetOrCreatePropertyInfos(savedCopy as IEditableObject);
            foreach (var prop in GetOrCreatePropertyInfos(this).Where(x => group == null || x.GetCustomAttributes(true).OfType<DisplayAttribute>().Any(a => a.GroupName == group)))
            {
                var copyProp = copyProps.First(x => x.Name == prop.Name);
                prop.SetValue(this, copyProp.GetValue(savedCopy));
            }
        }
    public virtual void Assigne(object entity)
        {
            if (entity == null || GetType() != entity.GetType())
                return;
            var entityProps = GetOrCreatePropertyInfos(entity as IEditableObject);
            foreach (var prop in GetOrCreatePropertyInfos(this))
            {
                var entityProp = entityProps.First(x => x.Name == prop.Name);
                prop.SetValue(this, entityProp.GetValue(entity));
            }
            if (savedCopy != null)
                savedCopy = MemberwiseClone();
        }
    public virtual void EndEdit()
        {
            savedCopy = null;
        }

    public virtual bool HasChanges => WasChanged();
    public virtual bool WasChanged(string group = null)
        {
            if (savedCopy == null)
                return false;
            var copy = GetOrCreatePropertyInfos(savedCopy as IEditableObject);
            foreach (var prop_value in GetOrCreatePropertyInfos(this).Where(x => group == null || x.GetCustomAttributes(true).OfType<DisplayAttribute>().Any(a => a.GroupName == group)))
            {
                var copy_value = copy.First(x => x.Name == prop_value.Name);
                object obj1 = prop_value.GetValue(this);
                object obj2 = copy_value.GetValue(savedCopy);
                if (obj1 is ProtoEntity editable && editable.HasChanges)
                    return true;
                if (!Convert.Equals(obj1, obj2))
                    return true;
            }
            return false;
        }
    public virtual bool ValueChanged(string name)
        {
            if (savedCopy == null)
                return false;
            var this_value = GetOrCreatePropertyInfos(this as IEditableObject).FirstOrDefault(x => x.Name == name);
            if (this_value == null)
                return false;
            var copy_value = GetOrCreatePropertyInfos(savedCopy as IEditableObject).FirstOrDefault(x => x.Name == name);
            if (copy_value == null)
                return false;
            object obj1 = this_value.GetValue(this);
            object obj2 = copy_value.GetValue(savedCopy);
            return !Convert.Equals(obj1, obj2);
        }
    public static IEnumerable<PropertyInfo> GetOrCreatePropertyInfos(IEditableObject model, bool withID = false)
        {
            return from p in model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.FlattenHierarchy)
                   where p.SetMethod != null && p.GetMethod != null && (withID || p.Name != "id")
                   select p;
        }
    public override string ToString()
        {
            List<string> lst = new List<string>();
            foreach (PropertyInfo prop_value in GetOrCreatePropertyInfos(this, true).Where(x => x.GetCustomAttributes(true).OfType<DisplayAttribute>() != null))
            {
                var att = prop_value.GetCustomAttributes(true).OfType<DisplayAttribute>().FirstOrDefault<DisplayAttribute>();
                if (att == null)
                    continue;
                object val = prop_value.GetValue(this);
                string val_str = ValToSting(val);
                if (prop_value.Name == "id")
                    lst.Insert(0, $"{att.Name}: {val_str}");
                else
                if (prop_value.Name == "name")
                {
                    if (val != null)
                        lst.Insert(1, $"{att.Name}: {val_str}");
                }
                else
                    lst.Add($"{att.Name}: {val_str}");
            }
            string result = null;
            foreach (string item in lst)
            {
                if (result == null)
                    result = item;
                else
                    result += Environment.NewLine + item;
            }
            return result ?? base.ToString();
        }
    public virtual Field[] GetFieldTitles()
        {
            List<Field> titles = new List<Field>();
            foreach (var prop_value in GetOrCreatePropertyInfos(this, true).Where(x => x.GetCustomAttributes(true).OfType<DisplayAttribute>() != null))
            {
                var att = prop_value.GetCustomAttributes(true).OfType<DisplayAttribute>().FirstOrDefault<DisplayAttribute>();
                if (att == null)
                    continue;
                titles.Add(new Field(prop_value.Name, att.Name, prop_value.GetValue(this)));
            }
            return titles.ToArray();
        }
    private string ValToSting(object val)
        {
            if (val == null)
                return "(Пусто)";
            Type type = val.GetType();
            if (type == typeof(int) || type == typeof(Nullable<int>))
                return string.Format("{0:N0}", val);
            if (type == typeof(double) || type == typeof(Nullable<double>))
                return string.Format("{0:f2}", val);
            if (type == typeof(DateTime) || type == typeof(Nullable<DateTime>))
                return string.Format("{0:d}", val);
            return val.ToString();
        }
    public virtual object AsValue(string name)
        {
            var this_value = GetOrCreatePropertyInfos(this as IEditableObject).FirstOrDefault(x => x.Name == name);
            if (this_value == null)
                return null;
            return this_value.GetValue(this);
        }
    public virtual void AsValue(string name, object value)
        {
            var this_value = GetOrCreatePropertyInfos(this as IEditableObject).FirstOrDefault(x => x.Name == name);
            if (this_value == null)
                return;
            this_value.SetValue(this, value);
        }
    public virtual int AsInteger(string name)
        {
            return (int)AsValue(name);
        }
    public virtual void AsInteger(string name, int value)
        {
            AsValue(name, value);
        }
    public virtual bool AsBoolean(string name)
        {
            return (bool)AsValue(name);
        }
    public virtual void AsBoolean(string name, bool value)
        {
            AsValue(name, value);
        }
    }

    public enum InfoType
    {
        None,
        Info,
        Warning,
        Error
    }
}
