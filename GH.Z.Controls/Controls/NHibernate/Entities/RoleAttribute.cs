using System;

namespace GH.NHibernate
{
    public class RoleAttribute : Attribute
    {
        public RoleAttribute(string roleName)
        {
            RoleName = roleName;
        }

        public string RoleName { get; set; } = "USER";
    }
}