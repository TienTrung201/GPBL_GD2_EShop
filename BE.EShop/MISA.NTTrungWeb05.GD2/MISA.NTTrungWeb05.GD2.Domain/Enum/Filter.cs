using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.NTTrungWeb05.GD2.Domain.Enum
{
    public enum PropertyType
    {
        [Description("int")]
        Int = 1,
        [Description("string")]
        String = 2,
        [Description("Date")]
        Date = 3,
        [Description("bool")]
        Bool = 4,
    }
    public enum Operator
    {
        [Description("Like")]
        Like = 1,
        [Description("Equal")]
        EQual = 2,
        [Description("NotEqual")]
        Date = 3,
        [Description("Contain")]
        Contain = 4,
        [Description("NotContain")]
        NotContain = 4,
        [Description("Greater")]
        Greater = 5,
        [Description("Smaller")]
        Smaller = 6,
    }
    public enum RelationType
    {
        [Description("And")]
        And = 1,
        [Description("Or")]
        Or = 2,
    }
}
