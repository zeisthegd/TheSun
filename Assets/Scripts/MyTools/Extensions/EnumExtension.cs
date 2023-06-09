using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Tools
{
    public static class EnumExtension
    {
        public static T RandomIndex<T>(this T enumValue)
        {
            var enumValueList = Enum.GetValues(enumValue.GetType());
            return (T)enumValueList.GetValue((int)Randomizer.Range(0, enumValueList.Length));
        }
    }
}
