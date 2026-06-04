using DevExpress.Utils;
using System.ComponentModel;
namespace GH.Components
{
    public class EditImages : Component
    {
        //static string edit_path = "images/actions/";
        //static string print_path = "images/print/";
        //static string small_ext = "_16x16.png";
        //static string large_ext = "_32x32.png";
        //private static EditImages _images;
        //public static EditImages DefaultImages
        //{
        //    get
        //    {
        //        if (_images == null)
        //            new EditImages();
        //        return _images;
        //    }
        //}
        //internal static Image GetImageByType(EditTypes buttonType, bool small)
        //{
        //    if (small)
        //        return ImageCollection.GetImageListImage(DefaultImages.SmallImages, buttonType.ToString());
        //    return ImageCollection.GetImageListImage(DefaultImages.LargeImages, buttonType.ToString());
        //}
        ImageCollection _smallImages = new ImageCollection();
    public ImageCollection SmallImages { get => _smallImages; set => _smallImages = value; }
        ImageCollection _largeImages = CreateLargeImages();
    private static ImageCollection CreateLargeImages()
        {
            ImageCollection largeImages = new ImageCollection();
            largeImages.ImageSize = new Size(32, 32);
            return largeImages;
        }

    public ImageCollection LargeImages { get => _largeImages; set => _largeImages = value; }

    public EditImages() : base()
        {
            //InitImages();
            //if (DesignMode)
            //    return;
            //if (_images != null)
            //    throw new Exception("EditImages в проекте может быть только один!!!");
            //_images = this;
        }
    private void SetFromResource(EditTypes editType)
        {
            //string name = null;
            //string path = edit_path;
            //switch (editType)
            //{
            //    case EditTypes.Insert:
            //        name = "addfile";
            //        break;
            //    case EditTypes.Edit:
            //        name = "editname";
            //        break;
            //    case EditTypes.Delete:
            //        name = "remove";
            //        break;
            //    case EditTypes.Save:
            //        name = "apply";
            //        break;
            //    case EditTypes.Cancel:
            //        name = "cancel";
            //        break;
            //    case EditTypes.RefreshAll:
            //        name = "refresh";
            //        break;
            //    case EditTypes.PrintPreview:
            //        name = "print";
            //        path = print_path;
            //        break;
            //    case EditTypes.Additional:
            //    default:
            //        return;
            //}
            //_smallImages.AddImage(ImageResourceCache.Default.GetImage(path + name + small_ext), editType.ToString());
            //_largeImages.AddImage(ImageResourceCache.Default.GetImage(path + name + large_ext), editType.ToString());
        }
    private void InitImages()
        {
            foreach (EditTypes editType in Enum.GetValues(typeof(EditTypes)))
                SetFromResource(editType);
            //{
            //    SetFromResource(editType);
            //    //switch (editType)
            //    //{
            //    //    case EditTypes.Insert:
            //    //        _smallImages.InsertGalleryImage(editType.ToString(), edit_path +"addfile" + small_ext, ImageResourceCache.Default.GetImage(edit_path +"addfile" + small_ext), (int)editType);
            //    //        _largeImages.InsertGalleryImage(editType.ToString(), edit_path +"addfile" + large_ext, ImageResourceCache.Default.GetImage(edit_path +"addfile" + large_ext), (int)editType);
            //    //        break;
            //    //    case EditTypes.Edit:
            //    //        _smallImages.InsertGalleryImage(editType.ToString(), edit_path +"editname" + small_ext, ImageResourceCache.Default.GetImage(edit_path +"editname" + small_ext), (int)editType);
            //    //        _largeImages.InsertGalleryImage(editType.ToString(), edit_path +"editname" + large_ext, ImageResourceCache.Default.GetImage(edit_path +"editname" + large_ext), (int)editType);
            //    //        break;
            //    //    case EditTypes.Delete:
            //    //        _smallImages.InsertGalleryImage(editType.ToString(), edit_path +"remove" + small_ext, ImageResourceCache.Default.GetImage(edit_path +"remove" + small_ext), (int)editType);
            //    //        _largeImages.InsertGalleryImage(editType.ToString(), edit_path +"remove" + large_ext, ImageResourceCache.Default.GetImage(edit_path +"remove" + large_ext), (int)editType);
            //    //        break;
            //    //    case EditTypes.Save:
            //    //        _smallImages.InsertGalleryImage(editType.ToString(), edit_path +"apply" + small_ext, ImageResourceCache.Default.GetImage(edit_path +"apply" + small_ext), (int)editType);
            //    //        _largeImages.InsertGalleryImage(editType.ToString(), edit_path +"apply" + large_ext, ImageResourceCache.Default.GetImage(edit_path +"apply" + large_ext), (int)editType);
            //    //        break;
            //    //    case EditTypes.Cancel:
            //    //        _smallImages.InsertGalleryImage(editType.ToString(), edit_path +"cancel" + small_ext, ImageResourceCache.Default.GetImage(edit_path +"cancel" + small_ext), (int)editType);
            //    //        _largeImages.InsertGalleryImage(editType.ToString(), edit_path +"cancel" + large_ext, ImageResourceCache.Default.GetImage(edit_path +"cancel" + large_ext), (int)editType);
            //    //        break;
            //    //    case EditTypes.RefreshAll:
            //    //        _smallImages.InsertGalleryImage(editType.ToString(), edit_path +"refresh" + small_ext, ImageResourceCache.Default.GetImage(edit_path +"refresh" + small_ext), (int)editType);
            //    //        _largeImages.InsertGalleryImage(editType.ToString(), edit_path +"refresh" + large_ext, ImageResourceCache.Default.GetImage(edit_path +"refresh" + large_ext), (int)editType);
            //    //        break;
            //    //    case EditTypes.PrintPreview:
            //    //        _smallImages.InsertGalleryImage(editType.ToString(), print_path +"print" + small_ext, ImageResourceCache.Default.GetImage(print_path +"print" + small_ext), (int)editType);
            //    //        _largeImages.InsertGalleryImage(editType.ToString(), print_path +"print" + large_ext, ImageResourceCache.Default.GetImage(print_path +"print" + large_ext), (int)editType);
            //    //        break;
            //    //    //case EditTypes.SearchDiscogs:
            //    //    //    _smallIcons.InsertGalleryImage(editType.ToString(), "images/business%20objects/bolocalization" + smal_ext, DevExpress.Images.ImageResourceCache.Default.GetImage("images/business%20objects/bolocalization" + smal_ext), (int)editType);
            //    //    //    _largeIcons.InsertGalleryImage(editType.ToString(), "images/business%20objects/bolocalization" + large_ext, DevExpress.Images.ImageResourceCache.Default.GetImage("images/business%20objects/bolocalization" + large_ext), (int)editType);
            //    //    //    break;
            //    //    //case EditTypes.SearchWikipedia:
            //    //    //    _smallIcons.InsertGalleryImage(editType.ToString(), "images/business%20objects/bolocalization" + smal_ext, DevExpress.Images.ImageResourceCache.Default.GetImage("images/business%20objects/bolocalization" + smal_ext), (int)editType);
            //    //    //    _largeIcons.InsertGalleryImage(editType.ToString(), "images/business%20objects/bolocalization" + large_ext, DevExpress.Images.ImageResourceCache.Default.GetImage("images/business%20objects/bolocalization" + large_ext), (int)editType);
            //    //    //    break;
            //    //    //case EditTypes.SearchAllMusic:
            //    //    //    _smallIcons.InsertGalleryImage(editType.ToString(), "images/business%20objects/bolocalization" + smal_ext, DevExpress.Images.ImageResourceCache.Default.GetImage("images/business%20objects/bolocalization" + smal_ext), (int)editType);
            //    //    //    _largeIcons.InsertGalleryImage(editType.ToString(), "images/business%20objects/bolocalization" + large_ext, DevExpress.Images.ImageResourceCache.Default.GetImage("images/business%20objects/bolocalization" + large_ext), (int)editType);
            //    //    //    break;
            //    //    //case EditTypes.SearchGoogle:
            //    //    //    _smallIcons.InsertGalleryImage(editType.ToString(), "images/business%20objects/bolocalization" + smal_ext, DevExpress.Images.ImageResourceCache.Default.GetImage("images/business%20objects/bolocalization" + smal_ext), (int)editType);
            //    //    //    _largeIcons.InsertGalleryImage(editType.ToString(), "images/business%20objects/bolocalization" + large_ext, DevExpress.Images.ImageResourceCache.Default.GetImage("images/business%20objects/bolocalization" + large_ext), (int)editType);
            //    //    //    break;
            //    //    default:
            //    //        break;
            //    //}
            //}
        }
    }
}
