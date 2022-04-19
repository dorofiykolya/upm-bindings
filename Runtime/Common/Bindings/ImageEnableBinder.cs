using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Bindings
{
    [BindTo(typeof(bool))]
    public class ImageEnableBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private Image _sprite;
#pragma warning restore 649

        private Func<bool> _getter;

        protected override void Bind(bool init)
        {
            _sprite.enabled = _getter();
            _sprite.SetAllDirty();
        }

        private void Awake()
        {
            if (_sprite == null)
                _sprite = GetComponent<Image>();
            Init(ref _getter);
        }
    }
}