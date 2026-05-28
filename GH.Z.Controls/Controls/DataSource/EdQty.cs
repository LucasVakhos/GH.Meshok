using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Windows.Forms;

namespace GH.Components
{
    public class EdQty : RepositoryItemSpinEdit
    {
        GridView view;
        DataSource source;
        private object _newValue = null;

        object OldValue { get; set; } = null;
        object NewValue
        {
            get => _newValue;
            set
            {
                _newValue = value;
                source.Entity.AsInteger(source.ColQty.FieldName, (int)value);
                source.ResetCurrentItem();
                //view.UpdateCurrentRow();
            }
        }
        public EdQty(DataSource dataSource) : base()
        {

            source = dataSource;
            view = dataSource.View as GridView;
            //ValidateOnEnterKey = true;
            EditValueChangedFiringMode = EditValueChangedFiringMode.Default;
            MaxValue = 1000000;
            MinValue = 0;
            IsFloatValue = false;
            AutoHeight = false;
            Buttons.Clear();

            view.GridControl.RepositoryItems.Add(this);
            dataSource.ColQty.ColumnEdit = this;
            KeyDown += EdQty_KeyDown;
            ValueChanged += EdQty_ValueChanged;
        }

        private void EdQty_ValueChanged(object sender, EventArgs e)
        {
            SpinEdit spinEdit = sender as SpinEdit;
            if (OldValue == null)
                OldValue = spinEdit.OldEditValue;
            NewValue = (int)spinEdit.Value;
        }

        private void Post()
        {
            //if (OldValue != NewValue)
            //{
            //    if (source.EditState != source.State)
            //        source.State = source.EditState;
            source.Post();
            //}

            OldValue = null;
            _newValue = null;
        }

        private void Cancel()
        {
            source.Cancel();
            OldValue = null;
            _newValue = null;
        }

        private void EdQty_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    //e.SuppressKeyPress = true;
                    e.Handled = true;
                    Post();
                    break;
                case Keys.Down:
                case Keys.Up:
                    view.PostEditor();
                    Post();
                    break;
                case Keys.Escape:
                    //e.Handled = true;
                    Cancel();
                    break;
                default:
                    break;
            }
        }
    }


}

