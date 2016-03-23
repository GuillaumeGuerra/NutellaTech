using System;
using System.Linq;

namespace PatchManager.TestFramework.Reflection
{
    public static class EnumHelper<TEnum>
    {
        public static TEnum GetOtherEnumValue(Enum value)
        {
            var intValue = Convert.ToInt32(value);
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().First(v => Convert.ToInt32(v) != intValue);
        }
    }
}
