using System;
using UnityEngine;

namespace Common.Bindings
{
    [BindTo(typeof(Enum))]
    public class GameObjectsEnumBinder : ABinder
    {
#pragma warning disable 649

        [Serializable]
        private struct EnumBindingPair
        {
            public string EnumVal;
            public GameObject[] TargetObjects;

            public void SetActive(bool active)
            {
                for (var i = 0; i < TargetObjects.Length; ++i)
                {
                    if (TargetObjects[i] == null) continue;
                    TargetObjects[i].SetActive(active);
                }
            }
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
                    if (bindingPair.TargetObjects != null)
                        bindingPair.SetActive(false);

                foreach (var bindingPair in _stateObjects)
                    if (bindingPair.TargetObjects != null && bindingPair.EnumVal == valueName)
                        bindingPair.SetActive(true);
            }
        }

        private void Awake()
        {
            Init(ref _getter);
        }
    }
}