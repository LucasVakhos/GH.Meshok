using System;
namespace NewsMaker
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            AppContextNM.RunInstance();
        }
    }
}
