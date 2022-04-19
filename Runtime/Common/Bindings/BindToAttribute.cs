using System;
using UnityEngine;

namespace Common.Bindings
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
    public class BindToAttribute : PropertyAttribute
    {
        public BindToAttribute(Type bindToType)
        {
            BindToType = bindToType;
        }

        public Type BindToType;

        public Type[] ArgumentTypes;
    }
}