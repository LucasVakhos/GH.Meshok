using System;
using System.ComponentModel;

namespace GH.Components
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GHPropertyAttribute : CategoryAttribute
    {
        public GHPropertyAttribute() : base("GH Propertys")
        {
        }
    }

}
