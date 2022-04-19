using System;
using UnityEngine;

namespace Common.Bindings
{
    [BindTo(typeof(bool))]
    public class BehaviourEnabledBooleanBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private Behaviour[] _true;
        [SerializeField] private Behaviour[] _false;
#pragma warning restore 649

        private Func<bool> _getter;

        protected override void Bind(bool init)
        {
            var isTrue = _getter();

            if (_true != null)
                foreach (var behaviour in _true)
                    behaviour.enabled = isTrue;

            if (_false != null)
                foreach (var behaviour in _false)
                    behaviour.enabled = !isTrue;
        }

        private void Awake()
        {
            Init(ref _getter);
        }
    }
}