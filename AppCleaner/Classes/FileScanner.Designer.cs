//FileScanner.Designer.cs
using DevExpress.XtraEditors.Controls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace AppCleaner
{
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    partial class FileScanner
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            components = new Container();
            EditorButtonImageOptions editorButtonImageOptions1 = new EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FileScanner));
            EditorButtonImageOptions editorButtonImageOptions2 = new EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject7 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject8 = new DevExpress.Utils.SerializableAppearanceObject();
            layoutControl = new DevExpress.XtraDataLayout.DataLayoutControl();
            cboPlaceFolder = new DevExpress.XtraEditors.ComboBoxEdit();
            cboSelectToDo = new DevExpress.XtraEditors.ComboBoxEdit();
            btnBegin = new DevExpress.XtraEditors.SimpleButton();
            btnCancel = new DevExpress.XtraEditors.SimpleButton();
            cboSearchExt = new DevExpress.XtraEditors.ComboBoxEdit();
            progressBar = new DevExpress.XtraEditors.ProgressBarControl();
            cboSearchFolder = new DevExpress.XtraEditors.ComboBoxEdit();
            foundFolders = new DevExpress.XtraEditors.TextEdit();
            foundFiles = new DevExpress.XtraEditors.TextEdit();
            logMemo = new DevExpress.XtraEditors.MemoEdit();
            cboDRY_RUN = new DevExpress.XtraEditors.ComboBoxEdit();
            txtFind = new DevExpress.XtraEditors.TextEdit();
            txtReplace = new DevExpress.XtraEditors.TextEdit();
            cboNET = new DevExpress.XtraEditors.ComboBoxEdit();
            btnSave = new DevExpress.XtraEditors.SimpleButton();
            Root = new DevExpress.XtraLayout.LayoutControlGroup();
            lgTop = new DevExpress.XtraLayout.LayoutControlGroup();
            lcSearchMask = new DevExpress.XtraLayout.LayoutControlItem();
            emptySearchExt = new DevExpress.XtraLayout.EmptySpaceItem();
            lgFolders = new DevExpress.XtraLayout.LayoutControlGroup();
            lcSearchFolder = new DevExpress.XtraLayout.LayoutControlItem();
            lcPlaceFolder = new DevExpress.XtraLayout.LayoutControlItem();
            lgToDo = new DevExpress.XtraLayout.LayoutControlGroup();
            lcToDo = new DevExpress.XtraLayout.LayoutControlItem();
            lgOptions = new DevExpress.XtraLayout.LayoutControlGroup();
            lcDRY_RUN = new DevExpress.XtraLayout.LayoutControlItem();
            lcFind = new DevExpress.XtraLayout.LayoutControlItem();
            lcReplace = new DevExpress.XtraLayout.LayoutControlItem();
            emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            lcNET_Version = new DevExpress.XtraLayout.LayoutControlItem();
            lgProcess = new DevExpress.XtraLayout.LayoutControlGroup();
            lcProgress = new DevExpress.XtraLayout.LayoutControlItem();
            lcBegin = new DevExpress.XtraLayout.LayoutControlItem();
            lcCancel = new DevExpress.XtraLayout.LayoutControlItem();
            lgInfo = new DevExpress.XtraLayout.LayoutControlGroup();
            lcLogMemo = new DevExpress.XtraLayout.LayoutControlItem();
            lgPocessed = new DevExpress.XtraLayout.LayoutControlGroup();
            lcFoundFiles = new DevExpress.XtraLayout.LayoutControlItem();
            emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            lcFoundFolders = new DevExpress.XtraLayout.LayoutControlItem();
            openFolderDlg = new FolderBrowserDialog();
            layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            bsFileScanner = new BindingSource(components);
            openFileDlg = new OpenFileDialog();
            ((ISupportInitialize)layoutControl).BeginInit();
            layoutControl.SuspendLayout();
            ((ISupportInitialize)cboPlaceFolder.Properties).BeginInit();
            ((ISupportInitialize)cboSelectToDo.Properties).BeginInit();
            ((ISupportInitialize)cboSearchExt.Properties).BeginInit();
            ((ISupportInitialize)progressBar.Properties).BeginInit();
            ((ISupportInitialize)cboSearchFolder.Properties).BeginInit();
            ((ISupportInitialize)foundFolders.Properties).BeginInit();
            ((ISupportInitialize)foundFiles.Properties).BeginInit();
            ((ISupportInitialize)logMemo.Properties).BeginInit();
            ((ISupportInitialize)cboDRY_RUN.Properties).BeginInit();
            ((ISupportInitialize)txtFind.Properties).BeginInit();
            ((ISupportInitialize)txtReplace.Properties).BeginInit();
            ((ISupportInitialize)cboNET.Properties).BeginInit();
            ((ISupportInitialize)Root).BeginInit();
            ((ISupportInitialize)lgTop).BeginInit();
            ((ISupportInitialize)lcSearchMask).BeginInit();
            ((ISupportInitialize)emptySearchExt).BeginInit();
            ((ISupportInitialize)lgFolders).BeginInit();
            ((ISupportInitialize)lcSearchFolder).BeginInit();
            ((ISupportInitialize)lcPlaceFolder).BeginInit();
            ((ISupportInitialize)lgToDo).BeginInit();
            ((ISupportInitialize)lcToDo).BeginInit();
            ((ISupportInitialize)lgOptions).BeginInit();
            ((ISupportInitialize)lcDRY_RUN).BeginInit();
            ((ISupportInitialize)lcFind).BeginInit();
            ((ISupportInitialize)lcReplace).BeginInit();
            ((ISupportInitialize)emptySpaceItem2).BeginInit();
            ((ISupportInitialize)lcNET_Version).BeginInit();
            ((ISupportInitialize)lgProcess).BeginInit();
            ((ISupportInitialize)lcProgress).BeginInit();
            ((ISupportInitialize)lcBegin).BeginInit();
            ((ISupportInitialize)lcCancel).BeginInit();
            ((ISupportInitialize)lgInfo).BeginInit();
            ((ISupportInitialize)lcLogMemo).BeginInit();
            ((ISupportInitialize)lgPocessed).BeginInit();
            ((ISupportInitialize)lcFoundFiles).BeginInit();
            ((ISupportInitialize)emptySpaceItem1).BeginInit();
            ((ISupportInitialize)layoutControlItem1).BeginInit();
            ((ISupportInitialize)lcFoundFolders).BeginInit();
            ((ISupportInitialize)layoutControlItem2).BeginInit();
            ((ISupportInitialize)layoutControlItem3).BeginInit();
            ((ISupportInitialize)layoutControlItem4).BeginInit();
            ((ISupportInitialize)layoutControlItem5).BeginInit();
            ((ISupportInitialize)bsFileScanner).BeginInit();
            SuspendLayout();
            // 
            // layoutControl
            // 
            layoutControl.Controls.Add(cboPlaceFolder);
            layoutControl.Controls.Add(cboSelectToDo);
            layoutControl.Controls.Add(btnBegin);
            layoutControl.Controls.Add(btnCancel);
            layoutControl.Controls.Add(cboSearchExt);
            layoutControl.Controls.Add(progressBar);
            layoutControl.Controls.Add(cboSearchFolder);
            layoutControl.Controls.Add(foundFolders);
            layoutControl.Controls.Add(foundFiles);
            layoutControl.Controls.Add(logMemo);
            layoutControl.Controls.Add(cboDRY_RUN);
            layoutControl.Controls.Add(txtFind);
            layoutControl.Controls.Add(txtReplace);
            layoutControl.Controls.Add(cboNET);
            layoutControl.Controls.Add(btnSave);
            layoutControl.Dock = DockStyle.Fill;
            layoutControl.Location = new Point(0, 0);
            layoutControl.Name = "layoutControl";
            layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new Rectangle(464, 243, 650, 400);
            layoutControl.Root = Root;
            layoutControl.Size = new Size(783, 497);
            layoutControl.TabIndex = 1;
            layoutControl.Text = "dataLayoutControl1";
            // 
            // cboPlaceFolder
            // 
            cboPlaceFolder.EditValue = "";
            cboPlaceFolder.Location = new Point(162, 67);
            cboPlaceFolder.Name = "cboPlaceFolder";
            cboPlaceFolder.Properties.AutoHeight = false;
            cboPlaceFolder.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Ellipsis, "", 15, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default) });
            cboPlaceFolder.Properties.Name = "searchFolder";
            cboPlaceFolder.Properties.NullText = "Установите папку куда копировать найденное...";
            cboPlaceFolder.Properties.ButtonClick += searchFolder_BtnClick;
            cboPlaceFolder.Size = new Size(602, 20);
            cboPlaceFolder.StyleController = layoutControl;
            cboPlaceFolder.TabIndex = 3;
            cboPlaceFolder.Tag = "2";
            cboPlaceFolder.EditValueChanged += txtFolder_EditValueChanged;
            // 
            // cboSelectToDo
            // 
            cboSelectToDo.Location = new Point(162, 115);
            cboSelectToDo.Name = "cboSelectToDo";
            cboSelectToDo.Properties.AppearanceReadOnly.BackColor = SystemColors.Info;
            cboSelectToDo.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            cboSelectToDo.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            cboSelectToDo.Properties.NullText = "Установите значение...";
            cboSelectToDo.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cboSelectToDo.Properties.UseReadOnlyAppearance = false;
            cboSelectToDo.Size = new Size(602, 20);
            cboSelectToDo.StyleController = layoutControl;
            cboSelectToDo.TabIndex = 4;
            cboSelectToDo.SelectedIndexChanged += cboSelectToDo_SelectedIndexChanged;
            // 
            // btnBegin
            // 
            btnBegin.Enabled = false;
            btnBegin.ImageOptions.Image = (Image)resources.GetObject("btnBegin.ImageOptions.Image");
            btnBegin.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            btnBegin.ImageOptions.ImageToTextIndent = 5;
            btnBegin.Location = new Point(560, 252);
            btnBegin.Name = "btnBegin";
            btnBegin.Size = new Size(118, 22);
            btnBegin.StyleController = layoutControl;
            btnBegin.TabIndex = 9;
            btnBegin.Text = "Приступить";
            btnBegin.Click += btnBegin_Click;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Enabled = false;
            btnCancel.ImageOptions.Image = (Image)resources.GetObject("btnCancel.ImageOptions.Image");
            btnCancel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            btnCancel.ImageOptions.ImageToTextIndent = 5;
            btnCancel.Location = new Point(682, 252);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(89, 22);
            btnCancel.StyleController = layoutControl;
            btnCancel.TabIndex = 10;
            btnCancel.Text = "Остановить";
            btnCancel.ToolTip = "Остановить работу";
            btnCancel.Click += btnCancel_Click;
            // 
            // cboSearchExt
            // 
            cboSearchExt.EditValue = "*.cs";
            cboSearchExt.Location = new Point(162, 19);
            cboSearchExt.Name = "cboSearchExt";
            cboSearchExt.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            cboSearchExt.Properties.NullText = "*.cs";
            cboSearchExt.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cboSearchExt.Size = new Size(227, 20);
            cboSearchExt.StyleController = layoutControl;
            cboSearchExt.TabIndex = 0;
            cboSearchExt.EditValueChanged += cboSearchExt_EditValueChanged;
            // 
            // progressBar
            // 
            progressBar.EditValue = 50;
            progressBar.Location = new Point(13, 253);
            progressBar.Name = "progressBar";
            progressBar.Properties.BorderStyle = BorderStyles.NoBorder;
            progressBar.Size = new Size(542, 18);
            progressBar.StyleController = layoutControl;
            progressBar.TabIndex = 1;
            // 
            // cboSearchFolder
            // 
            cboSearchFolder.EditValue = "";
            cboSearchFolder.Location = new Point(162, 43);
            cboSearchFolder.Name = "cboSearchFolder";
            cboSearchFolder.Properties.AutoHeight = false;
            cboSearchFolder.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Ellipsis, "", 15, true, true, false, editorButtonImageOptions2, new DevExpress.Utils.KeyShortcut(Keys.None), serializableAppearanceObject5, serializableAppearanceObject6, serializableAppearanceObject7, serializableAppearanceObject8, "", null, null, DevExpress.Utils.ToolTipAnchor.Default) });
            cboSearchFolder.Properties.Name = "searchFolder";
            cboSearchFolder.Properties.NullText = "Установите папку для сканирования...";
            cboSearchFolder.Properties.ButtonClick += searchFolder_BtnClick;
            cboSearchFolder.Size = new Size(602, 20);
            cboSearchFolder.StyleController = layoutControl;
            cboSearchFolder.TabIndex = 2;
            cboSearchFolder.Tag = "1";
            cboSearchFolder.EditValueChanged += searchFolder_EditValueChanged;
            // 
            // foundFolders
            // 
            foundFolders.Enabled = false;
            foundFolders.Location = new Point(162, 458);
            foundFolders.Name = "foundFolders";
            foundFolders.Properties.Appearance.BackColor = SystemColors.ControlLightLight;
            foundFolders.Properties.Appearance.ForeColor = SystemColors.ControlText;
            foundFolders.Properties.Appearance.Options.UseBackColor = true;
            foundFolders.Properties.Appearance.Options.UseForeColor = true;
            foundFolders.Properties.NullText = "Информация о сканировании...";
            foundFolders.Properties.ReadOnly = true;
            foundFolders.Size = new Size(218, 20);
            foundFolders.StyleController = layoutControl;
            foundFolders.TabIndex = 1;
            foundFolders.TabStop = false;
            // 
            // foundFiles
            // 
            foundFiles.Enabled = false;
            foundFiles.Location = new Point(162, 434);
            foundFiles.Name = "foundFiles";
            foundFiles.Properties.Appearance.BackColor = SystemColors.ControlLightLight;
            foundFiles.Properties.Appearance.ForeColor = SystemColors.ControlText;
            foundFiles.Properties.Appearance.Options.UseBackColor = true;
            foundFiles.Properties.Appearance.Options.UseForeColor = true;
            foundFiles.Properties.NullText = "Информация о сканировании...";
            foundFiles.Properties.ReadOnly = true;
            foundFiles.Size = new Size(218, 20);
            foundFiles.StyleController = layoutControl;
            foundFiles.TabIndex = 11;
            // 
            // logMemo
            // 
            logMemo.Location = new Point(162, 295);
            logMemo.Name = "logMemo";
            logMemo.Properties.Appearance.BackColor = SystemColors.ControlLightLight;
            logMemo.Properties.Appearance.ForeColor = SystemColors.ControlText;
            logMemo.Properties.Appearance.Options.UseBackColor = true;
            logMemo.Properties.Appearance.Options.UseForeColor = true;
            logMemo.Properties.NullText = "Лог процесса...";
            logMemo.Properties.ReadOnly = true;
            logMemo.Size = new Size(602, 135);
            logMemo.StyleController = layoutControl;
            logMemo.TabIndex = 1;
            logMemo.TabStop = false;
            // 
            // cboDRY_RUN
            // 
            cboDRY_RUN.Location = new Point(162, 163);
            cboDRY_RUN.Name = "cboDRY_RUN";
            cboDRY_RUN.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            cboDRY_RUN.Properties.Items.AddRange(new object[] { "Имитация", "Удаление" });
            cboDRY_RUN.Properties.NullText = "Установите тип удаления";
            cboDRY_RUN.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cboDRY_RUN.Size = new Size(227, 20);
            cboDRY_RUN.StyleController = layoutControl;
            cboDRY_RUN.TabIndex = 6;
            cboDRY_RUN.SelectedIndexChanged += cboDRY_RUN_EditValueChanged;
            cboDRY_RUN.EditValueChanged += cboDRY_RUN_EditValueChanged;
            // 
            // txtFind
            // 
            txtFind.Location = new Point(162, 187);
            txtFind.Name = "txtFind";
            txtFind.Size = new Size(227, 20);
            txtFind.StyleController = layoutControl;
            txtFind.TabIndex = 7;
            // 
            // txtReplace
            // 
            txtReplace.Location = new Point(162, 211);
            txtReplace.Name = "txtReplace";
            txtReplace.Size = new Size(602, 20);
            txtReplace.StyleController = layoutControl;
            txtReplace.TabIndex = 8;
            // 
            // cboNET
            // 
            cboNET.Location = new Point(162, 139);
            cboNET.Name = "cboNET";
            cboNET.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            cboNET.Properties.NullText = "Установите версию .NET";
            cboNET.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cboNET.Size = new Size(227, 20);
            cboNET.StyleController = layoutControl;
            cboNET.TabIndex = 5;
            cboNET.SelectedIndexChanged += cboNET_SelectedIndexChanged;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(646, 434);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(118, 22);
            btnSave.StyleController = layoutControl;
            btnSave.TabIndex = 12;
            btnSave.Text = "Сохранритиь лог";
            btnSave.Click += btnSave_Click;
            // 
            // Root
            // 
            Root.AppearanceItemCaption.Options.UseTextOptions = true;
            Root.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            Root.GroupBordersVisible = false;
            Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { lgTop, lgToDo, lgProcess, lgInfo });
            Root.Name = "Root";
            Root.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            Root.Size = new Size(783, 497);
            Root.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            Root.TextLocation = DevExpress.Utils.Locations.Left;
            Root.TextVisible = false;
            // 
            // lgTop
            // 
            lgTop.CaptionImageOptions.Padding = new DevExpress.XtraLayout.Utils.Padding(9, 9, 9, 9);
            lgTop.CustomizationFormText = "lgTOP";
            lgTop.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { lcSearchMask, emptySearchExt, lgFolders });
            lgTop.Location = new Point(0, 0);
            lgTop.Name = "lgTop";
            lgTop.Size = new Size(773, 96);
            lgTop.TextVisible = false;
            // 
            // lcSearchMask
            // 
            lcSearchMask.Control = cboSearchExt;
            lcSearchMask.CustomizationFormText = "lcSearchMask";
            lcSearchMask.Location = new Point(0, 0);
            lcSearchMask.MaxSize = new Size(0, 24);
            lcSearchMask.MinSize = new Size(197, 24);
            lcSearchMask.Name = "lcSearchMask";
            lcSearchMask.Size = new Size(374, 24);
            lcSearchMask.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            lcSearchMask.Text = "Маска поиска:";
            lcSearchMask.TextSize = new Size(131, 13);
            // 
            // emptySearchExt
            // 
            emptySearchExt.Location = new Point(374, 0);
            emptySearchExt.Name = "emptySearchExt";
            emptySearchExt.OptionsTableLayoutItem.RowIndex = 1;
            emptySearchExt.Size = new Size(375, 24);
            // 
            // lgFolders
            // 
            lgFolders.CustomizationFormText = "lgFOLDERS";
            lgFolders.GroupBordersVisible = false;
            lgFolders.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { lcSearchFolder, lcPlaceFolder });
            lgFolders.Location = new Point(0, 24);
            lgFolders.Name = "lgFolders";
            lgFolders.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            lgFolders.Size = new Size(749, 48);
            lgFolders.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            lgFolders.TextVisible = false;
            // 
            // lcSearchFolder
            // 
            lcSearchFolder.Control = cboSearchFolder;
            lcSearchFolder.CustomizationFormText = "lcSearchFolder";
            lcSearchFolder.Location = new Point(0, 0);
            lcSearchFolder.MaxSize = new Size(0, 24);
            lcSearchFolder.MinSize = new Size(197, 24);
            lcSearchFolder.Name = "lcSearchFolder";
            lcSearchFolder.OptionsTableLayoutItem.ColumnIndex = 1;
            lcSearchFolder.Size = new Size(749, 24);
            lcSearchFolder.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            lcSearchFolder.Text = "Папка для сканирования:";
            lcSearchFolder.TextSize = new Size(131, 13);
            // 
            // lcPlaceFolder
            // 
            lcPlaceFolder.Control = cboPlaceFolder;
            lcPlaceFolder.CustomizationFormText = "lcPlaceFolder";
            lcPlaceFolder.Location = new Point(0, 24);
            lcPlaceFolder.MaxSize = new Size(0, 24);
            lcPlaceFolder.MinSize = new Size(197, 24);
            lcPlaceFolder.Name = "lcPlaceFolder";
            lcPlaceFolder.OptionsTableLayoutItem.ColumnIndex = 1;
            lcPlaceFolder.OptionsTableLayoutItem.RowIndex = 1;
            lcPlaceFolder.Size = new Size(749, 24);
            lcPlaceFolder.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            lcPlaceFolder.Text = "Папка для найденного:";
            lcPlaceFolder.TextSize = new Size(131, 13);
            // 
            // lgToDo
            // 
            lgToDo.CustomizationFormText = "lgTODO";
            lgToDo.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { lcToDo, lgOptions });
            lgToDo.Location = new Point(0, 96);
            lgToDo.Name = "lgToDo";
            lgToDo.Size = new Size(773, 144);
            lgToDo.Text = "Сканирование текста файлов ";
            lgToDo.TextVisible = false;
            // 
            // lcToDo
            // 
            lcToDo.Control = cboSelectToDo;
            lcToDo.CustomizationFormText = "lcDelete";
            lcToDo.Location = new Point(0, 0);
            lcToDo.MaxSize = new Size(0, 24);
            lcToDo.MinSize = new Size(197, 24);
            lcToDo.Name = "lcToDo";
            lcToDo.Size = new Size(749, 24);
            lcToDo.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            lcToDo.Text = "Как и что делаем:";
            lcToDo.TextSize = new Size(131, 13);
            // 
            // lgOptions
            // 
            lgOptions.CustomizationFormText = "lgOPTONS";
            lgOptions.GroupBordersVisible = false;
            lgOptions.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { lcDRY_RUN, lcFind, lcReplace, emptySpaceItem2, lcNET_Version });
            lgOptions.Location = new Point(0, 24);
            lgOptions.Name = "lgOptions";
            lgOptions.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            lgOptions.Size = new Size(749, 96);
            lgOptions.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            lgOptions.TextVisible = false;
            // 
            // lcDRY_RUN
            // 
            lcDRY_RUN.Control = cboDRY_RUN;
            lcDRY_RUN.CustomizationFormText = "lcDRY_RUN";
            lcDRY_RUN.Location = new Point(0, 24);
            lcDRY_RUN.MaxSize = new Size(0, 24);
            lcDRY_RUN.MinSize = new Size(197, 24);
            lcDRY_RUN.Name = "lcDRY_RUN";
            lcDRY_RUN.Size = new Size(374, 24);
            lcDRY_RUN.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            lcDRY_RUN.Text = "Тип удаления:";
            lcDRY_RUN.TextSize = new Size(131, 13);
            // 
            // lcFind
            // 
            lcFind.Control = txtFind;
            lcFind.CustomizationFormText = "lcFind";
            lcFind.Location = new Point(0, 48);
            lcFind.MaxSize = new Size(0, 24);
            lcFind.MinSize = new Size(197, 24);
            lcFind.Name = "lcFind";
            lcFind.Size = new Size(374, 24);
            lcFind.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            lcFind.Text = "Что ищем:";
            lcFind.TextSize = new Size(131, 13);
            // 
            // lcReplace
            // 
            lcReplace.Control = txtReplace;
            lcReplace.CustomizationFormText = "lcReplace";
            lcReplace.Location = new Point(0, 72);
            lcReplace.MaxSize = new Size(0, 24);
            lcReplace.MinSize = new Size(197, 24);
            lcReplace.Name = "lcReplace";
            lcReplace.Size = new Size(749, 24);
            lcReplace.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            lcReplace.Text = "Заменяем на:";
            lcReplace.TextSize = new Size(131, 13);
            // 
            // emptySpaceItem2
            // 
            emptySpaceItem2.Location = new Point(374, 0);
            emptySpaceItem2.Name = "emptySpaceItem2";
            emptySpaceItem2.Size = new Size(375, 72);
            // 
            // lcNET_Version
            // 
            lcNET_Version.Control = cboNET;
            lcNET_Version.Location = new Point(0, 0);
            lcNET_Version.Name = "lcNetVersion";
            lcNET_Version.Size = new Size(374, 24);
            lcNET_Version.Text = "Версия .NET:";
            lcNET_Version.TextSize = new Size(131, 13);
            // 
            // lgProcess
            // 
            lgProcess.CustomizationFormText = "lgPROCESS";
            lgProcess.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { lcProgress, lcBegin, lcCancel });
            lgProcess.Location = new Point(0, 240);
            lgProcess.Name = "lgProcess";
            lgProcess.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            lgProcess.Size = new Size(773, 36);
            lgProcess.Text = "grpProcess";
            lgProcess.TextVisible = false;
            // 
            // lcProgress
            // 
            lcProgress.Control = progressBar;
            lcProgress.CustomizationFormText = "lcProgress";
            lcProgress.Location = new Point(0, 0);
            lcProgress.Name = "itemProgress";
            lcProgress.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            lcProgress.Size = new Size(548, 26);
            lcProgress.Text = "Progress:";
            lcProgress.TextVisible = false;
            // 
            // lcBegin
            // 
            lcBegin.Control = btnBegin;
            lcBegin.CustomizationFormText = "lcBegin";
            lcBegin.Location = new Point(548, 0);
            lcBegin.MaxSize = new Size(122, 26);
            lcBegin.MinSize = new Size(122, 26);
            lcBegin.Name = "Begin";
            lcBegin.Size = new Size(122, 26);
            lcBegin.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            lcBegin.TextVisible = false;
            // 
            // lcCancel
            // 
            lcCancel.Control = btnCancel;
            lcCancel.CustomizationFormText = "lcCancel";
            lcCancel.Location = new Point(670, 0);
            lcCancel.MaxSize = new Size(93, 26);
            lcCancel.MinSize = new Size(93, 26);
            lcCancel.Name = "itemCancel";
            lcCancel.Size = new Size(93, 26);
            lcCancel.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            lcCancel.Text = "Cancel";
            lcCancel.TextVisible = false;
            // 
            // lgInfo
            // 
            lgInfo.CustomizationFormText = "lgINFO";
            lgInfo.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { lcLogMemo, lgPocessed });
            lgInfo.Location = new Point(0, 276);
            lgInfo.Name = "lgInfo";
            lgInfo.Size = new Size(773, 211);
            lgInfo.TextVisible = false;
            // 
            // lcLogMemo
            // 
            lcLogMemo.AppearanceItemCaption.Options.UseTextOptions = true;
            lcLogMemo.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            lcLogMemo.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            lcLogMemo.Control = logMemo;
            lcLogMemo.CustomizationFormText = "lcLogMemo";
            lcLogMemo.Location = new Point(0, 0);
            lcLogMemo.MinSize = new Size(157, 18);
            lcLogMemo.Name = "lcLogMemo";
            lcLogMemo.Size = new Size(749, 139);
            lcLogMemo.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            lcLogMemo.Text = "Лог процесса:";
            lcLogMemo.TextSize = new Size(131, 13);
            // 
            // lgPocessed
            // 
            lgPocessed.CustomizationFormText = "lgPOCESSED";
            lgPocessed.GroupBordersVisible = false;
            lgPocessed.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { lcFoundFiles, emptySpaceItem1, layoutControlItem1, lcFoundFolders });
            lgPocessed.Location = new Point(0, 139);
            lgPocessed.Name = "lgPocessed";
            lgPocessed.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            lgPocessed.Size = new Size(749, 48);
            lgPocessed.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            lgPocessed.TextVisible = false;
            // 
            // lcFoundFiles
            // 
            lcFoundFiles.Control = foundFiles;
            lcFoundFiles.CustomizationFormText = "lcFoundFiles";
            lcFoundFiles.Location = new Point(0, 0);
            lcFoundFiles.Name = "lcFoundFiles";
            lcFoundFiles.Size = new Size(365, 24);
            lcFoundFiles.Text = "Просканировано файлов:";
            lcFoundFiles.TextSize = new Size(131, 13);
            // 
            // emptySpaceItem1
            // 
            emptySpaceItem1.Location = new Point(365, 0);
            emptySpaceItem1.Name = "emptySpaceItem1";
            emptySpaceItem1.Size = new Size(262, 48);
            // 
            // layoutControlItem1
            // 
            layoutControlItem1.Control = btnSave;
            layoutControlItem1.Location = new Point(627, 0);
            layoutControlItem1.MaxSize = new Size(122, 26);
            layoutControlItem1.MinSize = new Size(122, 26);
            layoutControlItem1.Name = "layoutControlItem1";
            layoutControlItem1.Size = new Size(122, 48);
            layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            layoutControlItem1.TextVisible = false;
            // 
            // lcFoundFolders
            // 
            lcFoundFolders.Control = foundFolders;
            lcFoundFolders.CustomizationFormText = "lcFoundFolders";
            lcFoundFolders.Location = new Point(0, 24);
            lcFoundFolders.Name = "lcFoundFolders";
            lcFoundFolders.Size = new Size(365, 24);
            lcFoundFolders.Text = "Просмотрено папок:";
            lcFoundFolders.TextSize = new Size(131, 13);
            // 
            // layoutControlItem2
            // 
            layoutControlItem2.Control = foundFolders;
            layoutControlItem2.Location = new Point(0, 241);
            layoutControlItem2.Name = "layoutControlItem1";
            layoutControlItem2.Size = new Size(470, 24);
            layoutControlItem2.TextSize = new Size(131, 13);
            // 
            // layoutControlItem3
            // 
            layoutControlItem3.Control = foundFolders;
            layoutControlItem3.Location = new Point(0, 241);
            layoutControlItem3.Name = "layoutControlItem1";
            layoutControlItem3.Size = new Size(470, 24);
            layoutControlItem3.TextSize = new Size(131, 13);
            // 
            // layoutControlItem4
            // 
            layoutControlItem4.Control = foundFolders;
            layoutControlItem4.Location = new Point(0, 241);
            layoutControlItem4.Name = "layoutControlItem1";
            layoutControlItem4.Size = new Size(470, 24);
            layoutControlItem4.TextSize = new Size(131, 13);
            // 
            // layoutControlItem5
            // 
            layoutControlItem5.Control = foundFolders;
            layoutControlItem5.Location = new Point(0, 241);
            layoutControlItem5.Name = "layoutControlItem1";
            layoutControlItem5.Size = new Size(470, 24);
            layoutControlItem5.TextSize = new Size(131, 13);
            // 
            // openFileDlg
            // 
            openFileDlg.Filter = "Project Files(*.csproj)|*.csproj";
            // 
            // FileScanner
            // 
            Controls.Add(layoutControl);
            Name = "FileScanner";
            Size = new Size(783, 497);
            ((ISupportInitialize)layoutControl).EndInit();
            layoutControl.ResumeLayout(false);
            ((ISupportInitialize)cboPlaceFolder.Properties).EndInit();
            ((ISupportInitialize)cboSelectToDo.Properties).EndInit();
            ((ISupportInitialize)cboSearchExt.Properties).EndInit();
            ((ISupportInitialize)progressBar.Properties).EndInit();
            ((ISupportInitialize)cboSearchFolder.Properties).EndInit();
            ((ISupportInitialize)foundFolders.Properties).EndInit();
            ((ISupportInitialize)foundFiles.Properties).EndInit();
            ((ISupportInitialize)logMemo.Properties).EndInit();
            ((ISupportInitialize)cboDRY_RUN.Properties).EndInit();
            ((ISupportInitialize)txtFind.Properties).EndInit();
            ((ISupportInitialize)txtReplace.Properties).EndInit();
            ((ISupportInitialize)cboNET.Properties).EndInit();
            ((ISupportInitialize)Root).EndInit();
            ((ISupportInitialize)lgTop).EndInit();
            ((ISupportInitialize)lcSearchMask).EndInit();
            ((ISupportInitialize)emptySearchExt).EndInit();
            ((ISupportInitialize)lgFolders).EndInit();
            ((ISupportInitialize)lcSearchFolder).EndInit();
            ((ISupportInitialize)lcPlaceFolder).EndInit();
            ((ISupportInitialize)lgToDo).EndInit();
            ((ISupportInitialize)lcToDo).EndInit();
            ((ISupportInitialize)lgOptions).EndInit();
            ((ISupportInitialize)lcDRY_RUN).EndInit();
            ((ISupportInitialize)lcFind).EndInit();
            ((ISupportInitialize)lcReplace).EndInit();
            ((ISupportInitialize)emptySpaceItem2).EndInit();
            ((ISupportInitialize)lcNET_Version).EndInit();
            ((ISupportInitialize)lgProcess).EndInit();
            ((ISupportInitialize)lcProgress).EndInit();
            ((ISupportInitialize)lcBegin).EndInit();
            ((ISupportInitialize)lcCancel).EndInit();
            ((ISupportInitialize)lgInfo).EndInit();
            ((ISupportInitialize)lcLogMemo).EndInit();
            ((ISupportInitialize)lgPocessed).EndInit();
            ((ISupportInitialize)lcFoundFiles).EndInit();
            ((ISupportInitialize)emptySpaceItem1).EndInit();
            ((ISupportInitialize)layoutControlItem1).EndInit();
            ((ISupportInitialize)lcFoundFolders).EndInit();
            ((ISupportInitialize)layoutControlItem2).EndInit();
            ((ISupportInitialize)layoutControlItem3).EndInit();
            ((ISupportInitialize)layoutControlItem4).EndInit();
            ((ISupportInitialize)layoutControlItem5).EndInit();
            ((ISupportInitialize)bsFileScanner).EndInit();
            ResumeLayout(false);
        }
        #endregion
        private string GetDebuggerDisplay()
        {
            return ToString();
        }
        private DevExpress.XtraDataLayout.DataLayoutControl layoutControl;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup lgTop;
        private DevExpress.XtraLayout.LayoutControlGroup lgProcess;
        private DevExpress.XtraLayout.LayoutControlGroup lgToDo;
        private DevExpress.XtraLayout.EmptySpaceItem emptySearchExt;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        // new controls
        private DevExpress.XtraLayout.LayoutControlItem lcSearchFolder;
        private DevExpress.XtraLayout.LayoutControlItem lcToDo;
        private DevExpress.XtraLayout.LayoutControlItem lcDRY_RUN;
        private DevExpress.XtraLayout.LayoutControlItem lcFind;
        private DevExpress.XtraLayout.LayoutControlItem lcReplace;
        private DevExpress.XtraLayout.LayoutControlItem lcSearchMask;
        private DevExpress.XtraLayout.LayoutControlItem lcProgress;
        private DevExpress.XtraLayout.LayoutControlItem lcCancel;
        private DevExpress.XtraLayout.LayoutControlItem lcBegin;
        private DevExpress.XtraLayout.LayoutControlItem lcPlaceFolder;
        private DevExpress.XtraEditors.ProgressBarControl progressBar;
        private DevExpress.XtraEditors.ComboBoxEdit cboSearchFolder;
        private DevExpress.XtraEditors.TextEdit foundFiles;
        private DevExpress.XtraEditors.MemoEdit logMemo;
        private DevExpress.XtraEditors.ComboBoxEdit cboSelectToDo;
        private DevExpress.XtraEditors.SimpleButton btnBegin;
        private DevExpress.XtraEditors.TextEdit foundFolders;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.ComboBoxEdit cboSearchExt;
        private DevExpress.XtraEditors.ComboBoxEdit cboDRY_RUN;
        private DevExpress.XtraEditors.TextEdit txtFind;
        private DevExpress.XtraEditors.TextEdit txtReplace;
        private DevExpress.XtraEditors.ComboBoxEdit cboPlaceFolder;
        private FolderBrowserDialog openFolderDlg;
        private BindingSource bsFileScanner;
        private DevExpress.XtraLayout.LayoutControlItem lcLogMemo;
        private DevExpress.XtraLayout.LayoutControlGroup lgInfo;
        private DevExpress.XtraLayout.LayoutControlItem lcFoundFiles;
        private DevExpress.XtraLayout.LayoutControlItem lcFoundFolders;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private OpenFileDialog openFileDlg;
        private DevExpress.XtraLayout.LayoutControlGroup lgOptions;
        private DevExpress.XtraEditors.ComboBoxEdit cboNET;
        private DevExpress.XtraLayout.LayoutControlItem lcNET_Version;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlGroup lgFolders;
        private DevExpress.XtraLayout.LayoutControlGroup lgPocessed;
    }
}