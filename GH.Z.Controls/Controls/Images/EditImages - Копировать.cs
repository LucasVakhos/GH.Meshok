using System;
using System.ComponentModel;
using System.Drawing;
using DevExpress.Utils;

namespace GH.Components
{
    public class EditImages : ImageCollection
    {
        private static EditImages _images;
        public static Images DefaultImages {
            get {
                if (_images == null)
                    new EditImages();
                return _images.Images;
            }
        }

        internal static Image GetImageByType(EditTypes buttonType)
        {
            return DefaultImages[buttonType.ToString()];
        }

        public EditImages() : base()
        {
            InitImages();
            if (IsDesignMode)
                return;

            if (_images != null)
                throw new Exception("EditImages в проекте может быть только один!!!");
            _images = this;
        }

        private void InitImages()
        {
            foreach (EditTypes editType in Enum.GetValues(typeof(EditTypes)))
            {
                switch (editType)
                {
                    case EditTypes.Insert:
                        InsertGalleryImage(editType.ToString(), "images/actions/addfile_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/addfile_16x16.png"), (int)editType);
                        break;
                    case EditTypes.Edit:
                        InsertGalleryImage(editType.ToString(), "images/actions/editname_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/editname_16x16.png"), (int)editType);
                        break;
                    case EditTypes.Delete:
                        InsertGalleryImage(editType.ToString(), "images/actions/remove_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/remove_16x16.png"), (int)editType);
                        break;
                    case EditTypes.Save:
                        InsertGalleryImage(editType.ToString(), "images/actions/apply_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/apply_16x16.png"), (int)editType);
                        break;
                    case EditTypes.Cancel:
                        InsertGalleryImage(editType.ToString(), "images/actions/cancel_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/cancel_16x16.png"), (int)editType);
                        break;
                    case EditTypes.RefreshAll:
                        InsertGalleryImage(editType.ToString(), "images/actions/refresh_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/refresh_16x16.png"), (int)editType);
                        break;
                    case EditTypes.PrintPreview:
                        InsertGalleryImage(editType.ToString(), "images/print/print_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/print/print_16x16.png"), (int)editType);
                        break;
                    //case EditTypes.SearchDiscogs:
                    //    InsertGalleryImage(editType.ToString(), "images/business%20objects/bolocalization_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/business%20objects/bolocalization_16x16.png"), (int)editType);
                    //    break;
                    //case EditTypes.SearchWikipedia:
                    //    InsertGalleryImage(editType.ToString(), "images/business%20objects/bolocalization_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/business%20objects/bolocalization_16x16.png"), (int)editType);
                    //    break;
                    //case EditTypes.SearchAllMusic:
                    //    InsertGalleryImage(editType.ToString(), "images/business%20objects/bolocalization_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/business%20objects/bolocalization_16x16.png"), (int)editType);
                    //    break;
                    //case EditTypes.SearchGoogle:
                    //    InsertGalleryImage(editType.ToString(), "images/business%20objects/bolocalization_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/business%20objects/bolocalization_16x16.png"), (int)editType);
                    //    break;
                    default:
                        //InsertGalleryImage(editType.ToString(), , (int)editType);
                        break;
                }
            }
        }

    }
}
