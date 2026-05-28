namespace GH.Components
{
    public class EditGrants
    {
        public EditGrants(bool allowNew, bool allowEdit, bool allowRemove)
        {
            AllowNew = allowNew;
            AllowEdit = allowEdit;
            AllowRemove = allowRemove;
        }

        public bool AllowNew;
        public bool AllowEdit;
        public bool AllowRemove;
    }

}

