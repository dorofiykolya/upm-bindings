using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Bindings
{
    [BindTo(typeof(Texture))]
    public class TextureBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private RawImage _texture;
        [SerializeField] private bool _makeTransparentOnNullSprite;
#pragma warning restore 649

        private Func<Texture> _getter;
        private Color _initialColor;

        protected override void Bind(bool init)
        {
            _texture.texture = _getter();

            if (_makeTransparentOnNullSprite)
                _texture.color = _texture.texture == null ? new Color(0, 0, 0, 0) : _initialColor;
        }

        private void Awake()
        {
            if (_texture == null)
                _texture = GetComponent<RawImage>();

            _initialColor = _texture.color;

            Init(ref _getter);
        }
    }
}