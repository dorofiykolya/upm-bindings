using System;
using UnityEngine;

namespace Common.Bindings
{
    [BindTo(typeof(bool))]
    public class GameObjectEnableBinderWithDelay : ABinder
    {
#pragma warning disable 649
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private bool _disableObjectsOnAwake;
        [SerializeField] private bool _revert;
        [SerializeField] private float _delay = 0;
#pragma warning restore 649

        private Func<bool> _getter;

        private bool _isDelay;
        private float _targetTime;

        protected override void Bind(bool init)
        {
            _isDelay = true;
            _targetTime = Time.realtimeSinceStartup + _delay;
        }

        private void BindAfterDelay()
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

        private void Update()
        {
            if (!_isDelay) return;

            if (Time.realtimeSinceStartup > _targetTime)
            {
                _isDelay = false;
                BindAfterDelay();
            }
        }
    }
}