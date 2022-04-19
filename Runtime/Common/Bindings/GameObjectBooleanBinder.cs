using System;
using UnityEngine;

namespace Common.Bindings
{
    [BindTo(typeof(bool))]
    public class GameObjectBooleanBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private bool _disableObjectsOnAwake;
        [SerializeField] private GameObject _false;
        [SerializeField] private GameObject _true;
#pragma warning restore 649

        private Func<bool> _getter;

        protected override void Bind(bool init)
        {
            var isTrue = _getter();

            if (_true != null) _true.SetActive(isTrue);
            if (_false != null) _false.SetActive(!isTrue);
        }

        private void Awake()
        {
            if (_disableObjectsOnAwake)
            {
                if (_true != null) _true.SetActive(false);
                if (_false != null) _false.SetActive(false);
            }

            Init(ref _getter);
        }
    }
}