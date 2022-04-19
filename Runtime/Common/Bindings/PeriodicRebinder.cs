using System;
using UnityEngine;

namespace Common.Bindings
{
    [RequireComponent(typeof(ABinder))]
    public class PeriodicRebinder : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private float _periodSeconds;
#pragma warning restore 649

        private float _timer;
        private ABinder[] _binders;

        private void Awake()
        {
            _binders = GetComponents<ABinder>();
        }

        private void OnEnable()
        {
            _timer = Time.unscaledTime;
        }

        private void Update()
        {
            if (Time.unscaledTime - _timer < _periodSeconds)
                return;

            _timer += _periodSeconds;

            if (_binders != null && _binders.Length > 0)
                foreach (var aBinder in _binders)
                    if (aBinder != null)
                    {
                        aBinder.Rebind();
                    }
                    else
                    {
#if DEBUG
                        Debug.LogWarning("[PeriodicRebinder] aBinder == null");
#endif
                    }
        }
    }
}