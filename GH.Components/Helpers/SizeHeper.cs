namespace GH.Components
{
    public static class SizeHeper
    {
        public static Size NewSize(Control value)
        {
            return new Size(value.Width, value.Height);
        }
        public static Size NewSize(Size value)
        {
            return new Size(value.Width, value.Height);
        }
    }
}
