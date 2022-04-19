using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Bindings
{
    [BindTo(typeof(bool))]
    public class GraphicBooleanAlphaBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] [Range(0.0F, 1.0F)] private float _falseVal;
        [SerializeField] [Range(0.0F, 1.0F)] private float _trueVal;
        [SerializeField] private bool _applyToChildren = false;
        [SerializeField] private Graphic _widget;
#pragma warning restore 649

        private Func<bool> _getter;

        protected override void Bind(bool init)
        {
            if (_widget != null)
            {
                var color = _widget.color;
                color.a = _getter() ? _trueVal : _falseVal;
                _widget.color = color;
            }

            if (_applyToChildren)
                foreach (var item in GetComponentsInChildren<Graphic>())
                {
                    var col = item.color;
                    col.a = _getter() ? _trueVal : _falseVal;
                    item.color = col;
                }
        }

        private void Awake()
        {
            if (_widget == null)
                _widget = GetComponent<Graphic>();

            Init(ref _getter);
        }
    }
}