using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Bindings
{
    [BindTo(typeof(bool))]
    public class InteractableEnableBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private Selectable _selectable;
        [SerializeField] private bool _revert;
#pragma warning restore 649

        private Func<bool> _getter;

        protected override void Bind(bool init)
        {
            _selectable.interactable = _revert ? !_getter() : _getter();
        }

        private void Awake()
        {
            if (_selectable == null)
                _selectable = GetComponent<Selectable>();

            Init(ref _getter);
        }
    }
}