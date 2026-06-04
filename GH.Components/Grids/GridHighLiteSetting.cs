using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
namespace GH.Components
{
    public partial class GridHighLiteSetting : XtraForm
    {
        protected GridHighLiteProcessor HighLiters => GridHighLiteProcessor.HighLiters;
    public GridHighLiteSetting()
        {
            InitializeComponent();
            SetupView(viewMain);
            SetupView(viewDetails);
            HighLiters.SetSources(bsMain, bsDetails);
        }
        void SetupView(GridView view)
        {
            view.OptionsCustomization.AllowColumnMoving = false;
            view.OptionsCustomization.AllowFilter = false;
            view.OptionsCustomization.AllowGroup = false;
            view.OptionsCustomization.AllowQuickHideColumns = false;
            view.OptionsCustomization.AllowRowSizing = false;
            view.OptionsCustomization.AllowSort = false;
            view.OptionsMenu.EnableColumnMenu = false;
            view.OptionsMenu.EnableFooterMenu = false;
            view.OptionsMenu.EnableGroupPanelMenu = false;
            view.OptionsView.EnableAppearanceEvenRow = true;
            view.OptionsView.ShowGroupPanel = false;
            view.OptionsView.ShowFooter = false;
            view.OptionsView.ColumnAutoWidth = true;
        }
    private void bsMain_PositionChanged(object sender, EventArgs e)
        {
        }
    private void actSave_Update(object sender, EventArgs e)
        {
            actSave.Enabled = HighLiters.HasChanges;
        }
    private void actLoadDefaults_Execute(object sender, EventArgs e)
        {
            HighLiters.LoadDefauls();
            HighLiters.Save();
        }
    private void GridHighLiteSetting_FormClosed(object sender, FormClosedEventArgs e)
        {
            HighLiters.Cancel();
        }
    private void actCancel_Execute(object sender, EventArgs e)
        {
            Close();
        }
    private void actSave_Execute(object sender, EventArgs e)
        {
            HighLiters.Save();
            Close();
        }
    }
}
