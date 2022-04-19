using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Bindings
{
    [BindTo(typeof(Enum))]
    public class ImageEnumBinder : ABinder
    {
#pragma warning disable 649

        [Serializable]
        private struct EnumBindingPair
        {
            public string EnumVal;
            public Sprite Sprite;
        }

        [SerializeField] private Image _image;
        [SerializeField] private bool _makeTransparentOnNullSprite;
        [SerializeField] private EnumBindingPair[] _stateObjects;
        [SerializeField] private Sprite _defaultSprite;
#pragma warning restore 649

        private Func<Enum> _getter;
        private Color _initialColor;
        private bool _binded;

        protected override void Bind(bool init)
        {
            _binded = false;
            var baseEnum = _getter();
            var valueName = Enum.GetName(baseEnum.GetType(), baseEnum);

            foreach (var bindingPair in _stateObjects)
                if (bindingPair.EnumVal == valueName)
                {
                    _image.sprite = bindingPair.Sprite;
                    _binded = true;
                }

            if (_makeTransparentOnNullSprite)
                _image.color = !_binded ? _initialColor * new Color(1f, 1f, 1f, 0f) : _initialColor;

            if (_defaultSprite != null && !_binded) _image.sprite = _defaultSprite;
        }

        private void Awake()
        {
            if (_image == null)
                _image = GetComponent<Image>();

            _initialColor = _image.color;

            Init(ref _getter);
        }
    }
}