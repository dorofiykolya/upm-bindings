using System;
using UnityEngine;

namespace Common.Bindings
{
    [BindTo(typeof(string))]
    public class GameObjectStringBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private string _comparison;
        [SerializeField] private GameObject _equals;
        [SerializeField] private GameObject _notEquals;
#pragma warning restore 649

        private Func<string> _getter;

        protected override void Bind(bool init)
        {
            var equals = _getter() == _comparison;

            if (_equals != null) _equals.SetActive(equals);
            if (_notEquals != null) _notEquals.SetActive(!equals);
        }

        private void Awake()
        {
            Init(ref _getter);
        }
    }
}