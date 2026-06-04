using System;
namespace GH.Components
{
    public class NotImplemented : NotImplementedException
    {
        public NotImplemented(string methodName, object obj) : base($"Перезапишите метод {methodName}() У {obj.GetType().Name}!!!") { }
    }
}
