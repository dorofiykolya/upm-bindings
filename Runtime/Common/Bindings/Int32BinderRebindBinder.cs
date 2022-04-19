using System;
using UnityEngine;

namespace Common.Bindings
{
    [BindTo(typeof(int))]
    public class Int32BinderRebindBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private ABinder _binder;
#pragma warning restore 649

        private Func<int> _getter;
        private int _data;

        protected override void Bind(bool init)
        {
            var data = _getter();

            if (data != _data)
            {
                _data = data;
                _binder.Rebind();
            }
        }

        private void Update()
        {
            Bind(false);
        }

        private void Awake()
        {
            Init(ref _getter);
        }
    }
}