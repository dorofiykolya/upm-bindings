using System;
using UnityEngine;

namespace Common.Bindings
{
    [BindTo(typeof(bool))]
    public class ScaleBooleanBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private Vector3 _true;
        [SerializeField] private Vector3 _false;
        [SerializeField] private Transform _targetTransform;
#pragma warning restore 649

        private Func<bool> _getter;

        protected override void Bind(bool init)
        {
            var isTrue = _getter();
            if (_targetTransform != null) _targetTransform.localScale = isTrue ? _true : _false;
        }

        private void Awake()
        {
            Init(ref _getter);
        }
    }
}