using System.ComponentModel;
using System.Drawing.Design;

namespace GH.Components
{
    #region Other
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class DocSupport
    {
        private DataSource _owner;

        [GHProperty, DefaultValue(null)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Editor(typeof(FieldBoolListEditor), typeof(UITypeEditor))]
        public string CloseOpenField
        {
            get => _owner._closeOpenField;
            set
            {
                if (_owner._closeOpenField == value)
                    return;

                if (value == "")
                    value = null;

                _owner._closeOpenField = value;
            }
        }

        [GHProperty, DefaultValue(null)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Editor(typeof(FieldIntListEditor), typeof(UITypeEditor))]
        public string CountField
        {
            get => _owner._countField;
            set
            {
                if (_owner._countField == value)
                    return;

                if (value == "")
                    value = null;

                _owner._countField = value;
            }
        }

        [GHProperty, DefaultValue(null)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Editor(typeof(FieldIntListEditor), typeof(UITypeEditor))]
        public string StatusField
        {
            get => _owner._statusField;
            set
            {
                if (_owner._statusField == value)
                    return;

                if (value == "")
                    value = null;

                _owner._statusField = value;
            }
        }

        [GHProperty, DefaultValue(0)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int StatusOpened
        {
            get => _owner._statusOpened;
            set
            {
                if (_owner._statusClosed > 0 && value >= _owner._statusClosed)
                    return;
                _owner._statusOpened = value;

                if (_owner._statusClosed <= _owner._statusOpened)
                    _owner._statusClosed = _owner._statusOpened + 1;
            }
        }

        [GHProperty, DefaultValue(0)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int StatusClosed
        {
            get => _owner._statusClosed;
            set
            {
                if (_owner._statusOpened > 0 && value <= _owner._statusOpened)
                    return;
                _owner._statusClosed = value;

                if (_owner._statusOpened >= _owner._statusClosed)
                    _owner._statusOpened = _owner._statusClosed - 1;
            }
        }


        public DocSupport(DataSource owner)
        {
            _owner = owner;
        }

        public override string ToString()
        {
            string res = "";
            if (CloseOpenField != null)
                res += "CloseOpenField = " + CloseOpenField;


            if (CountField != null)
            {
                if (res != "")
                    res += ", ";
                res += "CountField = " + CountField;
            }

            if (StatusField != null)
            {
                if (res != "")
                    res += ", ";
                res += "StatusField = " + StatusField;
            }

            if (StatusOpened != 0 || StatusClosed != StatusOpened)
            {
                if (res != "")
                    res += ", ";
                res += $"StatusOpened = {StatusOpened}";
            }

            if (StatusClosed != 0 || StatusClosed != StatusOpened)
            {
                if (res != "")
                    res += ", ";
                res += $"StatusClosed = {StatusClosed}";
            }

            return res;
        }
    }

    #endregion
}

