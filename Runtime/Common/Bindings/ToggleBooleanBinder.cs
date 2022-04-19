using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Bindings
{
    [BindTo(typeof(bool))]
    public class ToggleBooleanBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private Toggle _toggle;
        [SerializeField] private bool _revert;
#pragma warning restore 649

        private Func<bool> _getter;
        private Action<bool> _setter;

        protected override void Bind(bool init)
        {
            _toggle.isOn = _revert ? !_getter() : _getter();
        }

        private void Awake()
        {
            if (_toggle == null)
                _toggle = GetComponent<Toggle>();

            Init(ref _getter);
            Init(ref _setter);

            if (_setter != null && _toggle != null) _toggle.onValueChanged.AddListener(Setter);
        }

        private void Setter(bool isOn)
        {
            _setter(isOn);
        }
    }
}