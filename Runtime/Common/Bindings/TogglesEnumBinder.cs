using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Bindings
{
    [BindTo(typeof(Enum))]
    public class TogglesEnumBinder : ABinder
    {
#pragma warning disable 649
        [Serializable]
        private struct EnumBindingPair
        {
            public string EnumVal;
            public Toggle TargetToggle;
        }

        [SerializeField] private EnumBindingPair[] _stateObjects;
#pragma warning restore 649

        private Func<Enum> _getter;

        protected override void Bind(bool init)
        {
            var baseEnum = _getter();
            var valueName = Enum.GetName(baseEnum.GetType(), baseEnum);

            //DON'T CHANGE! Done on purpose for case when one gameobject set for two enum states
            {
                foreach (var bindingPair in _stateObjects)
                    if (bindingPair.TargetToggle != null)
                        bindingPair.TargetToggle.isOn = false;

                foreach (var bindingPair in _stateObjects)
                    if (bindingPair.TargetToggle != null && bindingPair.EnumVal == valueName)
                        bindingPair.TargetToggle.isOn = true;
            }
        }

        private void Awake()
        {
            Init(ref _getter);
        }
    }
}