using DevExpress.XtraEditors;
namespace AppCleaner
{
    public partial class MainForm : XtraForm
    {
        private FileScanner fileScanner;
        public MainForm()
        {
            InitializeComponent();
            // Создаём и добавляем FileScanner только если не в режиме дизайна
            if (this.IsDesignMode())
                return;
            fileScanner = new FileScanner
            {
                Name = "fileScanner",
                TabIndex = 0,
                Dock = DockStyle.Fill,
            };
            this.Controls.Add(fileScanner);
        }

        private readonly string _iniFilePath =
            Path.Combine(Application.StartupPath, "FileScanner.ini");

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.LoadState(_iniFilePath);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            this.SaveState(_iniFilePath);
            base.OnFormClosing(e);
        }
    }
}
