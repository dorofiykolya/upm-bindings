using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Bindings
{
    [BindTo(typeof(bool))]
    public class SliderBooleanBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private Slider _slider;
#pragma warning restore 649

        private Func<bool> _getter;
        private Action<bool> _setter;

        protected override void Bind(bool init)
        {
            //if( init )
            //	_slider.Set( _getter( ) ? 1 : 0, false );

            //else
            _slider.value = _getter() ? 1 : 0;
        }

        private void Awake()
        {
            if (_slider == null)
                _slider = GetComponent<Slider>();

            Init(ref _getter);
            Init(ref _setter);

            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            _setter(value > 0.5f);
        }
    }
}