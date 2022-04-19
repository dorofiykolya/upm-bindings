using System;
using UnityEngine;

namespace Common.Bindings
{
    [BindTo(typeof(bool))]
    public class BoxColliderBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private BoxCollider _collider;
#pragma warning restore 649

        private Func<bool> _getter;

        protected override void Bind(bool init)
        {
            _collider.enabled = _getter();
        }

        private void Awake()
        {
            Init(ref _getter);
        }
    }
}