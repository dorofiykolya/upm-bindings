using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Bindings
{
    [BindTo(typeof(Sprite))]
    public class ImageBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private Image _sprite;
        [SerializeField] private bool _makeTransparentOnNullSprite;
        [SerializeField] private bool _override;
#pragma warning restore 649

        private Func<Sprite> _getter;
        private Color _initialColor;

        protected override void Bind(bool init)
        {
            if (_override)
                _sprite.overrideSprite = _getter();

            else
                _sprite.sprite = _getter();
            _sprite.SetAllDirty();

            if (_makeTransparentOnNullSprite)
                _sprite.color = _sprite.sprite == null ? new Color(0, 0, 0, 0) : _initialColor;
        }

        private void Awake()
        {
            if (_sprite == null)
                _sprite = GetComponent<Image>();

            _initialColor = _sprite.color;

            Init(ref _getter);
        }
    }
}