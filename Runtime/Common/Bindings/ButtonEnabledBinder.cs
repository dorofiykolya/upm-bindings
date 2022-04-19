using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Bindings
{
    [BindTo(typeof(bool))]
    public class ButtonEnabledBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private Button _button;
        [SerializeField] private bool _revert;
#pragma warning restore 649

        private Func<bool> _getter;

        protected override void Bind(bool init)
        {
            _button.interactable = _revert ? !_getter() : _getter();
        }

        private void Awake()
        {
            if (_button == null)
                _button = GetComponent<Button>();

            Init(ref _getter);
        }
    }
}