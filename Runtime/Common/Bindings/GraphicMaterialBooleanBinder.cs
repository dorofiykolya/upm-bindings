using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Bindings
{
    [BindTo(typeof(bool))]
    public class GraphicMaterialBooleanBinder : ABinder
    {
        private static readonly Graphic[] _empty = new Graphic[0];

#pragma warning disable 649
        [SerializeField] private Material _true;
        [SerializeField] private Material _false;
        [SerializeField] private Graphic[] _widgets;
        [SerializeField] private Graphic _widget;
#pragma warning restore 649

        private Func<bool> _getter;

        protected override void Bind(bool init)
        {
            var mat = _getter() ? _true : _false;
            SetMaterial(_widget, mat);
            if (_widgets != null)
                foreach (var widget in _widgets)
                    SetMaterial(widget, mat);
        }

        private void SetMaterial(Graphic graphic, Material mat)
        {
            if (graphic != null)
            {
                graphic.material = mat;
                graphic.SetAllDirty();
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