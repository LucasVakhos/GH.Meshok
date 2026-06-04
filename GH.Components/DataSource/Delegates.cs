using DevExpress.XtraEditors;
using System.Collections;
namespace GH.Components
{
    public delegate void OnGetSqlString(SqlTypes sqlType, BaseEntity item, out string sqlString);
    public delegate void GetRepository(out INHRepository repo);
    public class ActionCreateParams
    {
        public readonly ActionDataGh Action;
    public bool Save = false;
    public bool CreateNext = true;
    public ActionCreateParams(ActionDataGh action)
        {
            Action = action;
        }
    }

    public delegate void CreateAdditional(ActionCreateParams e);
    public class CaptionItem
    {
        public CaptionItem(EditTypes edit, string caption, string tooltip)
        {
            EditType = edit;
            Caption = caption;
            ToolTip = tooltip;
        }
        readonly public EditTypes EditType;
    public string Caption;
    public string ToolTip;
    }

    public delegate void GetActionCaptionHandler(CaptionItem e);
    public class ActionUpdateArgs
    {
        public bool Enabled;
        readonly public object Entity;
        readonly public EditTypes EditType;
    public string Caption;
    public string ToolTip;
    public ActionUpdateArgs(EditTypes editType, bool enabled, object entity, string caption, string tooltip)
        {
            EditType = editType;
            Enabled = enabled;
            Entity = entity;
            Caption = caption;
            ToolTip = tooltip;
        }
    }

    public delegate void ActionUpdateHandler(ActionUpdateArgs e);
    public class CloseActionUpdateArgs
    {
        readonly public bool Closed;
    public bool Enabled;
        readonly public object Entity;
    public string Caption;
    public string ToolTip;
    public CloseActionUpdateArgs(bool closed, bool enabled, object entity, string caption, string tooltip)
        {
            Closed = closed;
            Enabled = enabled;
            Entity = entity;
            Caption = caption;
            ToolTip = tooltip;
        }
    }

    public delegate void CloseActionUpdateHandler(CloseActionUpdateArgs e);
    public class ExportArgs
    {
        public string FileName;
    public ExportArgs(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = "export grid";
            FileName = fileName;
        }
    }

    public delegate void ExportArgumenstHandler(ExportArgs e);
    public class ValidateEventArgs
    {
        public BaseEdit Control;
    public bool IsValid;
    public ValidateEventArgs(BaseEdit control, bool isValid)
        {
            Control = control;
            IsValid = isValid;
        }
    }

    public delegate void ValidateHandler(object sender, ValidateEventArgs e);
    public class ReadOnlyEventArgs
    {
        private BaseEdit _control;
    private bool _readOnly;
    public ReadOnlyEventArgs(BaseEdit control)
        {
            _control = control;
            _readOnly = false;
        }

    public BaseEdit Control { get => _control; }

    public bool ReadOnly { get => _readOnly; set => _readOnly = value; }
    }

    public delegate void ReadOnlyEventHandler(object sender, ReadOnlyEventArgs e);
    public delegate void GetWhereParamsHandler(object sender, IDictionary whereParams);
    public delegate void ActionWithoutParams();
    public delegate void OnAction(out BaseEntity item);
    public delegate void EditGrantHandler(object sender, EditGrants e);
    public delegate void OpenHandler(out IList list);
    public delegate bool CanEditHandler();
    public delegate void NeedOpenHandler(out bool needOpen);
    public class YesNoTextArgs
    {
        public readonly SqlTypes SqlType;
    protected readonly AbstractEntity Entity;
    public readonly bool Closed;
    public string QueryText;
    public YesNoTextArgs(SqlTypes sqlType, AbstractEntity entity, bool closed)
        {
            Entity = entity;
            Closed = closed;
            SqlType = sqlType;
            switch (SqlType)
            {
                case SqlTypes.CloseDocSql:
                    if (closed)
                        QueryText = "Желаете открыть?";
                    else
                        QueryText = "Желаете закрыть?";
                    break;
                case SqlTypes.DeleteSql:
                    if (closed)
                        QueryText = "Удалить документ?";
                    else
                        QueryText = "Удалить запись?";
                    break;
                default:
                    break;
            }
        }
    public override string ToString()
        {
            return Entity.ToString() + "\r\n\r\n" + QueryText;
        }
    }

    public delegate void YesNoTextHandler(YesNoTextArgs e);
}
