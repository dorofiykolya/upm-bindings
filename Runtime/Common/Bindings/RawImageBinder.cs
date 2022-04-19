using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Bindings
{
    [BindTo(typeof(Texture))]
    public class RawImageBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private RawImage _texture;
#pragma warning restore 649

        private Func<Texture> _getter;

        protected override void Bind(bool init)
        {
            _texture.texture = _getter();
        }

        private void Awake()
        {
            Init(ref _getter);
        }
    }
}