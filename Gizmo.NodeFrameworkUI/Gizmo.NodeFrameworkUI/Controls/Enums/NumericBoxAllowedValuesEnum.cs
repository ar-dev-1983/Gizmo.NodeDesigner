using System;

namespace Gizmo.NodeFrameworkUI
{
    [Flags]
    public enum NumericBoxAllowedValuesEnum
    {
        None = 0,
        NaN = 1,
        PositiveInfinity = 2,
        NegativeInfinity = 4,
        AnyInfinity = PositiveInfinity | NegativeInfinity,
        Any = NaN | AnyInfinity
    }
}
