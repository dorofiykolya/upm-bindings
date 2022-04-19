using System;
using UnityEngine;

namespace Common.Bindings
{
    [BindTo(typeof(int))]
    public class GameObjectStateBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private GameObject[] _stateObjects;
#pragma warning restore 649

        private Func<int> _getter;

        protected override void Bind(bool init)
        {
            var state = _getter();

            foreach (var stateObject in _stateObjects)
                if (stateObject != null && stateObject.activeSelf)
                    stateObject.SetActive(false);

            if (_stateObjects.Length >= state && _stateObjects[state] != null && !_stateObjects[state].activeSelf)
                _stateObjects[state].SetActive(true);
        }

        private void Awake()
        {
            Init(ref _getter);
        }
    }
}