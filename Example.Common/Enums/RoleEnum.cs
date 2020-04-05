using System.ComponentModel;

namespace Example.Common.Enums
{
    public enum RoleEnum
    {
        [Description("ADMIN")] Admin = 1,
        [Description("USER")] User = 2
    }
}
