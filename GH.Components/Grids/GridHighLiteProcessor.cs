using DevExpress.Utils;
using System.ComponentModel;
using System.Reflection;
using System.Xml.Linq;
namespace GH.Components
{
    public class GridHighLiteProcessor
    {
        private static GridHighLiteProcessor _HLproc;
    public static GridHighLiteProcessor HighLiters
        {
            get
            {
                if (_HLproc == null)
                    _HLproc = new GridHighLiteProcessor();
                return _HLproc;
            }
        }

    internal static BindingSource _bsMain;
    internal static BindingSource _bsDetails;
    internal static IGridHighLiter MainCurrent => _bsMain.Current as IGridHighLiter;
    public static void SetRowCellStyle(object entity, AppearanceObject appearance, bool focused)
        {
            HighLiters.GetRowCellStyle(entity, appearance, focused);
        }
    internal static void ShowSetup()
        {
            HighLiters.Show();
        }
    internal static void SelectType(Type concreteType)
        {
            HighLiters.Select(concreteType);
        }
    internal static void DeSelectType()
        {
            HighLiters.DeSelect();
        }
    private static void _bsMain_PositionChanged(object sender, EventArgs e)
        {
            HighLiters._selected = MainCurrent;
            _bsDetails.DataSource = MainCurrent?.Items;
        }
        IList<IGridHighLiter> _highLiters = new List<IGridHighLiter>();
    private IGridHighLiter _selected;
    public IGridHighLiter Selected => _selected;
    public IList<IGridHighLiter> GetList() => _highLiters;
    public void Select(Type bindable)
        {
            _selected = _highLiters.Where(h => h.SupportInterface(bindable)).FirstOrDefault();
        }
    internal void DeSelect()
        {
            _selected = null;
        }
    public void Add(IGridHighLiter highLiter)
        {
            _highLiters.Add(highLiter);
        }
    public void GetRowCellStyle(object entity, AppearanceObject appearance, bool focused)
        {
            if (Selected != null)
            {
                Selected.GetRowCellStyle(entity, appearance, focused);
                return;
            }
            foreach (IGridHighLiter item in _highLiters)
                if (item.GetRowCellStyle(entity, appearance, focused))
                    return;
        }
    private void ClearSources()
        {
            _bsMain.PositionChanged -= _bsMain_PositionChanged;
            _bsMain = null;
            _bsDetails = null;
        }
    internal void SetSources(BindingSource bsMain, BindingSource bsDetails)
        {
            _bsMain = bsMain;
            _bsDetails = bsDetails;
            _bsMain.PositionChanged += _bsMain_PositionChanged;
            _bsMain.DataSource = GetList();
        }
    public void Show()
        {
            using (GridHighLiteSetting setting = new GridHighLiteSetting())
            {
                setting.ShowDialog();
                ClearSources();
            }
        }
    public void Cancel()
        {
            foreach (IGridHighLiter item in _highLiters)
                item.Cancel();
        }

    public bool HasChanges
        {
            get
            {
                foreach (IGridHighLiter item in _highLiters)
                    if (item.HasChanges)
                        return true;
                return false;
            }
        }
    internal void Save()
        {
            foreach (IGridHighLiter item in _highLiters)
                item.Save();
        }
    internal void LoadDefauls()
        {
            MainCurrent?.LoadDefauls();
            _bsDetails.ResetBindings(false);
        }
    }

    public interface IGridHighLiter
    {
        string Name { get; }
        IList<HighLiterItem> Items { get; }
        bool HasChanges { get; }
        void Cancel();
        bool GetRowCellStyle(object entity, AppearanceObject appearance, bool focused);
        void LoadDefauls();
        void Save();
        bool SupportInterface(Type bindable);
    }

    public class GridHighLiter<T> : IGridHighLiter where T : BaseEntity
    {
        IList<HighLiterItem> _items = new List<HighLiterItem>();
    public IList<HighLiterItem> Items => _items;
    protected object _entity;
    public bool HasChanges
        {
            get
            {
                foreach (HighLiterItem item in _items)
                    if (item.HasChanges)
                        return true;
                return false;
            }
        }

    public int EntityId => GetEntityId();
    protected virtual Type Intf => typeof(IGridHighLiter);
    private readonly string _name;
        [UpdatableProperty(Caption = "Группа подсветки", ToolTip = "Одна из групп подсветки")]
        public string Name => _name;
    public GridHighLiter(string name)
        {
            _name = name;
            foreach (var item in GetList())
                _items.Add(new HighLiterItem(item));
            LoadFromXML();
        }
    public bool SupportInterface(Type bindable)
        {
            return bindable.GetInterfaces().Contains(Intf);
        }
    private IList<T> GetList()
        {
            return new NHRepository<T>().AsTypeList();
        }
    protected virtual int GetEntityId()
        {
            if (_entity is HighLiterItem item)
                return item.Id;
            return -1000;
        }
    public virtual void LoadDefauls()
        {
            foreach (HighLiterItem item in Items)
                item.NewAppearance();
        }
    public bool GetRowCellStyle(object entity, AppearanceObject appearance, bool focused)
        {
            if (entity == null)
                return false;
            _entity = entity;
            if (SpecialRowCellStyle(appearance, focused))
                return true;
            HighLiterItem item = _items.Where(x => x.Id == EntityId && x.Enabled).FirstOrDefault();
            if (item != null)
            {
                appearance.Font = new Font(AppearanceObject.DefaultFont, item.FontStyle);
                if (!focused || _entity is HighLiterItem)
                {
                    appearance.ForeColor = item.ForeColor;
                    appearance.BackColor = item.BackColor;
                }
                return true;
            }
            return false;
        }
    protected virtual bool SpecialRowCellStyle(AppearanceObject appearance, bool focused)
        {
            return false;
        }
    public override bool Equals(object obj)
        {
            var liter = obj as GridHighLiter<T>;
            return liter != null &&
                   Name == liter.Name;
        }
    public override int GetHashCode()
        {
            var hashCode = 5743141;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
    public void Cancel()
        {
            foreach (var item in _items)
                item.CancelEdit();
        }
    public void Save()
        {
            foreach (HighLiterItem item in Items)
                item.Save();
            SaveToXML();
        }
    private void LoadFromXML()
        {
            string file_name = RunContext.GetConfigsPath(this);
            if (!File.Exists(file_name))
            {
                LoadDefauls();
                return;
            }
            XDocument doc = XDocument.Load(file_name);
            foreach (HighLiterItem item in Items)
            {
                XElement elements = doc.Descendants(nameof(HighLiterItem)).Where(x => x.FirstAttribute.Value == item.Id.ToString()).FirstOrDefault();
                if (elements != null)
                {
                    foreach (XElement element in elements.Elements())
                    {
                        switch (element.Name.ToString())
                        {
                            case "Enabled":
                                item.Enabled = bool.Parse(element.Value);
                                continue;
                            case "ForeColor":
                                item.ForeColor = Color.FromArgb(int.Parse(element.Value));
                                continue;
                            case "BackColor":
                                item.BackColor = Color.FromArgb(int.Parse(element.Value));
                                continue;
                            case "FontBold":
                                item.FontBold = bool.Parse(element.Value);
                                continue;
                            case "FontItalic":
                                item.FontItalic = bool.Parse(element.Value);
                                continue;
                            case "FontUnderline":
                                item.FontUnderline = bool.Parse(element.Value);
                                continue;
                            case "FontStrikeout":
                                item.FontStrikeout = bool.Parse(element.Value);
                                continue;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    private void SaveToXML()
        {
            XElement root = new XElement(nameof(HighLiterItem) + "s");
            XDocument doc = new XDocument(root);
            string file_name = RunContext.GetConfigsPath(this);
            Directory.CreateDirectory(Path.GetDirectoryName(file_name));
            foreach (HighLiterItem item in Items)
            {
                XElement element = new XElement(nameof(HighLiterItem),
                        new XAttribute("ID", item.Id),
                        new XElement("Enabled", item.Enabled),
                        new XElement("ForeColor", item.ForeColor.ToArgb()),
                        new XElement("BackColor", item.BackColor.ToArgb()),
                        new XElement("FontBold", item.FontBold),
                        new XElement("FontItalic", item.FontItalic),
                        new XElement("FontUnderline", item.FontUnderline),
                        new XElement("FontStrikeout", item.FontStrikeout));
                root.Add(element);
            }
            doc.Save(file_name);
        }
    }

    public class HighLiterItem : IEditableObject
    {
        const string rootGroup = "<Root>";
        const string editGroup = rootGroup + "/" + "<EditGroup>";
        const string styleGroup = editGroup + "/" + "<SyleGroup>";
        object _savedCopy = null;
    public bool Saved => _savedCopy == null;
    public FontStyle FontStyle
        {
            get
            {
                FontStyle result = FontStyle.Regular;
                if (FontBold)
                    result = result | FontStyle.Bold;
                if (FontItalic)
                    result = result | FontStyle.Italic;
                if (FontUnderline)
                    result = result | FontStyle.Underline;
                if (FontStrikeout)
                    result = result | FontStyle.Strikeout;
                return result;
            }
        }

    private BaseEntity _prototype;
    public BaseEntity Prototype => _prototype;
    public bool HasChanges => WasChanged();
        [UpdatableProperty(Caption = "ID подсветки", Group = editGroup, Order = 0)]
        public int Id { get; }
        [UpdatableProperty(Caption = "Имя подсветки", Group = editGroup, Order = 1)]
        public string Name { get; }
        [UpdatableProperty(Caption = "Включена", ToolTip = "Подсветка включена для обработки", Group = editGroup, Order = 2)]
        public bool Enabled { get; set; }
        [UpdatableProperty(Caption = "Цвет шрифта", ToolTip = "Цвет шрифта", Group = editGroup, Order = 3)]
        public Color ForeColor { get; set; }
        [UpdatableProperty(Caption = "Цвет фона", ToolTip = "Цвет фона", Group = editGroup, Order = 4)]
        public Color BackColor { get; set; }
        [UpdatableProperty(Caption = "Жирный", Group = styleGroup, Order = 0)]
        public bool FontBold { get; set; }
        [UpdatableProperty(Caption = "Наклонный", Group = styleGroup, Order = 1)]
        public bool FontItalic { get; set; }
        [UpdatableProperty(Caption = "Подчеркнутый", Group = styleGroup, Order = 2)]
        public bool FontUnderline { get; set; }
        [UpdatableProperty(Caption = "Перечеркнутый", Group = styleGroup, Order = 3)]
        public bool FontStrikeout { get; set; }

    public HighLiterItem(BaseEntity item)
        {
            _prototype = item;
            Id = item.id;
            Name = GetOrCreatePropertyInfos(item).Where(x => x.Name.ToUpper().EndsWith("_NAME")).FirstOrDefault().GetValue(item).ToString();
        }
    public void NewAppearance()
        {
            Enabled = false;
            ForeColor = Color.Transparent;
            BackColor = Color.Transparent;
            FontBold = false;
            FontItalic = false;
            FontUnderline = false;
            FontStrikeout = false;
            Save();
        }
    public static Color Invert(Color color)
        {
            return Color.FromArgb(color.ToArgb() ^ 0xFFFFFF);
        }
    internal static IEnumerable<PropertyInfo> GetOrCreatePropertyInfos(object model)
        {
            return from p in model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty)
                   where p.SetMethod != null && p.GetMethod != null
                   select p;
        }
    public void BeginEdit()
        {
            GridHighLiteProcessor._bsDetails.ResetCurrentItem();
            if (!Saved)
                return;
            _savedCopy = MemberwiseClone();
        }
    public void Save()
        {
            _savedCopy = null;
        }
    public void EndEdit() { }
    public void CancelEdit()
        {
            if (Saved)
                return;
            var copyProps = GetOrCreatePropertyInfos(_savedCopy);
            foreach (var prop in GetOrCreatePropertyInfos(this))
            {
                var copyProp = copyProps.First(x => x.Name == prop.Name);
                prop.SetValue(this, copyProp.GetValue(_savedCopy));
            }
            Save();
        }
    private bool WasChanged()
        {
            if (_savedCopy == null)
                return false;
            return !Equals(_savedCopy);
        }
    public override bool Equals(object obj)
        {
            var item = obj as HighLiterItem;
            return item != null &&
                   Id == item.Id &&
                   Enabled == item.Enabled &&
                   FontBold == item.FontBold &&
                   FontItalic == item.FontItalic &&
                   FontUnderline == item.FontUnderline &&
                   FontStrikeout == item.FontStrikeout &&
                   EqualityComparer<Color>.Default.Equals(ForeColor, item.ForeColor) &&
                   EqualityComparer<Color>.Default.Equals(BackColor, item.BackColor);
        }
    public override int GetHashCode()
        {
            var hashCode = -1661384260;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + Enabled.GetHashCode();
            hashCode = hashCode * -1521134295 + FontBold.GetHashCode();
            hashCode = hashCode * -1521134295 + FontItalic.GetHashCode();
            hashCode = hashCode * -1521134295 + FontUnderline.GetHashCode();
            hashCode = hashCode * -1521134295 + FontStrikeout.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Color>.Default.GetHashCode(ForeColor);
            hashCode = hashCode * -1521134295 + EqualityComparer<Color>.Default.GetHashCode(BackColor);
            return hashCode;
        }
    }
}
