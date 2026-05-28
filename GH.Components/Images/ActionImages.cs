using DevExpress.Utils;
using System.ComponentModel;
using static DevExpress.Images.ImageResourceCache;
namespace GH.Components
{
    [ToolboxItem(false)]
    public partial class ActtionsImages : Component, ISupportInitialize
    {
        private ImageCollection imageSmall;
        public ImageCollection SmallImages { get => imageSmall; set => imageSmall = value; }
        private ImageCollection imageLarge;
        public ImageCollection LargeImages { get => imageLarge; set => imageLarge = value; }
        private IContainer components = null;
        private static ActtionsImages _images;
        public static ActtionsImages Instance
        {
            get
            {
                if (_images == null)
                    _images = new ActtionsImages();
                return _images;
            }
        }
        public ActtionsImages()
        {
            InitializeComponent();
        }
        public ActtionsImages(IContainer container) : this()
        {
            container.Add(this);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(ActtionsImages));
            imageSmall = new ImageCollection();
            imageLarge = new ImageCollection();
            BeginInit();
            ((ISupportInitialize)(imageSmall)).BeginInit();
            ((ISupportInitialize)(imageLarge)).BeginInit();
            // 
            // imageSmall
            // 
            imageSmall.ImageStream = ((ImageCollectionStreamer)(resources.GetObject("imageSmall.ImageStream")));
            // 
            // imageLarge
            // 
            imageLarge.ImageSize = new System.Drawing.Size(32, 32);
            imageLarge.ImageStream = ((ImageCollectionStreamer)(resources.GetObject("imageLarge.ImageStream")));
            EndInit();
            ((ISupportInitialize)(imageSmall)).EndInit();
            ((ISupportInitialize)(imageLarge)).EndInit();
        }
        public void BeginInit()
        {
        }
        public void EndInit()
        {
            if (this.IsDesignMode())
                return;
            foreach (EditTypes edit in Enum.GetValues(typeof(EditTypes)))
            {
                //string name = edit.ToString();
                //string path = edit.GetPath(false);
                //Image image = Default.GetImage(path);
                imageSmall.InsertGalleryImage(edit.ToString(), edit.GetPath(false), Default.GetImage(edit.GetPath(false)), (int)edit);
                imageSmall.Images.SetKeyName((int)edit, edit.ToString());
                imageLarge.InsertGalleryImage(edit.ToString(), edit.GetPath(), Default.GetImage(edit.GetPath()), (int)edit);
                imageLarge.Images.SetKeyName((int)edit, edit.ToString());
            }
        }
    }
}
