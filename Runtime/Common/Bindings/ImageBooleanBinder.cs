using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Bindings
{
    [BindTo(typeof(bool))]
    public class ImageBooleanBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private Image _widget;
        [Space(5)] [SerializeField] private Sprite _ableSprite;
        [SerializeField] private Sprite _unableSprite;
#pragma warning restore 649

        private Func<bool> _getter;

        protected override void Bind(bool init)
        {
            _widget.sprite = _getter() ? _ableSprite : _unableSprite;
        }

        private void Awake()
        {
            if (_widget == null)
                _widget = GetComponent<Image>();
            Init(ref _getter);
        }
    }
}