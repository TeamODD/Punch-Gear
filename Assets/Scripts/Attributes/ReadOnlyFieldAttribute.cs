using System;

using UnityEngine;

namespace PunchGear.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyFieldAttribute : PropertyAttribute
    {
    }
}
