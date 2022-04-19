using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Bindings
{
    [BindTo(typeof(Color))]
    public class GraphicColorBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private Graphic _widget;
#pragma warning restore 649

        private Func<Color> _getter;

        protected override void Bind(bool init)
        {
            _widget.color = _getter();
        }

        private void Awake()
        {
            if (_widget == null)
                _widget = GetComponent<Graphic>();

            Init(ref _getter);
        }
    }
}