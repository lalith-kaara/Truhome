using System.ComponentModel;

namespace Truhome.Business.Enums
{
    public enum CustomerType : byte
    {
        [Description("Individual Customer")]
        I = 1,
        [Description("Non-Individual Customer")]
        NI = 2
    }
}
