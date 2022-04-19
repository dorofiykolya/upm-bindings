using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Bindings
{
    [BindTo(typeof(float))]
    public class ImageFillAmountBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private Image _sprite;
#pragma warning restore 649

        private Func<float> _getter;

        protected override void Bind(bool init)
        {
            _sprite.fillAmount = _getter();
        }

        private void Awake()
        {
            Init(ref _getter);
        }
    }
}