using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Bindings
{
    [BindTo(typeof(bool))]
    public class GraphicColorBooleanBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private Color _ableColor = Color.gray;
        [SerializeField] private Color _unableColor = Color.red;
        [SerializeField] private Graphic _widget;
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
                _widget = GetComponent<Graphic>();
            Init(ref _getter);
        }
    }
}