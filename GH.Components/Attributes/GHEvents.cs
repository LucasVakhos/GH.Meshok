using System.ComponentModel;
namespace GH.Components
{
    [AttributeUsage(AttributeTargets.Event)]
    public class GHEventsAttribute : CategoryAttribute
    {
        public GHEventsAttribute() : base("GH Events")
        {
        }
    }
}
