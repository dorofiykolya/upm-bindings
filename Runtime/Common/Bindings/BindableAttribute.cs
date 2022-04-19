using System;

namespace Common.Bindings
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class BindableAttribute : Attribute
    {
        private Type _argumentType;

        public Type ArgumentType => _argumentType;

        public Type ReturnType;

        public BindableAttribute()
        {
        }

        public BindableAttribute(Type argumentType)
        {
            _argumentType = argumentType;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class BindableDescAttribute : Attribute
    {
        private string _description;
        private bool _isWarning;

        public string Description => _description;

        public bool IsWarning => _isWarning;

        public BindableDescAttribute(string desc)
        {
            _description = desc;
        }

        public BindableDescAttribute(string desc, bool isWarning)
        {
            _description = desc;
            _isWarning = isWarning;
        }
    }
}