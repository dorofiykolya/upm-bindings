using System;
using UnityEngine;

namespace Common.Bindings
{
    [BindTo(typeof(bool))]
    public class GameObjectRendererEnableBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private bool _disableObjectsOnAwake;
        [SerializeField] private bool _revert;
#pragma warning restore 649

        private Func<bool> _getter;
        private Renderer[] _renderers;

        protected override void Bind(bool init)
        {
            var state = _revert ? !_getter() : _getter();
            if (_gameObject != null) EnableRenderer(state);
        }

        private void EnableRenderer(bool enable)
        {
            if (_renderers != null)
                foreach (var r in _renderers)
                    r.enabled = enable;
        }

        private void Awake()
        {
            if (_gameObject != null)
            {
                _renderers = _gameObject.GetComponentsInChildren<Renderer>();
                if (_disableObjectsOnAwake)
                    if (_renderers != null)
                        EnableRenderer(false);

                Init(ref _getter);
            }
        }
    }
}