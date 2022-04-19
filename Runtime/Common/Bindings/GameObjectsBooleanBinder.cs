using System;
using UnityEngine;

namespace Common.Bindings
{
    [BindTo(typeof(bool))]
    public class GameObjectsBooleanBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private GameObject[] _true;
        [SerializeField] private GameObject[] _false;
        [SerializeField] private bool _disableObjectsOnAwake;
#pragma warning restore 649

        private Func<bool> _getter;

        protected override void Bind(bool init)
        {
            var isTrue = _getter();

            foreach (var go in _true)
                if (go != null)
                    go.SetActive(isTrue);

            foreach (var go in _false)
                if (go != null)
                    go.SetActive(!isTrue);
        }

        private void Awake()
        {
            if (_disableObjectsOnAwake)
            {
                foreach (var go in _true)
                    if (go != null)
                        go.SetActive(false);

                foreach (var go in _false)
                    if (go != null)
                        go.SetActive(false);
            }

            Init(ref _getter);
        }
    }
}