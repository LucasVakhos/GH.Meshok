namespace GH.Components
{
    partial class TestFrame
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.layoutControlGh1 = new GH.Controls.LayoutControlGh();
            this.comboBoxEdit2 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.comboBoxEdit1 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.value = new DevExpress.XtraEditors.XtraUserControl();
            this.rootGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcControl = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGh1)).BeginInit();
            this.layoutControlGh1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rootGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControlGh1
            // 
            this.layoutControlGh1.AllowCustomization = false;
            this.layoutControlGh1.Controls.Add(this.comboBoxEdit2);
            this.layoutControlGh1.Controls.Add(this.comboBoxEdit1);
            this.layoutControlGh1.Controls.Add(this.value);
            this.layoutControlGh1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControlGh1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGh1.Name = "layoutControlGh1";
            this.layoutControlGh1.OptionsFocus.AllowFocusGroups = false;
            this.layoutControlGh1.OptionsFocus.AllowFocusReadonlyEditors = false;
            this.layoutControlGh1.OptionsFocus.AllowFocusTabbedGroups = false;
            this.layoutControlGh1.Owner = this;
            this.layoutControlGh1.Root = this.rootGroup;
            this.layoutControlGh1.Size = new System.Drawing.Size(349, 150);
            this.layoutControlGh1.TabIndex = 0;
            this.layoutControlGh1.Text = "layoutControlGh1";
            this.layoutControlGh1.DefaultLayoutLoaded += new System.EventHandler(this.layoutControlGh1_DefaultLayoutLoaded);
            // 
            // comboBoxEdit2
            // 
            this.comboBoxEdit2.Location = new System.Drawing.Point(103, 125);
            this.comboBoxEdit2.Name = "comboBoxEdit2";
            this.comboBoxEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit2.Size = new System.Drawing.Size(241, 20);
            this.comboBoxEdit2.StyleController = this.layoutControlGh1;
            this.comboBoxEdit2.TabIndex = 6;
            // 
            // comboBoxEdit1
            // 
            this.comboBoxEdit1.Location = new System.Drawing.Point(103, 95);
            this.comboBoxEdit1.Name = "comboBoxEdit1";
            this.comboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit1.Properties.Items.AddRange(new object[] {
            "aaa",
            "bbb",
            "sss"});
            this.comboBoxEdit1.Size = new System.Drawing.Size(241, 20);
            this.comboBoxEdit1.StyleController = this.layoutControlGh1;
            this.comboBoxEdit1.TabIndex = 5;
            // 
            // value
            // 
            this.value.Location = new System.Drawing.Point(5, 5);
            this.value.Name = "value";
            this.value.Size = new System.Drawing.Size(339, 80);
            this.value.TabIndex = 4;
            // 
            // rootGroup
            // 
            this.rootGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.rootGroup.GroupBordersVisible = false;
            this.rootGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcControl,
            this.layoutControlItem1,
            this.layoutControlItem2});
            this.rootGroup.Name = "layoutControlGroup1";
            this.rootGroup.OptionsItemText.TextToControlDistance = 5;
            this.rootGroup.Size = new System.Drawing.Size(349, 150);
            this.rootGroup.TextVisible = false;
            // 
            // lcControl
            // 
            this.lcControl.Control = this.value;
            this.lcControl.Location = new System.Drawing.Point(0, 0);
            this.lcControl.Name = "layoutControlItem1";
            this.lcControl.Size = new System.Drawing.Size(349, 90);
            this.lcControl.TextSize = new System.Drawing.Size(0, 0);
            this.lcControl.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.comboBoxEdit1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 90);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(349, 30);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(93, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.comboBoxEdit2;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 120);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(349, 30);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(93, 13);
            // 
            // TestFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControlGh1);
            this.Name = "TestFrame";
            this.Size = new System.Drawing.Size(349, 150);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGh1)).EndInit();
            this.layoutControlGh1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rootGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.LayoutControlGh layoutControlGh1;
        private DevExpress.XtraLayout.LayoutControlGroup rootGroup;
        private DevExpress.XtraEditors.XtraUserControl value;
        private DevExpress.XtraLayout.LayoutControlItem lcControl;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit2;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
    }
}
