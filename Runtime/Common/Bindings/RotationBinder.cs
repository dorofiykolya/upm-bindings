using System;
using UnityEngine;

namespace Common.Bindings
{
    [BindTo(typeof(Quaternion))]
    public class RotationBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private RectTransform _targetRect;
#pragma warning restore 649

        private Func<Quaternion> _getter;

        protected override void Bind(bool init)
        {
            _targetRect.rotation = _getter();
        }

        private void Awake()
        {
            if (_targetRect == null)
                _targetRect = GetComponent<RectTransform>();
            Init(ref _getter);
        }
    }
}