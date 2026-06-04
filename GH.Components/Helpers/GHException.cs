namespace GH.Components
{
    public class GHException : Exception
    {
        public GHException(string message)
          : base(message)
        {
        }

    public GHException(string message, Exception innerException)
          : base(message, innerException)
        {
        }

    public GHException()
        {
        }
    }
}
