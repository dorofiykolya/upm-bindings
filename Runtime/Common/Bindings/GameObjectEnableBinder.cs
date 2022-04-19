using System;
using UnityEngine;

namespace Common.Bindings
{
    [BindTo(typeof(bool))]
    public class GameObjectEnableBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private bool _disableObjectsOnAwake;
        [SerializeField] private bool _revert;
#pragma warning restore 649

        private Func<bool> _getter;

        protected override void Bind(bool init)
        {
            var state = _revert ? !_getter() : _getter();

            if (_gameObject != null) _gameObject.SetActive(state);
        }

        private void Awake()
        {
            if (_disableObjectsOnAwake)
                if (_gameObject != null)
                    _gameObject.SetActive(false);

            Init(ref _getter);
        }
    }
}