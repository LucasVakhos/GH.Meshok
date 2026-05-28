using DevExpress.XtraLayout;
using System.ComponentModel;
namespace GH.Components
{
    [ProvideProperty("Grid", typeof(GridGh))]
    public class LayoutGridControlGh : LayoutControlGh
    {
        private GridGh gridGh;
        private ViewGh viewGh;
        //GridGh gridGh = new GridGh();
        //ViewGh viewGh = new ViewGh(); 
        private bool _intialization;
        public LayoutGridControlGh() : base()
        {
            InitializeGrid();
        }
        [Description("A data-aware control that displays data in one of several views, enables editing data, provides data filtering, sorting, grouping and summary calculation features.")]
        //[Designer("DevExpress.XtraGrid.Design.GridControlDesigner, DevExpress.XtraGrid.v17.2.Design", typeof(IDesigner))]
        [GH.ComponentsProperty]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GridGh Grid => gridGh;
        [GH.ComponentsProperty]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ViewGh GridView => viewGh;
        public override void BeginInit()
        {
            base.BeginInit();
            _intialization = true;
        }
        public override void EndInit()
        {
            if (Root != null && _intialization && DesignMode && Owner != null)
            {
                Root.Name = "lgRoot";
                Root.Text = "Root";
                Root.CustomizationFormText = "Root";
                gridGh.BeginInit();
                viewGh.BeginInit();
                SuspendLayout();
                LayoutControlGroup DridDroup = Items.OfType<LayoutControlGroup>().Where(x => x.Name == "wiewGroup").FirstOrDefault();
                if (DridDroup == null)
                {
                    DridDroup = AddGroup("Просмотр");
                    DridDroup.Name = "wiewGroup";
                    DridDroup.CustomizationFormText = "Wiew Group";
                    Root.Items.AddRange(new BaseLayoutItem[] { DridDroup });
                }
                //Controls.Add(gridGh);
                LayoutControlItem lcGrid = Items.OfType<LayoutControlItem>().Where(x => x.Name == "lcGrid").FirstOrDefault();
                if (lcGrid == null)
                {
                    lcGrid = new LayoutControlItem()
                    {
                        Name = "lcGrid",
                        Text = "Grid",
                        CustomizationFormText = "Grid",
                        Control = gridGh
                    };
                    DridDroup.Add(lcGrid);
                }
                else
                {
                    if (lcGrid.Parent == null)
                        DridDroup.Add(lcGrid);
                }
                gridGh.EndInit();
                viewGh.EndInit();
            }
            _intialization = false;
            base.EndInit();
            ResumeLayout(false);
        }
        private void InitializeGrid()
        {
            this.gridGh = new GH.Components.GridGh();
            this.viewGh = new GH.Components.ViewGh();
            ((System.ComponentModel.ISupportInitialize)(this.gridGh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewGh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // gridGh
            // 
            this.gridGh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridGh.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gridGh.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gridGh.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gridGh.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gridGh.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gridGh.Location = new System.Drawing.Point(0, 0);
            this.gridGh.MainView = this.viewGh;
            this.gridGh.Name = "gridGh";
            this.gridGh.Size = new System.Drawing.Size(400, 200);
            this.gridGh.TabIndex = 0;
            this.gridGh.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewGh});
            // 
            // viewGh
            // 
            this.viewGh.GridControl = this.gridGh;
            this.viewGh.Name = "viewGh";
            // 
            // LayoutGridGh
            // 
            this.OptionsFocus.AllowFocusGroups = false;
            this.OptionsFocus.AllowFocusReadonlyEditors = false;
            this.OptionsFocus.AllowFocusTabbedGroups = false;
            ((System.ComponentModel.ISupportInitialize)(this.gridGh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewGh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
