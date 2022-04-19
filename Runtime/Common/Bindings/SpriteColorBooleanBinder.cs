using System;
using UnityEngine;

namespace Common.Bindings
{
    [BindTo(typeof(bool))]
    public class SpriteColorBooleanBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private Color _ableColor = Color.gray;
        [SerializeField] private Color _unableColor = Color.red;
        [SerializeField] private SpriteRenderer _widget;
#pragma warning restore 649

        private Func<bool> _getter;

        protected override void Bind(bool init)
        {
            var color = _getter() ? _ableColor : _unableColor;
            _widget.color = color;
        }

        private void Awake()
        {
            if (_widget == null)
                _widget = GetComponent<SpriteRenderer>();
            Init(ref _getter);
        }
    }
}