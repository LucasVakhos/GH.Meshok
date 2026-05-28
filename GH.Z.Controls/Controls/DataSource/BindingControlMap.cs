using DevExpress.XtraEditors;
using System.Collections.Generic;

namespace GH.Components
{
    public class BindingControlMap
    {
        private BaseEdit _control;
        private bool _readOnly;

        public BindingControlMap(BaseEdit control)
        {
            _control = control;
            _readOnly = control.ReadOnly;
        }

        public BaseEdit Control { get => _control; }
        public bool ReadOnly { get => _readOnly; }

        public override bool Equals(object obj)
        {
            return obj is BindingControlMap && _control == (obj as BindingControlMap).Control;
        }

        public override int GetHashCode()
        {
            var hashCode = 119239435;
            hashCode = hashCode * -1521134295 + EqualityComparer<BaseEdit>.Default.GetHashCode(_control);
            hashCode = hashCode * -1521134295 + _readOnly.GetHashCode();
            return hashCode;
        }
    }


}

