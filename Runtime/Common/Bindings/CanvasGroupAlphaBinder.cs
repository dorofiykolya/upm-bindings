using System;
using UnityEngine;

namespace Common.Bindings
{
    [BindTo(typeof(float))]
    public class CanvasGroupAlphaBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private CanvasGroup _widget;
#pragma warning restore 649

        private Func<float> _getter;
        private Action<float> _setter;

        protected override void Bind(bool init)
        {
            if (_widget == null)
                _widget = GetComponent<CanvasGroup>();

            _widget.alpha = _getter();
        }

        private void Awake()
        {
            Init(ref _getter);
            Init(ref _setter, false);
        }
    }
}