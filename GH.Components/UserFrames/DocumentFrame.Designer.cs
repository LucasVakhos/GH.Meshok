namespace GH.Components
{ 
    partial class DocumentFrame
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
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this.Pages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.idSpinEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lgRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PageView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PageEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EditGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcIdEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // Pages
            // 
            this.Pages.SelectedTabPage = this.PageView;
            this.Pages.SelectedTabPageIndex = 0;
            this.Pages.Size = new System.Drawing.Size(482, 309);
            // 
            // idSpinEdit
            // 
            this.idSpinEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.idSpinEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.idSpinEdit.Properties.Mask.EditMask = "N0";
            this.idSpinEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // dataSource
            // 
            this.dataSource.PageSupport.EditGroup = this.EditGroup;
            // 
            // lgRoot
            // 
            this.lgRoot.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.PageEdit});
            this.lgRoot.Size = new System.Drawing.Size(502, 437);
            // 
            // layoutControl
            // 
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(412, 249, 650, 501);
            this.layoutControl.OptionsFocus.AllowFocusGroups = false;
            this.layoutControl.OptionsFocus.AllowFocusReadonlyEditors = false;
            this.layoutControl.OptionsFocus.AllowFocusTabbedGroups = false;
            this.layoutControl.Size = new System.Drawing.Size(941, 437);
            this.layoutControl.Controls.SetChildIndex(this.idSpinEdit, 0);
            // 
            // PageView
            // 
            this.PageView.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcGrid});
            // 
            // PageEdit
            // 
            // 
            // emptySpaceItem2
            // 
            // 
            // EditGroup
            // 
            this.EditGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem1,
            this.lcIdEdit,
            this.emptySpaceItem2});
            // 
            // lcGrid
            // 
            // 
            // lcIdEdit
            // 
            // 
            // emptySpaceItem1
            // 
            // 
            // DocumentFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "DocumentFrame";
            this.Size = new System.Drawing.Size(941, 437);
            ((System.ComponentModel.ISupportInitialize)(this.Pages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.idSpinEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lgRoot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PageView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PageEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EditGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcIdEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
